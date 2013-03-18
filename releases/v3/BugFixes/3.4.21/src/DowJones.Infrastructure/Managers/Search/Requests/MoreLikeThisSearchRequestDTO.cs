// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoreLikeThisSearchRequestDTO.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DowJones.Managers.Abstract;
using DowJones.Managers.Search.Base;
using DowJones.Search.Controller;
using DowJones.Search.Core;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.Requests
{
    public class MoreLikeThisSearchRequestDTO : IRequestDTO, ISearchRequest
    {
        #region Constants

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")] internal const int DEFAULT_RETURNABLE_RESULTS = 5;

        #endregion

        #region Private Variables

        private List<string> _contentLanguages = new List<string>(new[] {"en"});
        private DeduplicationMode _deduplicationMode = DeduplicationMode.NearExact;
        private string _documentVector;
        private int _maxResults = DEFAULT_RETURNABLE_RESULTS;
        private List<string> _restrictors;
        private List<SearchCollection> _searchCollection = new List<SearchCollection>();
        private bool _useAllDates;
        private SearchMode _filterQueryStrSearchMode = SearchMode.Traditional;
        private List<FIISearchList> _fiiSearchLists;
        private ResultSortOrder _resultSortOrder = ResultSortOrder.RelevanceHighFreshness;

        #endregion

        #region Properties

        public ResultSortOrder ResultSortOrder
        {
            get
            {
                return _resultSortOrder;
            }
            set
            {
                _resultSortOrder = value;
            }
        }

        public int MaxResults
        {
            get { return _maxResults; }
            set { _maxResults = value; }
        }

        public DeduplicationMode DeduplicationMode
        {
            get { return _deduplicationMode; }
            set { _deduplicationMode = value; }
        }

        public List<SearchCollection> SearchCollectionCollection
        {
            get { return _searchCollection; }
            set { _searchCollection = value; }
        }

        public DescriptorControl DescriptorControl { get; set; }

        public string DocumentVector
        {
            get { return _documentVector; }
            set { _documentVector = value; }
        }

        public MetaDataController MetaDataController { get; set; }

        public string AccessionNumber { get; set; }

        public List<string> ContentLanguages
        {
            get { return _contentLanguages; }
            set { _contentLanguages = value; }
        }

        public List<string> Restrictors
        {
            get
            {
                if (_restrictors == null)
                    _restrictors = new List<string>();
                return _restrictors;
            }
            set { _restrictors = value; }
        }

        public bool UseAllDates
        {
            get { return _useAllDates; }
            set { _useAllDates = value; }
        }

        public List<FIISearchList> FIISearchLists
        {
            get
            {
                if (_fiiSearchLists == null)
                    _fiiSearchLists = new List<FIISearchList>();
                return _fiiSearchLists;
            }
            set { _fiiSearchLists = value; }
        }

        public string FilterQueryStr { get; set; }

        public SearchMode FilterQueryStrSearchMode
        {
            get { return _filterQueryStrSearchMode; }
            set { _filterQueryStrSearchMode = value; }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #region Internal Methods

        internal bool HasDocumentVector
        {
            get
            {
                if (IsValid())
                {
                    if (string.IsNullOrEmpty(_documentVector) || string.IsNullOrEmpty(_documentVector.Trim()))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region Contructors

        #endregion

        #region IRequestDTO Members

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            return true;
        }

        #endregion

        #region ISearchRequest Members

        public IPerformContentSearchRequest GetPerformContentSearchRequest<T>() where T : IPerformContentSearchRequest, new()
        {
            if (_searchCollection == null || _searchCollection.Count == 0)
            {
                _searchCollection = new List<SearchCollection>(new[]
                                                                   {
                                                                       SearchCollection.Publications
                                                                   });
            }
            if (IsValid())
            {
                var request = new T
                                  {
                                      FirstResult = 0,
                                      MaxResults = MaxResults
                                  };

                request.StructuredSearch.Formatting.SnippetType = SnippetType.Fixed;
                request.StructuredSearch.Formatting.SortOrder = ResultSortOrder;
                request.StructuredSearch.Formatting.ClusterMode = ClusterMode.Off;
                request.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode;

                request.StructuredSearch.Query.SearchCollectionCollection.AddRange(_searchCollection.ToArray());
                request.StructuredSearch.Query.SimilarityFilter.Type = SimilarityType.Refine;
                request.StructuredSearch.Query.SimilarityFilter.Value = _documentVector;
                request.StructuredSearch.Query.SimilarityFilter.SortBy = true;
                request.StructuredSearch.Formatting.DeduplicationMode = _deduplicationMode;


                request.StructuredSearch.Query.Dates.Format = DateFormat.MMDDCCYY;
                if (_useAllDates)
                {
                    request.StructuredSearch.Query.Dates.All = true;
                }

                if (MetaDataController != null)
                {
                    request.NavigationControl = SearchUtility.GenerateNavigationControl(MetaDataController);
                }

                if (DescriptorControl != null)
                {
                    request.DescriptorControl = DescriptorControl;
                }

                if (ContentLanguages != null && ContentLanguages.Count > 0)
                {
                    request.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(SearchUtility.ScopeType.AnyLanguages, ContentLanguages.ToArray()));
                }

                if (Restrictors != null && Restrictors.Count > 0)
                {
                    request.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(SearchUtility.ScopeType.ExcludedRestrictors, Restrictors.ToArray()));
                }

                if (FIISearchLists != null && FIISearchLists.Count > 0)
                {
                    foreach (var list in FIISearchLists)
                    {
                        request.StructuredSearch.Query.SearchStringCollection.AddRange(list.GenerateSearchString().ToArray());
                    }
                }

                if (!string.IsNullOrEmpty(FilterQueryStr))
                {
                    var searchString = new SearchString
                                           {
                                               Id = "FilterQuery",
                                               Mode = FilterQueryStrSearchMode,
                                               Type = SearchType.Free,
                                               Value = FilterQueryStr,
                                               Validate = false,
                                           };
                    request.StructuredSearch.Query.SearchStringCollection.Add(searchString);
                }

                return request;
            }

            return null;
        }

        #endregion
    }
}