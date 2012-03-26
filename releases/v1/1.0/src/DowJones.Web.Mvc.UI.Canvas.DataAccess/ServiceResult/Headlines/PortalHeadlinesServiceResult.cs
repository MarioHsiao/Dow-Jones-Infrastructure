using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.Headlines;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Headlines;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Newsstand.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using Factiva.Gateway.V1_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using DowJones.Utilities.Managers.Search;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines
{
    [DataContract(Name = "portalHeadlinesServiceResult", Namespace = "")]
    public class PortalHeadlinesServiceResult : AbstractServiceResult, IPopulate<HeadlinesRequest>
    {
        [DataMember(Name = "package")]
        public PortalHeadlinesPackage Package { get; set; }

        public void Populate(ControlData controlData, HeadlinesRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (request == null || !request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                   }

                   GetData(request, controlData, preferences);
               },
               preferences);
        }

        protected internal void GetData(HeadlinesRequest request, ControlData controlData, IPreferences preferences)
        {
            var searchContextWrapper = JsonConvert.DeserializeObject<SearchContextWrapper>(request.SearchContextRef);
            IPerformContentSearchResponse performContentSearchResponse = null;

            //Get the module
            var module = GetModule(searchContextWrapper.PageId, searchContextWrapper.ModuleId, controlData, preferences);

            #region <<< IPerformContentSearch >>>
            if (searchContextWrapper.SearchContextType == typeof(CustomTopicsViewAllSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(RadarCellSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(RegionalMapBubbleSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(SourcesViewAllSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(TopNewsViewAllSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(TrendingItemSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(SummaryTrendingSearchContext).Name ||
                searchContextWrapper.SearchContextType == typeof(SummaryRegionalMapBubbleSearchContext).Name)
            {
                var searchRequest = SearchContextUtility.CreateSearchRequest<FreePerformContentSearchRequest>(searchContextWrapper,
                                                                                                              request.FirstResultToReturn,
                                                                                                              request.MaxResultsToReturn,
                                                                                                              preferences,
                                                                                                              module);

                Audit.AdditionalInfo = SerializeObjectToStream(searchRequest);

                RecordTransaction(
                    typeof(FreePerformContentSearchResponse).FullName,
                    MethodBase.GetCurrentMethod().Name,
                    manager =>
                    {
                        performContentSearchResponse = manager.Invoke<FreePerformContentSearchResponse>(searchRequest, controlData).ObjectResponse;
                    },
                    new SearchManager(controlData, preferences.InterfaceLanguage));
            }
            #endregion

            #region <<< GetMultipleNewsstandSectionHeadlinesRequest >>>
            else if (searchContextWrapper.SearchContextType == typeof(NewsstandSectionSearchContext).Name ||
                     searchContextWrapper.SearchContextType == typeof(NewsstandDiscoveredEntitiesSearchContext).Name)
            {
                var searchRequest = SearchContextUtility.CreateNewsstandHeadlinesRequest(searchContextWrapper, request.FirstResultToReturn, request.MaxResultsToReturn, module, controlData, preferences);
                GetMultipleNewsstandSectionHeadlinesResponse response = null;
                RecordTransaction(
                    typeof(GetMultipleNewsstandSectionHeadlinesResponse).FullName,
                    MethodBase.GetCurrentMethod().Name,
                    () =>
                    {
                        response = FactivaServices.Invoke<GetMultipleNewsstandSectionHeadlinesResponse>(ControlDataManager.Clone(controlData, true), searchRequest).ObjectResponse;
                    });
                Audit.AdditionalInfo = SerializeObjectToStream(searchRequest);
                performContentSearchResponse = response.NewsstandSectionHeadlinesResultSet.NewsstandSectionHeadlinesResultCollection[0].SearchResponse;
            }
            #endregion

            #region <<< CodedStructuredSearchRequest >>>
            else if (searchContextWrapper.SearchContextType == typeof(CompanyOverviewTrendingBubbleSearchContext).Name ||
                     searchContextWrapper.SearchContextType == typeof(CompanyOverviewRecentArticlesViewAllSearchContext).Name)
            {
                //Structured search
                if (module == null)
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
                var companyNewsPageModule = module as CompanyOverviewNewspageModule;
                if (companyNewsPageModule == null)
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleData);

                var company = GetCompany(companyNewsPageModule.FCodeCollection[0], controlData, preferences);

                var codedSearchRequest = SearchContextUtility.CreateCodedStructuredSearchRequest(searchContextWrapper, request.FirstResultToReturn, request.MaxResultsToReturn, company, controlData, preferences);

                Audit.AdditionalInfo = SerializeObjectToStream(codedSearchRequest);
                
                // Record the Transaction
                RecordTransaction(
                    typeof(FreePerformContentSearchResponse).FullName,
                    MethodBase.GetCurrentMethod().Name,
                    manager =>
                    {
                        performContentSearchResponse = manager.PerformCodedStructuredSearch<FreePerformContentSearchRequest, FreePerformContentSearchResponse>(codedSearchRequest);
                    },
                    new SearchManager(controlData, preferences.InterfaceLanguage));
            }
            #endregion

            else
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
            }
            
            var conversionManager = new HeadlineListConversionManager(new DateTimeFormatter(preferences));
            Package = new PortalHeadlinesPackage
            {
                Result = conversionManager.Process(performContentSearchResponse).Convert(request.TruncationType),
                ViewAllSearchContextRef = request.SearchContextRef
            };
        }

        private Company GetCompany(string fcode, ControlData controlData, IPreferences preferences)
        {
            var elements = new List<CompanyElements>
                                  {
                                      CompanyElements.AllCodes,
                                      CompanyElements.Status,
                                      CompanyElements.FactivaTaxonomyCodes
                                  };

            var getCompaniesRequest = new GetCompaniesRequest();
            getCompaniesRequest.CodeCollection.Add(fcode);
            getCompaniesRequest.CodeType = CompanyCodeType.FactivaCompany;
            getCompaniesRequest.ElementsToReturnCollection.AddRange(elements);
            getCompaniesRequest.Language = preferences.InterfaceLanguage;

            GetCompaniesResponse getCompaniesResponse = null;
            RecordTransaction(
                typeof(GetCompaniesRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                () =>
                {
                    var dataRetrievalManager = new ModuleDataRetrievalManager(controlData, preferences);
                    var tempResponse = dataRetrievalManager.Invoke<GetCompaniesResponse>(getCompaniesRequest);
                    getCompaniesResponse = tempResponse.ObjectResponse;
                });

            if (getCompaniesResponse.CompanyResultSet.Count > 0)
            {
                if (getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status == null ||
                    getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value == 0)
                {
                    return getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].ResultCompany;
                }

                if (getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value != 0)
                {
                    throw new DowJonesUtilitiesException(getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value);
                }
            }
            throw new DowJonesUtilitiesException();
        }

        public static string SerializeObjectToStream(object obj)
        {
            using (var stream = new MemoryStream())
            {
                // Data Contract Serialization
                new XmlSerializer(obj.GetType()).Serialize(stream, obj);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
