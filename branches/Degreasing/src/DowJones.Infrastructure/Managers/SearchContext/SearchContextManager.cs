using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Exceptions;
using DowJones.Managers.Search.Requests;
using DowJones.Preferences;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Utilities;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Newsstand.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.V1_0;
using Hammock;

namespace DowJones.Managers.SearchContext
{
    public class SearchContextManager
    {
        readonly string searchRequestServiceUrl = Settings.Default.SearchRequestServicePath;
        readonly string headlinesServiceUrl = Settings.Default.HeadlinesServicePath;

        private readonly IControlData controlData;
        private readonly IPreferences preferences;

        public SearchContextManager(IControlData controlData, IPreferences preferences)
        {
            this.controlData = controlData;
            this.preferences = preferences;
        }

        public PortalHeadlineListDataResult GetHeadlines(string searchContextString, int firstResult, int maxResults)
        {
            var client = new RestClient
            {
                Authority = Settings.Default.DashboardServiceBaseUrl
            };
            client.AddHeader("Content-Type", "text/xml");
            client.AddHeader("preferences", preferences.Serialize(Formatting.None, Encoding.Default));

            var request = new RestRequest
            {
                Path = string.Format("{0}?{1}&searchContextRef={2}&firstResultToReturn={3}&maxResultsToReturn={4}",
                                     headlinesServiceUrl,
                                     GetAuthorizationString(),
                                     searchContextString,
                                     firstResult,
                                     maxResults)
            };

            var response = client.Request(request);

            var xDocument = XDocument.Parse(response.Content);

            var errorNode = GetNode(xDocument, "error");
            if (errorNode != null)
            {
                throw new DowJonesUtilitiesException(long.Parse(GetValue(errorNode, "code")));
            }

            var returnCode = GetValue(xDocument, "returnCode");

            if (returnCode != null && returnCode != "0")
                throw new DowJonesUtilitiesException(GetValue(xDocument, "statusMessage"), long.Parse(returnCode));

            try
            {
                var portalHeadlineListDataResultNode = GetNode(xDocument, "portalHeadlineListDataResult");
                return portalHeadlineListDataResultNode.ToString().Deserialize<PortalHeadlineListDataResult>();
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException("Failed to deserialize PortalHeadlineListDataResult xml.", ex);
            }
        }

        public TSearch CreateSearchRequest<TSearch>(string searchContextString, int firstResult, int maxResults)
            where TSearch : class, IPerformContentSearchRequest, new()
        {
            return GetSearchRequest<TSearch>(searchContextString, firstResult, maxResults);
        }

        public GetMultipleNewsstandSectionHeadlinesRequest CreateNewsstandHeadlinesRequest(string searchContextString, int firstResult, int maxResults)
        {
            return GetSearchRequest<GetMultipleNewsstandSectionHeadlinesRequest>(searchContextString, firstResult, maxResults);
        }

        public CodedStructuredSearchRequest CreateCodedStructuredSearchRequest(string searchContextString, int firstResult, int maxResults)
        {
            return GetSearchRequest<CodedStructuredSearchRequest>(searchContextString, firstResult, maxResults);
        }

        private TSearchRequest GetSearchRequest<TSearchRequest>(string searchContextString, int firstResultToReturn, int maxResultsToReturn)
            where TSearchRequest : class, new()
        {
            var client = new RestClient
            {
                Authority = Settings.Default.DashboardServiceBaseUrl
            };
            client.AddHeader("Content-Type", "text/xml");
            client.AddHeader("preferences", preferences.Serialize(Formatting.None, Encoding.Default));

            var request = new RestRequest
            {
                Path = string.Format("{0}?{1}&searchContextRef={2}&firstResultToReturn={3}&maxResultsToReturn={4}&dbg=false",
                                     searchRequestServiceUrl,
                                     GetAuthorizationString(true),
                                     searchContextString,
                                     firstResultToReturn,
                                     maxResultsToReturn)
            };

            var response = client.Request(request);

            var xDocument = XDocument.Parse(response.Content);

            var errorNode = GetNode(xDocument, "error");
            if (errorNode != null)
            {
                throw new DowJonesUtilitiesException(long.Parse(GetValue(errorNode, "code")));
            }

            var returnCode = GetValue(xDocument, "returnCode");

            if (returnCode!= null && returnCode != "0")
                throw new DowJonesUtilitiesException(GetValue(xDocument, "statusMessage"), long.Parse(returnCode));
            
            if (typeof(TSearchRequest).FullName != GetValue(xDocument, "searchRequestTypeFullName"))
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SearchContextTypeIsInvalid);
            
            try
            {
                var searchRequestXml = GetValue(xDocument, "searchRequestXml");
                // TODO: NN - this is a work around until we have deserialize for freesearch - PerformContentSearchRequest in gateway
                //if (typeof(TSearchRequest) == typeof(Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest))
                //{
                //    var pcs = searchRequestXml.XmlDeserialize<PerformContentSearchRequest>();
                //    return new Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest
                //    {
                //        DescriptorControl = pcs.DescriptorControl,
                //        FirstResult = pcs.FirstResult,
                //        MaxResults = pcs.MaxResults,
                //        NavigationControl = pcs.NavigationControl,
                //        SearchContext = pcs.SearchContext,
                //        StructuredSearch = pcs.StructuredSearch,
                                   
                //    } as TSearchRequest;
                //}

                // Will not work until gateway is updated with Deserialize methods for Newsstand and FreeSearch requests
                return searchRequestXml.Deserialize<TSearchRequest>();
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException("Failed to deserialize search request xml.", ex);
            }
        }

        /// <summary>
        /// Get sessionId or encryptedToken from ControlData.
        /// If both are empty, try to get it by calling GetUserAuthorizations
        /// </summary>
        /// <returns>Authorization parameter (sessionId or encryptedToken) for Dashboard API call.</returns>
        private string GetAuthorizationString(bool isTokenRequired = false)
        {
            if (!string.IsNullOrWhiteSpace(controlData.SessionID))
                return "sessionId=" + controlData.SessionID;

            if (!string.IsNullOrWhiteSpace(controlData.EncryptedToken))
                return "encryptedtoken=" + controlData.EncryptedToken;

            if (isTokenRequired)
                throw new DowJonesUtilitiesException(680003); // session id is null

            try
            {
                var response = Factiva.Gateway.Services.V1_0.MembershipService.AuthorizeUser(ControlDataManager.Convert(controlData), new AuthorizeUserRequest());

                if (response == null)
                    throw new DowJonesUtilitiesException("GetUserAuthorizations response is null.");

                if (response.rc != 0)
                    throw new DowJonesUtilitiesException("GetUserAuthorizations rc is not 0.", response.rc);

                object obj;
                response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);

                var getUserAuthorizationsResponse = obj as GetUserAuthorizationsResponse;

                if (getUserAuthorizationsResponse == null)
                    throw new DowJonesUtilitiesException("Failed to cast GetUserAuthorizations response to GetUserAuthorizationsResponse.");

                return "sessionId=" + getUserAuthorizationsResponse.SessionId;
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, -1);
            }
        }

        private XElement GetNode(XContainer document, string nodeName)
        {
            var nodes = document.Descendants(nodeName);
            return nodes.Count() == 0 ? null : nodes.First();
        }

        private string GetValue(XContainer document, string nodeName)
        {
            var node = GetNode(document, nodeName);
            return node == null ? null : node.Value;
        }
    }
}
