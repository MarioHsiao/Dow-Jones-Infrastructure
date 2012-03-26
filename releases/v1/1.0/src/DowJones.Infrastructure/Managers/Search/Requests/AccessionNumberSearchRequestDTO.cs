using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Search.Controller;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Managers.Search.Requests
{
    public enum SortBy
    {
        [XmlEnum(Name = "FIFO")]
        FIFO,
        [XmlEnum(Name = "LIFO")]
        LIFO,
        [XmlEnum(Name = "PublicationDateChronological")]
        PublicationDateChronological,
        [XmlEnum(Name = "PublicationDateReverseChronological")]
        PublicationDateReverseChronological
    }


    /// <summary>
    /// </summary>
    public class AccessionNumberSearchRequestDTO : IRequestDTO, ISearchRequest
    {
        internal const int MaxReturnableResults = 100;
        internal string[] UniqueAccessionNumbers;

        private readonly List<string> accessionNumbersList = new List<string>();
        private string[] accessionNumbers;
        private ClusterMode clusterMode = ClusterMode.Off;
        private DescriptorControl descriptorControl;
        private MetaDataController metaDataController;
        private SearchCollectionCollection searchCollectionCollection;
        private SortBy sortBy = SortBy.LIFO;
        private bool useAllDates;

        public SearchCollectionCollection SearchCollectionCollection
        {
            get
            {
                if (searchCollectionCollection == null)
                {
                    searchCollectionCollection = new SearchCollectionCollection();
                    searchCollectionCollection.AddRange(
                        new[]
                            {
                                SearchCollection.Publications,
                                SearchCollection.WebSites,
                                SearchCollection.Pictures,
                                SearchCollection.Multimedia,
                            });
                }
                return searchCollectionCollection;
            }
            set { searchCollectionCollection = value; }
        }

        public MetaDataController MetaDataController
        {
            get { return metaDataController ?? (metaDataController = new MetaDataController()); }
            set { metaDataController = value; }
        }

        public ClusterMode ClusterMode
        {
            get { return clusterMode; }
            set { clusterMode = value; }
        }

        public string[] AccessionNumbers
        {
            get { return accessionNumbers; }
            set
            {
                accessionNumbers = value;
                foreach (string t in accessionNumbers)
                {
                    if (!accessionNumbersList.Contains(t))
                        accessionNumbersList.Add(t);
                }
            }
        }

        public SortBy SortBy
        {
            get { return sortBy; }
            set { sortBy = value; }
        }

        public DescriptorControl DescriptorControl
        {
            get { return descriptorControl ?? (descriptorControl = new DescriptorControl()); }
            set { descriptorControl = value; }
        }

        internal string[] GetUniqueAccessionNumbers
        {
            get { return accessionNumbersList.ToArray(); }
        }

        internal bool UseAllDates
        {
            get { return useAllDates; }
            set { useAllDates = value; }
        }

        #region IRequestDTO Members

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            return GetUniqueAccessionNumbers != null && GetUniqueAccessionNumbers.Length != 0;
        }

        #endregion

        #region ISearchRequest Members

        public IPerformContentSearchRequest GetPerformContentSearchRequest<T>() where T : IPerformContentSearchRequest, new()
        {

            if (!IsValid())
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SearchManagerInvalidDto);
            }

            var request = new T
                              {
                                  FirstResult = 0, 
                                  MaxResults = MaxReturnableResults
                              };

            if (metaDataController != null)
            {
                request.NavigationControl = SearchUtility.GenerateNavigationControl(metaDataController);
            }
            if (descriptorControl != null)
            {
                request.DescriptorControl = descriptorControl;
            }

            request.StructuredSearch = GetStructuredQuery(this);
            return request;
        }

        #endregion

        private StructuredSearch GetStructuredQuery(AccessionNumberSearchRequestDTO dto)
        {
            var structuredSearch = new StructuredSearch {Query = {SearchCollectionCollection = dto.SearchCollectionCollection}};
            structuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(SearchUtility.ScopeType.AnyAccessionNumber, dto.GetUniqueAccessionNumbers));
            structuredSearch.Formatting.SortOrder = ResultSortOrder.PublicationDateReverseChronological;
            structuredSearch.Formatting.SnippetType = SnippetType.Fixed;
            structuredSearch.Formatting.ClusterMode = dto.clusterMode;
            structuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Off;
            structuredSearch.Formatting.SnippetType = SnippetType.Fixed;
            structuredSearch.Formatting.MarkupType = MarkupType.None;

            if (useAllDates)
            {
                structuredSearch.Query.Dates.All = true;
            }
            return structuredSearch;
        }
    }
}