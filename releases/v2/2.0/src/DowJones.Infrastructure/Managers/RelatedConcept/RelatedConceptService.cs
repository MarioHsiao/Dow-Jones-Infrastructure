using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using DowJones.Extensions;
using DowJones.Preferences;
using DowJones.Session;
using Factiva.Gateway.Messages.Semantic.V1_0;
using Factiva.Gateway.Services.V1_0;

namespace DowJones.Managers.RelatedConcept
{
    public interface IRelatedConceptService
    {
        ConceptSearchResult PerformConceptSearch(string simpleSearchText, int maxTerms);
    }

    public class RelatedConceptService : IRelatedConceptService
    {
        private const int MaxTerm = 10;
        private const float Threshold = 0.45f;
        private const string RequestVersion = "1.1.0";
        private readonly IControlData _controlData;
        private readonly IPreferences _preferences;      

        public RelatedConceptService(IControlData controlData, IPreferences preferences)
        {
            _controlData = controlData;
            _preferences = preferences;
        }

        public ConceptSearchResult PerformConceptSearch(string simpleSearchText, int maxTerms = MaxTerm)
        {
            var request = new ConceptSearchRequest
                              {
                                  category = Category.category1,
                                  maxTerms = maxTerms,
                                  threshold = Threshold,
                                  thresholdSpecified = true,
                                  version = RequestVersion,
                                  language = _preferences.InterfaceLanguage,
                                  query = simpleSearchText
                              };
            return PerformConceptSearch(request);
        }

        private ConceptSearchResult PerformConceptSearch(ConceptSearchRequest conceptSearchRequest)
        {
            var searchResult = new ConceptSearchResult();
            var request = new PerformConceptSearchRequest { Data = GetRequestXml(conceptSearchRequest) };
            var serviceResponse = SemanticService.PerformConceptSearch(ControlDataManager.Convert(this._controlData), request);
            var response = serviceResponse.GetObject<PerformConceptSearchResponse>();
            
            if (response != null && !string.IsNullOrEmpty(response.Data))
            {
                searchResult = GetResponse(response.Data);
            }
            /* temp solution until backend resolve the issue */
            if (searchResult != null && searchResult.conceptSearchResultInfo != null )
            {
                if (searchResult.conceptSearchResultInfo.Length > conceptSearchRequest.maxTerms)
                {
                    searchResult.conceptSearchResultInfo = searchResult.conceptSearchResultInfo.Slice(0,
                                                                                                      conceptSearchRequest
                                                                                                          .maxTerms);
                }
            }
            return searchResult;
        }      

        private static string GetRequestXml(ConceptSearchRequest structuredSearch)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var ser = new XmlSerializer(structuredSearch.GetType());
                ser.Serialize(sw, structuredSearch);
            }
            sb.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
            return sb.ToString();
        }

        private static ConceptSearchResult GetResponse(string data)
        {
            var ser = new XmlSerializer(typeof(ConceptSearchResult));
            var result = (ConceptSearchResult)ser.Deserialize(new XmlTextReader(new StringReader(data)));
            return result;
        }
    }

    public class ConceptSpaceDto
    {
        public ConceptSpaceDto(string category, string code, string description)
        {
            this.Category = category;
            this.Code = code;
            this.Description = description;
        }

        public string Category { get; private set; }

        public string Code { get; private set; }

        public string Description { get; private set; }
    }

    public static class ContentSpaceDtoMapper
    {
        private static readonly Dictionary<String, ConceptSpaceDto> list = new Dictionary<String, ConceptSpaceDto>();

        static ContentSpaceDtoMapper()
        {
            list.Add(Category.category1.ToString(), new ConceptSpaceDto(string.Empty, string.Empty, "General"));
            list.Add(Category.category9.ToString(), new ConceptSpaceDto("rst", "IACC", "Accounting/Consulting"));
            list.Add(Category.category7.ToString(), new ConceptSpaceDto("rst", "IADV", "Advertising/Public Relations/Marketing"));
            list.Add(Category.category10.ToString(), new ConceptSpaceDto("rst", "I0", "Agriculture/Forestry"));
            list.Add(Category.category11.ToString(), new ConceptSpaceDto("rst", "IAUT", "Automobiles"));
            list.Add(Category.category4.ToString(), new ConceptSpaceDto("rst", "IBNK", "Banking/Credit"));
            list.Add(Category.category2.ToString(), new ConceptSpaceDto("rst", "i3302", "Computers"));
            list.Add(Category.category12.ToString(), new ConceptSpaceDto("rst", "ICRE", "Construction/Real Estate"));
            list.Add(Category.category6.ToString(), new ConceptSpaceDto("rst", "ICNP", "Consumer Products"));
            list.Add(Category.category13.ToString(), new ConceptSpaceDto("rst", "I1", "Energy"));
            list.Add(Category.category5.ToString(), new ConceptSpaceDto("rst", "I951", "Health Care"));
            list.Add(Category.category14.ToString(), new ConceptSpaceDto("rst", "I82", "Insurance"));
            list.Add(Category.category15.ToString(), new ConceptSpaceDto("rst", "IINV", "Investing/Securities"));
            list.Add(Category.category16.ToString(), new ConceptSpaceDto("rst", "IMED", "Media"));
            list.Add(Category.category3.ToString(), new ConceptSpaceDto("rst", "i257", "Pharmaceuticals"));
            list.Add(Category.category8.ToString(), new ConceptSpaceDto("rst", "I7902", "Telecommunications"));
        }


        public static ListItemCollection GetListItemCollection
        {
            get
            {
                var collection = new ListItemCollection();
                foreach (var pair in list)
                {
                    collection.Add(new ListItem(pair.Value.Description, pair.Key));
                }
                return collection;
            }
        }

        public static string GetIndustryCode(string cat)
        {
            return list[cat].Code;
        }
    }

    #region THIS WILL BE IN GATEWAY 

    /// <remarks/>
    [GeneratedCode("System.Xml", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://types.dj.net/searchassist")]
    public class ConceptSearchResult
    {
        /// <remarks/>
        [XmlElement("conceptSearchResultInfo")]
        public ConceptSearchResultInfo[] conceptSearchResultInfo { get; set; }

        /// <remarks/>
        public string reserved { get; set; }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://types.dj.net/searchassist")]
    public class ConceptSearchResultInfo
    {
        /// <remarks/>
        public string term { get; set; }

        /// <remarks/>
        public float score { get; set; }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://types.dj.net/searchassist")]
    public class ConceptSearchRequest
    {
        /// <remarks/>
        public string query { get; set; }

        /// <remarks/>
        public Category category { get; set; }

        /// <remarks/>
        public int maxTerms { get; set; }

        /// <remarks/>
        public string language { get; set; }

        /// <remarks/>
        public float threshold { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool thresholdSpecified { get; set; }

        /// <remarks/>
        public string version { get; set; }
    }

    #region Category enum

    /// <remarks/>
    [GeneratedCode("System.Xml", "4.0.30319.1")]
    [Serializable]
    [XmlType(Namespace = "http://types.dj.net/searchassist")]
    public enum Category
    {
        category1,
        category2,
        category3,
        category4,
        category5,
        category6,
        category7,
        category8,
        category9,
        category10,
        category11,
        category12,
        category13,
        category14,
        category15,
        category16,
        category17,
        category18,
        category19,
        category20,
        category21,
        category22,
        category23,
        category24,
        category25,
        category26,
        category27,
        category28,
        category29,
        category30,
        category31,
        category32,
        category33,
        category34,
        category35,
        category36,
        category37,
        category38,
        category39,
        category40,
        category41,
        category42,
        category43,
        category44,
        category45,
        category46,
        category47,
        category48,
        category49,
        category50
    }

    #endregion

    #endregion
}
