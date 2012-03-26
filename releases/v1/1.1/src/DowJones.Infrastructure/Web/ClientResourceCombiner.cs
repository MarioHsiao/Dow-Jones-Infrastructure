using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DowJones.Properties;

namespace DowJones.Web
{
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

            string globalResourcesUrl = 
                CombineResourcesForDependencyLevel(resources, ClientResourceDependencyLevel.Global);
            
            string midLevelResourcesUrl =
                CombineResourcesForDependencyLevel(resources, ClientResourceDependencyLevel.MidLevel);

            string componentResourcesUrl =
                CombineResourcesForDependencyLevel(resources, ClientResourceDependencyLevel.Component);

            var combinedResourcesUrls = new[] {
                    globalResourcesUrl, 
                    midLevelResourcesUrl, 
                    componentResourcesUrl
                };

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
                return null;

            foreach (var resource in dependencyLevelResources)
                resources.Remove(resource);

            return _clientResourceManager.GenerateUrl(dependencyLevelResources, _culture);
        }
    }
}