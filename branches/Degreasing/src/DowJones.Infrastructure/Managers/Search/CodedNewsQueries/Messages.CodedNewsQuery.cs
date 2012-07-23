using System.ComponentModel;
using System.Xml.Serialization;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;

namespace DowJones.Managers.Search.CodedNewsQueries
{

    public class NewsFiltersExtended : NewsFilters
    {

        [DefaultValue(true)]
        public bool KeywordAsPhrase = true;

        [XmlElement("occuresCompany")]
        public FilterItem[] OccuresCompany;
    }

    public class CompanyFilter
    {
        public string Fcode;
        public string NewsSearch;
        public DescriptorType Descriptor;
        //public bool IsNewsCoded;
        //public bool IsOccurrenceCoded;
        //public CodeStatus ActiveStatus;
        public CompanyStatus Status;

    }

    public class ExecutiveFilter
    {
        public Name Name;
        public CompanyFilter CompanyFilter;
    }
    
    public class Name
    {
        /// <remarks/>
        public string FirstName;

        /// <remarks/>
        public string MiddleNames;

        /// <remarks/>
        public string LastName;

        /// <remarks/>
        public string Suffix;

        /// <remarks/>
        public string FullName;
    }

    /// <remarks/>
    public enum CodedNewsType
    {

        /// <remarks/>
        _InvalidValue,

        /// <remarks/>
        IndustryNews,

        /// <remarks/>
        IndustryEditorsChoice,

        /// <remarks/>
        IndustryReportAll,

        /// <remarks/>
        Investext,

        /// <remarks/>
        SpSummary,

        /// <remarks/>
        MergentReport,

        /// <remarks/>
        ForresterResearch,

        /// <remarks/>
        FreedoniaSummary,

        /// <remarks/>
        IBIS,

        /// <remarks/>
        MarketResearch,

        /// <remarks/>
        LatestNews,

        /// <remarks/>
        ManagementMoves,

        /// <remarks/>
        ContractsOrders,

        /// <remarks/>
        NewProductsServices,

        /// <remarks/>
        LegalJudicial,

        /// <remarks/>
        Performance,

        /// <remarks/>
        OwnershipChanges,

        /// <remarks/>
        MergersAcquisition,

        /// <remarks/>
        PressReleases,

        /// <remarks/>
        TradeArticles,

        /// <remarks/>
        KeyDevAll,

        /// <remarks/>
        KeyDevBankruptcy,

        /// <remarks/>
        KeyDevManagementChanges,

        /// <remarks/>
        KeyDevMAOC,

        /// <remarks/>
        KeyDevMarketChanges,

        /// <remarks/>
        KeyDevNewFundingCapital,

        /// <remarks/>
        KeyDevNewProductsServices,

        /// <remarks/>
        KeyDevPerformance,

        /// <remarks/>
        KeyDevRegGvtPolicy,

        /// <remarks/>
        ReportAll,

        /// <remarks/>
        HooversBasicReport,

        /// <remarks/>
        HooversInDepthReport,

        /// <remarks/>
        DataMonBusinessDescription,

        /// <remarks/>
        DataMonCompanyLocations,

        /// <remarks/>
        DataMonHistory,

        /// <remarks/>
        DataMonKeyEmployees,

        /// <remarks/>
        DataMonKeyFacts,

        /// <remarks/>
        DataMonMajorProducts,

        /// <remarks/>
        DataMonSWOTAnalysis,

        /// <remarks/>
        DataMonTopCompetitors,

        /// <remarks/>
        DataMonCompanyOverview,

        /// <remarks/>
        JobsonsMiningYearBook,

        /// <remarks/>
        JobsonsYearBookPublicCompany,

        /// <remarks/>
        ExecutiveBusinessNews,

        /// <remarks/>
        ExecutiveLatestNews,

        GovernmentGeneralNews,

        GovernmentExecutiveNews,

        GovernmentOpportunitiesContracts,

        //Added to implement the Company analysis and profiles
        //page to view more data monitor reports if they exist.
        CompanyReportDataMon,

        //Added to implement the Company analysis and profiles
        //page to view more zachs reports if they exist.
        CompanyReportZachs,

        //Added to implement the Industry analysis and profiles
        //page to view more zachs reports if they exist.
        IndustryReportZachs,

        //Added to implement the additional company news subject
        //added to the dropdown
        Earnings,

        //Added to implement the additional company news subject
        //added to the dropdown
        CapacitiesFacilities,

        //Added to implement the additional company news subject
        //added to the dropdown
        Bankruptcy,

        //Added to implement the additional company news subject
        //added to the dropdown
        EarningsCalls,

        // mt 2008 Q3 - add Business Monitor International (EMDN) source
        BusinessMonitor,

        //os 2009 Q3 China Industries Report (sc=BCIROS or sc=BCIRDT or sc=BCIREN) 
        IndustryChinaReport,

        //os 2009 Q3 China Coal Monthly (sc=BCOLEN ) 
        ChinaCoal,

        //os 2009 Q3 Asia Pulse (sc=APULSE  and hd=briefing) 
        AsiaPulse,
    }

    /// <remarks/>
    public enum SearchContentCategory
    {

        /// <remarks/>
        Unspecified,

        /// <remarks/>
        Publications,

        /// <remarks/>
        Pictures,

        /// <remarks/>
        WebSites,

        /// <remarks/>
        Multimedia,

        /// <remarks/>
        Blogs,

        /// <remarks/>
        NewsSites,

        /// <remarks/>
        Video,

        /// <remarks/>
        Audio,

    }

    /// <remarks/>
    public enum RemoveDuplicate
    {

        /// <remarks/>
        None,

        /// <remarks/>
        High,

        /// <remarks/>
        Medium,
        
    }

    /// <remarks/>
    public enum TruncationType
    {

        /// <remarks/>
        XSmall,

        /// <remarks/>
        Small,

        /// <remarks/>
        Medium,

        /// <remarks/>
        Large,

        /// <remarks/>
        None,
    }

    public enum ServerName
    {
        /// <summary/>
        Unknown,
        /// <summary/>
        Search20,
        /// <summary/>
        Index
    }
    
}