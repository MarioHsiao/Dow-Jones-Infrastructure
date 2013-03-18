using System;
using System.Collections.Generic;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using System.Linq;
using DowJones.Exceptions;
using Factiva.Gateway.Utils.V1_0;
using System.Threading;
using System.Text;

namespace DowJones.Managers.QueryUtility
{
    public class QueryManager
    {
        long counter;
        //private QueryResponse response = null;
        FCPServiceWrapper fsw;

        public QueryManagerResponse GetQueryManagerResponse(QueryManagerRequest request, ControlData controlData, bool dbg, bool cache)
        {
            QueryManagerResponse response = new QueryManagerResponse();

            List<Thread> threads = new List<Thread>();
            BuildStructuredSearchRequest buildStructuredSearchRequest;

            foreach (var queryRequest in request.QueryRequests)
            {
                response.QueryResponses.Add(new QueryResponse { QueryId = queryRequest.QueryId, CustomId = queryRequest.CustomId });
                buildStructuredSearchRequest = new BuildStructuredSearchRequest(queryRequest, response.QueryResponses.LastOrDefault(), controlData);
                var t = new Thread(new ParameterizedThreadStart(BuildStructuredSearchQuery));
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                threads.Add(t);
                t.Start(buildStructuredSearchRequest);
            }            
            foreach (var thread in threads)
                thread.Join();

            if (request.CombineState.Enabled)
            {
                CombineQueryResponses(response, request.CombineState);
            }

            return response;
        }

        public void CombineQueryResponses(QueryManagerResponse response,CombineState combineState)
        {
            QueryResponse result = new QueryResponse();
            StringBuilder sb_result =  new StringBuilder();

            result.StructuredSearch.Query.SearchStringCollection = new Factiva.Gateway.Messages.Search.V2_0.SearchStringCollection();
            foreach (QueryResponse qresponse in response.QueryResponses)
            {
                sb_result
                    .Append("(")
                    .Append(qresponse.StructuredSearch.Query.SearchStringCollection.FirstOrDefault().Value)
                    .Append(") ")
                    .Append(combineState.CombineOperator.ToString() + " ");
            }

            ContentStructuredQueryBuilder contentBuilder = new ContentStructuredQueryBuilder();
            contentBuilder.RemoveLastOpFromTraditional(sb_result);

            result.StructuredSearch.Query.SearchStringCollection.Add(new Factiva.Gateway.Messages.Search.V2_0.SearchString { Type = Factiva.Gateway.Messages.Search.V2_0.SearchType.Free, Mode = Factiva.Gateway.Messages.Search.V2_0.SearchMode.Traditional, Value = sb_result.ToString() });

            contentBuilder.StructuredSearch = result.StructuredSearch;
            List<QueryFilter> dateFilters = new List<QueryFilter>();
            if (combineState.DateFilter != null)
            {
                dateFilters.Add(combineState.DateFilter);
            }
            else if (combineState.DaysFilter != null)
            {
                dateFilters.Add(combineState.DaysFilter);
            }
            if (dateFilters.Count > 0)
            {
                new StructuredQueryBuilder().ProcessQueryFilter(dateFilters, contentBuilder, null,null);
            }
            response.QueryResponses.Clear();
            response.QueryResponses.Add(result);
        }
        private void BuildStructuredSearchQuery(object buildStructuredSearchRequest)
        {
           
            // Try to retrieve data from platform cache
            /*var platformCache = new PlatformCache(request, controlData);
            response = platformCache.GetItem();
            if (response != null)
                return response;
            */


            //var queryForSearch = ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query;
            //ruleManager = new RuleManager(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Rules);
            fsw = new FCPServiceWrapper(((BuildStructuredSearchRequest)buildStructuredSearchRequest).controlData);
            try
            {
                if (((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.QueryId > 0)
                {
                    ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query = fsw.GetQueryById(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.QueryId);
                }

                if (((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query != null)
                {
                    // must first update query contains values, 
                    // since they are not set by the backend when query is retrieved
                    UpdateQueryContains(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query);
                }
                else {
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_INVALID_QUERY);
                }

                // if query type is not SavedQuery, then throw an exception
                var queryType = GetQueryType(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query);

                //if (queryType != QueryType.)
                //{
                //    throw new EmgUtilitiesException(string.Format("Unsupported query type {0}.", queryType));
                //}

                FillQueryReferences(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query, new RuleManager(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Rules), ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse);
                //if (fillQueryReferencesResult.Status == StructuredQueryBuilderStatus.Truncated) response.StructuredQueryStatus = StructuredQueryStatus.Truncated;
                FillQueryReferences(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.AdditionalFilters, new RuleManager(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Rules), ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse);

                var status = (new StructuredQueryBuilder())
                   .UpdateStructuredSearchForContentSearch(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse.StructuredSearch,
                       ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Query, new RuleManager(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.Rules), ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryRequest.AdditionalFilters, ((BuildStructuredSearchRequest)buildStructuredSearchRequest).controlData);

                if (SearchStringMaxLengthExceeded(((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse))
                    ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse.Errors.Add(new Error { Code = DowJonesUtilitiesException.QM_SEARCH_STRING_LENGTH_EXCEEDED });
                
            }
            catch (DowJonesUtilitiesException ex)
            {
                ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse.Errors.Add(new Error { Code = ex.ReturnCode });
            }
            catch (Exception ex)
            {
                ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse.Errors.Add(new Error { Code = -1, Message = ex.Message });
            }
            if (((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse.Errors.Count > 0)
                ((BuildStructuredSearchRequest)buildStructuredSearchRequest).queryResponse.ReturnCode = 1;
            
            // Cache the response
            //platformCache.StoreItem(response);
            
            //return response;

        }

        public bool SearchStringMaxLengthExceeded(QueryResponse response)
        {
            var searchStringLen = response.StructuredSearch.Query.SearchStringCollection.Sum(searchString => searchString.Value.Length);

            var maxLengthExceeded = searchStringLen > DowJones.Properties.Settings.Default.MaximumAllowedStructuredSearchLength ? true : false;
            return maxLengthExceeded;
        }

        public void UpdateQueryContains(Query query)
        {
            foreach (var group in query.Groups.Where(g => g.FilterGroup != null))
            {
                foreach (var filter in group.FilterGroup.Filters)
                {
                    if (filter is TriggerTypeFilter)
                    {
                        query.QueryContains.__triggerTypes = true;
                    }
                    else if (filter is FreeTextFilter)
                    {
                        query.QueryContains.__freeText = true;
                    }
                }
            }
        }
        public void FillQueryReferences(Query query,RuleManager ruleManager,QueryResponse response)
        {
            
            if (query.QueryContains.References) return;
            foreach (var group in query.Groups)
            {
                if (group.FilterGroup == null)
                    continue;
                // foreach query filter
                FillQueryReferences(group.FilterGroup.Filters,ruleManager,response);
            }

            //while (Interlocked.Read(ref Counter) < countOfOperations) { }
            
            query.QueryContains.__references = true;

        }

        public void FillQueryReferences(List<QueryFilter> filters,RuleManager ruleManager,QueryResponse response )
        {
            var threads = new List<Thread>();
            FillQueryReferencesRequest fillReferenceRequest = null;


            foreach (var filter in filters)
            {
                fillReferenceRequest = new FillQueryReferencesRequest((QueryFilter)filter, ruleManager,response);
                // queue worker and increment the total count of operations
                var t = new Thread(FillQueryReferencesStep);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                threads.Add(t);
                t.Start(fillReferenceRequest);
            }

            foreach (var thread in threads)
                thread.Join();

        }
        public void FillQueryReferencesStep(object fillReferenceRequest)
        {            
            try
            {
                // based on the type of filter, call the FCP service 
                // to get news coded query information for dereferencing.
                var filter = ((FillQueryReferencesRequest)fillReferenceRequest).filter;
                /*if (filter is SourcesFilter) //Mocked
                {
                    try
                    {
                        var f = (SourcesFilter)filter;
                        foreach (var sg in f.SourceGroups)
                        {
                            sg.SourceNewsQueries = fsw.GetSourceNewsQueriesFromSourceGroup(sg);
                        }  
                    }
                    catch (EmgUtilitiesException ex)
                    {
                        response.Errors.Add(new Error { FilterType = typeof(DJOrganizationCodeFilter).ToString(), Code = ex.ReturnCode });
                    }
                }
                else
                if (filter is AuthorListFilter) //Mocked
                {
                    try {
                        var f = (AuthorListFilter)filter;
                        foreach (var au in f.AuthorLists)
                        {
                            f.PersonCodes = fsw.GetAuthorCodesFromAuthorId(au);
                        }                                                    
                    }
                    catch (EmgUtilitiesException ex)
                    {
                        response.Errors.Add(new Error { FilterType = typeof(AuthorListFilter).ToString(), Code = ex.ReturnCode });                            
                    }
                }
                else*/
                if (filter is DJOrganizationCodeFilter)
                {
                    try
                    {
                        var f = (DJOrganizationCodeFilter)filter;
                        if (f.OrganizationCodes.Count > 0)
                            f.CompanyNewsQueries = fsw.GetCompanyNewsQueryFromCodes(f.OrganizationCodes);
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { FilterType = typeof(DJOrganizationCodeFilter).ToString(), Code = ex.ReturnCode });
                    }
                }
                else if (filter is CompanyListFilter)
                {
                    try
                    {
                        var f = (CompanyListFilter)filter;
                        foreach (var cl in f.CompanyLists)
                        {
                            cl.CompanyNewsQueries = fsw.GetCompanyNewsQueryFromCompanyListId(cl.ListID.ToString(), ((FillQueryReferencesRequest)fillReferenceRequest).ruleManager);
                        }
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { FilterType = typeof(CompanyListFilter).ToString(), Code = ex.ReturnCode });
                    }
                }
                else if (filter is CompanyScreeningFilter)
                {
                    try
                    {
                        var f = (CompanyScreeningFilter)filter;
                        foreach (var sc in f.ScreeningCriterias)
                        {
                            sc.CompanyNewsQueries = fsw.GetCompanyNewsQueryFromScreeningCriteriaId(sc.ScreeningCriteriaID, ((FillQueryReferencesRequest)fillReferenceRequest).ruleManager);
                        }
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { FilterType = typeof(CompanyScreeningFilter).ToString(), Code = ex.ReturnCode });
                    }
                }
                else if (filter is DJPersonCodeFilter)
                {
                    try
                    {
                        var f = (DJPersonCodeFilter)filter;
                        if (f.PersonCodes.Count > 0)
                            f.PersonNewsQueries = fsw.GetPersonNewsQueryFromCodes(f.PersonCodes);
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { FilterType = typeof(DJPersonCodeFilter).ToString(), Code = ex.ReturnCode });
                    }

                }
                else if (filter is ExecutiveListFilter)
                {
                    try
                    {
                        var f = (ExecutiveListFilter)filter;

                        foreach (var el in f.ExecuteLists)
                        {
                            el.PersonNewsQueries = fsw.GetPersonNewsQueryFromListId(el.ListID);
                        }
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { FilterType = typeof(ExecutiveListFilter).ToString(), Code = ex.ReturnCode });
                    }

                }
                else if (filter is ExecutiveScreeningFilter)
                {
                    try
                    {
                        var f = (ExecutiveScreeningFilter)filter;

                        foreach (var sc in f.ScreeningCriterias)
                        {
                            sc.PersonNewsQueries = fsw.GetPersonNewsQueryFromScreeningCriteriaId(sc.ScreeningCriteriaID, ((FillQueryReferencesRequest)fillReferenceRequest).ruleManager);
                            //if (fsw.LastOperationTruncated) ctx.CommonContext.Status = StructuredQueryBuilderStatus.Truncated;
                        }
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { FilterType = typeof(ExecutiveScreeningFilter).ToString(), Code = ex.ReturnCode });
                    }

                }
            }
            catch (Exception ex)
            {
                //ctx.CommonContext.LastException = ex;
                ((FillQueryReferencesRequest)fillReferenceRequest).queryResponse.Errors.Add(new Error { Message = ex.Message });
            }
            finally
            {
                Interlocked.Increment(ref counter);
            }
        }

        private static QueryType GetQueryType(Query query)
        {
            if (query is CommunicatorTopicQuery)
            {
                return QueryType.CommunicatorTopicQuery;
            }
            //else if (query is SnapshotQuery)
            //{
            //    return QueryType.SnapshotQuery;
            //}
            if (query is SourceListQuery)
            {
                return QueryType.SourceListQuery;
            }
            if (query is MediaMonitorQuery)
            {
                return QueryType.MediaMonitorQuery;
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_QUERY_TYPE_NOT_SUPPORTED);
        }
    }

    public class BuildStructuredSearchRequest
    {
        public BuildStructuredSearchRequest(QueryRequest request, QueryResponse response, ControlData controlData)
        {
            this.queryRequest = request;
            this.queryResponse = response;
            this.controlData = controlData;
        }

        public QueryRequest queryRequest { get; set; }
        public QueryResponse queryResponse { get; set; }
        public ControlData controlData { get; set; }

    }

    public class FillQueryReferencesRequest
    {
        public FillQueryReferencesRequest(QueryFilter filter, RuleManager ruleManager, QueryResponse response)
        {
            this.filter = filter;
            this.ruleManager = ruleManager;
            this.queryResponse = response;
        }

        public QueryFilter filter { get; set; }
        public RuleManager ruleManager { get; set; }
        public QueryResponse queryResponse { get; set; }
    }
}
