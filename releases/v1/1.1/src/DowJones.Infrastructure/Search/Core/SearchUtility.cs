// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchUtility.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Utilities.Search.Attributes;
using DowJones.Utilities.Search.Controller;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Search.Core
{
    /// <summary>
    /// The search utility.
    /// </summary>
    internal class SearchUtility
    {
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
    }
}