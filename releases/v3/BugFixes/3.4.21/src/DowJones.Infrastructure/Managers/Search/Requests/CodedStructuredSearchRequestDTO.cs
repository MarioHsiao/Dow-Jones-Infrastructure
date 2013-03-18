using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Managers.Abstract;
using DowJones.Managers.Search.CodedNewsQueries;
using DowJones.Managers.Search.CodedNewsQueries.Government;
using DowJones.Search.Controller;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.Requests
{

    [XmlRoot("codedStructuredSearchRequest", IsNullable = false)]
    public class CodedStructuredSearchRequest : CodedNewsSearchRequest, IRequestDTO
    {
        /// <summary>
        /// This fields is a replacement for filterFragment, going forward we should be using this field.
        /// </summary>
        [XmlElement("newsFilters")]
        public NewsFiltersExtended NewsFilters;

        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool OccurrenceSearch;

        public ResultFormatting Formatting;

        public MetaDataController MetaDataController;

        public DateController DateController;

        public List<string> KeywordBlackList;

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

        public bool IncludeNiweFilter { get; set; }

        #endregion

        public static DeduplicationMode MapDedupType(RemoveDuplicate dedupType)
        {
            switch (dedupType)
            {
                case RemoveDuplicate.High:
                    return DeduplicationMode.NearExact;
                case RemoveDuplicate.Medium:
                    return DeduplicationMode.Similar;
                default:
                    return DeduplicationMode.Off;
            }
        }
    }



    /// <summary>
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("CodedNewsSearch", IsNullable = false)]
    public class CodedNewsSearchRequest
    {
        /// <remarks/>
        public CodedNewsType NewsType;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("companies")]
        public CompanyFilter[] CompanyFilters;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("industries")]
        public Industry[] Industries;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("executives")]
        public ExecutiveFilter[] ExecutiveFilters;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("organization")]
        public Organization[] Organization;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("official")]
        public GovernmentOfficial[] Official;

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int HeadlineStart;

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(20)]
        public int HeadlineCount = 20;

        /// <remarks/>
        public string SearchContext;

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UseAltQueries;

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(1)]
        public int Scope = 1;

        public string FilterFragment;

        public bool ReturnNavSet = true;

        public SearchContentCategory[] ContentCategory;
        
        public int HitCountForExpandQuery = 5;

        public List<string> PreferenceLanguage;

        public SearchStringCollection SearchStringCollection;
        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
        //public RemoveDuplicate RemoveDuplicate = RemoveDuplicate.None;

    }


}