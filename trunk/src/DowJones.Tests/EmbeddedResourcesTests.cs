using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI;
using DowJones.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones
{
    [TestClass]
    public class EmbeddedResourcesTests
    {
#pragma warning disable 169
        // HACK: Refer to the Components assembly so it's included!
        private static readonly Type ArbitraryComponentTypeReference = typeof (DowJones.Web.Mvc.UI.Components.Menu.MenuExtensions);
#pragma warning restore 169


        [TestMethod]
        public void AllEmbeddedClientResourcesShouldBeAvailableAsAssemblyResources()
        {
            var assemblyNames = Directory.GetFiles(Path.GetDirectoryName(GetType().Assembly.Location), "DowJones.*.dll");
            var ignoredAssemblyNames = assemblyNames.Where(name => Regex.IsMatch(Path.GetFileName(name), @"\.Test", RegexOptions.IgnoreCase)).ToArray();

            Debug.WriteLine("Found assemblies: " + string.Join("; ", assemblyNames));
            Debug.WriteLine("Ignoring assemblies: " + string.Join("; ", ignoredAssemblyNames));

            var assemblies = 
                assemblyNames
                    .Except(ignoredAssemblyNames)
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
                        select attribute
                    select new { assembly, definitions }
                ).ToArray();

            Assert.AreNotEqual(0, clientResourceAssemblies.Count(),
                               "No Client Resources found");

            IList<string> failures = new List<string>();

            foreach(var resourceAssembly in clientResourceAssemblies)
            {
                var embeddedClientResources =
                    resourceAssembly.definitions
                        .Where(x => !string.IsNullOrWhiteSpace(x.ResourceName))
                        .ToArray();

                Debug.WriteLine("No Embedded Client Resources found in assembly " + resourceAssembly.assembly.FullName);

                foreach (var resource in embeddedClientResources)
                {
                    ValidateEmbeddedClientResource(resource, resourceAssembly.assembly, failures);
                }
            }

            if(failures.Any())
                Assert.Fail("\r\n" + string.Join("\r\n", failures));

            Debug.WriteLine("Validated {0} embedded client resources across {1} assemblies",
                            clientResourceAssemblies.SelectMany(x => x.definitions).Count(), 
                            clientResourceAssemblies.Count());
        }

        private static void ValidateEmbeddedClientResource(ClientResourceAttribute resource, Assembly assembly, IList<string> failures)
        {
            var resourceName = resource.ResourceName;
            var assemblyName = resource.DeclaringAssembly;
            var clientResourceAssembly =
                (string.IsNullOrWhiteSpace(assemblyName))
                    ? assembly
                    : Assembly.Load(assemblyName);

            Debug.WriteLine("Validating existence of {0} in {1}", resourceName, clientResourceAssembly.FullName);

            if(clientResourceAssembly.GetManifestResourceStream(resourceName) == null)
                failures.Add(string.Format("Could not find Embedded Resource {0} in {1}", resourceName, clientResourceAssembly.FullName));

            var webResources = GetWebResourceAttributes(assembly);
            if(webResources.Any(x => x.WebResource == resourceName) == false)
                failures.Add(string.Format("Could not find WebResource Attribute for {0} in {1}", resourceName, clientResourceAssembly.FullName));
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
