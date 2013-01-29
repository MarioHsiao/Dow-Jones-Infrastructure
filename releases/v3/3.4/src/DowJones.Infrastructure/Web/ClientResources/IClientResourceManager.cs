using System.Collections.Generic;
using System.Globalization;

namespace DowJones.Web
{
    public interface IClientResourceManager
    {
        /// <summary>
        /// Retrieves the registered alias for a given Client Resource name
        /// </summary>
        string Alias(string clientResourceName);

        /// <summary>
        /// Retrieves the "fully-qualified" Client Resource name for a given alias
        /// </summary>
        string Dealias(string alias);

        /// <summary>
        /// Generates a client URL for a set of Client Resources for a given culture/language
        /// </summary>
        string GenerateUrl(IEnumerable<ClientResource> resources, CultureInfo culture);

        ClientResource GetClientResource(ClientResource resource);

        /// <summary>
        /// Gets the client resources for a given set of Client Resource names and/or aliases
        /// </summary>
        IEnumerable<ClientResource> GetClientResources(IEnumerable<string> resourceNames);

        /// <summary>
        /// Accepts a string containing one or more Client Resource names and/or aliases
        /// and returns the de-aliases Client Resource names
        /// </summary>
        /// <returns>List of real (not aliased) Client Resource names</returns>
        IEnumerable<string> ParseClientResourceNames(string serializedResourceNames);

    }


    public static class IClientResourceManagerExtensions
    {

        public static string Alias(this IClientResourceManager manager, ClientResource resource)
        {
            return manager.Alias(resource.Name);
        }

        public static string GenerateUrl(this IClientResourceManager manager, ClientResource resource, CultureInfo culture)
        {
            return manager.GenerateUrl(new [] { resource }, culture);
        }

        public static IEnumerable<ClientResource> GetClientResources(this IClientResourceManager manager, string serializedResourceNames)
        {
            var resourceNames = manager.ParseClientResourceNames(serializedResourceNames);
            return manager.GetClientResources(resourceNames);
        }
    }
}
