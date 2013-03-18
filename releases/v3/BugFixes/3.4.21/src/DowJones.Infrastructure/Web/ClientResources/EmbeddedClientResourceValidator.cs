using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace DowJones.Web.ClientResources
{
    /// <summary>
    /// Diagnostics class to ensure that Client Resources are properly embedded
    /// </summary>
    public class EmbeddedClientResourceValidator
    {
        public IEnumerable<string> ValidateClientResources(string directoryName, string searchPattern, string ignorePattern)
        {
            var assemblyNames = Directory.GetFiles(directoryName, searchPattern);
            var ignoredAssemblyNames = assemblyNames.Where(name => Regex.IsMatch(Path.GetFileName(name), ignorePattern, RegexOptions.IgnoreCase)).ToArray();

            Debug.WriteLine("Found assemblies: " + string.Join("; ", assemblyNames));
            Debug.WriteLine("Ignoring assemblies: " + string.Join("; ", ignoredAssemblyNames));

            var assemblies =
                assemblyNames
                    .Except(ignoredAssemblyNames)
                    .Select(Assembly.LoadFile)
                    .ToArray();

            return ValidateClientResources(assemblies);
        }

        public IEnumerable<string> ValidateClientResources(params Assembly[] assemblies)
        {
            IList<string> failures = new List<string>();

            var clientResourceAssemblies =
                (
                    from assembly in assemblies
                    let definitions = assembly.GetTypes().SelectMany(type => type.GetClientResourceAttributes(false))
                    select new ClientResourceAssemblyAttribute
                        {
                            Assembly = assembly, 
                            Definitions = definitions,
                        }
                ).ToArray();

            if(clientResourceAssemblies.Any() == false)
               failures.Add("No Client Resources found");
            else
                ValidateClientResourceAssemblies(clientResourceAssemblies, failures);

            return failures;
        }

        private static void ValidateClientResourceAssemblies(IEnumerable<ClientResourceAssemblyAttribute> clientResourceAssemblies, IList<string> failures)
        {
            foreach (var resourceAssembly in clientResourceAssemblies)
            {
                var embeddedClientResources =
                    resourceAssembly.Definitions
                        .Where(x => !string.IsNullOrWhiteSpace(x.ResourceName))
                        .ToArray();

                Debug.WriteLine("No Embedded Client Resources found in assembly " + resourceAssembly.Assembly.FullName);

                foreach (var resource in embeddedClientResources)
                {
                    ValidateEmbeddedClientResource(resource, resourceAssembly.Assembly, failures);
                }
            }

            Debug.WriteLine("Validated {0} embedded client resources across {1} assemblies",
                            clientResourceAssemblies.SelectMany(x => x.Definitions).Count(),
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

            if (clientResourceAssembly.GetManifestResourceStream(resourceName) == null)
                failures.Add(string.Format("Could not find Embedded Resource {0} in {1}", resourceName, clientResourceAssembly.FullName));

            var webResources = GetWebResourceAttributes(clientResourceAssembly);
            if (webResources.Any(x => x.WebResource == resourceName) == false)
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


        private class ClientResourceAssemblyAttribute
        {
            public Assembly Assembly { get; set; }
            public IEnumerable<ClientResourceAttribute> Definitions { get; set; }
        }
    }

}
