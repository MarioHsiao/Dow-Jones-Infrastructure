using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.Diagnostics.ClientResources
{
    public class ClientResourcesInfo
    {
        public const string NotFoundValue = "[ NOT FOUND ]";

        public string Alias { get; set; }

        public IEnumerable<ClientResourceAliasInfo> Aliases
        {
            get { return aliases ?? Enumerable.Empty<ClientResourceAliasInfo>(); }
            set { aliases = value; }
        }
        private IEnumerable<ClientResourceAliasInfo> aliases;

        public IEnumerable<ClientResourceInfo> ClientResources
        {
            get { return clientResources ?? Enumerable.Empty<ClientResourceInfo>(); }
            set { clientResources = value; }
        }
        private IEnumerable<ClientResourceInfo> clientResources;

        public bool HasAlias
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Alias) && Alias != NotFoundValue;
            }
        }

        public bool HasResourceName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ResourceName) && ResourceName != NotFoundValue;
            }
        }

        public string ResourceName { get; set; }

        public string SearchQuery { get; set; }

    }

    public class ClientResourceAliasInfo
    {
        public string Alias { get; set; }

        public bool? IsSearchMatch { get; set; }

        public string Name { get; set; }
        

        public ClientResourceAliasInfo()
        {
        }

        public ClientResourceAliasInfo(ClientResourceAlias alias)
        {
            Alias = alias.Alias;
            Name = alias.Name;
        }
    }

    public class ClientResourceInfo
    {
        public ClientResourceInfo(ClientResource resource)
        {

        }
    }
}
