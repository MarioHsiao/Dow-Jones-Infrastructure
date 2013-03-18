using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Managers.Abstract;
using DowJones.Search.Controller;
using DowJones.Search.Core;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.Requests
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
    [XmlRoot(ElementName = "AccessionNumberSearchRequestDTO", Namespace = Declarations.SchemaVersion), Serializable]
    public class AccessionNumberSearchRequestDTO : IRequestDTO, ISearchRequest
    {
        internal const int MaxReturnableResults = 100;
        internal string[] UniqueAccessionNumbers;

        private readonly List<string> accessionNumbersList = new List<string>();
        private string[] accessionNumbers;
        
        [XmlElement(ElementName = "clusterMode", Namespace = Declarations.SchemaVersion, IsNullable = false, Form = XmlSchemaForm.Qualified)]
        public ClusterMode __clusterMode = ClusterMode.Off;

        [XmlElement(Type = typeof(DescriptorControl), ElementName = "descriptorControl", Namespace = Declarations.SchemaVersion, IsNullable = false, Form = XmlSchemaForm.Qualified)]
        public DescriptorControl __descriptorControl;

        [XmlElement(Type = typeof(MetaDataController), ElementName = "metaDataController", Namespace = Declarations.SchemaVersion, IsNullable = false, Form = XmlSchemaForm.Qualified)]
        public MetaDataController __metaDataController;
        
        [XmlElement(Type = typeof(SearchCollection), ElementName = "collection", Namespace = Declarations.SchemaVersion, IsNullable = false, Form = XmlSchemaForm.Qualified)]
        public SearchCollectionCollection __searchCollectionCollection;

        [XmlElement(ElementName = "sortBy", Namespace = Declarations.SchemaVersion, IsNullable = false, Form = XmlSchemaForm.Qualified)]
        public SortBy __sortBy = SortBy.LIFO;
        //private bool useAllDates;

        [XmlIgnore]
        public SearchCollectionCollection SearchCollectionCollection
        {
            get
            {
                if (__searchCollectionCollection == null)
                {
                    __searchCollectionCollection = new SearchCollectionCollection();
                    __searchCollectionCollection.AddRange(
                        new[]
                            {
                                SearchCollection.Publications,
                                SearchCollection.WebSites,
                                SearchCollection.Pictures,
                                SearchCollection.Multimedia,
                            });
                }
                return __searchCollectionCollection;
            }
            set { __searchCollectionCollection = value; }
        }

        [XmlIgnore]
        public MetaDataController MetaDataController
        {
            get { return __metaDataController ?? (__metaDataController = new MetaDataController()); }
            set { __metaDataController = value; }
        }

        [XmlIgnore]
        public ClusterMode ClusterMode
        {
            get { return __clusterMode; }
            set { __clusterMode = value; }
        }

        [XmlElement(ElementName = "accessionNumbers", Type = typeof(string[]), Namespace = Declarations.SchemaVersion)]
        public string[] AccessionNumbers
        {
            get { return accessionNumbers; }
            set
            {
                accessionNumbers = value;
                accessionNumbersList.AddRangeUnique(value);
            }
        }

        [XmlIgnore]
        public SortBy SortBy
        {
            get { return __sortBy; }
            set { __sortBy = value; }
        }

        [XmlIgnore]
        public DescriptorControl DescriptorControl
        {
            get { return __descriptorControl ?? (__descriptorControl = new DescriptorControl()); }
            set { __descriptorControl = value; }
        }

        internal string[] GetUniqueAccessionNumbers
        {
            get { return accessionNumbersList.ToArray(); }
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

            if (__metaDataController != null)
            {
                request.NavigationControl = SearchUtility.GenerateNavigationControl(__metaDataController);
            }

            if (__descriptorControl != null)
            {
                request.DescriptorControl = __descriptorControl;
            }


            request.StructuredSearch = GetStructuredQuery(this);
            return request;
        }

        #endregion

        private static ResultSortOrder Map(SortBy sortBy)
        {
            switch (sortBy)
            {
                case SortBy.FIFO:
                    return ResultSortOrder.FIFO;
                case SortBy.LIFO:
                    return ResultSortOrder.LIFO;
                case SortBy.PublicationDateChronological:
                    return ResultSortOrder.PublicationDateChronological;
                default:
                    return ResultSortOrder.PublicationDateReverseChronological;
            }
        }

        private StructuredSearch GetStructuredQuery(AccessionNumberSearchRequestDTO dto)
        {
            var structuredSearch = new StructuredSearch {Query = {SearchCollectionCollection = dto.SearchCollectionCollection}};
            
            //structuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(SearchUtility.ScopeType.AnyAccessionNumber, dto.GetUniqueAccessionNumbers));
            structuredSearch.Query.AccessionNumberFilter.AccessionNumberCollection.AddRange( dto.accessionNumbersList );
            structuredSearch.Formatting.SortOrder = Map(dto.SortBy);
            structuredSearch.Formatting.SnippetType = SnippetType.Fixed;
            structuredSearch.Formatting.ClusterMode = dto.__clusterMode;
            structuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Off;
            structuredSearch.Formatting.SnippetType = SnippetType.Fixed;
            structuredSearch.Formatting.MarkupType = MarkupType.None;
            ////explicitly set for all dates???
            structuredSearch.Query.Dates = new Dates { All = true };
            return structuredSearch;
        }
    }
}