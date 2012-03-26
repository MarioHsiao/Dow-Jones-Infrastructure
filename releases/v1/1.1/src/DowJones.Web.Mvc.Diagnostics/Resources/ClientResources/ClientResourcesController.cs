using System;
using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.ClientResources
{
    public class ClientResourcesController : DiagnosticsController
    {
        private readonly IClientResourceManager _resourceManager;

        public ClientResourcesController(IClientResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public ActionResult Index(string query, string resourceName, string alias)
        {
            if (!string.IsNullOrWhiteSpace(resourceName))
                return GetAliasFor(resourceName);

            if (!string.IsNullOrWhiteSpace(alias))
                return Dealias(alias);

            return GetAliasList(query);
        }

        private ActionResult Dealias(string alias)
        {
            var resourceName = _resourceManager.Dealias(alias);

            if (string.IsNullOrWhiteSpace(resourceName) || string.Equals(alias, resourceName, StringComparison.OrdinalIgnoreCase))
                resourceName = ClientResourcesInfo.NotFoundValue;

            return View<ClientResourceDebuggerView>(new ClientResourcesInfo {
                                                            Alias = alias,
                                                            ResourceName = resourceName,
                                                        });
        }

        private ActionResult GetAliasFor(string resourceName)
        {
            var alias = _resourceManager.Alias(resourceName);

            if (string.IsNullOrWhiteSpace(alias) || string.Equals(alias, resourceName, StringComparison.OrdinalIgnoreCase))
                alias = ClientResourcesInfo.NotFoundValue;

            return View<ClientResourceDebuggerView>(new ClientResourcesInfo {
                                                            Alias = alias,
                                                            ResourceName = resourceName,
                                                        });
        }

        private ActionResult GetAliasList(string query)
        {
            var manager = _resourceManager as ClientResourceManager;

            var aliases =
                from alias in manager.Aliases
                orderby alias.Name
                select new ClientResourceAliasInfo(alias);

            if(!string.IsNullOrWhiteSpace(query))
                aliases = aliases.Where(alias =>
                        alias.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                     || alias.Alias.Contains(query, StringComparison.OrdinalIgnoreCase)
                );

            var resourceInfo = new ClientResourcesInfo{
                Aliases = aliases,
                ClientResources = manager.ClientResources.Select(x => new ClientResourceInfo(x)),
                SearchQuery = query
            };

            return View<ClientResourceDebuggerView>(resourceInfo);
        }
    }
}
