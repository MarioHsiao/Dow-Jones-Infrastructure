namespace DowJones.Utilities.Ajax.Converters
{
    using ConnectionResults;
    using ConnectionResults.Converters;
    using Factiva.Gateway.Messages.RelationshipMapping.V1_0;
    using log4net;
    using Tools.Ajax;

    public delegate string GenerateStartConnectionNavigateUrl(StartConnection connection);
    public delegate string GenerateConnectionNavigateUrl(Connection connection);
    public delegate string GenerateEndConnectionNavigateUrl(EndConnection connection);

    public class ConnectionResultsConversionManager
    {
        protected static readonly ILog s_Log = LogManager.GetLogger(typeof(ConnectionResultsConversionManager));

        public ConnectionResultsDataResult Process(PerformRelationshipMappingResponse response)
        {
            IListDataResultConverter converter = new PerformRelationshipMappingResponseConverter(response);

            return (ConnectionResultsDataResult)converter.Process();
        }

        public ConnectionResultsDataResult Process(
            PerformRelationshipMappingResponse response,
            GenerateConnectionNavigateUrl generateConnectionNavigateURL)
        {
            IListDataResultConverter converter = new PerformRelationshipMappingResponseConverter(
                response, generateConnectionNavigateURL);

            return (ConnectionResultsDataResult)converter.Process();
        }

        public ConnectionResultsDataResult Process(
            PerformRelationshipMappingResponse response,
            GenerateStartConnectionNavigateUrl generateStartConnectionNavigateURL,
            GenerateConnectionNavigateUrl generateConnectionNavigateURL,
            GenerateEndConnectionNavigateUrl generateEndConnectionNavigateURL)
        {
            IListDataResultConverter converter = new PerformRelationshipMappingResponseConverter(
                response, generateStartConnectionNavigateURL, generateConnectionNavigateURL, generateEndConnectionNavigateURL);

            return (ConnectionResultsDataResult)converter.Process();
        }
    }
}
