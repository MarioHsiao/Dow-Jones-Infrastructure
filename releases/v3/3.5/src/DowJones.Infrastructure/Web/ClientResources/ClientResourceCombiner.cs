using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DowJones.Properties;

namespace DowJones.Web
{
    using Level = ClientResourceDependencyLevel;

    public class ClientResourceCombiner
    {
        private readonly IClientResourceManager _clientResourceManager;
        private readonly CultureInfo _culture;
        
        public ClientResourceCombiner(IClientResourceManager clientResourceManager, CultureInfo culture)
        {
            _clientResourceManager = clientResourceManager;
            _culture = culture;
        }
        
        public IEnumerable<string> GenerateCombinedResourceUrls(IEnumerable<ClientResource> resourcesToCombine)
        {
            if (!Settings.Default.ClientResourceCombiningEnabled)
            {
                var urls = resourcesToCombine.Select(x => _clientResourceManager.GenerateUrl(x, _culture));
                return urls.ToArray();
            }

            IList<ClientResource> resources = new List<ClientResource>(resourcesToCombine);

            var combinedResourcesUrls =
                new[] { Level.Core, Level.Global, Level.MidLevel, Level.Component, Level.Independent }
                    .Select(level => CombineResourcesForDependencyLevel(resources, level))
                    .ToArray();

            var uncombinedResourcesUrls = 
                resources.Select(x => _clientResourceManager.GenerateUrl(x, _culture));

            return combinedResourcesUrls
                    .Union(uncombinedResourcesUrls)
                    .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        internal string CombineResourcesForDependencyLevel(IList<ClientResource> resources, ClientResourceDependencyLevel dependencyLevel)
        {
            var dependencyLevelResources = 
                resources
                    .Where(x => x.DependencyLevel == dependencyLevel)
                    .ToArray();

            if (!dependencyLevelResources.Any())
            {
                return null;
            }

            foreach (var resource in dependencyLevelResources)
            {
                resources.Remove(resource);
            }

            return _clientResourceManager.GenerateUrl(dependencyLevelResources, _culture);
        }
    }
}