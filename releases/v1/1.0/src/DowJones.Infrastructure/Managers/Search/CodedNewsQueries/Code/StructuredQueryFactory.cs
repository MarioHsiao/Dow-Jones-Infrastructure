// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuredQueryFactory.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using Factiva.Gateway.Messages.CodedNews.CodedNewsQueries;
using Factiva.Gateway.Messages.CodedNews.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace Factiva.Gateway.Messages.CodedNews
{
    public class StructuredQueryFactory
    {
        private static readonly ILog Logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static IStructuredQuery Create(CodedStructuredSearchRequest request, ControlData controlData)
        {
            Logger.Debug("IStructuredQuery::Create->  request for query type " + request.NewsType);
            IStructuredQuery query;
            var occurrenceSearch = request.OccurrenceSearch;

            switch (request.NewsType)
            {
                    ////*********************
                    //// COMPANY NEWS
                    ////********************

                case CodedNewsType.ManagementMoves:
                case CodedNewsType.ContractsOrders:
                case CodedNewsType.NewProductsServices:
                case CodedNewsType.LegalJudicial:
                case CodedNewsType.Performance:
                case CodedNewsType.OwnershipChanges:
                case CodedNewsType.Earnings:
                case CodedNewsType.CapacitiesFacilities:
                case CodedNewsType.Bankruptcy:
                case CodedNewsType.LatestNews:
                    var companyStructuredQuery = new CompanyStructuredQuery
                                                     {
                                                         CompanyFilter = request.CompanyFilters[0],
                                                         IncludeNiweFilter = request.IncludeNiweFilter,
                                                     };
                    if (!occurrenceSearch)
                    {
                        occurrenceSearch = companyStructuredQuery.CompanyFilter.Status != null && companyStructuredQuery.CompanyFilter.Status.IsNewsCoded;
                    }

                    query = companyStructuredQuery;
                    break;
                case CodedNewsType.PressReleases:
                    var companyPressReleasesStructuredQuery = new CompanyPressReleasesAndTradeStructuredQuery("tqpr")
                                               {
                                                  CompanyFilter = request.CompanyFilters[0]
                                               };
                    query = companyPressReleasesStructuredQuery;
                    break;
                case CodedNewsType.TradeArticles:
                    var structuredQuery = new CompanyPressReleasesAndTradeStructuredQuery("ttrd")
                                              {
                                                  CompanyFilter = request.CompanyFilters[0]
                                              };
                    query = structuredQuery;
                    break;
                case CodedNewsType.KeyDevBankruptcy:
                case CodedNewsType.KeyDevManagementChanges:
                case CodedNewsType.KeyDevMAOC:
                case CodedNewsType.KeyDevMarketChanges:
                case CodedNewsType.KeyDevNewFundingCapital:
                case CodedNewsType.KeyDevNewProductsServices:
                case CodedNewsType.KeyDevPerformance:
                case CodedNewsType.KeyDevRegGvtPolicy:
                case CodedNewsType.KeyDevAll:
                    var keyDevelopmentStructuredQuery = new CompanyKeyDevelopmentStructuredQuery
                                                            {
                                                                CompanyFilter = request.CompanyFilters[0]
                                                            };
                    query = keyDevelopmentStructuredQuery;
                    break;
                case CodedNewsType.ReportAll:
                case CodedNewsType.DataMonBusinessDescription:
                case CodedNewsType.DataMonCompanyLocations:
                case CodedNewsType.DataMonCompanyOverview:
                case CodedNewsType.DataMonHistory:
                case CodedNewsType.DataMonKeyEmployees:
                case CodedNewsType.DataMonKeyFacts:
                case CodedNewsType.DataMonMajorProducts:
                case CodedNewsType.DataMonSWOTAnalysis:
                case CodedNewsType.DataMonTopCompetitors:
                    var companyAnalysisStructuredQuery = new CompanyAnalysisStructuredQuery
                                                {
                                                    CompanyFilter = request.CompanyFilters[0]
                                                };
                    query = companyAnalysisStructuredQuery;
                    break;
                case CodedNewsType.EarningsCalls:
                    var earningCalls = new CompanyEarningCalls
                                           {
                                               CompanyFilter = request.CompanyFilters[0]
                                           };
                    query = earningCalls;
                    break;

                    ////*********************
                    //// INDUSTRY NEWS
                    ////********************
                case CodedNewsType.IndustryNews:
                    var industryLatestNewsStructuredQuery = new IndustryLatestNewsStructuredQuery
                                           {
                                               Industry = request.Industries[0]
                                           };
                    query = industryLatestNewsStructuredQuery;
                    break;
                case CodedNewsType.IndustryEditorsChoice:
                    var industryEditorChoiceStructuredQuery = new IndustryEditorChoiceStructuredQuery
                                      {
                                          Industry = request.Industries[0]
                                      };
                    query = industryEditorChoiceStructuredQuery;
                    break;
                case CodedNewsType.AsiaPulse:
                    var industryAsiaPulseStructuredQuery = new IndustryAsiaPulseStructuredQuery
                                      {
                                          Industry = request.Industries[0]
                                      };
                    query = industryAsiaPulseStructuredQuery;
                    break;
                case CodedNewsType.IndustryChinaReport:
                    var icrQuery = new IndustryChinaReportStructuredQuery
                                       {
                                           Industry = request.Industries[0]
                                       };
                    query = icrQuery;
                    break;
                case CodedNewsType.ForresterResearch:
                case CodedNewsType.FreedoniaSummary:
                case CodedNewsType.IBIS:
                case CodedNewsType.MarketResearch:
                case CodedNewsType.MergentReport:
                case CodedNewsType.SpSummary:
                case CodedNewsType.BusinessMonitor:
                case CodedNewsType.ChinaCoal:
                    var profiles = new IndustryAnalysisAndProfilesStructuredQuery
                                       {
                                           Industry = request.Industries[0]
                                       };
                    query = profiles;
                    break;

                    ////*********************
                    //// EXECUTIVE NEWS
                    ////********************
                case CodedNewsType.ExecutiveLatestNews:
                case CodedNewsType.ExecutiveBusinessNews:
                    var executiveStructuredQuery = new ExecutiveStructuredQuery
                                                       {
                                                           Executives = request.ExecutiveFilters[0]
                                                       };
                    query = executiveStructuredQuery;
                    break;
                    ////*********************
                    // GOVERNMENT NEWS
                    ////********************
                case CodedNewsType.GovernmentExecutiveNews:
                    var governmentExecutiveNewsQuery = new GovernmentExecutiveNewsQuery
                                                           {
                                                               Official = request.Official[0]
                                                           };
                    query = governmentExecutiveNewsQuery;
                    break;
                case CodedNewsType.GovernmentGeneralNews:
                    var governmentGeneralNewsQuery = new GovernmentGeneralNewsQuery
                                                         {
                                                             Organization = request.Organization[0]
                                                         };
                    query = governmentGeneralNewsQuery;
                    break;
                case CodedNewsType.GovernmentOpportunitiesContracts:
                    var contractsQuery = new GovernmentOpportunitiesContractsQuery
                                             {
                                                 Organization = request.Organization[0]
                                             };
                    query = contractsQuery;
                    break;
                default:
                    throw new NotSupportedException(request.NewsType.ToString());
            }
            
            query.Filters = request.NewsFilters;
            query.OccurrenceSearch = occurrenceSearch;
            query.QueryType = request.NewsType;

            if (request.PreferenceLanguage != null && request.PreferenceLanguage.Count > 0)
            {
                query.LanguagePreference = request.PreferenceLanguage.ToArray();
            }

            var arrCat = new SearchCollectionCollection();

            if (request.ContentCategory != null && request.ContentCategory.Length > 0)
            {
                foreach (var cat in request.ContentCategory)
                {
                    switch (cat)
                    {
                        case SearchContentCategory.Pictures:
                            arrCat.Add(SearchCollection.Pictures);
                            break;
                        case SearchContentCategory.Blogs:
                        case SearchContentCategory.NewsSites:
                        case SearchContentCategory.WebSites:
                            arrCat.Add(SearchCollection.WebSites);
                            break;
                        case SearchContentCategory.Multimedia:
                            arrCat.Add(SearchCollection.Multimedia);
                            break;
                        case SearchContentCategory.Video:
                            arrCat.Add(SearchCollection.Video);
                            break;
                        case SearchContentCategory.Audio:
                            arrCat.Add(SearchCollection.Audio);
                            break;
                        default:
                            arrCat.Add(SearchCollection.Publications);
                            break;
                    }
                }
            }

            query.ControlData = controlData;

            if (arrCat.Count > 0)
            {
                // Build the query object and set the collection.
                query.Query.SearchCollectionCollection = arrCat;
            }
            else
            {
                // Build the query object and set the collection.
                query.Query.SearchCollectionCollection = new SearchCollectionCollection
                                                             {
                                                                 SearchCollection.Publications
                                                             };
            }

            return query;
        }
    }
}