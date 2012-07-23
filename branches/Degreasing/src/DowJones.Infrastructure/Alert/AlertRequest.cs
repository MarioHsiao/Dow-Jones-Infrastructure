using DowJones.Search;
using DowJones.Search.Filters;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;

namespace DowJones.AlertEditor
{
    public class AlertRequestBase
    {
        public AbstractSearchQuery SearchQuery { get; set; }

        public AlertProperties Properties { get; set; }
    }

    public class AlertDetails
    {
        public AlertDetails(AbstractSearchQuery searchQuery, AlertProperties properties, FolderDetails folderDetails)
        {
            SearchQuery = searchQuery;
            Properties = properties;
            FolderDetails = folderDetails;
        }

        public AbstractSearchQuery SearchQuery { get; private set; }

        public AlertProperties Properties { get; private set; }

        public FolderDetails FolderDetails { get; private set; }

        public SearchSetupScreen SearchType { get; set; }
    }
}