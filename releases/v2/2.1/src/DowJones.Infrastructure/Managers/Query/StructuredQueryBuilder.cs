using System;
using System.Collections;
using System.Collections.Generic;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;
using System.Linq;
using System.Text;
using log4net;
using Factiva.Gateway.Utils.V1_0;
using DowJones.Exceptions;
using DowJones.Search;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Managers.QueryUtility
{
    public enum StructuredQueryBuilderStatus
    {
        Success,
        Failed,
        Truncated,
    }

    public class StructuredQueryBuilder
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(StructuredQueryBuilder));

        private const string ScopeNS = "ns=";
        private const string ScopeIN = "in=";
        private const string ScopeLA = "la=";
        private const string ScopeRE = "re=";
        private const string ScopeAU = "au=";
        private const string ScopePE = "pe=";

        private const string NewsSubjectID = "NewsSubject";
        private const string IndustryID = "Industry";
        private const string FreeTextID = "FreeText";
        private const string RegionID = "Region";
        private const string LanguageID = "Language";
        private const string AuthorID = "Author";
        private const string SourcesID = "Sources";
        private const string OrganizationID = "Organization";
        private const string PersonID = "Person";
        private const string CompanyListID ="CompanyList";
        private const string CompanyScreeningID = "CompanyScreening";
        private const string ExecutiveScreeningID = "ExecutiveScreening";
        private const string SearchAdditionalID = "SearchAdditional";
        private const string SearchExclusionID = "SearchExclusion";
        private const string SourceEntityID = "SourceEntity";
        private const string KeywordsAllID = "KeywordsAll";
        private const string KeywordsAnyID = "KeywordsAny";
        private const string KeywordsNoneID = "KeywordsNone";
        private const string KeywordsPhraseID = "KeywordsPhrase";

        private static IEnumerable<string> GetExcludedSubject<T>(T value)
        {
            Type enumType = typeof(T);
            string fieldName = value.ToString();
            var attribute = (ExcludeSubject)Attribute.GetCustomAttribute(enumType.GetField(fieldName), typeof(ExcludeSubject));
            return attribute != null ? attribute.Subjects : null;
        }

        public StructuredQueryBuilderStatus UpdateStructuredSearchForContentSearch(StructuredSearch structuredSearch, Query query,RuleManager ruleManager,List<QueryFilter> additionalFilterCollection,ControlData controlData)
        {    
        
           if (structuredSearch == null)
               throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_STRUCTUREDSEARCH_IS_NULL);

           if (query == null)
               throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_INVALID_QUERY);

//            if (Logger.IsDebugEnabled) LogMethodStart();

            StructuredQueryBuilderStatus rv = StructuredQueryBuilderStatus.Failed;
            StructuredQueryBuilderBase builder = null;

            StringBuilder sbTemp = null;

            if (Log.IsDebugEnabled)
            {
                sbTemp = new StringBuilder(2048);
                sbTemp
                    .AppendLine()
                    .AppendLine("********************** Original structured search XML   **********************")
                    .AppendLine(GeneralUtils.serialize(structuredSearch, true, true));
            }

            try
            {
                //if (!query.QueryContains.References) throw new ApplicationException("Query must have references already filled in.");

                builder = GetStructuredQueryBuilderForContentSearch(structuredSearch, query.EditIn);

                // build the structured query
                rv = BuildStructuredQuery(builder, query,ruleManager,additionalFilterCollection,controlData);

                return rv;
            }
               
            finally
            {
                if (Log.IsDebugEnabled)
                {
                    string structuredSearchXml = (structuredSearch != null) ? GeneralUtils.serialize(structuredSearch, true, true) : "structuredSearch is null.";
                    string structuredSearchLen = (structuredSearch != null) ? GeneralUtils.serialize(structuredSearch, true, false).Length.ToString() : "unknown";

                    sbTemp
                        .AppendLine()
                        .AppendLine("********************** Original Query **********************")
                        .AppendLine(GeneralUtils.serialize(query, true, true))
                        .AppendLine("********************** StructuredSearch **********************")
                        .Append("Status: ")
                        .AppendLine(rv.ToString())
                        .Append("Estimated query length: ")
                        .AppendLine((builder != null) ? builder.CurrentLength.ToString() : "builder was not created.")
                        .Append("Actual query length: ")
                        .AppendLine(structuredSearchLen)
                        //.Append((builder != null) ? builder.GetDetailedQueryStatistics() : "")
                        .AppendLine(structuredSearchXml);

                    Log.InfoFormat("Response = ", sbTemp.ToString());
                    //LogMethodInfo(sbTemp.ToString());
                }

                //if (Logger.IsDebugEnabled) LogMethodEnd();
            }
        }
        
        private StructuredQueryBuilderBase GetStructuredQueryBuilderForContentSearch(StructuredSearch structuredSearch, EditIn editIn)
        {
            return new ContentStructuredQueryBuilder() { StructuredSearch = structuredSearch };
        }

        #region Building structured search

        private static Hashtable _queryFilterSortMap;
        private static Hashtable _operatorSortMap;
        private static Hashtable GetOperatorSortMap()
        {
            if (_operatorSortMap == null)
            {
                _operatorSortMap = new Hashtable();
                _operatorSortMap.Add(Operator.And, 1);
                _operatorSortMap.Add(Operator.Or, 2);
                _operatorSortMap.Add(Operator.Not, 3);
            }
            return _operatorSortMap;
        }

        private static Hashtable GetQueryFilterSortMap()
        {
            if (_queryFilterSortMap == null)
            {
                _queryFilterSortMap = new Hashtable();
                _queryFilterSortMap.Add(typeof(DaysFilter), 1);
                _queryFilterSortMap.Add(typeof(DateFilter), 2);
                _queryFilterSortMap.Add(typeof(SearchSectionFilter), 3);
                _queryFilterSortMap.Add(typeof(FreeTextFilter), 4);
                _queryFilterSortMap.Add(typeof(DJOrganizationCodeFilter), 5);
                _queryFilterSortMap.Add(typeof(SourcesFilter), 6);
                _queryFilterSortMap.Add(typeof(AuthorCodeFilter), 7);
                _queryFilterSortMap.Add(typeof(IndustryCodeFilter), 8);
                _queryFilterSortMap.Add(typeof(NewsSubjectCodeFilter), 9);
                _queryFilterSortMap.Add(typeof(RegionCodeFilter), 10);
                _queryFilterSortMap.Add(typeof(ContentLanguageFilter), 11);
                _queryFilterSortMap.Add(typeof(CompanyListFilter), 12);
                _queryFilterSortMap.Add(typeof(CompanyScreeningFilter), 13);
                _queryFilterSortMap.Add(typeof(DJPersonCodeFilter), 14);
                _queryFilterSortMap.Add(typeof(ExecutiveListFilter), 15);
                _queryFilterSortMap.Add(typeof(ExecutiveScreeningFilter), 16);
                _queryFilterSortMap.Add(typeof(SearchExclusionFilter), 18);
                _queryFilterSortMap.Add(typeof(SourceEntityFilter), 19);
                _queryFilterSortMap.Add(typeof(SourceEntityListIDFilter), 20);
                _queryFilterSortMap.Add(typeof(SearchAdditionalFilters), 21);                
            }
            return _queryFilterSortMap;
        }

        private static List<QueryFilter> GetSortedQueryFilters(List<QueryFilter> filters)
        {
            Hashtable sortMap = GetQueryFilterSortMap();
            Hashtable sortOpMap = GetOperatorSortMap();
            return (from f in filters
                    orderby sortMap[f.GetType()],sortOpMap[f.Operator]
                    select f).ToList();
        }
        private bool GroupHasNoFilters(Group group)
        {
            if (group.FilterGroup.Filters.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void RemoveEmptyGroups(Query query)
        {
            query.Groups.RemoveAll(GroupHasNoFilters);
        }


        public StructuredQueryBuilderStatus BuildStructuredQuery(StructuredQueryBuilderBase structuredQueryBuilder, Query query, RuleManager ruleManager, List<QueryFilter> additionalFilterCollection,ControlData controlData)
        {
            StructuredQueryBuilderStatus rv = StructuredQueryBuilderStatus.Success;
            List<QueryFilter> sortedQueryFilters;
            List<QueryFilter> filters;
            RemoveEmptyGroups(query);

            var distinctGroups = (from g1 in query.Groups
                                       select g1.FilterGroup.GetType()).Distinct().ToList();

            if (distinctGroups.Count != query.Groups.Count)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_ONE_OF_EACH_GROUP_ALLOWED);

            foreach (Group group in query.Groups)
            {
                if (group.FilterGroup.Filters.Count > 0)
                {
                    // flatten and sort the filter list for processing
                    sortedQueryFilters = GetSortedQueryFilters(group.FilterGroup.Filters);

                    var distinctFilterTypes = (from f1 in sortedQueryFilters
                                               select f1.GetType()).Distinct().ToList();



                    foreach (var temp in distinctFilterTypes)
                    {
                        filters = (from f in sortedQueryFilters
                                   where f.GetType().Name == temp.Name
                                   select f).ToList();

                        if (!ProcessQueryFilter(filters, structuredQueryBuilder,ruleManager,controlData))
                        {
                            rv = StructuredQueryBuilderStatus.Truncated;
                        }
                    }
                    
                    structuredQueryBuilder.MergeGroups(group);
                }
            }
            
            structuredQueryBuilder.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)structuredQueryBuilder).mergedGroupTradtionalString);
            
            #region Additional Filters
            if (additionalFilterCollection.Count > 0)
            {
                
                foreach (QueryFilter filter in additionalFilterCollection)
                {
                    filters = new List<QueryFilter> { filter };
                    if (!ProcessQueryFilter(filters, structuredQueryBuilder, ruleManager,controlData))
                    {
                        rv = StructuredQueryBuilderStatus.Truncated;
                    }
                }
                structuredQueryBuilder.AddOperatorToMergedString(Operator.And);
                ((ContentStructuredQueryBuilder)structuredQueryBuilder).MergeWithOperator(Operator.And, GroupOperator.And);
                structuredQueryBuilder.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)structuredQueryBuilder).mergedGroupTradtionalString);
            }
            #endregion

            structuredQueryBuilder.CommitMergedString();

            return rv;
        }

        

        private void PreProcessQueryFilter(QueryFilter filter, StructuredQueryBuilderBase b)
        {
            if (filter.Operator == Operator.Not)
                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
            else
                b.AddOpenBracketToTraditionalString();
        }

        private void PostProcessQueryFilter(QueryFilter filter, StructuredQueryBuilderBase b)
        {
            b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
            if (filter.Operator != Operator.Not)
                b.AddCloseBracketToTraditionalString();
            b.AddOperatorToTraditionalString(Operator.And);
        }
        private string GetIdFromFilterType(QueryFilter filter)
        {
            if (filter.GetType() == typeof(CompanyListFilter))
                return CompanyListID;
            else if (filter.GetType() == typeof(DJOrganizationCodeFilter))
                return OrganizationID;
            else if (filter.GetType() == typeof(CompanyScreeningFilter))
                return CompanyScreeningID;
            else if (filter.GetType() == typeof(DJPersonCodeFilter))
                return PersonID;
            else if (filter.GetType() == typeof(ExecutiveScreeningFilter))
                return ExecutiveScreeningID;

            return string.Empty;
        }

        private FilterType MapFilterType(QueryFilter filter)
        {
            if (filter is CompanyScreeningFilter)
                return FilterType.CompanyScreeningFilter;
            else if (filter is ExecutiveScreeningFilter)
                return FilterType.ExecutiveScreeningFilter;
            else if (filter is CompanyListFilter)
                return FilterType.CompanyListFilter;
            else if (filter is DJOrganizationCodeFilter)
                return FilterType.DJOrganizationCodeFilter;
            else if (filter is DJPersonCodeFilter)
                return FilterType.DJPersonCodeFilter;
            else
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_INVALID_FILTER_TYPE);

        }
        public bool ProcessQueryFilter(List<QueryFilter> filters, StructuredQueryBuilderBase b,RuleManager ruleManager,ControlData controlData)
        {
            bool rv = true;
            if (filters.First() is NewsSubjectCodeFilter)
            {
                #region NewsSubjectCodeFilter
                foreach (NewsSubjectCodeFilter filter in filters)
                {
                    if (filter.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string newsSubjectCode in filter.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeNS, filter.Operator, newsSubjectCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(NewsSubjectID);
                #endregion
            }
            else if (filters.First() is AuthorCodeFilter)
            {
                #region AuthorCodeFilter
                foreach (AuthorCodeFilter filter in filters)
                {
                    if (filter.PersonCodes.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string authorCode in filter.PersonCodes)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeAU, filter.Operator, authorCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(AuthorID);
                #endregion
            }
            else if (filters.First() is QueryPersonFilter)
            {
                #region QueryPersonFilter
                foreach (QueryPersonFilter filter in filters)
                {
                    if (filter.PersonNewsQueries.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (PersonNewsQuery personNewsQuery in filter.PersonNewsQueries)
                        {
                            ProximityRule rule = (ProximityRule)ruleManager.GetRuleByFilterType(MapFilterType(filter), typeof(ProximityRule));
                            rv &= b.AddPerson(filter.Operator, personNewsQuery,rule);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText,Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(GetIdFromFilterType(filters.First()));
                #endregion
            }
            else if (filters.First() is QueryOrganizationFilter)
            {
                #region QueryOrganizationFilter
                foreach (QueryOrganizationFilter filter in filters)
                {
                    if (filter.CompanyNewsQueries.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (companyNewsQuery companyNewsQuery in filter.CompanyNewsQueries)
                        {
                            rv &= b.AddOrganization(filter.Operator, companyNewsQuery);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(GetIdFromFilterType(filters.First()));
                #endregion
            }
            else if (filters.First() is IndustryCodeFilter)
            {
                #region IndustryCodeFilter
                foreach (IndustryCodeFilter filter in filters)
                {
                    if (filter.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string industryCode in filter.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeIN, filter.Operator, industryCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(IndustryID);
                #endregion
            }
            else if (filters.First() is RegionCodeFilter)
            {
                #region RegionCodeFilter
                foreach (RegionCodeFilter filter in filters)
                {
                    if (filter.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string regionCode in filter.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeRE, filter.Operator, regionCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText,Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(RegionID);
                #endregion
            }
            else if (filters.First() is SourcesFilter)
            {
                #region SourcesFilter
                //bool firstNot = true;
                bool isAllSources = false;
                foreach (SourcesFilter filter in filters)
                {
                    isAllSources = filter.IsAllSources;

                    #region if Any of the Source Categories is set to true
                    if (filter.AllSourcesCategories.IsAllPublications)
                        b.AddSearchFormatCategory(filter.Operator,SearchFormatCategory.Publications );
                    if (filter.AllSourcesCategories.IsAllBlogs)
                        b.AddSearchFormatCategory(filter.Operator, SearchFormatCategory.Blogs);
                    if (filter.AllSourcesCategories.IsAllBoards)
                        b.AddSearchFormatCategory(filter.Operator, SearchFormatCategory.Boards);
                    if (filter.AllSourcesCategories.IsAllMultimedia)
                        b.AddSearchFormatCategory(filter.Operator, SearchFormatCategory.Multimedia);
                    if (filter.AllSourcesCategories.IsAllPictures)
                        b.AddSearchFormatCategory(filter.Operator, SearchFormatCategory.Pictures);
                    if (filter.AllSourcesCategories.IsAllWebNews)
                        b.AddSearchFormatCategory(filter.Operator, SearchFormatCategory.WebNews);

                    #endregion
                    //if (filter.Operator == Operator.Not && firstNot )
                    //{
                    //    firstNot = false;
                    //    b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                    //    b.AddOperatorToTraditionalString(Operator.And);
                    //    b.AddOpenBracketToTraditionalString();
                    //}

                    foreach (SourceGroup eachSourceGroup in filter.SourceGroups)
                    {
                        rv &= b.AddSourceCode(filter.Operator, eachSourceGroup);
                        if (!rv) break;
                    }
                     
                    PostProcessQueryFilter(filter, b);
                }
                // can be And or Or
                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);

                //if (filters.Last().Operator == Operator.Not)
                //{
                //    b.AddCloseBracketToTraditionalString();
                //}
                
                // if still running, then commit the search string to query
                if (!isAllSources)
                    b.CommitSearchString(SourcesID);

                #endregion
            }
            else if (filters.First() is SearchSectionFilter)
            {
                #region Processing SearchSectionFilter
                SearchSectionFilter searchSectionFilter = (SearchSectionFilter)filters.First();
                rv &= b.AddSearchSectionCode(searchSectionFilter.Operator, searchSectionFilter.SearchSection);
                #endregion
            }
            else if (filters.First() is ContentLanguageFilter)
            {
                #region ContentLanguageFilter
                bool allLanguages = false;
                foreach (ContentLanguageFilter filter in filters)
                {
                    allLanguages = filter.IsAllLanguages;
                    if (filter.LanguageCodes.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (SearchLanguageCode languageCode in filter.LanguageCodes)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeLA, filter.Operator, GeneralUtils.GetXmlEnumName<SearchLanguageCode>(languageCode));
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText,Operator.And);
                // if still running, then commit the search string to query

                if (!allLanguages)
                    b.CommitSearchString(LanguageID); 
                
                #endregion
            }
            else if (filters.First() is FreeTextFilter)
            {
                #region FreeTextFilter
                foreach (FreeTextFilter filter in filters)
                {
                    if (filter.Texts.Count > 0)
                    {
                        if (filter.SearchMode == Factiva.Gateway.Messages.Assets.Queries.V1_0.SearchMode.Traditional)
                        {
                            PreProcessQueryFilter(filter, b);

                            foreach (string freeText in filter.Texts)
                            {
                                rv &= b.AddFreeText(filter.Operator, freeText);
                                if (!rv) break;
                            }
                            PostProcessQueryFilter(filter, b);
                        }
                        else
                        {
                            rv &= b.AddKeywordsFilterText(SearchMode.Simple, filter.Texts.FirstOrDefault(), FreeTextID);
                        }
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
                b.CheckSearchSectionForFreeText();
                // if still running, then commit the search string to query
                b.CommitSearchString(FreeTextID);
                #endregion
            }
            else if (filters.First() is KeywordsFilter)
            {
                #region Processing Keywords Filter
                foreach (KeywordsFilter filter in filters)
                {
                    if (filter.AllWords.Texts.Count > 1)
                        throw new NotSupportedException(filters.First().ToString() + " cannot have multiple texts in All Words");
                    if (filter.OrWords.Texts.Count > 1)
                        throw new NotSupportedException(filters.First().ToString() + " cannot have multiple texts in Or Words");
                    if (filter.NotWords.Texts.Count > 1)
                        throw new NotSupportedException(filters.First().ToString() + " cannot have multiple texts in None Words");
                    if (filter.ExactPhrase.Texts.Count > 1)
                        throw new NotSupportedException(filters.First().ToString() + " cannot have multiple texts in Phrase Words");

                    if (filter.AllWords.Texts.Count > 0)
                    {
                        rv &= b.AddKeywordsFilterText(SearchMode.All, filter.AllWords.Texts.FirstOrDefault(), KeywordsAllID);
                        if (!rv) break;
                    }
                    if (filter.OrWords.Texts.Count > 0)
                    {
                        rv &= b.AddKeywordsFilterText(SearchMode.Any, filter.OrWords.Texts.FirstOrDefault(), KeywordsAnyID);
                        if (!rv) break;
                    }
                    if (filter.NotWords.Texts.Count > 0)
                    {
                        rv &= b.AddKeywordsFilterText(SearchMode.None, filter.NotWords.Texts.FirstOrDefault(), KeywordsNoneID);
                        if (!rv) break;
                    }
                    if (filter.ExactPhrase.Texts.Count > 0)
                    {
                        rv &= b.AddKeywordsFilterText(SearchMode.Phrase, filter.ExactPhrase.Texts.FirstOrDefault(), KeywordsPhraseID);
                        if (!rv) break;
                    }

                }



                #endregion
            }
            else if (filters.First() is SearchExclusionFilter)
            {
                #region SearchExclustionFilter
                foreach (SearchExclusionFilter filter in filters)
                {
                    //filter.Operator = Operator.Not;
                    List<string> codes = new List<string>();
                    if (filter.AbstractsCaptionsAndHeadline)                    
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.AbstractsCaptionsAndHeadline));                                            
                    if (filter.CalendarsAndMediaAdvisories)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.CalendarsAndMediaAdvisories));
                    if (filter.CommentaryOpinionAndAnalysis)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.CommentaryOpinionAndAnalysis));
                    if (filter.Crime)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.Crime));
                    if (filter.EntertainmentAndLifestyle)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.EntertainmentAndLifestyle));
                    if (filter.NewsSummaries)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.NewsSummaries));
                    if (filter.RepublishedNews)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.RepublishedNews));
                    if (filter.RoutineGeneralNews)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.RoutineGeneralNews));
                    if (filter.RoutineMarketFinancialNews)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.RoutineGeneralNews));
                    if (filter.Sports)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.Sports));
                    if (filter.Transcripts)
                        codes.AddRange(GetExcludedSubject(ExclusionFilter.Transcripts));

                    b.AddSearchExclusionFilterSubjectCodes(ScopeNS, codes);
                }
                b.CommitSearchString(SearchExclusionID);

                #endregion
            }
            else if (filters.First() is SourceEntityFilter)
            {
                #region SourceEntityFilter
                foreach (SourceEntityFilter filter in filters)
                {
                    PreProcessQueryFilter(filter, b);

                    if (filter.SourceEntitiesCollection.Count > 0)
                    {                        
                        foreach (SourceEntities srcEntities in filter.SourceEntitiesCollection)
                        {
                            rv &= b.AddSourceEntities(srcEntities,controlData);
                            if (!rv) break;
                            b.AddOperatorToTraditionalString(filter.Operator); // Should be OR filter
                        }
                                           
                    }
                    PostProcessQueryFilter(filter, b);
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, Operator.And);
                // if still running, then commit the search string to query
                b.CommitSearchString(SourceEntityID);
                #endregion
            }
            else if (filters.First() is SourceEntityListIDFilter)
            {
                #region Source Entity List ID filter
                FCPServiceWrapper fsw = new FCPServiceWrapper(controlData);
                Query query = null;
                foreach (SourceEntityListIDFilter filter in filters)
                {
                    foreach (var id in filter.IdCollectionCollection)
                    {
                        query = fsw.GetQueryById(Convert.ToInt64(id));
                        if (query.Groups.Count() > 0)
                        {
                            var orFilterGroup = (from orGroup in query.Groups
                                                 where orGroup.FilterGroup.GetType() == typeof(OrFilterGroup)
                                                 select orGroup.FilterGroup);

                            if (orFilterGroup.Count() > 0)
                            {
                                var srcEntityFilters = (from srEntityFilter in orFilterGroup.FirstOrDefault().Filters
                                                   where srEntityFilter.GetType() == typeof(SourceEntityFilter)
                                                   select srEntityFilter).ToList();

                                if (srcEntityFilters.Count() > 0)
                                {
                                    b.AddOpenBracketToTraditionalString();
                                    foreach (SourceEntityFilter srcEntityFilter in srcEntityFilters)
                                    {
                                        srcEntityFilter.Operator = Operator.Or;                                        
                                        if (srcEntityFilter.SourceEntitiesCollection.Count > 0)
                                        {
                                            foreach (SourceEntities srcEntities in srcEntityFilter.SourceEntitiesCollection)
                                            {
                                                rv &= b.AddSourceEntities(srcEntities, controlData);
                                                if (!rv) break;
                                                b.AddOperatorToTraditionalString(srcEntityFilter.Operator); // Should be OR filter
                                            }
                                            b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText,srcEntityFilter.Operator);
                                        }                                                                                
                                        b.AddOperatorToTraditionalString(Operator.Or);
                                    }
                                    b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                                    b.AddCloseBracketToTraditionalString();                                    
                                }                               
                            }
                        }
                        b.AddOperatorToTraditionalString(filter.Operator);                        
                    }
                    b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                    // if still running, then commit the search string to query
                    b.CommitSearchString(SourceEntityID);
                }
                #endregion
            }
            else if (filters.First() is SearchAdditionalFilters)
            {
                #region Search Additional Filters
                foreach (SearchAdditionalFilters filter in filters)
                {
                    #region Industry collection

                    if (filter.IndustryFilters != null && filter.IndustryFilters.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string industryCode in filter.IndustryFilters.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeIN, Operator.And, industryCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                        b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                        b.AddOperatorToTraditionalString(filter.Operator);
                        //b.CommitSearchString(IndustryID);
                    }
                    #endregion
                    #region Author Code collection
                    if (filter.AuthorFilters != null && filter.AuthorFilters.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string authorCode in filter.AuthorFilters.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeAU, Operator.And, authorCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                        b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                        b.AddOperatorToTraditionalString(filter.Operator);
                        //b.CommitSearchString(IndustryID);
                    }
                    #endregion
                    #region Region Code collection
                    if (filter.RegionFilters != null && filter.RegionFilters.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string regionCode in filter.RegionFilters.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeRE, Operator.And, regionCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                        b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                        b.AddOperatorToTraditionalString(filter.Operator);
                        //b.CommitSearchString(IndustryID);
                    }
                    #endregion
                    #region Subject Code collection
                    if (filter.SubjectFilters != null && filter.SubjectFilters.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string subjectCode in filter.SubjectFilters.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopeNS, Operator.And, subjectCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                        b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                        b.AddOperatorToTraditionalString(filter.Operator);
                        //b.CommitSearchString(IndustryID);
                    }
                    #endregion
                    #region Executive Code collection
                    if (filter.ExecutiveFilters != null && filter.ExecutiveFilters.CodeCollection.Count > 0)
                    {
                        PreProcessQueryFilter(filter, b);

                        foreach (string executiveCode in filter.ExecutiveFilters.CodeCollection)
                        {
                            rv &= b.AddGenericTraditonalString(ScopePE, Operator.And, executiveCode);
                            if (!rv) break;
                        }
                        PostProcessQueryFilter(filter, b);
                        b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                        b.AddOperatorToTraditionalString(filter.Operator);
                        //b.CommitSearchString(IndustryID);
                    }
                    #endregion
                    #region Date Filter
                    if (filter.DateFilter != null && !filter.DateFilter.StartDate.Equals(DateTime.MinValue))
                    {
                        b.AddDates(GetDateDTO(filter.DateFilter), filter.DateFilter.Target);
                    }
                    #endregion
                    #region Sources Filter
                    if (filter.SourceFilters.SourceCollection.Count > 1)
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_SEARCHADDITIONALFILTERS_MULTIPLE_SOURCE_FILTERS_NOT_ALLOWED);
                    if (filter.SourceFilters.SourceCollection.Count == 1)
                    {
                        b.AddSearchAdditionalSourceFilterItem(filter.SourceFilters.SourceCollection.FirstOrDefault());
                    }
                    b.AddOperatorToTraditionalString(filter.Operator);
                    #endregion
                    #region Company collection
                    if (filter.CompanyFilters != null && filter.CompanyFilters.CodeCollection.Count > 0)
                    {
                        FCPServiceWrapper fsw = new FCPServiceWrapper(controlData);
                        companyNewsQueryCollection companyNewsQueries = fsw.GetCompanyNewsQueryFromCodes(filter.CompanyFilters.CodeCollection);
                        if (companyNewsQueries.Count > 0)
                        {
                            PreProcessQueryFilter(filter, b);

                            foreach (companyNewsQuery companyNewsQuery in companyNewsQueries)
                            {
                                rv &= b.AddOrganization(Operator.And, companyNewsQuery);
                                if (!rv) break;
                            }
                            PostProcessQueryFilter(filter, b);
                        }

                        b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                        b.AddOperatorToTraditionalString(filter.Operator);
                        //b.CommitSearchString(IndustryID);
                    }
                    #endregion
                    #region Kewyword collection (Not supported)
                    if (filter.Keywords != null && filter.Keywords.CodeCollection.Count > 0)
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_SEARCHADDITIONALFILTERS_KEYWORDS_NOT_SUPPORTED);
                    }
                    #endregion
                    b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText);
                    b.CommitSearchString(SearchAdditionalID);
                }
                #endregion
            }

            else if (filters.First() is DaysFilter)
            {
                b.AddDates(GetDateDTO((DaysFilter)filters.First()), ((DaysFilter)filters.First()).Target);
            }
            else if (filters.First() is DateFilter)
            {
                b.AddDates(GetDateDTO((DateFilter)filters.First()), ((DateFilter)filters.First()).Target);
            }
            //else
            //{
            //    throw new NotSupportedException(filters.First().ToString() + " is not supported for ContentSearch.");
            //}
            return rv;
        }
        /*private bool ProcessQueryFilter(QueryFilter filter, StructuredQueryBuilderBase b)
        {
            bool rv = true;

            if (filter is AuthorCodeFilter) // should be before QueryPersonFilter
            {
                #region Processing AuthorCodeFilter
                AuthorCodeFilter authorFilter = (AuthorCodeFilter)filter;

                foreach (string eachAuthor in authorFilter.PersonCodes)
                {
                    rv &= b.AddGenericTraditonalString(ScopeAU, filter.Operator, eachAuthor);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is QueryPersonFilter)
            {
                #region Processing QueryPersonFilter
                QueryPersonFilter execFilter = (QueryPersonFilter)filter;

                foreach (PersonNewsQuery eachExecutive in execFilter.PersonNewsQueries)
                {
                    rv &= b.AddPerson(filter.Operator, eachExecutive);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is QueryOrganizationFilter)
            {
                #region Processing QueryOrganizationFilter
                QueryOrganizationFilter orgFilter = (QueryOrganizationFilter)filter;

                foreach (companyNewsQuery eachCompany in orgFilter.CompanyNewsQueries)
                {
                    rv &= b.AddOrganization(filter.Operator, eachCompany);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is NewsSubjectCodeFilter)
            {
                #region Processing NewsSubjectCodeFilter
                NewsSubjectCodeFilter newsSubjectFilter = (NewsSubjectCodeFilter)filter;

                foreach (string eachNewsSubject in newsSubjectFilter.CodeCollection)
                {
                    rv &= b.AddGenericTraditonalString(ScopeNS, filter.Operator, eachNewsSubject);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is IndustryCodeFilter)
            {
                #region Processing IndustryCodeFilter
                IndustryCodeFilter industryCodeFilter = (IndustryCodeFilter)filter;

                foreach (string eachIndustry in industryCodeFilter.CodeCollection)
                {
                    rv &= b.AddGenericTraditonalString(ScopeIN, filter.Operator, eachIndustry);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);

                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is RegionCodeFilter)
            {
                #region Processing RegionCodeFilter
                RegionCodeFilter regionCodeFilter = (RegionCodeFilter)filter;

                foreach (string eachRegion in regionCodeFilter.CodeCollection)
                {
                    rv &= b.AddGenericTraditonalString(ScopeRE, filter.Operator, eachRegion);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is SourcesFilter)
            {
                #region Processing QueryOrganizationFilter
                SourcesFilter sourcesFilter = (SourcesFilter)filter;

                b.AddOpenBracketToTraditionalString();

                foreach (SourceGroup eachSourceGroup in sourcesFilter.SourceGroups)
                {
                    rv &= b.AddSourceCode(filter.Operator, eachSourceGroup);
                    if (!rv) break;
                }

                b.AddCloseBracketToTraditionalString();

                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is SearchSectionFilter)
            {
                #region Processing QueryOrganizationFilter
                SearchSectionFilter searchSectionFilter = (SearchSectionFilter)filter;

                //foreach (string eachRegion in regionCodeFilter.CodeCollection)
                //{
                rv &= b.AddSearchSectionCode(filter.Operator, searchSectionFilter.SearchSection);
                //                    if (!rv) break;
                //}

                // if still running, then commit the search string to query
                //b.CommitSearchString(filter.Operator);

                #endregion
            }

            else if (filter is ContentLanguageFilter)
            {
                #region Processing QueryOrganizationFilter
                ContentLanguageFilter contentLanguageFilter = (ContentLanguageFilter)filter;

                if (!contentLanguageFilter.IsAllLanguages)
                {
                    foreach (SearchLanguageCode languageCode in contentLanguageFilter.LanguageCodes)
                    {
                        rv &= b.AddGenericTraditonalString(ScopeLA, filter.Operator,GeneralUtils.GetXmlEnumName<SearchLanguageCode>(languageCode));
                        if (!rv) break;
                    }
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is FreeTextFilter)
            {
                #region Processing FreeTextFilter

                FreeTextFilter ftFilter = (FreeTextFilter)filter;

                //Return if no Texts are there.
                if (ftFilter.Texts == null || ftFilter.Texts.Count == 0) return true;

                foreach (string str in ftFilter.Texts)
                {
                    rv &= b.AddFreeText(filter.Operator, str);
                    if (!rv) break;
                }

                b.RemoveLastOpFromTraditional(((ContentStructuredQueryBuilder)b)._searchStringContext.TraditionalSearchStringText, filter.Operator);
                b.CheckSearchSectionForFreeText();
                // if still running, then commit the search string to query
                b.CommitSearchString();

                #endregion
            }
            else if (filter is TriggerTypeFilter)
            {
                TriggerTypeFilter ttFilter = (TriggerTypeFilter)filter;

                for (int i = 0; i < ttFilter.TriggerTypes.Count; i++)
                {
                    rv &= b.AddTriggerType(filter.Operator, ttFilter.TriggerTypes[i].Code);
                    if (!rv) break;
                    rv &= b.AddTriggerType(filter.Operator, ttFilter.TriggerTypes[i].Values);
                    if (!rv) break;
                }

                // if still running, then commit the search string to query
                b.CommitSearchString();
            }
            else if (filter is DaysFilter)
            {
                b.AddDates(GetDateDTO((DaysFilter)filter),((DaysFilter)filter).Target);
            }
            else if (filter is DateFilter)
            {
                b.AddDates(GetDateDTO((DateFilter)filter), ((DateFilter)filter).Target);
            }
            else
            {
                throw new NotSupportedException(filter.ToString() + " is not supported for ContentSearch.");
            }

            return rv;
        }
        */
        private DateDTO GetDateDTO(DateFilter dateFilter)
        {
            DateDTO dtDTO = new DateDTO();

            dtDTO.dateDtoType = DateDtoType.DateRange;

            if (dateFilter.EndDate > dateFilter.StartDate)
            {
                dtDTO.dateRange = SearchDateRange.Custom;

                dtDTO.fromDate = new YearMonthDay
                {
                    day = dateFilter.StartDate.Day,
                    month = dateFilter.StartDate.Month,
                    year = dateFilter.StartDate.Year
                };

                dtDTO.toDate = new YearMonthDay
                {
                    day = dateFilter.EndDate.Day,
                    month = dateFilter.EndDate.Month,
                    year = dateFilter.EndDate.Year
                };
            }
            else
            {
                dtDTO.dateRange = SearchDateRange._Unspecified;
            }

            return dtDTO;
        }

        private DateDTO GetDateDTO(DaysFilter daysFilter)
        {
            DateDTO dtDTO = new DateDTO();
            dtDTO.SinceDays = daysFilter.SinceDays.ToString();
            dtDTO.dateRange = SearchDateRange.SinceDays;
            dtDTO.dateDtoType = DateDtoType.DateSinceDays;
            return dtDTO;
        }

        #endregion
        

    }
}
