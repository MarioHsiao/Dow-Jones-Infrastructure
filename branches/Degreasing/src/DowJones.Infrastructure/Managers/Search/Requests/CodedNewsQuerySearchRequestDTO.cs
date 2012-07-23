//using System.Collections.Generic;
//using System.Xml.Serialization;
//using EMG.Utility.Exceptions;
//using EMG.Utility.Search.Controller;
//using EMG.Utility.Search.Core;
//using Factiva.Gateway.Messages.Search.V2_0;

//namespace EMG.Utility.Managers.Search.Requests
//{
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("CodedNewsSearch", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CodedNewsQuerySearchRequestDTO : IRequestDTO, ISearchRequest
//    {


//        /// <remarks/>
//        public CodedNewsType newsType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companies")]
//        public Company[] companies;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("industries")]
//        //public Industry[] industries;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("executives")]
//        //public Person[] executives;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("organization")]
//        //public Organization[] organization;


//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("official")]
//        //public GovernmentOfficial[] official;


//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(0)]
//        //public int headlineStart = 0;

//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(20)]
//        //public int headlineCount = 20;

//        ///// <remarks/>
//        //public string searchContext;

//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(TruncationType.Large)]
//        //public TruncationType truncationType = TruncationType.Large;

//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(false)]
//        //public bool useAltQueries = false;

//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(1)]
//        //public int scope = 1;

//        //public string filterFragment;

//        //public bool returnNavSet;

//        //public SearchContentCategory[] contentCategory;

//        //[System.ComponentModel.DefaultValueAttribute(ServerName.Index)]
//        //public ServerName serverName = ServerName.Index;

//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        //public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        #region IRequestDTO Members

//        /// <summary>
//        /// Determines whether this instance is valid.
//        /// </summary>
//        /// <returns>
//        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
//        /// </returns>
//        public bool IsValid()
//        {
//            throw new System.NotImplementedException();
//        }
//        #endregion

//        #region ISearchRequest Members

//        public PerformContentSearchRequest GetPerformContentSearchRequest()
//        {
//            throw new System.NotImplementedException();
//        }
//        #endregion
//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CodedNewsType
//    {

//        /// <remarks/>
//        _InvalidValue,

//        /// <remarks/>
//        IndustryNews,

//        /// <remarks/>
//        IndustryEditorsChoice,

//        /// <remarks/>
//        IndustryReportAll,

//        /// <remarks/>
//        Investext,

//        /// <remarks/>
//        SpSummary,

//        /// <remarks/>
//        MergentReport,

//        /// <remarks/>
//        ForresterResearch,

//        /// <remarks/>
//        FreedoniaSummary,

//        /// <remarks/>
//        IBIS,

//        /// <remarks/>
//        MarketResearch,

//        /// <remarks/>
//        LatestNews,

//        /// <remarks/>
//        ManagementMoves,

//        /// <remarks/>
//        ContractsOrders,

//        /// <remarks/>
//        NewProductsServices,

//        /// <remarks/>
//        LegalJudicial,

//        /// <remarks/>
//        Performance,

//        /// <remarks/>
//        OwnershipChanges,

//        /// <remarks/>
//        MergersAcquisition,

//        /// <remarks/>
//        PressReleases,

//        /// <remarks/>
//        TradeArticles,

//        /// <remarks/>
//        KeyDevAll,

//        /// <remarks/>
//        KeyDevBankruptcy,

//        /// <remarks/>
//        KeyDevManagementChanges,

//        /// <remarks/>
//        KeyDevMAOC,

//        /// <remarks/>
//        KeyDevMarketChanges,

//        /// <remarks/>
//        KeyDevNewFundingCapital,

//        /// <remarks/>
//        KeyDevNewProductsServices,

//        /// <remarks/>
//        KeyDevPerformance,

//        /// <remarks/>
//        KeyDevRegGvtPolicy,

//        /// <remarks/>
//        ReportAll,

//        /// <remarks/>
//        HooversBasicReport,

//        /// <remarks/>
//        HooversInDepthReport,

//        /// <remarks/>
//        DataMonBusinessDescription,

//        /// <remarks/>
//        DataMonCompanyLocations,

//        /// <remarks/>
//        DataMonHistory,

//        /// <remarks/>
//        DataMonKeyEmployees,

//        /// <remarks/>
//        DataMonKeyFacts,

//        /// <remarks/>
//        DataMonMajorProducts,

//        /// <remarks/>
//        DataMonSWOTAnalysis,

//        /// <remarks/>
//        DataMonTopCompetitors,

//        /// <remarks/>
//        DataMonCompanyOverview,

//        /// <remarks/>
//        JobsonsMiningYearBook,

//        /// <remarks/>
//        JobsonsYearBookPublicCompany,

//        /// <remarks/>
//        ExecutiveBusinessNews,

//        /// <remarks/>
//        ExecutiveLatestNews,

//        GovernmentGeneralNews,

//        GovernmentExecutiveNews,

//        GovernmentOpportunitiesContracts,

//        //Added to implement the Company analysis and profiles
//        //page to view more data monitor reports if they exist.
//        CompanyReportDataMon,

//        //Added to implement the Company analysis and profiles
//        //page to view more zachs reports if they exist.
//        CompanyReportZachs,

//        //Added to implement the Industry analysis and profiles
//        //page to view more zachs reports if they exist.
//        IndustryReportZachs,

//        //Added to implement the additional company news subject
//        //added to the dropdown
//        Earnings,

//        //Added to implement the additional company news subject
//        //added to the dropdown
//        CapacitiesFacilities,

//        //Added to implement the additional company news subject
//        //added to the dropdown
//        Bankruptcy,

//        //Added to implement the additional company news subject
//        //added to the dropdown
//        EarningsCalls,

//        // mt 2008 Q3 - add Business Monitor International (EMDN) source
//        BusinessMonitor,

//        //os 2009 Q3 China Industries Report (sc=BCIROS or sc=BCIRDT or sc=BCIREN) 
//        IndustryChinaReport,

//        //os 2009 Q3 China Coal Monthly (sc=BCOLEN ) 
//        ChinaCoal,

//        //os 2009 Q3 Asia Pulse (sc=APULSE  and hd=briefing) 
//        AsiaPulse,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanySymbology : Company
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompanySymbology))]
//    [System.Xml.Serialization.XmlRootAttribute("company", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Company
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        ///// <remarks/>
//        //public string name;

//        ///// <remarks/>
//        //public int newsMentions;

//        //public int newsHits;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("address")]
//        //public string[] address;

//        ///// <remarks/>
//        //public string city;

//        ///// <remarks/>
//        //public string state;

//        ///// <remarks/>
//        //public string zip;

//        ///// <remarks/>
//        //public string country;

//        ///// <remarks/>
//        //public string distance;

//        ///// <remarks/>
//        //public string countryIsoCode;

//        ///// <remarks/>
//        //public string latitude;

//        ///// <remarks/>
//        //public string longitude;

//        ///// <remarks/>
//        //public string geoAccuracy;

//        ///// <remarks/>
//        //public string phone;

//        ///// <remarks/>
//        //public string phoneAreaCode;

//        ///// <remarks/>
//        //public string phoneCountryCode;

//        ///// <remarks/>
//        //public string fax;

//        ///// <remarks/>
//        //public string faxAreaCode;

//        ///// <remarks/>
//        //public string faxCountryCode;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("website")]
//        //public Website[] website;

//        ///// <remarks/>
//        //[System.ComponentModel.DefaultValueAttribute(OwnershipType.Unspecified)]
//        //public OwnershipType ownershipType = OwnershipType.Unspecified;

//        ///// <remarks/>
//        //public string corporateType;

//        ///// <remarks/>
//        //public string currency;

//        ///// <remarks/>
//        //public string locationType;

//        ///// <remarks/>
//        //public string creationDate;

//        ///// <remarks/>
//        //public string language;

//        ///// <remarks/>
//        //public Person contact;

//        ///// <remarks/>
//        //public string businessDescription;

//        ///// <remarks/>
//        //public CompanyPrimaryIndustryClassification primaryIndustryClassification;

//        ///// <remarks/>
//        //public Region region;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("secondaryRegions")]
//        //public Region[] secondaryRegions;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("person")]
//        //public Person[] person;

//        ///// <remarks/>
//        //public InstrumentReference primaryDJInstrument;

//        ///// <remarks/>
//        //public InstrumentReference primaryRicInstrument;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("marketIndices")]
//        //public InstrumentReference[] marketIndices;

//        ///// <remarks/>
//        //public string dunsNumber;

//        ///// <remarks/>
//        //public CompanyStatus companyStatus;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("ultimateParent")]
//        //public CompanyParent[] ultimateParent;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("parent")]
//        //public CompanyParent[] parent;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("secretary")]
//        //public Secretary[] secretary;

//        ///// <remarks/>
//        //public string accessionNo;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlElementAttribute("provider")]
//        //public Provider[] provider;

//        ///// <remarks/>
//        //public FormattedDate publicationDate;

//        ///// <remarks/>
//        //public bool hasCoreRecord;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlIgnoreAttribute()]
//        //public bool hasCoreRecordSpecified;

//        ///// <remarks/>
//        //public string newsSearch;

//        ///// <remarks/>
//        //public FamilyTreeInfo familyTreeInfo;

//        ///// <remarks/>
//        //[System.Xml.Serialization.XmlAttributeAttribute()]
//        //public string rank;

//        //[System.Xml.Serialization.XmlElementAttribute("child")]
//        //public CompanyParent[] child;

//        //public LegalStatus legalStatus;

//        //public string orgCode;


//    }


//}



using System.Collections.Generic;
using System.Xml.Serialization;
using EMG.Utility.Exceptions;
using EMG.Utility.Search.Controller;
using EMG.Utility.Search.Core;
using Factiva.Gateway.Messages.Search.V2_0;

namespace EMG.Utility.Managers.Search.Requests
{

    [XmlType(Namespace = "http://global.factiva.com/fvs/1.0")]
    [XmlRoot("codedStructuredSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
    public class CodedStructuredSearchRequest : CodedNewsSearchRequest
    {
        ///// <summary>
        ///// This fields is a replacement for filterFragment, going forward we should be using this field.
        ///// </summary>
        //[XmlElement("newsFilters")]
        //public NewsFilters newsFilters;

        //[System.ComponentModel.DefaultValueAttribute(false)]
        //public bool occurrenceSearch = false;
    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
    [System.Xml.Serialization.XmlRootAttribute("CodedNewsSearch", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
    public class CodedNewsSearchRequest
    {

        ///// <remarks/>
        //public CodedNewsType newsType;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("companies")]
        //public Company[] companies;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("industries")]
        //public Industry[] industries;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("executives")]
        //public Person[] executives;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("organization")]
        //public Organization[] organization;


        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("official")]
        //public GovernmentOfficial[] official;


        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(0)]
        //public int headlineStart = 0;

        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(20)]
        //public int headlineCount = 20;

        ///// <remarks/>
        //public string searchContext;

        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(TruncationType.Large)]
        //public TruncationType truncationType = TruncationType.Large;

        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(false)]
        //public bool useAltQueries = false;

        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(1)]
        //public int scope = 1;

        //public string filterFragment;

        //public bool returnNavSet;

        //public SearchContentCategory[] contentCategory;

        //[System.ComponentModel.DefaultValueAttribute(ServerName.Index)]
        //public ServerName serverName = ServerName.Index;

        ///// <remarks/>
        //[System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
        //public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

    }


}