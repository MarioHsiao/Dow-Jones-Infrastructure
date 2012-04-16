// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchUtility.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Search.Attributes;
using DowJones.Search.Controller;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using DowJones.Utilities.Search.Core;
using System.Text;

namespace DowJones.Search.Core
{
    /// <summary>
    /// The search utility.
    /// </summary>
    public class SearchUtility
    {
        #region ---- Public Constants ----
        public const string ALL_SOURCES_CODE = "All";
        public const string ALL_PUBLICATIONS_CODE = "P|";
        public const string ALL_WEBSITES_CODE = "W|";
        public const string ALL_PICTURES_CODE = "I|";
        public const string ALL_MULTIMEDIAS_CODE = "M|";
        public const string ALL_BLOGS_CODE = "B|";
        public const string ALL_BOARDS_CODE = "O|";
        public const string ALL_INTERNAL_CODE = "T|";

        public const string DOW_JONES_NEWSWIRES = "TDJW";
        public const string MAJOR_NEWS_AND_BUSINESS_PUBLICATIONS = "TMNB";
        public const string PRESS_RELEASE_WIRES = "TPRW";
        public const string REUTERS_NEWSWIRES = "TRTW";

        private const string BLOG_FORMAT = "fmt=blog";
        private const string BOARD_FORMAT = "fmt=board";
        private const string CUSTOMERDOC_FORMAT = "fmt=customerdoc";
        private const string MULTIMEDIA_FORMAT = "fmt=multimedia";
        private const string PICTURE_FORMAT = "fmt=picture";
        private const string PUBLICATION_FORMAT = "fmt=(article or report or file)";
        private const string WEBPAGE_FORMAT = "fmt=webpage";
        private const string FMT_OPERATOR = " or ";
        #endregion

        #region ScopeType enum

        /// <summary>
        /// The scope type.
        /// </summary>
        public enum ScopeType
        {
            /// <summary>
            /// The any languages.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "la")] AnyLanguages, 

            /// <summary>
            /// The any source.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "sc")] AnySource, 

            /// <summary>
            /// The excluded sources.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "sc")] ExcludedSources, 

            /// <summary>
            /// The any group source code.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "rst")] AnyGroupSourceCode, 

            /// <summary>
            /// The excluded group source codes.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "rst")] ExcludedGroupSourceCodes, 

            /// <summary>
            /// The any restrictor.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "rst")] AnyRestrictor, 

            /// <summary>
            /// The all restrictors.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "rst")] AllRestrictors, 

            /// <summary>
            /// The excluded restrictors.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "rst")] ExcludedRestrictors, 

            /// <summary>
            /// The any news subject.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "ns")] AnyNewsSubject, 

            /// <summary>
            /// The all news subjects.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "ns")] AllNewsSubjects, 

            /// <summary>
            /// The excluded new subjects.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "ns")] ExcludedNewSubjects, 

            /// <summary>
            /// The any company.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "co")] AnyCompany, 

            /// <summary>
            /// The all companies.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "co")] AllCompanies, 

            /// <summary>
            /// The excluded companies.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "co")] ExcludedCompanies, 

            /// <summary>
            /// The any company occurance.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "co:occur")] AnyCompanyOccurance, 

            /// <summary>
            /// The all companies ocurrance.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "co:occur")] AllCompaniesOcurrance, 

            /// <summary>
            /// The excluded companies occurance.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "co:occur")] ExcludedCompaniesOccurance, 

            /// <summary>
            /// The any author.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "au")] AnyAuthor, 

            /// <summary>
            /// The all authors.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "au")] AllAuthors, 

            /// <summary>
            /// The excluded authors.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "au")] ExcludedAuthors, 

            /// <summary>
            /// The any region.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "re")] AnyRegion, 

            /// <summary>
            /// The all regions.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "re")] AllRegions, 

            /// <summary>
            /// The excluded regions.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "re")] ExcludedRegions, 

            /// <summary>
            /// The any fds.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "fds")] AnyFDS, 

            /// <summary>
            /// The all fd ss.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "fds")] AllFDSs, 

            /// <summary>
            /// The excluded fd ss.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "fds")] ExcludedFDSs, 

            /// <summary>
            /// The keywords.
            /// </summary>
            [SearchStringInfo(SearchMode.Simple, false, false, SearchType.Free, "")] Keywords, 

            /// <summary>
            /// The editors choice.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "ns")] EditorsChoice, 

            /// <summary>
            /// The any industry.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "in")] AnyIndustry, 

            /// <summary>
            /// The all industries.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "in")] AllIndustries, 

            /// <summary>
            /// The excluded industries.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "in")] ExcludedIndustries, 

            /// <summary>
            /// The any accession number.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "an")] AnyAccessionNumber, 

            /// <summary>
            /// The any people.
            /// </summary>
            [SearchStringInfo(SearchMode.Any, false, true, SearchType.Controlled, "pe")] AnyPeople, 

            /// <summary>
            /// The all people.
            /// </summary>
            [SearchStringInfo(SearchMode.All, false, true, SearchType.Controlled, "pe")] AllPeople, 

            /// <summary>
            /// The excluded people.
            /// </summary>
            [SearchStringInfo(SearchMode.None, false, true, SearchType.Controlled, "pe")] ExcludedPeople, 
        }

        #endregion

        /// <summary>
        /// The generate navigation control.
        /// </summary>
        /// <param name="metaDataController">The meta data controller.</param>
        /// <returns></returns>
        public static NavigationControl GenerateNavigationControl(MetaDataController metaDataController)
        {
            if (metaDataController == null)
            {
                return null;
            }

            var navigationControl = new NavigationControl();

            if (metaDataController.Mode != CodeNavigatorMode.None ||
                (metaDataController.CustomCodeNavigatorIds != null && metaDataController.CustomCodeNavigatorIds.Length > 0))
            {
                var objCodeNavigatorControl = new CodeNavigatorControl
                                                  {
                                                      MaxBuckets = metaDataController.MaxBuckets, 
                                                      MinBucketValue = metaDataController.MinBucketValue, 
                                                      Mode = metaDataController.Mode
                                                  };

                if (metaDataController.CustomCodeNavigatorIds != null && metaDataController.CustomCodeNavigatorIds.Length > 0)
                {
                    foreach (string s in metaDataController.CustomCodeNavigatorIds)
                    {
                        var control = new NavigatorControl
                                          {
                                              Id = s, 
                                              MaxBuckets = metaDataController.MaxBuckets, 
                                              MinBucketValue = metaDataController.MinBucketValue
                                          };
                        objCodeNavigatorControl.CustomCollection.Add(control);
                    }
                }

                navigationControl.CodeNavigatorControl = objCodeNavigatorControl;
            }


            if (metaDataController.CustomContextualNavigatorIds != null && metaDataController.CustomContextualNavigatorIds.Length > 0)
            {
                foreach (ContextualNavigatorControl objContextualNavigatorControl in metaDataController.CustomContextualNavigatorIds.Select(s => new ContextualNavigatorControl
                                                                                                                                                     {
                                                                                                                                                         CountOncePerDocument = metaDataController.CountCustomContextualNavigatorIdsOncePerDocument, Id = s, MaxBuckets = metaDataController.MaxBuckets, MinBucketValue = metaDataController.MinBucketValue
                                                                                                                                                     }))
                {
                    navigationControl.ContextualNavigatorControlCollection.Add(objContextualNavigatorControl);
                }
            }

            if (metaDataController.ReturnKeywordsSet)
            {
                var objKeywordControl = new KeywordControl
                                            {
                                                MaxKeywords = metaDataController.MaxKeywords, 
                                                MinWeight = (float)metaDataController.MinWeightKeywords, 
                                                ReturnKeywords = metaDataController.ReturnKeywordsSet
                                            };
                navigationControl.KeywordControl = objKeywordControl;
            }

            navigationControl.ReturnCollectionCounts = metaDataController.ReturnCollectionCounts;
            navigationControl.TimeNavigatorMode = metaDataController.TimeNavigatorMode;
            return navigationControl;
        }

        /// <summary>
        /// The get dates.
        /// </summary>
        /// <param name="dateController">The date controller.</param>
        /// <param name="dateType">The date type.</param>
        /// <returns></returns>
        public static Dates GetDates(DateController dateController, DateType dateType = DateType.Publication)
        {
            if (dateController == null)
            {
                return null;
            }

            var objDates = new Dates
                               {
                                   Type = dateType, 
                                   Format = dateController.DateFormat, 
                               };

            if (dateController.Before != null)
            {
                objDates.Before = dateController.Before.ToString();
            }

            switch (dateController.DateQualifier)
            {
                case DateQualifier.All:
                    objDates.All = true;
                    break;
                case DateQualifier.CustomDateRange:
                    objDates.Range = EnsureDateRange(dateController.Range);
                    break;
                case DateQualifier.EqualsDate:
                    objDates.EqualsDate = dateController.Equal;
                    break;
                case DateQualifier.AfterAndOrBefore:
                    if (dateController.Before != null)
                    {
                        objDates.Before = dateController.Before.ToString();
                    }

                    if (dateController.After != null)
                    {
                        objDates.After = dateController.After.ToString();
                    }

                    break;
                default:
                    objDates.After = GetTimeSlice(dateController.DateQualifier);
                    break;
            }

            return objDates;
        }

        /// <summary>
        /// The ensure date range.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns></returns>
        protected static DateRange EnsureDateRange(DateRange current)
        {
            current.To = Ensure(current.To);
            current.From = Ensure(current.From);
            return current;
        }

        /// <summary>
        /// The ensure.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The ensure.
        /// </returns>
        private static string Ensure(string date)
        {
            return !date.Contains("/") ? string.Concat(date.Substring(0, 2), "/", date.Substring(2, 2), "/", date.Substring(4)) : date;
        }

        /// <summary>
        /// The get time slice.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The get time slice.
        /// </returns>
        protected static string GetTimeSlice<T>(T value)
        {
            var enumType = typeof(T);
            var s = value.ToString();
            var timeSlice = (TimeSlice)Attribute.GetCustomAttribute(enumType.GetField(s), typeof(TimeSlice));
            return timeSlice != null ? timeSlice.Slice.ToString() : null;
        }

        /// <summary>
        /// The get similarity filter.
        /// </summary>
        /// <param name="documentVector">The document vector.</param>
        /// <returns></returns>
        public static SimilarityFilter GetSimilarityFilter(string documentVector)
        {
            var similarityFilter = new SimilarityFilter
                                       {
                                           Type = SimilarityType.Refine, 
                                           Value = documentVector, 
                                           SortBy = true
                                       };
            return similarityFilter;
        }

        /// <summary>
        /// The get search string by scope type.
        /// </summary>
        /// <param name="scopeType">The scope type.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        ///   </exception>
        public static SearchString GetSearchStringByScopeType(ScopeType scopeType, string[] list)
        {
            var attr = (SearchStringInfoAttribute)Attribute.GetCustomAttribute(typeof(ScopeType).GetField(scopeType.ToString()), typeof(SearchStringInfoAttribute));
            if (attr != null)
            {
                var searchString = new SearchString();
                switch (attr.SearchMode)
                {
                    case SearchMode.Advanced:
                        throw new NotSupportedException("SearchMode.Advanced is not supported");
                    case SearchMode.Simple:
                    case SearchMode.Traditional:
                    case SearchMode.Phrase:
                        searchString.Id = scopeType.ToString();
                        searchString.Mode = attr.SearchMode;
                        searchString.Type = attr.SearchType;
                        searchString.Value = string.Join(" ", list);
                        searchString.Validate = attr.Validate;
                        break;
                    case SearchMode.All:
                    case SearchMode.Any:
                    case SearchMode.None:
                        searchString.Id = scopeType.ToString();
                        searchString.Mode = attr.SearchMode;
                        searchString.Type = attr.SearchType;
                        searchString.Value = string.Join(" ", list);
                        searchString.Scope = attr.Scope;
                        searchString.Filter = attr.Filter;
                        searchString.Validate = attr.Validate;
                        break;
                }

                return searchString;
            }

            return null;
        }

        /// <summary>
        /// The get search string by scope type.
        /// </summary>
        /// <param name="scopeType">The scope type.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        ///   </exception>
        public static MetadataSearchString GetMetaSearchStringByScopeType(ScopeType scopeType, string[] list)
        {
            var attr = (SearchStringInfoAttribute)Attribute.GetCustomAttribute(typeof(ScopeType).GetField(scopeType.ToString()), typeof(SearchStringInfoAttribute));
            if (attr != null)
            {
                var searchString = new MetadataSearchString();
                switch (attr.SearchMode)
                {
                    case SearchMode.Advanced:
                        throw new NotSupportedException("SearchMode.Advanced is not supported");
                    case SearchMode.Simple:
                    case SearchMode.Traditional:
                    case SearchMode.Phrase:
                        searchString.Id = scopeType.ToString();
                        searchString.Mode = MapSearchModetoMetadataSearchMode(attr.SearchMode);
                        searchString.Type = attr.SearchType;
                        searchString.Value = string.Join(" ", list);
                        break;
                    case SearchMode.All:
                    case SearchMode.Any:
                    case SearchMode.None:
                        searchString.Id = scopeType.ToString();
                        searchString.Mode = MapSearchModetoMetadataSearchMode(attr.SearchMode);
                        searchString.Type = attr.SearchType;
                        searchString.Value = string.Join(" ", list);
                        searchString.Scope = attr.Scope;
                        break;
                }

                return searchString;
            }

            return null;
        }

        /// <summary>
        /// The get dictionary of content headlines.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static Dictionary<string, ContentHeadline> GetDictionaryOfContentHeadlines(IPerformContentSearchResponse response)
        {
            return GetDictionaryOfContentHeadlines(new Dictionary<string, ContentHeadline>(), response);
        }

        /// <summary>
        /// The get dictionary of content headlines.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static Dictionary<string, ContentHeadline> GetDictionaryOfContentHeadlines(Dictionary<string, ContentHeadline> data, IPerformContentSearchResponse response)
        {
            if (response != null &&
                response.ContentSearchResult != null &&
                response.ContentSearchResult.ContentHeadlineResultSet != null &&
                response.ContentSearchResult.ContentHeadlineResultSet.Count > 0)
            {
                // Do not have to check for dups. None should be returned from Search.
                foreach (ContentHeadline headline in response.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection)
                {
                    string accNo = (string.IsNullOrEmpty(headline.AccessionNo) || string.IsNullOrEmpty(headline.AccessionNo.Trim())) ? String.Empty : headline.AccessionNo.Trim();
                    if (!string.IsNullOrEmpty(accNo) && !data.ContainsKey(accNo))
                    {
                        data.Add(accNo, headline);
                    }
                }
            }

            return data;
        }

        public static bool IsUiSourceCode(string code)
        {
            code = code.Trim().ToUpper();
            switch (code)
            {
                case ALL_PUBLICATIONS_CODE:
                case ALL_WEBSITES_CODE:
                case ALL_PICTURES_CODE:
                case ALL_MULTIMEDIAS_CODE:
                case ALL_BLOGS_CODE:
                case ALL_BOARDS_CODE:
                case ALL_INTERNAL_CODE:
                    return true;
                default:
                    return false;
            }

        }

        public static SearchCollectionCollection GetAvailableContentCategories(
           SearchSourceGroupPreferenceItem searchSourceGroupPreferenceItem)
        {
            var searchCollectionCollection = new SearchCollectionCollection();

            bool isPubIncluded = IsSourceTypeIncluded(SourceType.Publication, searchSourceGroupPreferenceItem);
            bool isWebIncluded = IsSourceTypeIncluded(SourceType.Website, searchSourceGroupPreferenceItem);
            bool isBlogs = IsSourceTypeIncluded(SourceType.Blogs, searchSourceGroupPreferenceItem);
            bool isPicIncluded = IsSourceTypeIncluded(SourceType.Picture, searchSourceGroupPreferenceItem);
            bool isMltIncluded = IsSourceTypeIncluded(SourceType.Multimedia, searchSourceGroupPreferenceItem);

            bool isEmptySource = !(isPubIncluded || isWebIncluded || isBlogs || isPicIncluded || isMltIncluded);

            //If you have a empty source list means all sources! 
            if (isEmptySource || searchSourceGroupPreferenceItem.Value.IsAllSourceTypesSelected) // Take safe route
            {
                isPubIncluded = true;
                isWebIncluded = true;
                isBlogs = true;
                isPicIncluded = true;
                isMltIncluded = true;
            }
            if (isPubIncluded)
            {
                searchCollectionCollection.Add(SearchCollection.Publications);
            }
            if (isWebIncluded)
            {
                searchCollectionCollection.Add(SearchCollection.WebSites);
            }
            if (isBlogs)
            {
                searchCollectionCollection.Add(SearchCollection.Blogs);
            }
            if (isPicIncluded)
            {
                searchCollectionCollection.Add(SearchCollection.Pictures);
            }
            if (isMltIncluded)
            {
                searchCollectionCollection.Add(SearchCollection.Multimedia);
            }

            return searchCollectionCollection;
        }

        private static bool IsSourceTypeIncluded(SourceType type,
                                                 SearchSourceGroupPreferenceItem searchSourceGroupPreferenceItem)
        {
            bool included = false;
            if (searchSourceGroupPreferenceItem != null && searchSourceGroupPreferenceItem.Value.Count > 0)
            {
                if (searchSourceGroupPreferenceItem.Value.Any(sourceList => sourceList.Type == type))
                {
                    included = true;
                }
            }
            return included;
        }

        public static SearchStringCollection BuildSourceSearchStringCollection(SearchSourceGroupPreferenceItem ssgpItem)
        {
            var searchStringCollection = new SearchStringCollection();
            if (ssgpItem != null && ssgpItem.Value != null && ssgpItem.Value.Count > 0)
            {
                var includedSources = new List<string>();
                var excludedSources = new List<string>();
                foreach (SourceList sourceList in ssgpItem.Value)
                {
                    includedSources.AddRange(sourceList.CodeIncluded);
                    excludedSources.AddRange(sourceList.CodeExcluded);
                }
                excludedSources = excludedSources.Where(d => includedSources.IndexOf(d) == -1).ToList();

                bool allPub = GetAllSelected(ssgpItem, ALL_PUBLICATIONS_CODE);
                bool allWeb = GetAllSelected(ssgpItem, ALL_WEBSITES_CODE);

                if (allPub || allWeb)
                {
                    var fmtString = String.Empty;
                    if (allPub)
                    {
                        fmtString = PUBLICATION_FORMAT;
                    }
                    if (allWeb)
                    {
                        if (fmtString.Length > 0)
                        {
                            fmtString += FMT_OPERATOR;
                        }
                        fmtString += WEBPAGE_FORMAT;
                    }
                    searchStringCollection.Add(
                        new SearchString {Mode = SearchMode.Traditional, Id = "BSSSourceFmtPart", Value = fmtString}
                    );
                }

                if (includedSources.Count > 0)
                {
                    searchStringCollection.Add(new SearchString
                                                   {
                                                       Scope = "rst",
                                                       Id = "AnySources",
                                                       Mode = SearchMode.Any,
                                                       Value = String.Join(" ", includedSources),
                                                       Filter = true
                                                   });
                }
                if (excludedSources.Count > 0)
                {
                    searchStringCollection.Add(new SearchString
                                                   {
                                                       Scope = "rst",
                                                       Id = "NotSources",
                                                       Mode = SearchMode.None,
                                                       Value = String.Join(" ", excludedSources),
                                                       Filter = true
                                                   });
                }
            }
            return searchStringCollection;
        }

        public static SearchStringCollection BuildSearchStringCollection(
            SearchSourceGroupPreferenceItem searchSourceGroupPreferenceItem)
        {
            var searchStringCollection = new SearchStringCollection();
            //Build SouceList search string
            if (searchSourceGroupPreferenceItem != null)
            {
                //Included sources
                List<Source> includedSources = GetSourceCodes(searchSourceGroupPreferenceItem, true);

                //<<Exclude Sources >>//
                List<Source> excludedSources = GetSourceCodes(searchSourceGroupPreferenceItem, false);

                bool allPub = GetAllSelected(searchSourceGroupPreferenceItem, ALL_PUBLICATIONS_CODE);
                bool allWeb = GetAllSelected(searchSourceGroupPreferenceItem, ALL_WEBSITES_CODE);
                bool allBlog = GetAllSelected(searchSourceGroupPreferenceItem, ALL_BLOGS_CODE);
                bool allPic = GetAllSelected(searchSourceGroupPreferenceItem, ALL_PICTURES_CODE);
                bool allMult = GetAllSelected(searchSourceGroupPreferenceItem, ALL_MULTIMEDIAS_CODE);

                
                //Add source search string
                if ((includedSources != null && includedSources.Count > 0) ||
                    (excludedSources != null && excludedSources.Count > 0))
                {
                    searchStringCollection.Add(CreateSourceSearchString(includedSources, excludedSources, allPub,
                                                                        allWeb, allBlog, allPic, allMult));
                }
            }

            return searchStringCollection;
        }

        private static bool GetAllSelected(SearchSourceGroupPreferenceItem searchSourceGroupPreferenceItem, string allCode)
        {
            if (searchSourceGroupPreferenceItem != null && searchSourceGroupPreferenceItem.Value != null &&
                searchSourceGroupPreferenceItem.Value.Count > 0)
            {
                return searchSourceGroupPreferenceItem.Value.Any(sourceList => sourceList.IsAllSourcesSelected && sourceList.Type == MapAllCode(allCode));
            }
            return false;
        }

        private static SourceType MapAllCode(string allCode)
        {
            switch (allCode)
            {
                case ALL_WEBSITES_CODE:
                    return SourceType.Website;
                case ALL_BLOGS_CODE:
                    return SourceType.Blogs;
                case ALL_PICTURES_CODE:
                    return SourceType.Picture;
                case ALL_MULTIMEDIAS_CODE:
                    return SourceType.Multimedia;
                case ALL_BOARDS_CODE:
                    return SourceType.Boards;
                case ALL_INTERNAL_CODE:
                    return SourceType.Internal;
                default:
                    return SourceType.Publication;
            }

        }

        private static List<Source> GetSourceCodes(SearchSourceGroupPreferenceItem searchSourceGroupPreferenceItem,
                                                   bool isIncluded)
        {
            var sources = new List<Source>();
            // source lists
            if (searchSourceGroupPreferenceItem != null && searchSourceGroupPreferenceItem.Value != null &&
                searchSourceGroupPreferenceItem.Value.Count > 0)
            {
                foreach (SourceList sourceList in searchSourceGroupPreferenceItem.Value)
                {
                    CodeList codeList = isIncluded ? sourceList.CodeIncluded : sourceList.CodeExcluded;
                    foreach (string code in codeList)
                    {
                        switch (sourceList.Type)
                        {
                            case SourceType.Publication:
                                sources.Add(new Source
                                {
                                    Type = SourceType.Publication,
                                    SourceCode = code
                                });
                                break;
                            case SourceType.Picture:
                                sources.Add(new Source
                                {
                                    Type = SourceType.Picture,
                                    SourceCode = code
                                });
                                break;
                            case SourceType.Website:
                                sources.Add(new Source
                                {
                                    Type = SourceType.Website,
                                    SourceCode = code
                                });
                                break;
                            case SourceType.Multimedia:
                                sources.Add(new Source
                                {
                                    Type = SourceType.Multimedia,
                                    SourceCode = code
                                });
                                break;
                            case SourceType.Blogs:
                                sources.Add(new Source
                                {
                                    Type = SourceType.Blogs,
                                    SourceCode = code
                                });
                                break;
                        }
                    }
                }
            }
            return sources;
        }

        public static SearchString CreateSourceSearchString(List<Source> includedSources, List<Source> excludedSources,
                                                            bool allPublication, bool allWebsites, bool allBlog,
                                                            bool allPicutres, bool allMultimedia)
        {
            var sb = new StringBuilder();
            SourceCodes includedSourceCodes = GetSourceCodes(includedSources, false);
            SourceCodes excludedSourceCodes = GetSourceCodes(excludedSources, true);

            string sourceQuery = GetSourceRestrictorQuery(includedSourceCodes, excludedSourceCodes);
            sb.Append(sourceQuery);

            if (allPublication && (string.IsNullOrEmpty(includedSourceCodes.Pubication.Query) &&
                                   string.IsNullOrEmpty(excludedSourceCodes.Pubication.Query)))
            {
                sb.Append(GetFormattedQuery(sb.ToString(), PUBLICATION_FORMAT));
            }
            if (allWebsites && (string.IsNullOrEmpty(includedSourceCodes.WebPage.Query) &&
                                string.IsNullOrEmpty(excludedSourceCodes.WebPage.Query)))
            {
                sb.Append(GetFormattedQuery(sb.ToString(), WEBPAGE_FORMAT));
            }
            if (allBlog && (string.IsNullOrEmpty(includedSourceCodes.Blogs.Query) &&
                            string.IsNullOrEmpty(excludedSourceCodes.Blogs.Query)))
            {
                sb.Append(GetFormattedQuery(sb.ToString(), BLOG_FORMAT));
            }
            if (allPicutres && (string.IsNullOrEmpty(includedSourceCodes.Picture.Query) &&
                                string.IsNullOrEmpty(excludedSourceCodes.Picture.Query)))
            {
                sb.Append(GetFormattedQuery(sb.ToString(), PICTURE_FORMAT));
            }
            if (allMultimedia && (string.IsNullOrEmpty(includedSourceCodes.Multimedia.Query) &&
                                  string.IsNullOrEmpty(excludedSourceCodes.Multimedia.Query)))
            {
                sb.Append(GetFormattedQuery(sb.ToString(), MULTIMEDIA_FORMAT));
            }

            return CreateBSSSourceSearchString(sb.ToString());
        }

        private static SourceCodes GetSourceCodes(IEnumerable<Source> sources, bool isExcluded)
        {
            var sourceCodes = new SourceCodes();
            foreach (Source source in sources)
            {
                string code = source.SourceCode ?? source.GroupCode;

                switch (source.Type)
                {
                    case SourceType.Publication:
                        sourceCodes.Pubication.Codes.Add(code);
                        break;
                    case SourceType.Website:
                        sourceCodes.WebPage.Codes.Add(code);
                        break;
                    case SourceType.Blogs:
                        sourceCodes.Blogs.Codes.Add(code);
                        break;
                    case SourceType.Picture:
                        sourceCodes.Picture.Codes.Add(code);
                        break;
                    case SourceType.Multimedia:
                        sourceCodes.Multimedia.Codes.Add(code);
                        break;
                }
            }

            string querySeparator = isExcluded ? "not" : "and";

            if (sourceCodes.Pubication.Codes.Count > 0)
            {
                sourceCodes.Pubication.Query = GetRSTQuery(sourceCodes.Pubication.Codes, querySeparator);
            }
            if (sourceCodes.WebPage.Codes.Count > 0)
            {
                sourceCodes.WebPage.Query = GetRSTQuery(sourceCodes.WebPage.Codes, querySeparator);
            }
            if (sourceCodes.Blogs.Codes.Count > 0)
            {
                sourceCodes.Blogs.Query = GetRSTQuery(sourceCodes.Blogs.Codes, querySeparator);
            }
            if (sourceCodes.Picture.Codes.Count > 0)
            {
                sourceCodes.Picture.Query = GetRSTQuery(sourceCodes.Picture.Codes, querySeparator);
            }
            if (sourceCodes.Multimedia.Codes.Count > 0)
            {
                sourceCodes.Multimedia.Query = GetRSTQuery(sourceCodes.Multimedia.Codes, querySeparator);
            }

            return sourceCodes;
        }

        private static string GetRSTQuery(List<string> codes, string querySeparator)
        {
            string format = codes.Count > 1 ? "{0} rst=({1})" : "{0} rst={1}";
            return string.Format(format, querySeparator, string.Join(" or ", codes.ToArray()));
        }

        //Adding Sources to Query

        private static string GetSourceRestrictorQuery(SourceCodes includedSourceCodes,
                                                       SourceCodes excludedSourceCodes)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(includedSourceCodes.Pubication.Query) ||
                !string.IsNullOrEmpty(excludedSourceCodes.Pubication.Query))
            {
                sb.AppendFormat(GetFormattedQuery(sb.ToString(),
                                                  GetRSTQueryWithFMT(PUBLICATION_FORMAT,
                                                                     includedSourceCodes.Pubication.Query,
                                                                     excludedSourceCodes.Pubication.Query)));
            }
            if (!string.IsNullOrEmpty(includedSourceCodes.WebPage.Query) ||
                !string.IsNullOrEmpty(excludedSourceCodes.WebPage.Query))
            {
                sb.AppendFormat(GetFormattedQuery(sb.ToString(),
                                                  GetRSTQueryWithFMT(WEBPAGE_FORMAT, includedSourceCodes.WebPage.Query,
                                                                     excludedSourceCodes.WebPage.Query)));
            }
            if (!string.IsNullOrEmpty(includedSourceCodes.Blogs.Query) ||
                !string.IsNullOrEmpty(excludedSourceCodes.Blogs.Query))
            {
                sb.AppendFormat(GetFormattedQuery(sb.ToString(),
                                                  GetRSTQueryWithFMT(BLOG_FORMAT, includedSourceCodes.Blogs.Query,
                                                                     excludedSourceCodes.Blogs.Query)));
            }
            if (!string.IsNullOrEmpty(includedSourceCodes.Picture.Query) ||
                !string.IsNullOrEmpty(excludedSourceCodes.Picture.Query))
            {
                sb.AppendFormat(GetFormattedQuery(sb.ToString(),
                                                  GetRSTQueryWithFMT(PICTURE_FORMAT, includedSourceCodes.Picture.Query,
                                                                     excludedSourceCodes.Picture.Query)));
            }
            if (!string.IsNullOrEmpty(includedSourceCodes.Multimedia.Query) ||
                !string.IsNullOrEmpty(excludedSourceCodes.Multimedia.Query))
            {
                sb.AppendFormat(GetFormattedQuery(sb.ToString(),
                                                  GetRSTQueryWithFMT(MULTIMEDIA_FORMAT,
                                                                     includedSourceCodes.Multimedia.Query,
                                                                     excludedSourceCodes.Multimedia.Query)));
            }


            return sb.ToString();
        }

        private static string GetFormattedQuery(string query, string formatQuery)
        {
            return !string.IsNullOrEmpty(query)
                       ? string.Format(" or ({0})", formatQuery)
                       : string.Format("({0})", formatQuery);
        }

        public static SearchString CreateBSSSourceSearchString(string strValue)
        {
            SearchString searchString = null;
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = string.Format("({0})", strValue);
                searchString = new SearchString { Mode = SearchMode.Traditional, Id = "BSSSource", Value = strValue };
            }
            return searchString;
        }

        private static string GetRSTQueryWithFMT(string formatQuery, string includedRSTQuery, string exlucdedRSTQuery)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}", formatQuery);
            if (!string.IsNullOrEmpty(includedRSTQuery))
            {
                sb.AppendFormat(" {0}", includedRSTQuery);
            }
            if (!string.IsNullOrEmpty(exlucdedRSTQuery))
            {
                sb.AppendFormat(" {0}", exlucdedRSTQuery);
            }
            return sb.ToString();
        }

        private static MetadataSearchMode MapSearchModetoMetadataSearchMode(SearchMode searchMode)
        {
            switch (searchMode)
            {
                case SearchMode.All:
                    return MetadataSearchMode.All;
                case SearchMode.Any:
                    return MetadataSearchMode.Any;
                case SearchMode.None:
                    return MetadataSearchMode.None;
                case SearchMode.Traditional:
                    return MetadataSearchMode.Traditional;
                case SearchMode.Phrase:
                    return MetadataSearchMode.Contains;
                default:
                    return MetadataSearchMode.Any;
            }
        }
    }
}