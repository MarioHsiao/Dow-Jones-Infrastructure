using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.QueryUtility
{
    #region Global namespace declaration struct
    public struct Declarations
    {
        public const string SchemaVersion = "";
    }
    #endregion

    #region RuleOperator enum
    [Serializable]
    public enum RuleOperator
    {
        [XmlEnum("w")]
        Within        
    }
    #endregion

    #region FilterType enum
    [Serializable]
    public enum FilterType
    {
        [XmlEnum("DJOrganizationCodeFilter")]
        DJOrganizationCodeFilter,

        [XmlEnum("SearchSectionFilter")]
        SearchSectionFilter,

        [XmlEnum("FreeTextFilter")]
        FreeTextFilter,

        [XmlEnum("NewsSubjectCodeFilter")]
        NewsSubjectCodeFilter,

        [XmlEnum("IndustryCodeFilter")]
        IndustryCodeFilter,

        [XmlEnum("RegionCodeFilter")]
        RegionCodeFilter,

        [XmlEnum("AuthorCodeFilter")]
        AuthorCodeFilter,

        [XmlEnum("SourcesFilter")]
        SourcesFilter,

        [XmlEnum("DaysFilter")]
        DaysFilter,

        [XmlEnum("ContentLanguageFilter")]
        ContentLanguageFilter,

        [XmlEnum("DJPersonCodeFilter")]
        DJPersonCodeFilter,

        [XmlEnum("CompanyListFilter")]
        CompanyListFilter,

        [XmlEnum("CompanyScreeningFilter")]
        CompanyScreeningFilter,

        [XmlEnum("ExecutiveScreeningFilter")]
        ExecutiveScreeningFilter,

        [XmlEnum("DateFilter")]
        DateFilter,

        [XmlEnum("ExecutiveListFilter")]
        ExecutiveListFilter,
    }
    #endregion


    #region Error definition
    [Serializable]
    public class ErrorCollection : List<Error> { }

    [XmlType(TypeName = "error", Namespace = Declarations.SchemaVersion), Serializable]
    public class Error
    {
        [XmlElement("queryId")]
        public long QueryId { get; set; }

        [XmlElement("customId")]
        public string CustomId { get; set; }

        [XmlElement("filter")]
        public string FilterType { get; set; }

        [XmlElement("code")]
        public long Code { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }
    }
    #endregion

    #region Query Manager Rule Definition
       
    [Serializable]
    [XmlInclude(typeof(ProximityRule))]
    [XmlInclude(typeof(ScreeningRule))]
    [XmlInclude(typeof(ListRule))]
    [XmlRoot("rule", Namespace = Declarations.SchemaVersion)]
    [XmlType(TypeName = "QueryManagerRule", Namespace = Declarations.SchemaVersion)]
    public abstract class QueryManagerRule
    {
        private List<FilterType> _filterTypes;

        [XmlArray(ElementName = "filterTypes", IsNullable = true)]
        [XmlArrayItem("filterType")]
        public List<FilterType> FilterTypes 
        {
            get
            {
                if (this._filterTypes == null) this._filterTypes = new List<FilterType>();
                return this._filterTypes;
                
            }
            set
            {
                this._filterTypes = value;
            }
        }
    }

    [Serializable]
    [XmlType(TypeName = "ProximityRule", Namespace = Declarations.SchemaVersion)]
    public class ProximityRule : QueryManagerRule
    {
        [XmlElement("ruleOperator")]
        public RuleOperator RuleOperator { get; set; }

        [XmlElement("value")]
        public int Value { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "ScreeningRule", Namespace = Declarations.SchemaVersion)]
    public class ScreeningRule : QueryManagerRule
    {
        [XmlElement("maxValueToProcess")]
        public int MaxValueToProcess { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "ListRule", Namespace = Declarations.SchemaVersion)]
    public class ListRule : QueryManagerRule
    {
        [XmlElement("maxValueToProcess")]
        public int MaxValueToProcess { get; set; }
    }
    #endregion

    #region QueryManager

    [Serializable]
    [XmlRoot(Namespace = Declarations.SchemaVersion, IsNullable = false)]
    public class QueryManagerRequest
    {
        public QueryManagerRequest()
        {
            QueryRequests = new List<QueryRequest>();
            CombineState = new CombineState();
        }

        [XmlArray(ElementName = "queryRequests", IsNullable = true)]
        [XmlArrayItem("queryRequest")]
        public List<QueryRequest> QueryRequests { get; set; }

        [XmlElement(Type = typeof(CombineState), ElementName = "combineState", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        public CombineState CombineState { get; set; }
    }

    [Serializable]
    [XmlRoot(Namespace = Declarations.SchemaVersion, IsNullable = false)]
    public class QueryRequest
    {
        public QueryRequest()
        {
            Rules = new List<QueryManagerRule>();
            AdditionalFilters = new List<QueryFilter>();
        }
        [XmlElement("queryId")]
        public long QueryId { get; set; }

        [XmlElement("query")]
        public Query Query { get; set; }

        [XmlElement("customId")]
        public string CustomId { get; set; }

        [XmlArray(ElementName = "additionalFilters", IsNullable = true)]
        [XmlArrayItem("filter")]
        public List<QueryFilter> AdditionalFilters { get; set; }

        [XmlArray(ElementName = "rules", IsNullable = true)]
        [XmlArrayItem("queryManagerRule")]
        public List<QueryManagerRule> Rules { get; set; }

    }

    [Serializable]
    [XmlRoot(Namespace = Declarations.SchemaVersion, IsNullable = false)]
    public class QueryManagerResponse
    {
        public QueryManagerResponse()
        {
            QueryResponses = new List<QueryResponse>();
        }

        [XmlArray(ElementName = "queryResponses", IsNullable = true)]
        [XmlArrayItem("queryResponse")]
        public List<QueryResponse> QueryResponses { get; set; }
    }

    [Serializable]
    [XmlRoot(Namespace = Declarations.SchemaVersion, IsNullable = false)]
    public class QueryResponse
    {
        public QueryResponse()
        {
            StructuredSearch = new StructuredSearch();
            Errors = new ErrorCollection();
            ReturnCode = 0;
        }

        [XmlElement("queryId")]
        public long QueryId { get; set; }

        [XmlElement("customId")]
        public string CustomId { get; set; }

        [XmlElement(Type = typeof(StructuredSearch), ElementName = "structuredSearch", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        public StructuredSearch StructuredSearch { get; set; }

        [XmlElement("returnCode")]
        public long ReturnCode { get; set; }

        [XmlElement(Type = typeof(Error), ElementName = "errors", IsNullable = true, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        public ErrorCollection Errors { get; set; }
    }
    #endregion

    #region Combine State

    [Serializable]
    public enum CombineOperator
    {
        [XmlEnum("And")]
        And,
        [XmlEnum("Or")]
        Or
    }
    
    [Serializable]
    [XmlRoot(Namespace = Declarations.SchemaVersion, IsNullable = false)]
    public class CombineState
    {
        [XmlElement("enabled")]
        public bool Enabled { get; set; }

        [XmlElement("operator")]
        public CombineOperator CombineOperator { get; set; }

        [XmlElement("dateFilter")]
        public DateFilter DateFilter { get; set; }

        [XmlElement("daysFilter")]
        public DaysFilter DaysFilter { get; set; }
    }
    #endregion
}
