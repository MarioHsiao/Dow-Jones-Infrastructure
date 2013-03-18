using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Web.ClientResources;

namespace DowJones.Web
{
    public class ClientResourceManager : IClientResourceManager
    {
        public const string ResourceNameDelimiter = ";";
        private const StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

        private readonly IDictionary<string, string> _aliasCache;
        private readonly IDictionary<string, string> _nameCache;
        private readonly IEnumerable<IClientResourceRepository> _repositories;

        public IEnumerable<ClientResourceAlias> Aliases { get; private set; }

        public IEnumerable<ClientResource> ClientResources
        {
            get { return _repositories.SelectMany(x => x.GetClientResources()); }
        }

        public Func<string, CultureInfo, string> UrlResolver { get; set; }

        [Inject("")]
        public ClientResourceManager(IEnumerable<IClientResourceRepository> repositories, ClientResourceConfiguration config)
            :this(repositories, config.Aliases)
        {
        }

        internal ClientResourceManager(IEnumerable<IClientResourceRepository> repositories, IEnumerable<ClientResourceAlias> aliases)
        {
            _aliasCache = new Dictionary<string, string>();
            _nameCache = new Dictionary<string, string>();

            _repositories = repositories ?? Enumerable.Empty<IClientResourceRepository>();

            Aliases = aliases ?? Enumerable.Empty<ClientResourceAlias>();
            UrlResolver = ClientResourceHandler.GenerateUrl;
        }


        public string Alias(string name)
        {
            var alias = Resolve(name, x => x.Name, x => x.Alias, _aliasCache);
            return alias;
        }

        public string Dealias(string alias)
        {
            if ((alias ?? string.Empty).IsUrl())
                return alias;

            var name = Resolve(alias, x => x.Alias, x => x.Name, _nameCache);
            return name;
        }

        public string GenerateUrl(IEnumerable<ClientResource> resources, CultureInfo culture)
        {
            IEnumerable<string> aliases = resources.Select(resource => this.Alias(resource));
            string combinedAliases = string.Join(ResourceNameDelimiter, aliases);

            string url = UrlResolver(combinedAliases, culture);

            return url;
        }

        public ClientResource GetClientResource(ClientResource resource)
        {
            var globalResource = ClientResources.FirstOrDefault(x => x == resource);
            return globalResource;
        }

        public IEnumerable<ClientResource> GetClientResources(IEnumerable<string> resourceNames)
        {
            var dealiased = resourceNames.SelectMany(ParseClientResourceNames).ToArray();

            var registeredResources = ClientResources.Where(x => dealiased.Contains(x.Name)).ToList();

            var unregisteredResources = GetUnregisteredResources(dealiased, registeredResources);

            return registeredResources.Union(unregisteredResources).ToArray();
        }

        private static IEnumerable<ClientResource> GetUnregisteredResources(IEnumerable<string> resourceNames, IEnumerable<ClientResource> registeredResources)
        {
            var unresolvedResourceNames = resourceNames.Except(registeredResources.Select(x => x.Name));
            var unregisteredResources =
                from name in unresolvedResourceNames
                where !string.IsNullOrWhiteSpace(name)
                select new ClientResource(name);

            return unregisteredResources;
        }

        /// <summary>
        /// Accepts a string containing one or more Client Resource names and/or aliases
        /// and returns the de-aliases Client Resource names
        /// </summary>
        /// <param name="serializedResourceNames"></param>
        /// <returns>List of real (not aliased) Client Resource names</returns>
        public IEnumerable<string> ParseClientResourceNames(string serializedResourceNames)
        {
            if (string.IsNullOrWhiteSpace(serializedResourceNames))
                return Enumerable.Empty<string>();

            IEnumerable<string> parts = serializedResourceNames.Split(ResourceNameDelimiter);
            IEnumerable<string> dealiasedNames = parts.Select(Dealias).ToArray();
            IEnumerable<string> resolvedResources = dealiasedNames.Where(x => !x.Contains(ResourceNameDelimiter)).ToArray();
            IEnumerable<string> recursiveAliases = dealiasedNames.Except(resolvedResources).SelectMany(ParseClientResourceNames).ToArray();

            return resolvedResources.Union(recursiveAliases).ToArray();
        }

        /// <summary>
        /// Resolves a given key to a given value,
        /// including values requiring multiple levels of resolution.
        /// Caches the final result to increase the speed
        /// of future resolutions of the same key.
        /// </summary>
        private string Resolve(
                string key,
                Expression<Func<ClientResourceAlias, string>> keyPropertyExpression,
                Expression<Func<ClientResourceAlias, string>> valuePropertyExpression,
                IDictionary<string, string> cache
            )
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;

            var getKey = keyPropertyExpression.Compile();
            var getValue = valuePropertyExpression.Compile();

            string cachedValue;
            if (cache.TryGetValue(key, out cachedValue))
                return cachedValue;

            var value = key;

            // Cycle through the list to resolve multiple levels
            var tempValue = key;
            do
            {
                tempValue =
                    Aliases
                        .Where(x => string.Equals(getKey(x), tempValue, IgnoreCase))
                        .Select(getValue)
                        .FirstOrDefault();

                value = tempValue ?? value;

            } while (tempValue != null);

            if (value != key)
            {
                try { cache.Add(key, value); }
                catch { }
            }

            return value;
        }
    }
}