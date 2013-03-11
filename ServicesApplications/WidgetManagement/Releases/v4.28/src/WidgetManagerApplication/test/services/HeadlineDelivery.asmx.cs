using System;
using System.Web.Script.Services;
using System.Web.Services;
using System.ComponentModel;
using EMG.Utility.Managers;
using EMG.widgets.ui.delegates.output;
using EMG.widgets.ui.services.ajax;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.Gateway.Messages.Search.V2_0;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.ajaxDelegates;

namespace emg.widgets.ui.demo.services
{
    /// <summary>
    /// Summary description for HeadlineDelivery
    /// </summary>
    [WebService(Namespace = "urn:emg:widgets.ui:demo:services")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class HeadlineDelivery : BaseWebService
    {

        /// <summary>
        /// Gets the headlines.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="overiddenPreview">The overidden preview.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        [GenerateScriptType(typeof(HeadlinesPluginDelegate))]
        public HeadlinesPluginDelegate GetHeadlines(string searchText, string accessPointCode, string interfaceLanguage, string overiddenPreview)
        {
            HeadlinesPluginDelegate headlinesPluginDelegate = new HeadlinesPluginDelegate();
            try
            {
                headlinesPluginDelegate = new HeadlinesPluginDelegate(GetPerformContentSearchRequest(searchText));
                // Initialize SessionData.
                new SessionData(accessPointCode, interfaceLanguage, 0, true, overiddenPreview, string.Empty);
                headlinesPluginDelegate.Fill();
            }
            catch (FactivaBusinessLogicException fbe)
            {
                
                UpdateAjaxDelegate(fbe, headlinesPluginDelegate);
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), headlinesPluginDelegate);
            }

            return headlinesPluginDelegate;
        }

        /// <summary>
        /// Gets the headlines.
        /// </summary>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="overiddenPreview">The overidden preview.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        [GenerateScriptType(typeof(HeadlinesPluginDelegate))]
        public HeadlinesPluginDelegate GetSummaryHeadlines(string accessPointCode, string interfaceLanguage, string overiddenPreview)
        {
            HeadlinesPluginDelegate headlinesPluginDelegate = new HeadlinesPluginDelegate();
            try
            {
                new SessionData(accessPointCode, interfaceLanguage, 0, true, overiddenPreview, string.Empty);
                headlinesPluginDelegate = new HeadlinesPluginDelegate(GetPerformContentSearchRequestForSummary("rst:tpre Gazprom"), ControlDataManager.GetLightWeightUserControlData("brians", "passwd", "16"));
                // Initialize SessionData.
                
                headlinesPluginDelegate.Fill();
            }
            catch (FactivaBusinessLogicException fbe)
            {

                UpdateAjaxDelegate(fbe, headlinesPluginDelegate);
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), headlinesPluginDelegate);
            }

            return headlinesPluginDelegate;
        }


        /// <summary>
        /// Gets the headlines.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="overiddenPreview">The overidden preview.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        [GenerateScriptType(typeof(HeadlinesPluginDelegate))]
        public HeadlinesPluginDelegate GetRssFeed(string uri, string accessPointCode, string interfaceLanguage, string overiddenPreview)
        {

            HeadlinesPluginDelegate headlinesPluginDelegate = new HeadlinesPluginDelegate();
            try
            {
                headlinesPluginDelegate = new HeadlinesPluginDelegate(uri);
                // Initialize SessionData.
                new SessionData(accessPointCode, interfaceLanguage, 0, true, overiddenPreview, string.Empty);
                headlinesPluginDelegate.Fill();
            }
            catch (FactivaBusinessLogicException fbe)
            {

                UpdateAjaxDelegate(fbe, headlinesPluginDelegate);
            }
            catch (Exception exception)
            {
                UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), headlinesPluginDelegate);
            }

            return headlinesPluginDelegate;
          
        }

        private static PerformContentSearchRequest GetPerformContentSearchRequestForSummary(string searchText)
        {
            PerformContentSearchRequest request = new PerformContentSearchRequest();
            request.StructuredSearch.Query.SearchCollectionCollection.Add(SearchCollection.Summary);
            SearchString searchstring = new SearchString();
            searchstring.Value = searchText;
            //objSearchString.Id = "FreeText0";
            searchstring.Type = SearchType.Free;
            searchstring.Mode = SearchMode.Simple;
            searchstring.Combine = true;
            searchstring.Filter = false;
            searchstring.Scope = "";
            searchstring.Validate = true;
            request.StructuredSearch.Query.SearchStringCollection.Add(searchstring);
            request.DescriptorControl.Mode = DescriptorControlMode.All;
            request.FirstResult = 0;
            request.MaxResults = 5;
            request.StructuredSearch.Formatting.SnippetType = SnippetType.Contextual;
            request.StructuredSearch.Formatting.MarkupType = MarkupType.All;
            request.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Off;
            request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.RelevanceHighFreshness;
            request.StructuredSearch.Formatting.ClusterMode = ClusterMode.On;
            request.StructuredSearch.Formatting.FreshnessDate = DateTime.Now;


            return request;
        }

        private static PerformContentSearchRequest GetPerformContentSearchRequest(string searchText)
        {
            PerformContentSearchRequest request = new PerformContentSearchRequest();

            SearchString searchstring = new SearchString();
            searchstring.Value = searchText;
            //objSearchString.Id = "FreeText0";
            searchstring.Type = SearchType.Free;
            searchstring.Mode = SearchMode.Simple;
            searchstring.Combine = true;
            searchstring.Filter = false;
            searchstring.Scope = "";
            searchstring.Validate = true;
            request.StructuredSearch.Query.SearchStringCollection.Add(searchstring);
            request.DescriptorControl.Mode = DescriptorControlMode.All;
            request.FirstResult = 0;
            request.MaxResults = 20;
            request.StructuredSearch.Formatting.SnippetType = SnippetType.Contextual;
            request.StructuredSearch.Formatting.MarkupType = MarkupType.All;
            request.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode.NearExact;
            request.StructuredSearch.Formatting.ClusterMode = ClusterMode.On;
            request.StructuredSearch.Formatting.FreshnessDate = DateTime.Now;
            request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.RelevanceHighFreshness;


            return request;
        }

        private static void UpdateAjaxDelegate(FactivaBusinessLogicException fbe, IAjaxDelegate ajaxDelegate)
        {
            ajaxDelegate.ReturnCode = fbe.ReturnCodeFromFactivaService;
            ajaxDelegate.StatusMessage = ResourceText.GetInstance.GetErrorMessage(ajaxDelegate.ReturnCode.ToString());
        }

    }
}
