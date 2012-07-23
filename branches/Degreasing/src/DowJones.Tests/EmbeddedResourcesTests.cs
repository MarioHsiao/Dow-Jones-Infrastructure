using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using DowJones.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones
{
    [TestClass]
    public class EmbeddedResourcesTests
    {
        [TestMethod]
        public void AllEmbeddedClientResourcesShouldBeAvailableAsAssemblyResources()
        {
            var assemblies = 
                Directory.GetFiles(Path.GetDirectoryName(GetType().Assembly.Location), "DowJones.*.dll")
                    .Select(Assembly.LoadFile)
                    .ToArray();

            ValidateAssemblyClientResources(assemblies);
        }

        private void ValidateAssemblyClientResources(params Assembly[] assemblies)
        {
            var clientResourceAssemblies =
                (
                    from assembly in assemblies
                    let definitions = 
                        from type in assembly.GetTypes()
                        from attribute in type.GetClientResourceAttributes()
                        select attribute.ToClientResourceDefinition()
                    select new { assembly, definitions }
                ).ToArray();

            Assert.AreNotEqual(0, clientResourceAssemblies.Count(),
                               "No Client Resources found");

            foreach(var resourceAssembly in clientResourceAssemblies)
            {
                var embeddedClientResources =
                    resourceAssembly.definitions
                        .Where(x => !string.IsNullOrWhiteSpace(x.ResourceName))
                        .ToArray();

                Debug.WriteLine("No Embedded Client Resources found in assembly " + resourceAssembly.assembly.FullName);

                foreach (var resource in embeddedClientResources)
                {
                    ValidateEmbeddedClientResource(resource, resourceAssembly.assembly);
                }
            }

            Debug.WriteLine("Validated {0} embedded client resources across {1} assemblies",
                            clientResourceAssemblies.SelectMany(x => x.definitions).Count(), 
                            clientResourceAssemblies.Count());
        }

        private static void ValidateEmbeddedClientResource(ClientResourceDefinition resource, Assembly assembly)
        {
            var resourceName = resource.ResourceName;
            var assemblyName = resource.DeclaringAssembly;
            var clientResourceAssembly =
                (string.IsNullOrWhiteSpace(assemblyName))
                    ? assembly
                    : Assembly.Load(assemblyName);

            Debug.WriteLine("Validating existence of {0} in {1}", resourceName, clientResourceAssembly.FullName);

            Assert.IsNotNull(clientResourceAssembly.GetManifestResourceStream(resourceName),
                             "Could not find Embedded Resource {0} in {1}", resourceName, clientResourceAssembly.FullName);

            var webResources = GetWebResourceAttributes(assembly);
            Assert.IsTrue(webResources.Any(x => x.WebResource == resourceName),
                             "Could not find WebResource Attribute for {0} in {1}", resourceName, clientResourceAssembly.FullName);
        }

        
        private static readonly IDictionary<Assembly, IEnumerable<WebResourceAttribute>> WebResourceAttributes = new Dictionary<Assembly, IEnumerable<WebResourceAttribute>>();
        private static IEnumerable<WebResourceAttribute> GetWebResourceAttributes(Assembly assembly)
        {
            if (!WebResourceAttributes.ContainsKey(assembly))
            {
                var webResources = assembly.GetCustomAttributes(false).OfType<WebResourceAttribute>();
                WebResourceAttributes.Add(assembly, webResources.ToArray());
            }

            return WebResourceAttributes[assembly];
        }
    }
}
