//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace EMG.Utility.Managers.Search
//{
    
//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("newsSubject", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class NewsSubject
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasChildren = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("parent")]
//        public NewsSubject[] parent;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("child")]
//        public NewsSubject[] child;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("alias")]
//        public string[] alias;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        public TaxonomyCodeStatus taxonomyCodeStatus;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int position;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("taxonomyCodeStatus", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class TaxonomyCodeStatus
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime startDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool startDateSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime endDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool endDateSpecified;

//        /// <remarks/>
//        public bool visible;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool visibleSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public bool active;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("passwordResetRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PasswordResetRequest
//    {

//        /// <remarks/>
//        public UpdatePasswordRequest updatePasswordRequest;

//        /// <remarks/>
//        public bool getEncryptedToken;
//        /// <summary>
//        /// q207
//        /// </summary>
//        public PasswordUpdateType passwordUpdateType;
//        public StartConversionRequest startConvsersionRequest;
//        public UpdateEmailAddressRequest updateEmailAddressRequest;

//    }


//    /// <summary>
//    /// q207
//    /// </summary>
//    public enum PasswordUpdateType
//    {
//        _Unspecified,
//        startconvert,
//        updateconvert,
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [XmlRoot("startConversionRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class StartConversionRequest
//    {
//        public string EmailAddress;
//        public string ipAddress;
//        public string SecondaryURL;
//        public string LanguagePref;
//        public string LoginPageURL;
//    }


//    /// <summary>
//    /// 
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [XmlRoot("updateEmailAddressRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UpdateEmailAddressRequest
//    {
//        public string EmailAddress;
//        public string IpAddress;
//        public string SecondaryURL;
//        public string RefHost;
//        public string LanguagePref;
//        public string LoginPageURL;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("updatePasswordRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UpdatePasswordRequest
//    {

//        /// <remarks/>
//        public string oldPassword;

//        /// <remarks/>
//        public string newPassowrd;

//        /// <remarks/>
//        public string verifyNewPassword;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("snippet", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Snippet
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("paragraph")]
//        public Paragraph[] paragraph;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("paragraph", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Paragraph
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("eLink", typeof(ELink))]
//        [System.Xml.Serialization.XmlElementAttribute("text", typeof(Text))]
//        [System.Xml.Serialization.XmlElementAttribute("hlt", typeof(HighlightedText))]
//        [System.Xml.Serialization.XmlElementAttribute("entityReference", typeof(EntityReference))]
//        [System.Xml.Serialization.XmlElementAttribute("eventReference", typeof(EventReference))]
//        public object[] Items;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ParagraphDisplay display;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ParagraphTruncation truncation;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ContentLanguage lang;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("eLink", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ELink : RichText
//    {

//        /// <remarks/>
//        public Text text;

//        /// <remarks/>
//        public string caption;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("part")]
//        public Part[] part;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string type;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string reference;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("entityReference", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class EntityReference
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlElementAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] items;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string category;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("eventReference", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class EventReference
//    {
//        [System.Xml.Serialization.XmlElementAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlElementAttribute("entityReference", typeof(EntityReference), IsNullable = false)]
//        [System.Xml.Serialization.XmlElementAttribute("text", typeof(Text), IsNullable = false)]
//        public object[] items;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string category;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("text", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Text
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlTextAttribute()]
//        public string Value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("part", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Part
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string type;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string subType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string mimeType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string size;

//     /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string reference;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HighlightedText))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ELink))]
//    public class RichText
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("hlt", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class HighlightedText : RichText
//    {

//        /// <remarks/>
//        public Text text;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ParagraphDisplay
//    {

//        /// <remarks/>
//        Proportional,

//        /// <remarks/>
//        Fixed,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ParagraphTruncation
//    {

//        /// <remarks/>
//        None,

//        /// <remarks/>
//        Pre,

//        /// <remarks/>
//        Post,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ContentLanguage
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        ALL,

//        /// <remarks/>
//        BG,

//        /// <remarks/>
//        CA,

//        /// <remarks/>
//        ZHTW,

//        /// <remarks/>
//        ZHCN,

//        /// <remarks/>
//        CS,

//        /// <remarks/>
//        DA,

//        /// <remarks/>
//        NL,

//        /// <remarks/>
//        EN,

//        /// <remarks/>
//        FI,

//        /// <remarks/>
//        FR,

//        /// <remarks/>
//        DE,

//        /// <remarks/>
//        HU,

//        /// <remarks/>
//        IT,

//        /// <remarks/>
//        JA,

//        /// <remarks/>
//        NO,

//        /// <remarks/>
//        PL,

//        /// <remarks/>
//        PT,

//        /// <remarks/>
//        RU,

//        /// <remarks/>
//        SK,

//        /// <remarks/>
//        ES,

//        /// <remarks/>
//        SV,

//        /// <remarks/>
//        TR,

//        /// <remarks/>
//        KO,

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reportListIdentifier", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReportListIdentifier
//    {

//        /// <remarks/>
//        public Status status;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualBalanceSheet = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualBalanceSheetRef = "";

//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDNBPCIAnnualBalanceSheet = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DNBPCIAnnualBalanceSheetRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualCashFlow = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualCashFlowRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualIncomeStatement = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualIncomeStatementRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDNBPCIAnnualIncomeStatement = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DNBPCIAnnualIncomeStatementRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasInterimBalanceSheet = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string InterimBalanceSheetRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasInterimCashFlow = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string InterimCashFlowRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasInterimIncomeStatement = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string InterimIncomeStatementRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasGeographicSegmentBreakDown = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string GeographicSegmentBreakDownRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasBusinessSegmentBreakDown = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string BusinessSegmentBreakDownRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasKeyRatios = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string KeyRatiosRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasKeyFinancials = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string KeyFinancialsRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDNBPCIKeyFinancials = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DNBPCIKeyFinancialsRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasKeyFinancialsLastFiscalPeriod = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string KeyFinancialsLastFiscalPeriodRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasProductsAndServices = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string ProductsAndServicesRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasOverviewAndHistory = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string OverviewAndHistoryRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasOfficersAndExecutives = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string OfficersAndExecutivesRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasMajorCustomers = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string MajorCustomersRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasCompanyGeneralInfoPrimary = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string CompanyGeneralInfoPrimaryRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasCompanyGeneralInfoSecondary = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string CompanyGeneralInfoSecondaryRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasKeySubsidiaries = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string KeySubsidiariesRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasCorporateEvents = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string CorporateEventsRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasUKKeyRatios = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string UKKeyRatiosRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDNBPCIKeyRatios = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DNBPCIKeyRatiosRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasLongBusinessDescription = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string LongBusinessDescriptionRef = "";

//        //q207
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasHoppfLongBusinessDescription = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string HoppfLongBusinessDescriptionRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasCompanyStatement = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string CompanyStatementRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasSwotAnalysis = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string SwotAnalysisRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasTechnologyInformation = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string TechnologyInformationRef = "";

//        // mt 08/2007 - add company brands
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasCompanyBrands = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string CompanyBrandsRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasZoomInfoBusinessDescription = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string ZoomInfoBusinessDescriptionRef = "";
      
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDataMonitorBusinessLongDescription = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DataMonitorBusinessLongDescriptionRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDataMonitorBusinessDescription = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DataMonitorBusinessDescriptionRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDataMonitorCompetitionList = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DataMonitorCompetitionListRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDataMonitorProductsAndServices = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DataMonitorProductsAndServicesRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDataMonitorOfficersAndExecutives = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DataMonitorOfficersAndExecutivesRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDataMonitorOverviewAndHistory = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DataMonitorOverviewAndHistoryRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasReutersCompetitionList = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string ReutersCompetitionListRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasIndustryRatios = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string IndustryRatiosRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasIndustrySecRatios = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string IndustrySecRatiosRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasIndustrySandPRatios = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string IndustrySandPRatiosRef = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasCoporateAffiliations = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDetailedCompanyProfile = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDetailedCompanyProfilePlusFinancialStmts = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasFinacialStatementsReports = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDNBPCIFinacialStatementsReports = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasFinancialHealth = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasFinancialHealthUKdata = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasTearsheet = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasSnapshot = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasRatioComparison = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasRegulatoryFilings = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasInvestexReports = false;

//        /// <remarks/>
//        public ReportList companyInventoryReportList;

//        /// <remarks/>
//        public FamilyTreeInfo familyTreeInfo;

//        /// <remarks/>
//        public CompanyPrimaryIndustryClassification primaryIndustryClassification;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("marketIndices")]
//        public InstrumentReference[] marketIndices;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool showExcelLink = false;
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool odeAdminBlocked = false;

//        public PreferencesDTO preferencesDTO;

//        //Added to check whether there is any info for this
//        //company in screening. We use this in inline tagging
//        //for companies that are in symbology but not in screening.
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool noScreeningInfo = false;

//        //q307 Hoppenstedt CredInform

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasKeyFiguresHoppenstedt = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string KeyFiguresHoppenstedtRef = "";

//        /// <remarks/> ///1
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualBalanceSheetGermanConsolidated = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualBalanceSheetGermanConsolidatedRef = "";

//        /// <remarks/>///2
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualBalanceSheetGermanUnconsolidated = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualBalanceSheetGermanUnconsolidatedRef = "";

//        /// <remarks/>///3
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualIncomeStatementGermanConsolidated = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualIncomeStatementGermanConsolidatedRef = "";

//        /// <remarks/>///4
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasAnnualIncomeStatementGermanUnconsolidated = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string AnnualIncomeStatementGermanUnconsolidatedRef = "";

//        /// <remarks/>///5
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasRatiosBGermanConsolidated = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string RatiosBGermanConsolidatedRef = "";

//        /// <remarks/>///6
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasRatiosBGermanUnconsolidated = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string RatiosBGermanUnconsolidatedRef = "";

//        //q307 CredInform
//        /// <remarks/>//1
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasImportExportTable = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string ImportExportTableRef = "";

//        /// <remarks/>//2
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasRegistrationTable = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string RegistrationTableRef = "";

//        /// <remarks/>//3
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasShareholdersTable = false;
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string ShareholdersTableRef = "";

//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasDNBKeyFinancialsSecondary = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string DNBKeyFinancialsSecondaryRef = "";


//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasGenerateBusinessDescription = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string GenerateBusinessDescriptionRef = "";

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("status", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Status
//    {

//        /// <remarks/>
//        public Error error;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("warning")]
//        public Error[] warning;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Error
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlTextAttribute()]
//        public string Value;
//    }

//    /// <remarks/>
//    /// <summary>
//    /// This is the service response message that is
//    /// returned from the ReportService
//    ///</summary>
//    /// 
//    /// <chg>
//    ///		<bucket>Q107</bucket>
//    ///		<date>17-12-2007</date>
//    ///		<id>??</id>
//    /// This service has been modified to do the following
//    /// 1. Add the hitResponse member to return the array of hitcounts per each
//    /// sub-group of analysis and profile reports from the industry and company page.
//    /// </chg>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reportList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReportList : ResponseMessage
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Person executive;

//        /// <remarks/>
//        public bool hasAnalyst;

//        /// <remarks/>
//        public bool hasRegulatory;

//        /// <remarks/>
//        public int count;

//        /// <remarks/>
//        public int totalCount;

//        /// <remarks/>
//        public int indexOfFirstHeadline;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("reportReference")]
//        public ReportReference[] reportReference;

//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool contentRestricted = false;

//        //Added to return the array of hitcounts per each
//        //sub-group of analysis and profile reports from the industry and company page.
//        public CodedNewsSearchHitResponse[] hitReponse;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompanySymbology))]
//    [System.Xml.Serialization.XmlRootAttribute("company", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Company : ResponseMessage
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public int newsMentions;

//        public int newsHits;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("address")]
//        public string[] address;

//        /// <remarks/>
//        public string city;

//        /// <remarks/>
//        public string state;

//        /// <remarks/>
//        public string zip;

//        /// <remarks/>
//        public string country;

//        /// <remarks/>
//        public string distance;

//        /// <remarks/>
//        public string countryIsoCode;

//        /// <remarks/>
//        public string latitude;

//        /// <remarks/>
//        public string longitude;

//        /// <remarks/>
//        public string geoAccuracy;

//        /// <remarks/>
//        public string phone;

//        /// <remarks/>
//        public string phoneAreaCode;

//        /// <remarks/>
//        public string phoneCountryCode;

//        /// <remarks/>
//        public string fax;

//        /// <remarks/>
//        public string faxAreaCode;

//        /// <remarks/>
//        public string faxCountryCode;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("website")]
//        public Website[] website;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(OwnershipType.Unspecified)]
//        public OwnershipType ownershipType = OwnershipType.Unspecified;

//        /// <remarks/>
//        public string corporateType;

//        /// <remarks/>
//        public string currency;

//        /// <remarks/>
//        public string locationType;

//        /// <remarks/>
//        public string creationDate;

//        /// <remarks/>
//        public string language;

//        /// <remarks/>
//        public Person contact;

//        /// <remarks/>
//        public string businessDescription;

//        /// <remarks/>
//        public CompanyPrimaryIndustryClassification primaryIndustryClassification;

//        /// <remarks/>
//        public Region region;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("secondaryRegions")]
//        public Region[] secondaryRegions;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("person")]
//        public Person[] person;

//        /// <remarks/>
//        public InstrumentReference primaryDJInstrument;

//        /// <remarks/>
//        public InstrumentReference primaryRicInstrument;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("marketIndices")]
//        public InstrumentReference[] marketIndices;

//        /// <remarks/>
//        public string dunsNumber;

//        /// <remarks/>
//        public CompanyStatus companyStatus;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("ultimateParent")]
//        public CompanyParent[] ultimateParent;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("parent")]
//        public CompanyParent[] parent;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("secretary")]
//        public Secretary[] secretary;

//        /// <remarks/>
//        public string accessionNo;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;

//        /// <remarks/>
//        public FormattedDate publicationDate;

//        /// <remarks/>
//        public bool hasCoreRecord;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool hasCoreRecordSpecified;

//        /// <remarks/>
//        public string newsSearch;

//        /// <remarks/>
//        public FamilyTreeInfo familyTreeInfo;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string rank;

//        [System.Xml.Serialization.XmlElementAttribute("child")]
//        public CompanyParent[] child;

//        public LegalStatus legalStatus;

//        public string orgCode;

        
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("website", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Website
//    {

//        /// <remarks/>
//        public string url;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("ownershipType", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum OwnershipType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        @public,

//        /// <remarks/>
//        @private,

//        /// <remarks/>
//        all,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("person", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Person
//    {

//        /// <remarks/>
//        public string consolidatedId;

//        /// <remarks/>
//        public string consolidatedExecs;

//        /// <remarks/>
//        public Name name;

//        /// <remarks/>
//        public Provider provider;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isBoardMember = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isDirector = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isOfficer = false;

//        /// <remarks/>
//        public ReportList reportList;

//        /// <remarks/>
//        public PersonLevel level;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("aka")]
//        public Name[] aka;

//        /// <remarks/>
//        public int age;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("position")]
//        public Position[] position;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("jobFunction")]
//        public Position[] jobFunction;

//        /// <remarks/>
//        public CareerInfo careerInfo;

//        /// <remarks/>
//        public string phone;

//        /// <remarks/>
//        public string fax;

//        /// <remarks/>
//        public Biography biography;

//        /// <remarks/>
//        public string providerDocumentId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(PersonType._Unspecified)]
//        public PersonType type = PersonType._Unspecified;

//        ///q207
//        public PhoneNumber phoneNumber;

//        public string execCode;


//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Name
//    {

//        /// <remarks/>
//        public string firstName;

//        /// <remarks/>
//        public string middleNames;

//        /// <remarks/>
//        public string lastName;

//        /// <remarks/>
//        public string suffix;

//        /// <remarks/>
//        public string fullName;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("provider", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Provider
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public string longDescriptor;

//        /// <remarks/>
//        public string url;

//        /// <remarks/>
//        public bool delistedSource;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool delistedSourceSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ProviderCode code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ProviderCode
//    {

//        /// <remarks/>
//        Unknown,

//        /// <remarks/>
//        BVD,

//        /// <remarks/>
//        DATMON,

//        /// <remarks/>
//        DBCUK,

//        /// <remarks/>
//        DNB,

//        /// <remarks/>
//        DBPCI,

//        /// <remarks/>
//        Factiva,

//        /// <remarks/>
//        Hoovers,

//        /// <remarks/>
//        Marquis,

//        /// <remarks/>
//        Multex,

//        /// <remarks/>
//        ReutersResearch,

//        /// <remarks/>
//        StandardAndPoorsRegister,

//        /// <remarks/>
//        Thomson,

//        /// <remarks/>
//        Hemscott,

//        /// <remarks/>
//        Tradeline,

//        /// <remarks/>
//        ZoomInfo,

//        /// <remarks/>
//        DataMonitor,

//        /// <remarks/>
//        EditionsJacquesLafitte,

//        /// <remarks/>
//        HARTHNK,

//        /// <remarks/>//q207
//        CredInform,
//        /// <remarks/>//q207
//        Hoppenstedt,

//        /// <remarks/>//q307
//        DBEUR,
//        /// <remarks/>//q307
//        DBCEUR,

//        // mt 08/2007 - add company brands
//        GBRANDS,

//        // mt 2008 Q4 - attribute Dow Jones
//        DowJones,

//        //DowJonesEditorial
//        DowJonesEditorial,

//        /// <remarks/>//q408
//        Generate

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum PersonLevel
//    {
//        Unspecified,

//        /// <remarks/>
//        Both,

//        /// <remarks/>
//        Director,

//        /// <remarks/>
//        Officer,

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Position
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public string startYear;

//        /// <remarks/>
//        public string endYear;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CareerInfo
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("jobPosition")]
//        public Position[] jobPosition;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("careerItem")]
//        public CareerItem[] careerItem;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("tenureDates")]
//        public TenureDates[] tenureDates;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("directorship")]
//        public string[] directorship;

//        /// <remarks/>
//        public string additionalInformation;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CareerItem
//    {

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        public YearMonthDay start;

//        /// <remarks/>
//        public YearMonthDay end;

//        /// <remarks/>
//        public YearMonthDay instant;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class YearMonthDay
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int year;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool yearSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int month;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool monthSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int day;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool daySpecified;


//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool IsValidDate
//        {
//            get{return (yearSpecified && monthSpecified && daySpecified);}
//        }
        
//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public DateTime GetDate
//        {
//            get { return new DateTime(year, month, day);} 
//        }
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class TenureDates
//    {

//        /// <remarks/>
//        public YearMonthDay officerStart;

//        /// <remarks/>
//        public YearMonthDay officerEnd;

//        /// <remarks/>
//        public YearMonthDay directorStart;

//        /// <remarks/>
//        public YearMonthDay directorEnd;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Biography
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(BiographyGender._Unspecified)]
//        public BiographyGender gender = BiographyGender._Unspecified;

//        /// <remarks/>
//        public FormattedDate birthDate;

//        /// <remarks/>
//        public string birthYear;

//        /// <remarks/>
//        public Address birthPlace;

//        /// <remarks/>
//        public string parents;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("degree", IsNullable = false)]
//        public EducationDegree[] education;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("additionalEducation")]
//        public string[] additionalEducation;

//        /// <remarks/>
//        public string companyMembership;

//        /// <remarks/>
//        public string fraternalMembership;

//        /// <remarks/>
//        public string politicalMembership;

//        /// <remarks/>
//        public string religiousMembership;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("creativeWorks")]
//        public string[] creativeWorks;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("achievements")]
//        public string[] achievements;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("thoughts")]
//        public string[] thoughts;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("book")]
//        public string[] book;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("avocations")]
//        public string[] avocations;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("awards")]
//        public string[] awards;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("certifications")]
//        public string[] certifications;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("civicInformation")]
//        public string[] civicInformation;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("militaryInformation")]
//        public string[] militaryInformation;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("additionalInformation")]
//        public string[] additionalInformation;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("memberships")]
//        public string[] memberships;

//        /// <remarks/>
//        public string biographicalText;

//        /// <remarks/>
//        public string committees;

//        /// <remarks/>
//        public string trusteeships;

//        /// <remarks/>
//        public string interests;

//        /// <remarks/>
//        public string nationality;

//        /// <remarks/>
//        public string languages;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum BiographyGender
//    {

//        /// <remarks/>
//        _Unspecified,

//        /// <remarks/>
//        Male,

//        /// <remarks/>
//        Female,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("formattedDate", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FormattedDate
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("dateTime", typeof(System.DateTime))]
//        [System.Xml.Serialization.XmlElementAttribute("date", typeof(System.DateTime), DataType = "date")]
//        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
//        public System.DateTime Item;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public ItemChoiceType ItemElementName;

//        /// <remarks/>
//        public Text text;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IncludeInSchema = false)]
//    public enum ItemChoiceType
//    {

//        /// <remarks/>
//        dateTime,

//        /// <remarks/>
//        date,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Address
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("address")]
//        public string[] address;

//        /// <remarks/>
//        public string city;

//        /// <remarks/>
//        public string state;

//        /// <remarks/>
//        public string zip;

//        /// <remarks/>
//        public string country;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class EducationDegree
//    {

//        /// <remarks/>
//        public string school;

//        /// <remarks/>
//        public string level;

//        /// <remarks/>
//        public string type;

//        /// <remarks/>
//        public string major;

//        /// <remarks/>
//        public FormattedDate graduationDate;

//        /// <remarks/>
//        public string graduationYear;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum PersonType
//    {

//        /// <remarks/>
//        _Unspecified,

//        /// <remarks/>
//        main,

//        /// <remarks/>
//        other,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("primaryIndustryClassification", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CompanyPrimaryIndustryClassification
//    {

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        public Industry sicIndustry;

//        /// <remarks/>
//        public Industry naceIndustry;

//        /// <remarks/>
//        public Industry naicsIndustry;

//        /// <remarks/>
//        public Industry okvedIndustry;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("naceIndustry", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Industry : ResponseMessage
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("fiiIndustry")]
//        public Industry[] fiiIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("sicIndustry")]
//        public Industry[] sicIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("naceIndustry")]
//        public Industry[] naceIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("naicsIndustry")]
//        public Industry[] naicsIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("child")]
//        public Industry[] child;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("parent")]
//        public Industry[] parent;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("instrument")]
//        public InstrumentReference[] instrument;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("website")]
//        public Website[] website;

//        /// <remarks/>
//        public Number totalCompanies;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasChildren = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("alias")]
//        public string[] alias;

//        /// <remarks/>
//        public TaxonomyCodeStatus taxonomyCodeStatus;

//        /// <remarks/>
//        public int level;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool levelSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class InstrumentReference
//    {

//        /// <remarks/>
//        public string instrumentName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrecisionNumber))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Money))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoubleMoneyCurrency))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoubleMoney))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LongNumber))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WholeNumber))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Percent))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoubleNumber))]
//    [System.Xml.Serialization.XmlRootAttribute("lifetimeHighPrice", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Number
//    {

//        /// <remarks/>
//        public Text text;

//        /// <remarks/>
//        public Text rawText;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isPositive = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public int exp = 0;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("precisionNumber", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PrecisionNumber : Number
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public System.Double value = 0;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(1)]
//        public int precision = 1;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoubleMoneyCurrency))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoubleMoney))]
//    [System.Xml.Serialization.XmlRootAttribute("money", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Money : Number
//    {

//        /// <remarks/>
//        public Currency currency;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("currency", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Currency : FactivaListItem
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("code", IsNullable = false)]
//        public string[] crossRates;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ManagementLevel))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Currency))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Degree))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Department))]
//    public class FactivaListItem
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("managementLevel", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ManagementLevel : FactivaListItem
//    {
//    }
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("degree", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Degree : FactivaListItem
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("mappointLogin", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class MappointLogin
//    {
//        public string loginId;
//        public string primaryPwd;
//        public string secondaryPwd;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("department", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Department : FactivaListItem
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("bank", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Bank : FactivaListItem
//    {
//    }


//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("locationType", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class LocationType : FactivaListItem
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("doubleMoneyCurrency", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DoubleMoneyCurrency : Money
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public System.Double value;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool valueSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("doubleMoney", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DoubleMoney : Money
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public System.Double value;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool valueSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool displayInMillions = false;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("longNumber", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class LongNumber : Number
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(typeof(long), "0")]
//        public long value = ((long)(0));
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("wholeNumber", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class WholeNumber : Number
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public System.Double value = 0;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("percent", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Percent : Number
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public System.Double value;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool valueSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("doubleNumber", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DoubleNumber : Number
//    {
//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public System.Double value = 0;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("responseMessage", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ResponseMessage
//    {

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getFolderInfoResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetFolderInfoResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("folderInfo")]
//        public FolderInfo[] folderInfo;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class FolderInfo
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public EditorInfo editorInfo;

//        /// <remarks/>
//        public string highlightString;

//        /// <remarks/>
//        public bool newHits;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool newHitsSpecified;

//        /// <remarks/>
//        public Owner owner;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public int queryHitCount;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool queryHitCountSpecified;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public Status status;

//        /// <remarks/>
//        public DeliveryMethod deliveryMethod;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DocumentFormat documentFormat;
//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool documentFormatSpecified;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        public string timeZone;

//        /// <remarks/>
//        public bool adjustDaylight;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool adjustDaylightSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        /// <remarks/>
//        public SharedFolderInfo sharedFolderInfo;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SharedFolderInfo
//    {
//        /// <remarks>Folder ownership </remarks>
//        public bool isOwner;

//        public SharingAssetType assetType;

//        /// <remarks>Folder access (currentely public or private)</remarks>
//        public SharingAccessScope accessScope;

//        public SharingAccessScope previouseAccessScope;

//        /// <remarks/>
//        public bool isActive;

//        public SharingAccessStatus internalAccessStatus;

//        public SharingAccessStatus externalAccessStatus;

//        public string internalHashKey;

//        public string externalHashKey;

//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class EditorInfo
//    {

//        /// <remarks/>
//        public int postedHits;

//        /// <remarks/>
//        public int unpostedHits;

//        /// <remarks/>
//        public PostMethod postMethod;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum PostMethod
//    {

//        /// <remarks/>
//        Automatic,

//        /// <remarks/>
//        Manual,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Owner
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string userID;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string @namespace;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("watchlistProductId", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum ProductId
//    {

//        /// <remarks/>
//        Global,

//        /// <remarks/>
//        SelectHeadlines,

//        /// <remarks/>
//        SelectFullText,

//        /// <remarks/>
//        FastTrack,

//        /// <remarks/>
//        FcpIndustry,

//        /// <remarks/>
//        FcpCompany,

//        /// <remarks/>
//        FcpExecutive,

//        /// <remarks/>
//        Iwe,

//        /// <remarks/>
//        Search20,

//        /// <remarks/>
//        All,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum RevisionPrivileges
//    {

//        /// <remarks/>
//        Owner,

//        /// <remarks/>
//        Administrator,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DeliveryMethod
//    {

//        /// <remarks/>
//        Online,

//        /// <remarks/>
//        Batch,

//        /// <remarks/>
//        Continuous,

//        /// <remarks/>
//        Feed,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum FolderSubType
//    {

//        /// <remarks/>
//        Group,

//        /// <remarks/>
//        Personal,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getQuoteResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetQuoteResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("instrument")]
//        public Instrument[] instrument;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("instrument", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Instrument : ResponseMessage
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public InstrumentType type;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("quote")]
//        public Quote[] quote;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("chart")]
//        public Chart[] chart;

//        /// <remarks/>
//        public Exchange exchange;

//        /// <remarks/>
//        public CodeStatus codeStatus;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool codeStatusSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("capitalChange", IsNullable = false)]
//        public CapitalChange[] capitalChangeReport;

//        /// <remarks/>
//        public string requestedSymbol;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        public FormattedDate listed;

//        /// <remarks/>
//        public FormattedDate delisted;

//        /// <remarks/>
//        public string sedol;

//        /// <remarks/>
//        public string cusip;

//        /// <remarks/>
//        public string factivaCompanyCode;

//        /// <remarks/>
//        public string djTicker;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(InstrumentCodeType.RIC)]
//        public InstrumentCodeType codeType = InstrumentCodeType.RIC;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("instrumentType", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum InstrumentType
//    {

//        /// <remarks/>
//        Equity,

//        /// <remarks/>
//        Fund,

//        /// <remarks/>
//        Currency,

//        /// <remarks/>
//        Index,

//        /// <remarks/>
//        DelistedOrMergedStock,

//        /// <remarks/>
//        Debt,

//        /// <remarks/>
//        Bond,

//        /// <remarks/>
//        MarketIndexesByName,

//        /// <remarks/>
//        UnitTrust,

//        /// <remarks/>
//        USGovernmentDebt,

//        /// <remarks/>
//        CapitalChanges,

//        /// <remarks/>
//        DelistedOrMergedCapitalChanges,

//        /// <remarks/>
//        DirectUrlBackwardsCompatibility,

//        /// <remarks/>
//        Unspecified,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("quote", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Quote : ResponseMessage
//    {

//        /// <remarks/>
//        public Number askPrice;

//        /// <remarks/>
//        public Number bidPrice;

//        /// <remarks/>
//        public Number change;

//        /// <remarks/>
//        public Number changeInNetAssetValue;

//        /// <remarks/>
//        public FormattedDate closeDate;

//        /// <remarks/>
//        public Number closePrice;

//        /// <remarks/>
//        public string currency;

//        /// <remarks/>
//        public Number dayHighBidPrice;

//        /// <remarks/>
//        public Number dayHighPrice;

//        /// <remarks/>
//        public Number dayLowBidPrice;

//        /// <remarks/>
//        public Number dayLowPrice;

//        /// <remarks/>
//        public string debtType;

//        /// <remarks/>
//        public Number dividend;

//        /// <remarks/>
//        public FormattedDate dividendDate;

//        /// <remarks/>
//        public string djTicker;

//        /// <remarks/>
//        public Number earningsPerShare;

//        /// <remarks/>
//        public Exchange exchange;

//        /// <remarks/>
//        public FormattedDate exDividendDate;

//        /// <remarks/>
//        public Instrument instrument;

//        /// <remarks/>
//        public string interestPaymentFrequency;

//        /// <remarks/>
//        public FormattedDate issueDate;

//        /// <remarks/>
//        public FormattedDate last52WeekHighDate;

//        /// <remarks/>
//        public Number last52WeekHighPrice;

//        /// <remarks/>
//        public FormattedDate last52WeekLowDate;

//        /// <remarks/>
//        public Number last52WeekLowPrice;

//        /// <remarks/>
//        public Number lastTradePrice;

//        /// <remarks/>
//        public FormattedDate lastTradeDateTime;

//        /// <remarks/>
//        public FormattedDate lifetimeHighDate;

//        /// <remarks/>
//        public Number lifetimeHighPrice;

//        /// <remarks/>
//        public FormattedDate lifetimeLowDate;

//        /// <remarks/>
//        public Number lifetimeLowPrice;

//        /// <remarks/>
//        public FormattedDate maturityDate;

//        /// <remarks/>
//        public Number midDayPrice;

//        /// <remarks/>
//        public Number netAssetValue;

//        /// <remarks/>
//        public FormattedDate netAssetValueDate;

//        /// <remarks/>
//        public FormattedDate nextInterestDate;

//        /// <remarks/>
//        public Number nextInterestRate;

//        /// <remarks/>
//        public Number offerPrice;

//        /// <remarks/>
//        public Number openPrice;

//        /// <remarks/>
//        public Number pERatio;

//        /// <remarks/>
//        public Number previousAskPrice;

//        /// <remarks/>
//        public Number previousBidPrice;

//        /// <remarks/>
//        public Number percentageChange;

//        /// <remarks/>
//        public Number previousNetAssetValue;

//        /// <remarks/>
//        public FormattedDate previousNetAssetValueDate;

//        /// <remarks/>
//        public string rating;

//        /// <remarks/>
//        public string ratingID;

//        /// <remarks/>
//        public bool spotRateInverted;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool spotRateInvertedSpecified;

//        /// <remarks/>
//        public FormattedDate updateTime;

//        /// <remarks/>
//        public Number volume;

//        /// <remarks/>
//        public Number yield;

//        /// <remarks/>
//        public Number last52WeekChange;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public TimelinessType timeliness;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool timelinessSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("exchange", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Exchange
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public string descriptor;

//        /// <remarks/>
//        public string regionCode;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TimelinessType
//    {

//        /// <remarks/>
//        Delayed,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("Real-time")]
//        Realtime,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("chartResp", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ChartResponse : ResponseMessage
//    {
//        /// <remarks/>
//        public Chart Chart;

//        /// <remarks/>
//        public Company Company;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("chart", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Chart : ResponseMessage
//    {

//        /// <remarks/>
//        public FormattedDate fromDate;

//        /// <remarks/>
//        public FormattedDate toDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("data")]
//        public Data[] data;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string currency;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(Periodicity.UnSpecified)]
//        public Periodicity periodicity = Periodicity.UnSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("data", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Data
//    {

//        /// <remarks/>
//        public FormattedDate closeDate;

//        /// <remarks/>
//        public Number openPrice;

//        /// <remarks/>
//        public Number dayHighPrice;

//        /// <remarks/>
//        public Number dayLowPrice;

//        /// <remarks/>
//        public Number closePrice;

//        /// <remarks/>
//        public Number volume;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum Periodicity
//    {

//        /// <remarks/>
//        UnSpecified,

//        /// <remarks/>
//        Daily,

//        /// <remarks/>
//        Weekly,

//        /// <remarks/>
//        Monthly,

//        /// <remarks/>
//        Quarterly,

//        /// <remarks/>
//        Yearly,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("codeStatus", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum CodeStatus
//    {

//        /// <remarks/>
//        Active,

//        /// <remarks/>
//        Inactive,

//        /// <remarks/>
//        Duplicate,

//        /// <remarks/>
//        Suspended,

//        /// <remarks/>
//        Delisted,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CapitalChange
//    {

//        /// <remarks/>
//        public FormattedDate executionDate;

//        /// <remarks/>
//        public DividendType dividendType;

//        /// <remarks/>
//        public PaymentMethodCode paymentMethod;

//        /// <remarks/>
//        public PaymentOrderCode paymentOrder;

//        /// <remarks/>
//        public TaxCode tax;

//        /// <remarks/>
//        public LatenessRevisionMethodCode latenessRevisionMethodCode;

//        /// <remarks/>
//        public Number dividendRate;

//        /// <remarks/>
//        public string splitRatio;

//        /// <remarks/>
//        public FormattedDate recordDate;

//        /// <remarks/>
//        public FormattedDate paymentDate;

//        public Provider provider;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DividendType
//    {

//        /// <remarks/>
//        CashDividend,

//        /// <remarks/>
//        CashEquivalentDividend,

//        /// <remarks/>
//        StockDividend,

//        /// <remarks/>
//        StockSplit,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum PaymentMethodCode
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        USDollars,

//        /// <remarks/>
//        CanadianDollars,

//        /// <remarks/>
//        OtherCurrency,

//        /// <remarks/>
//        InCash,

//        /// <remarks/>
//        StockDividend,

//        /// <remarks/>
//        StockSplit,

//        /// <remarks/>
//        StockOfAnotherIssuer,

//        /// <remarks/>
//        ExchangeOfStock,

//        /// <remarks/>
//        Right,

//        /// <remarks/>
//        Warrant,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum PaymentOrderCode
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        NoPaymentOrder,

//        /// <remarks/>
//        AfterStockSplit,

//        /// <remarks/>
//        AfterStockDividend,

//        /// <remarks/>
//        DistributionUnit,

//        /// <remarks/>
//        InitialDividend,

//        /// <remarks/>
//        ApproximateRate,

//        /// <remarks/>
//        RateNotKnown,

//        /// <remarks/>
//        ExtrasOrCapitalGainsIncluded,

//        /// <remarks/>
//        CumulativePreferredStock,

//        /// <remarks/>
//        AdditionalDividend,

//        /// <remarks/>
//        FractionsInCash,

//        /// <remarks/>
//        ShortPayment,

//        /// <remarks/>
//        LongPayment,

//        /// <remarks/>
//        SalesLiquidationOfAssets,

//        /// <remarks/>
//        FractionsInStock,

//        /// <remarks/>
//        Approximate,

//        /// <remarks/>
//        ApproximateForSpecialDividend,

//        /// <remarks/>
//        CloseOfBusiness,

//        /// <remarks/>
//        ReverseSplit,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TaxCode
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        NoTaxApplies,

//        /// <remarks/>
//        NonResidentTax10,

//        /// <remarks/>
//        NonResidentTax15,

//        /// <remarks/>
//        UnknownOrVariableTax,

//        /// <remarks/>
//        NoUSTaxFromNonResident,

//        /// <remarks/>
//        IncomeAndCapitalGainsDistribution,

//        /// <remarks/>
//        PartiallyOrCompletelyTaxExempt,

//        /// <remarks/>
//        IncomeAndCapitalGainsDistributionWithTaxExempt,

//        /// <remarks/>
//        CapitalGainsDistributionOnly,

//        /// <remarks/>
//        BeforeTaxesToUSResidents,

//        /// <remarks/>
//        AfterTaxesToUSResidents,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum LatenessRevisionMethodCode
//    {

//        /// <remarks/>
//        Normal,

//        /// <remarks/>
//        LateEntry,

//        /// <remarks/>
//        Revision,

//        /// <remarks/>
//        RevisionAfterLateEntry,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum InstrumentCodeType
//    {

//        /// <remarks/>
//        RIC,

//        /// <remarks/>
//        TLID,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("iqSearchResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IqSearchResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public string iqContinuationContextString;

//        /// <remarks/>
//        public IqProcess iqProcess;

//        /// <remarks/>
//        public ContentSearchResult contentSearchResult;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("iqProcess", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IqProcess
//    {

//        /// <remarks/>
//        public IQTree iqTree;

//        /// <remarks/>
//        public IQueryStrings queryStrings;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("iqTree", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IQTree
//    {

//        /// <remarks/>
//        public string iqString;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("operator")]
//        public string[] @operator;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("stopWord")]
//        public string[] stopWord;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("spell")]
//        public string[] spell;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("phrase")]
//        public string[] phrase;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("focus")]
//        public Focus[] focus;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("alternatePhrase")]
//        public string[] alternatePhrase;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Focus
//    {

//        /// <remarks/>
//        public bool primary;

//        /// <remarks/>
//        public string code;

//        /// <remarks/>
//        public IQCategory category;

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        public string fcode;

        

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IQCategory
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Company,

//        /// <remarks/>
//        Industry,

//        /// <remarks/>
//        Region,

//        /// <remarks/>
//        Source,

//        /// <remarks/>
//        NewsSubject,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("queryStrings", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IQueryStrings
//    {

//        /// <remarks/>
//        public string bss;

//        /// <remarks/>
//        public string rrs;

//        /// <remarks/>
//        public string rstbss;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("contentSearchResult", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ContentSearchResult : ResponseMessage
//    {

//        /// <remarks/>
//        public int queryHitCount;

//        public int duplicatesCount;

//        public int count;

//        /// <remarks/>
//        public int indexOfFirstHeadline;

//        /// <remarks/>
//        public string highlightString;

//        /// <remarks/>
//        public string searchContext;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("contentHeadline")]
//        public ContentHeadline[] contentHeadline;

//        /// <remarks/>
//        public int contentServerAddress;

//        /// <remarks/>
//        public int contextId;

//        [System.ComponentModel.DefaultValueAttribute(ServerName.Index)]
//        public ServerName serverName = ServerName.Index;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("contentHeadline", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ContentHeadline
//    {

//        /// <remarks/>
//        public string accessionNo;

//        /// <remarks/>
//        public string baseLanguage;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] byline;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] credit;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] columnName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] copyright;

//        /// <remarks/>
//        public ContentParts contentParts;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("paragraph", IsNullable = false)]
//        public Paragraph[] headline;

//        /// <remarks/>
//        public string ipDocumentID;

//        /// <remarks/>
//        public FormattedDate publicationDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] sectionName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("paragraph", IsNullable = false)]
//        public Paragraph[] snippet;

//        /// <remarks/>
//        public Source source;

//        /// <remarks/>
//        public TruncationRules truncationRules;

//        /// <remarks/>
//        public Number wordCount;

//        /// <remarks/>
//        public DocumentManagementData documentManagementData;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public System.Double score;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool scoreSpecified;

//        public Search.DuplicateHeadlineDTO duplicateHeadlineDTO;
//    }

//    /// <summary>
//    /// This object is used when we use emg utility ti do accession search.
//    /// This indicates for a particular an if the headline was found or not.
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("contentHeadlineEx", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ContentHeadlineEx : ContentHeadline
//    {
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool HasBeenFound;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("contentParts", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ContentParts
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("part")]
//        public Part[] part;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string contentType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string primaryReference;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("source", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Source
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(AvailabilityStatus.Unspecified)]
//        public AvailabilityStatus availabilityStatus = AvailabilityStatus.Unspecified;

//        /// <remarks/>
//        public string listName;

//        /// <remarks/>
//        public string sortName;

//        /// <remarks/>
//        public string sourceName;

//        /// <remarks/>
//        public string sourceCode;

//        /// <remarks/>
//        public string groupName;

//        /// <remarks/>
//        public string groupCode;

//        /// <remarks/>
//        public Paragraph description;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SourceChildrenTypes.Unspecified)]
//        public SourceChildrenTypes sourceChildrenTypes = SourceChildrenTypes.Unspecified;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SourceGroupType.Unspecified)]
//        public SourceGroupType sourceGroupType = SourceGroupType.Unspecified;

//        /// <remarks/>
//        public bool isTrack;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool isTrackSpecified;

//        /// <remarks/>
//        public bool isIndex;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool isIndexSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(SourceType.Unspecified)]
//        public SourceType type = SourceType.Unspecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum AvailabilityStatus
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Active,

//        /// <remarks/>
//        InActive,

//        /// <remarks/>
//        Discontinued,

//        /// <remarks/>
//        Deleted,

//        /// <remarks/>
//        Pending,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SourceChildrenTypes
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Source,

//        /// <remarks/>
//        Group,

//        /// <remarks/>
//        Mixed,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SourceGroupType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Industry,

//        /// <remarks/>
//        Region,

//        /// <remarks/>
//        Language,

//        /// <remarks/>
//        Type,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SourceType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Publications,

//        /// <remarks/>
//        Pictures,

//        /// <remarks/>
//        WebSites,

//        /// <remarks/>
//        Reports,

//        Multimedia
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("truncationRules", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class TruncationRules
//    {

//        /// <remarks/>
//        public int extraSmall;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool extraSmallSpecified;

//        /// <remarks/>
//        public int small;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool smallSpecified;

//        /// <remarks/>
//        public int medium;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool mediumSpecified;

//        /// <remarks/>
//        public int large;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool largeSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class DocumentManagementData
//    {

//        /// <remarks/>
//        public string comment;

//        /// <remarks/>
//        public long commentId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(DocumentPriority.None)]
//        public DocumentPriority priority = DocumentPriority.None;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DocumentPriority
//    {

//        /// <remarks/>
//        None,

//        /// <remarks/>
//        Hot,

//        /// <remarks/>
//        New,

//        /// <remarks/>
//        MustRead,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("competitors", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Competitors : ResponseMessage
//    {

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("competitor")]
//        public Competitor[] competitor;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("competitor", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Competitor
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Financials financials;

//        /// <remarks/>
//        public Instrument instrument;
//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("financials", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Financials : ResponseMessage
//    {

//        /// <remarks/>
//        public Number lastYearEmployees;

//        /// <remarks/>
//        public Currency reportingCurrency;

//        /// <remarks/>
//        public Number sales;

//        /// <remarks/>
//        public Number salesUSDollars;

//        /// <remarks/>
//        public Number lastYearSales;

//        /// <remarks/>
//        public Number marketCap;

//        /// <remarks/>
//        public FormattedDate marketCapDate;

//        /// <remarks/>
//        public Number firstYearSalesGrowth;

//        public Number salesGrowthFirstYearOnly;

//        public Number salesGrowthThreeYearOnly;

//        /// <remarks/>
//        public Number netProfitMargin;

//        /// <remarks/>
//        public Number profitMargin;

//        /// <remarks/>
//        public Number netWorth;

//        public YearMonthDay netWorthEffectiveDate;

//        /// <remarks/>
//        public Number peRatio;

//        /// <remarks/>
//        public Number currentRatio;

//        /// <remarks/>
//        public Number debtToEquityRatio;

//        /// <remarks/>
//        public Number totalAssets;

//        /// <remarks/>
//        public FormattedDate totalAssetsFilingDate;

//        /// <remarks/>
//        public int totalAssetsFiscalYear;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool totalAssetsFiscalYearSpecified;

//        /// <remarks/>
//        public Number totalLiabilities;

//        /// <remarks/>
//        public Number returnOnEquity;

//        /// <remarks/>
//        public Number returnOnAssets;

//        /// <remarks/>
//        public Number inventoryTurnover;

//        /// <remarks/>
//        public Number assetTurnover;

//        /// <remarks/>
//        public Number employeeGrowth;

//        public Number employeeGrowth1Year;

//        public Number employeeGrowth3Year;

//        /// <remarks/>
//        public Number epsRatio;

//        /// <remarks/>
//        public Number beta60Months;

//        /// <remarks/>
//        public Number netIncome;

//        /// <remarks/>
//        public Number debtAssetsRatio;

//        /// <remarks/>
//        public Number mostRecentClose;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DataAccuracy.NotAvailable)]
//        public DataAccuracy employeesDataAccuracy = DataAccuracy.NotAvailable;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DataAccuracy.NotAvailable)]
//        public DataAccuracy salesDataAccuracy = DataAccuracy.NotAvailable;

//        /// <remarks/>
//        public Auditor auditor;

//        /// <remarks/>
//        public Number auditFees;

//        /// <remarks/>
//        public Number nonAuditFees;

//        /// <remarks/>
//        public FormattedDate filingDate;

//        /// <remarks/>
//        // mt 2008 Q3
//        public FormattedDate statementDate;

//        // os 2009 Q1
//        public FormattedDate endDate;
        
//        /// <remarks/>
//        public FormattedDate filingDateForEmployees;

//        /// <remarks/>
//        public FormattedDate filingDateForAuditor;

//        /// <remarks/>
//        public FormattedDate lastReportedPeriod;

//        /// <remarks/>
//        public int fiscalYear;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool fiscalYearSpecified;

//        /// <remarks/>
//        public string accessionNo;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;

//        /// <remarks/>
//        public FormattedDate publicationDate;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DataAccuracy
//    {

//        /// <remarks/>
//        NotAvailable,

//        /// <remarks/>
//        Actual,

//        /// <remarks/>
//        LowEndOfRange,

//        /// <remarks/>
//        Estimated,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("auditor", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Auditor
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string scheme;
//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("fiscalAccountingStandard", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FiscalAccountingStandard
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string scheme;
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("ppsarticle", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]

//    public class PPSArticle
//    {

//        [System.Xml.Serialization.XmlElementAttribute()]
//        public string[] pro;

//        public Article article;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("article", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Article : ResponseMessage
//    {

//        /// <remarks/>
//        public string accessionNo;

//        /// <remarks/>
//        public FormattedDate publicationDate;

//        /// <remarks/>
//        public string baseLanguage;

//        /// <remarks/>
//        public Number wordCount;

//        /// <remarks/>
//        public string publisherGroupName;

//        /// <remarks/>
//        public string publisherGroupCode;

//        /// <remarks/>
//        public string publisherName;

//        /// <remarks/>
//        public string sourceCode;

//        /// <remarks/>
//        public string sourceName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("company")]
//        public Company[] company;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public Industry[] industry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("region")]
//        public Region[] region;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubject")]
//        public NewsSubject[] newsSubject;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("descField")]
//        public BasicCode[] descField;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] artWork;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] byline;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] credit;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] columnName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] contact;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] copyright;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("paragraph", IsNullable = false)]
//        public Paragraph[] corrections;

//        /// <remarks/>
//        public string edition;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] notes;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("page", IsNullable = false)]
//        public string[] pages;

//        /// <remarks/>
//        public string reference;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("eLink", typeof(ELink), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("text", typeof(Text), IsNullable = false)]
//        [System.Xml.Serialization.XmlArrayItemAttribute("hlt", typeof(HighlightedText), IsNullable = false)]
//        public object[] sectionName;

//        /// <remarks/>
//        public SourceLogo sourceLogo;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("paragraph", IsNullable = false)]
//        public Paragraph[] headline;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("paragraph", IsNullable = false)]
//        public Paragraph[] leadParagraph;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("paragraph", IsNullable = false)]
//        public Paragraph[] tailParagraphs;

//        /// <remarks/>
//        public string volume;

//        public ContentParts contentParts;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("region", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Region
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool hasChildren = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("parent")]
//        public Region[] parent;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("child")]
//        public Region[] child;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("alias")]
//        public string[] alias;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(RegionType.Unspecified)]
//        public RegionType regionType = RegionType.Unspecified;

//        /// <remarks/>
//        public TaxonomyCodeStatus taxonomyCodeStatus;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        public Number totalCompanies;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum RegionType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Country,

//        /// <remarks/>
//        MetropolitanArea,

//        /// <remarks/>
//        StateProvince,

//        /// <remarks/>
//        SubNationalRegion,

//        /// <remarks/>
//        SupraNationalRegion,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class BasicCode
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlTextAttribute()]
//        public string Value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("sourceLogo", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class SourceLogo
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
//        public string link;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
//        public string source;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string image;
//    }

//    /// <remarks/>
//    /// <summary>
//    /// This is the service response message that is
//    /// returned from the CodedNewsSearchService
//    ///</summary>
//    /// 
//    /// <chg>
//    ///		<bucket>Q107</bucket>
//    ///		<date>17-12-2007</date>
//    ///		<id>??</id>
//    /// This service has been modified to do the following
//    /// 1. Add the hitResponse member to return the array of hitcounts per each
//    /// sub-group of analysis and profile reports from the industry and company page.
//    /// </chg>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("codedNewsSearchResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CodedNewsSearchResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public CodedNewsType newsType;

//        /// <remarks/>
//        public string query;

//        /// <remarks/>
//        public ContentSearchResult contentSearchResult;

//        /// <remarks/>
//        public Search2_0.ContentSearchResult discoveryPane;

//        /// <remarks/>
//        public HeadlineFormat headlineFormat;

//        //Added to return the array of hitcounts per each
//        //sub-group of analysis and profile reports from the industry and company page.
//        public CodedNewsSearchHitResponse[] hitReponse;
//    }

//    ///<summary>
//    /// This is the service response message that is
//    /// used to return the array of hit counts per each
//    /// sub-group of analysis and profile reports
//    /// from the industry and company page.
//    ///</summary>
//    ///
//    /// <created bucket=”Q107” date=”17-12-2007” id=”ssd” >
//    /// Created by Alka.
//    /// </created>
//    /// 
//    /// <usedby> 
//    /// This is the list of classes that use this class
//    /// dotcom[VersionX]sln/MessageModel/MessageModel/Messages.cs - CodedNewsSearchResponse
//    /// dotcom[VersionX]sln/MessageModel/MessageModel/Headlines/SnapshotHeadlineResponse.cs - SnapshotHeadlineResponse
//    /// dotcom[VersionX]sln/dotcom[VersionX]/controls/headlines/BaseReportHeadlines.cs
//    /// dotcom[VersionX]sln/VirtualServices/VirtualServices/CodedNewsSearchService.cs
//    ///</usedby>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("codedNewsSearchHitResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CodedNewsSearchHitResponse : ResponseMessage
//    {
//        public string sourceCode;

//        public int queryHitCount;
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
//    public enum HeadlineFormat
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        inline,

//        /// <remarks/>
//        mouseover,

//        /// <remarks/>
//        none,
//    }
//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UsageData : ResponseMessage
//    {

//        /// <remarks/>
//        public UsageType usageType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("accesspoints", typeof(UsageByAccesspoint))]
//        [System.Xml.Serialization.XmlElementAttribute("usageByClient", typeof(UsageByClient))]
//        public SummaryItem[] Items;

//        /// <remarks/>
//        public string userName;

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public Currency currency;

//        /// <remarks/>
//        public DoubleNumber total;

//        /// <remarks/>
//        public DoubleNumber whatIfTotal;

//        /// <remarks/>
//        public int startRecord;

//        /// <remarks/>
//        public int recordCount;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime startDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime endDate;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum UsageType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        AccountSummary,

//        /// <remarks/>
//        UserSummary,

//        /// <remarks/>
//        ClientProjectSummary,

//        /// <remarks/>
//        IndividualSummary,

//        /// <remarks/>
//        IndividualClientProjectSummary,

//        /// <remarks/>
//        UserClientProjectSummary,

//        /// <remarks/>
//        UserIndividualClientProjectSummary,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UsageByAccesspoint : SummaryItem
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("sources")]
//        public UsageBySource[] sources;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UsageBySource : SummaryItem
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("items")]
//        public UsageByItem[] items;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UsageByItem : SummaryItem
//    {

//        /// <remarks/>
//        public DoubleNumber count;

//        /// <remarks/>
//        public DoubleNumber pricePerItem;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UsageByClient))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UsageByItem))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UsageBySource))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UsageByAccesspoint))]
//    public abstract class SummaryItem
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public DoubleNumber total;

//        /// <remarks/>
//        public DoubleNumber whatIfTotal;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UsageByClient : SummaryItem
//    {

//        /// <remarks/>
//        public string userId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("sourceSearchResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class SourceSearchResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("source")]
//        public Source[] source;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("articlePopularityResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ArticlePopularityResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("articlePopularity")]
//        public ArticlePopularity[] articlePopularity;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ArticlePopularity
//    {

//        /// <remarks/>
//        public string accessionNo;

//        /// <remarks/>
//        public int fetchcount;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("passwordResetResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PasswordResetResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public string token;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("businessDescription", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class BusinessDescription : ResponseMessage
//    {

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        public Provider provider;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UsageAccountInfo : ResponseMessage
//    {

//        /// <remarks/>
//        public string companyName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("address")]
//        public string[] address;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("city")]
//        public string[] city;

//        /// <remarks/>
//        public string state;

//        /// <remarks/>
//        public string zip;

//        /// <remarks/>
//        public string countryCode;

//        /// <remarks/>
//        public string countryName;

//        /// <remarks/>
//        public string accessLevel;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime dateRange;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("codes")]
//        public string[] codes;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("industryFinancials", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IndustryFinancials : ResponseMessage
//    {

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        public Financials financials;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("headlineSearchResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class HeadlineSearchResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public ContentSearchResult contentSearchResult;

//        /// <remarks/>
//        public Person person;

//        /// <remarks/>
//        public Department department;

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Region region;

//        /// <remarks/>
//        public Source source;

//        /// <remarks/>
//        public NewsSubject newsSubject;

//        /// <remarks/>
//        public string query;

//        /// <remarks/>
//        public IqProcess iqProcess;

//        /// <remarks/>
//        public PersonalContentResponse personalContentResponse;

//        /// <remarks/>
//        public CompanyQuote companyQuote;

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public int folderId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderIdSpecified;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(HeadlineSearchType._Unspecified)]
//        public HeadlineSearchType searchType = HeadlineSearchType._Unspecified;

//        /// <remarks/>
//        public HeadlineFormat headlineFormat;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isWebPageBlocked = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isPictureBlocked = false;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("personalContentResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PersonalContentResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(PersonalContentType._Unspecified)]
//        public PersonalContentType personalContentType = PersonalContentType._Unspecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("watchlist")]
//        public Watchlist[] watchlist;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companyQuote")]
//        public CompanyQuote[] companyQuote;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("personalContentType", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum PersonalContentType
//    {

//        /// <remarks/>
//        _Unspecified,

//        /// <remarks/>
//        DefaultAll,

//        /// <remarks/>
//        DefaultIwe,

//        /// <remarks/>
//        WatchlistAll,

//        /// <remarks/>
//        WatchlistIwe,

//        /// <remarks/>
//        RecentCompanySearches,

//        /// <remarks/>
//        RemoveRecentCompanySearch,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Watchlist
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("folderInfo")]
//        public FolderInfo[] folderInfo;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public FolderCategory category;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum FolderCategory
//    {

//        /// <remarks/>
//        Undefined,

//        /// <remarks/>
//        Customer,

//        /// <remarks/>
//        Prospect,

//        /// <remarks/>
//        Competitor,

//        /// <remarks/>
//        Other,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("companyQuote", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CompanyQuote
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Instrument instrument;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("searchType", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum HeadlineSearchType
//    {

//        /// <remarks/>
//        _Unspecified,

//        /// <remarks/>
//        Advanced,

//        /// <remarks/>
//        IndustryReport,

//        /// <remarks/>
//        CompanyReport,

//        /// <remarks/>
//        News,

//        /// <remarks/>
//        KeyDevelopments,

//        /// <remarks/>
//        Track,

//        /// <remarks/>
//        EditorsChoice,

//        /// <remarks/>
//        LatestFromSource,

//        /// <remarks/>
//        LatestFromAuthor,

     
//        /// <remarks/>
//        LatestNews,

//        /// <remarks/>
//        ExecutiveNews,

//        /// <remarks/>
//        CompanyNews,

//        /// <remarks/>
//        IndustryNews,

//        /// <remarks/>
//        AccessionNo,

//        /// <remarks/>
//        MoreLikeThis,

//        GovernmentOfficialNews,

//        GovernmentOrganizationNews,

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("folderIdResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FolderIdResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("instrumentsResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class InstrumentsResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("instrument")]
//        public Instrument[] instrument;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("watchlistResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class WatchlistResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("watchlist")]
//        public Watchlist[] watchlist;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("folderHeadlinesResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FolderHeadlinesResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("folder")]
//        public Folder[] folder;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("folder", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Folder
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        public Status status;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public string bookmark;

//        /// <remarks/>
//        public string sessionmark;

//        /// <remarks/>
//        public EditorInfo editorInfo;

//        /// <remarks/>
//        public SharedFolderInfo sharedInfo;

//        /// <remarks/>
//        public string highlightString;

//        /// <remarks/>
//        public int queryHitCount;

//        /// <remarks/>
//        public int newHitsCount;

//        /// <remarks/>
//        public bool moreHeadlines;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("contentHeadline")]
//        public ContentHeadline[] contentHeadline;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("industrySalesByTier", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IndustrySalesByTier : ResponseMessage
//    {

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        public CompaniesBySalesTier companiesBySalesTier;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompaniesBySalesTier
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("salesTier")]
//        public SalesTier[] salesTier;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int totalCompanies;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool totalCompaniesSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SalesTier
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public CompanySize companySize;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool companySizeSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int count;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool countSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CompanySize
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Small,

//        /// <remarks/>
//        Medium,

//        /// <remarks/>
//        Large,

//        /// <remarks/>
//        ExtraLarge,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyStatus
//    {

//        /// <remarks/>
//        public CodeStatus activeStatus;

//        /// <remarks/>
//        public ListingStatus listingStatus;

//        /// <remarks/>
//        public bool isFocal;

//        /// <remarks/>
//        public bool isNewsCoded;

//        public bool isOccurrenceCoded;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(CompanySize.Unspecified)]
//        public CompanySize tierBySales = CompanySize.Unspecified;

//        /// <remarks/>
//        public bool isCompanyReportAvailable;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool isCompanyReportAvailableSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ListingStatus
//    {

//        /// <remarks/>
//        Listed,

//        /// <remarks/>
//        Unlisted,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyParent
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string dunsNumber;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;

//        /// <remarks/>
//        public bool isCompanyReportAvailable;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool isCompanyReportAvailableSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("secretary", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Secretary
//    {

//        /// <remarks/>
//        public Name name;

//        /// <remarks/>
//        public PhoneNumber phoneNumber;

//        /// <remarks/>
//        public PhoneNumber faxNumber;

//        /// <remarks/>
//        public string email;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("phoneNumber", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PhoneNumber
//    {

//        /// <remarks/>
//        public string countryRegionCode;

//        /// <remarks/>
//        public string cityAreaCode;

//        /// <remarks/>
//        public string number;

//        /// <remarks/>
//        public string extension;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanySymbology : Company
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportReference
//    {

//        /// <remarks/>
//        public Provider provider;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("contentHeadline")]
//        public ContentHeadline[] contentHeadline;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public bool isPrimary;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string @ref;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ReportReferenceType type;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool typeSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ReportReferenceSubType subType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool subTypeSpecified;

//        ///// <remarks>q307 multilanguage</remarks>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string language;
//    }

//    ///<summary>
//    /// This enumeratin is used to define all the report
//    /// reference types
//    ///</summary>
//    ///
//    /// <usedby> 
//    /// This is used by all pages and controls that retrieve and display reports.
//    ///</usedby>
//    ///
//    /// <chg>
//    ///		<bucket>Q107</bucket>
//    ///		<date>17-12-2007</date>
//    ///		<id>??</id>
//    /// This method was modified to add report reference types for Zacks company
//    /// and industry reports.
//    /// </chg>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ReportReferenceType
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        cogen,

//        /// <remarks/>
//        cokey,

//        /// <remarks/>
//        coabs,

//        /// <remarks/>
//        cotec,

//        /// <remarks/>
//        coacf,

//        /// <remarks/>
//        coais,

//        /// <remarks/>
//        cobsb,

//        /// <remarks/>
//        coibs,

//        /// <remarks/>
//        coicf,

//        /// <remarks/>
//        coiis,

//        /// <remarks/>
//        coovh,

//        /// <remarks/>
//        cocrp,

//        /// <remarks/>
//        coevt,

//        /// <remarks/>
//        coexe,

//        /// <remarks/>
//        corat,

//        /// <remarks/>
//        cocus,

//        /// <remarks/>
//        cogsb,

//        /// <remarks/>
//        cokcs,

//        /// <remarks/>
//        cokcr,

//        /// <remarks/>
//        copas,

//        /// <remarks/>
//        cosub,

//        /// <remarks/>
//        corta,

//        /// <remarks/>
//        colbd,

//        /// <remarks/>
//        costa,

//        /// <remarks/>
//        coswt,

//        /// <remarks/>
//        inrat,

//        /// <remarks/>
//        insrt,

//        /// <remarks/>
//        in500,

//        /// <remarks/>
//        ttfil,

//        /// <remarks/>
//        ivtxco,

//        /// <remarks/>
//        ivtxin,

//        /// <remarks/>
//        exbio,

//        zacksco,

//        zacksin,
//        ///<remarks>q307 Hoppenstedt fins and key figures</remarks>
//        cokfg,
//        /// <remarks>Hoppenstedt fins</remarks>
//        coabc,
//        /// <remarks>Hoppenstedt fins</remarks>
//        coaic,
//        /// <remarks>Hoppenstedt fins</remarks>
//        coabu,
//        /// <remarks>Hoppenstedt fins</remarks>
//        coaiu,
//        /// <remarks>Hoppenstedt fins</remarks>
//        cortc,
//        /// <remarks>Hoppenstedt fins</remarks>
//        cortu,
//        ///<remarks>q307 CredInform</remarks>
//        coimp,
//        ///<remarks>q307 CredInform</remarks>
//        coreg,
//        ///<remarks>q307 CredInform</remarks>
//        coshh,

//        // mt 08/2007 - add company brands
//        /// <remarks/>
//        cotrn,
//    }

//    ///<summary>
//    /// This enumeratin is used to define all the report
//    /// reference sub types
//    ///</summary>
//    ///
//    /// <usedby> 
//    /// This is used by all pages and controls that retrieve and display reports.
//    ///</usedby>
//    ///
//    /// <chg>
//    ///		<bucket>Q107</bucket>
//    ///		<date>17-12-2007</date>
//    ///		<id>??</id>
//    /// This method was modified to add report reference subtypes for investext and
//    /// zachs reports.Since the screening database has the investext and zacks reports
//    /// we need to define the subtype in order to retrieve either of the reports.
//    /// </chg>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ReportReferenceSubType
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        tf08k,

//        /// <remarks/>
//        tf10k,

//        /// <remarks/>
//        tf10q,

//        /// <remarks/>
//        tf20f,

//        /// <remarks/>
//        tfprx,

//        /// <remarks/>
//        tfins,

//        /// <remarks/>
//        tfino,

//        /// <remarks/>
//        tfreg,
//        /// <remarks/>
//        zachs,
//        /// <remarks/>
//        invest,
//        /// <remarks/>
//        other,

//        /// <remarks/>
//        tf06k,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reportResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReportResponse
//    {

//        /// <remarks/>
//        public ReportListIdentifier reportListIdentifier;

//        /// <remarks/>
//        public CorporateEventsReport corporateEventsReport;

//        public TechnologyInformationReport technologyInformationReport;

//        // mt 08/2007 - add company brands
//        /// <remarks/>
//        public CompanyBrandsReport companyBrandsReport;

//        /// <remarks/>
//        public SubsidiariesAffiliatesReport subsidiariesAffiliatesReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("overviewAndHistoryReport")]
//        public OverviewAndHistoryReport[] overviewAndHistoryReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("productAndServicesReport")]
//        public ProductAndServicesReport[] productAndServicesReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("executivesandOfficersReport")]
//        public ExecutivesAndOfficersReport[] executivesandOfficersReport;

//        /// <remarks/>
//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("keyFinancialsReport")]
//        public KeyFinancialsReport[] keyFinancialsReport;

//        /// <remarks/>
//        public CustomerInformationReport customerInformationReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companyInformationReport")]
//        public CompanyInformationReport[] companyInformationReport;

//        /// <remarks/>
//        public KeyRatiosReport keyRatiosReport;

//        /// <remarks/>
//        public IndexAggregatesReport indexAggregatesReport;

//        /// <remarks/>
//        public IndustryAggregatesReport industryAggregatesReport;

//        /// <remarks/>
//        public IndustrySectorAggregatesReport industrySectorAggregatesReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("segmentReport")]
//        public SegmentReport[] segmentReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("keyCompetitorsReport")]
//        public KeyCompetitorsReport[] keyCompetitorsReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("executiveBiographyReport")]
//        public BiographyReport[] executiveBiographyReport;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("genericReport")]
//        public GenericReport[] genericReport;

//        /// <remarks/>
//        public UKRatiosReport ukRatiosReport;

//        /// <remarks/>
//        public USRatiosReport usRatiosReport;


//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("longBusinessDescriptionReport")]
//        public LongBusinessDescriptionReport[] longBusinessDescriptionReport;

//        /// <remarks/>
//        public SwotAnalysisReport swotAnalysisReport;

//        /// <remarks/>
//        public CompanyStatementReport companyStatementReport;

//        /// <remarks/>
//        public FactivaPeerComparison factivaPeerComparison;

//        /// <remarks/>
//        public FactivaCompany factivaCompanies;

//        /// <remarks/>
//        public FactivaSubsidiaries factivaSubsidiaries;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsCodedSearchResponse")]
//        public CodedNewsSearchReportResponse[] newsCodedSearchResponse;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("keyDevHeadlinesSearchResponse")]
//        public KeyDevHeadlinesSearchReportResponse[] keyDevHeadlinesSearchResponse;

//        /// <remarks/>
//        public ArticleResponse articleResponse;

//        /// <remarks/>
//        public Status status;

//        public PreferencesDTO preferencesDTO;

//        /// <remarks>q307 Hoppenstedt</remarks>
//        [System.Xml.Serialization.XmlElementAttribute("keyFiguresReport")]
//        public KeyFiguresReport keyFiguresReport;


//        /// <remarks>q307 CredInform</remarks>
//        [System.Xml.Serialization.XmlElementAttribute("importExportReport")]
//        public ImportExportReport importExportReport;

//        /// <remarks>q307 CredInform</remarks>
//        [System.Xml.Serialization.XmlElementAttribute("registrationReport")]
//        public RegistrationReport registrationReport;

//        /// <remarks>q307 CredInform</remarks>
//        [System.Xml.Serialization.XmlElementAttribute("shareholdersReport")]
//        public ShareholdersReport shareholdersReport;



//    }





//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class TechnologyInformationReport
//    {
//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        public TechnologyInformationReportData technologyInformationReportData;
//    }

//    /// <summary>
//    /// location specific numbers for technology reports (Harte-Hanks)
//    /// </summary>
//    public class TechnologyInformationReportData
//    {
//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("primaryGroupware")]
//        public string[] primaryGroupware;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("secondaryGroupware")]
//        public string[] secondaryGroupware;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("primaryERP")]
//        public string[] primaryERP;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("secondaryERP")]
//        public string[] secondaryERP;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("primaryCRM")]
//        public string[] primaryCRM;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("secondaryCRM")]
//        public string[] secondaryCRM;


//        public Number personalComputers;

//        public Number servers;

//        public Number phoneExtensions;

//        public bool broadband;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool broadbandSpecified;

//        public bool wifi;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wifiSpecified;

//        public bool erpSoftwareExistance;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool erpSoftwareExistanceSpecified;

//        public bool crmSoftwareExistance;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool crmSoftwareExistanceSpecified;

//        /// <remarks/>
//        public ExecutivesAndOfficersReportData executivesAndOfficersReportData;

//        public DataAccuracy personalComputersAccuracy;
//        public DataAccuracy serversAccuracy;
//        public DataAccuracy phoneExtensionsAccuracy;

//        public EnterpriseTotals enterpriseTotals;


//    }

//    // mt 08/2007 - add company brands
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyBrandsReport
//    {
//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        public CompanyBrandsReportData companyBrandsReportData;
//    }

//    // mt 08/2007 - add company brands
//    /// <summary>
//    /// specific data for company brands report
//    /// </summary>
//    public class CompanyBrandsReportData
//    {
//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("tradeNameInfo")]
//        public TradeNameInfo[] tradeNameInfo;
//    }

//    // mt 08/2007 - add company brands
//    public class TradeNameInfo
//    {
//        public string tradeName;

//        [System.Xml.Serialization.XmlElementAttribute("genericProductCode")]
//        public BrandItem genericProductCode;

//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public BrandItem industry;
//    }

//    // mt 08/2007 - add company brands
//    public class BrandItem
//    {
//        public string descriptor;

//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string scheme;

//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    //q207
//    /// <summary>
//    /// container for identifying technology enterprise-wise (compared to at location)
//    /// </summary>
//    public class EnterpriseTotals
//    {
//        public Number personalComputers;
//        public DataAccuracy personalComputersAccuracy;
//        public Number servers;
//        public DataAccuracy serversAccuracy;
//        public Number printers;
//        public DataAccuracy printersAccuracy;
//        public Number storageCapacity;
//        public DataAccuracy storageCapacityAccuracy;
//        public Number highSpeedLines;
//        public DataAccuracy highSpeedLinesAccuracy;
//    }
//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CorporateEventsReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("events", IsNullable = false)]
//        public Event[] corporateEventsReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportMetaData
//    {

//        /// <remarks/>
//        public ReportMetaDataAccessionNo accessionNo;

//        /// <remarks/>
//        public FormattedDate publicationDate;

//        /// <remarks/>
//        public FormattedDate modificationDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        public Region region;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportMetaDataAccessionNo
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string fid;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Event
//    {

//        /// <remarks/>
//        public FormattedDate date;

//        /// <remarks/>
//        public string title;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SubsidiariesAffiliatesReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("subsidiariesAffiliates", IsNullable = false)]
//        public Company[] subsidiariesAffiliatesReportData;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("credSubsidiariesAffiliates", IsNullable = false)]
//        public CredSubsidiariesAffiliate[] credSubsidiariesAffiliatesReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class OverviewAndHistoryReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public OverviewAndHistoryReportOverviewAndHistoryReportData overviewAndHistoryReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class OverviewAndHistoryReportOverviewAndHistoryReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] overview;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] history;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportParagraphPara
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public ParagraphDisplay type;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool typeSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlTextAttribute()]
//        public string Value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ProductAndServicesReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public ProductAndServicesReportData productAndServicesReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ProductAndServicesReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] salesByLocation;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] salesByOperation;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ExecutivesAndOfficersReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public ExecutivesAndOfficersReportData executivesAndOfficersReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ExecutivesAndOfficersReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public Person associatedPerson;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("colleague", IsNullable = false)]
//        public ExecutiveOfficerDetail[] colleagues;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("officer", IsNullable = false)]
//        public ExecutiveOfficerDetail[] officers;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("director", IsNullable = false)]
//        public ExecutiveOfficerDetail[] directors;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ExecutiveOfficerDetail
//    {

//        /// <remarks/>
//        public Person person;

//        /// <remarks/>
//        public string level;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyFinancialsReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public KeyFinancialsReportData keyFinancialsReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyFinancialsReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        /// <remarks/>
//        public Number marketCap;

//        /// <remarks/>
//        public FormattedDate marketCapDate;

//        /// <remarks/>
//        public Number netProfitMargin;

//        /// <remarks/>
//        public Number profitMargin;

//        /// <remarks/>
//        public KeyFinancialFiscalPeriod lastAnnualFiscalPeriod;

//        /// <remarks/>
//        public KeyFinancialFiscalPeriod lastInterimFiscalPeriod;

//        /// <remarks/>
//        public KeyFinancialFiscalPeriod interimOneYearAgoFiscalPeriod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("previousAnnualFiscalPeriod")]
//        public KeyFinancialFiscalPeriod[] previousAnnualFiscalPeriod;

//        /// <remarks/>
//        public Number sharesOutstandingCommonStockPrimaryIssue;

//        /// <remarks/>
//        public Number sharesOutstandingPreferredStockPrimaryIssue;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DataAccuracy.NotAvailable)]
//        public DataAccuracy employeesDataAccuracy = DataAccuracy.NotAvailable;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DataAccuracy.NotAvailable)]
//        public DataAccuracy salesDataAccuracy = DataAccuracy.NotAvailable;

//        /// <remarks/>
//        public Auditor auditor;

//        /// <remarks/>
//        public FormattedDate filingDate;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyFinancialFiscalPeriod
//    {

//        /// <remarks/>
//        public FiscalPeriodDescription fiscalPeriodDescription;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyFinancialItem", IsNullable = false)]
//        public KeyFinancialItem[] keyFinancialItems;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class FiscalPeriodDescription
//    {

//        /// <remarks/>
//        public FormattedDate endDate;

//        /// <remarks/>
//        public string fiscalYear;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public int periodNumber = 0;

//        /// <remarks/>
//        public FormattedDate statementDate;

//        /// <remarks/>
//        public string updateType;

//        /// <remarks/>
//        public string sourceDocument;

//        /// <remarks/>
//        public FormattedDate filingDate;

//        /// <remarks/>
//        public Auditor auditor;

//        /// <remarks/>
//        public string auditorOpinion;

//        public WholeNumber numberOfEmployees;

//        public WholeNumber numberOfCompanies;

//        public string figuresInformation;

//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] auditorOpinionText;

//        public FiscalAccountingStandard fiscalAccountingStandard;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string type;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyFinancialItem
//    {

//        /// <remarks/>
//        public ItemDetails keyFinancialItemDetails;

//        /// <remarks/>
//        public FormattedDate ratioDate;

//        /// <remarks/>
//        public Number rawData;

//        public YearMonthDay ratioDateYear;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ItemDetails
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string multexCode;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string factivaCode;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool detail = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool summary = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string rollUp;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlTextAttribute()]
//        public string Value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CustomerInformationReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public CustomerInformationData customerInformationData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CustomerInformationData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("customerPeriod", IsNullable = false)]
//        public CustomerPeriod[] customerPeriods;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CustomerPeriod
//    {

//        /// <remarks/>
//        public FiscalPeriodDescription fiscalPeriodDescription;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("customerOrder")]
//        public CustomerOrder[] customerOrder;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CustomerOrder
//    {

//        /// <remarks/>
//        public string customerCode;

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public Number revenue;

//        /// <remarks/>
//        public Number revenuePercent;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyInformationReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public CompanyInformationReportData companyInformationReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyInformationReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public GeneralInformation generalInformation;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GeneralInformation
//    {

//        /// <remarks/>
//        public string name;

//        /// 
//        [System.Xml.Serialization.XmlElementAttribute("tradingName")]
//        public string[] tradingName;

//        /// <remarks/>
//        public string code;

//        /// <remarks/>
//        public CompanyLocationInformation companyLocationInformation;

//        /// <remarks/>
//        public string dunsNumber;

//        /// <remarks/>
//        public InstrumentReference primaryDJInstrument;

//        /// <remarks/>
//        public InstrumentReference primaryRicInstrument;

//        /// <remarks/>
//        public InstrumentReference primaryExchange;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("split", IsNullable = false)]
//        public Split[] splits;

//        /// <remarks/>
//        public CompanyPrimaryIndustryClassification primaryIndustryClassification;

//        /// <remarks/>
//        public SecondaryIndustryClassification secondaryIndustryClassification;

//        /// <remarks/>
//        public string businessDescription;

//        /// <remarks/>
//        public Person contact;

//        /// <remarks/>
//        public string topShareholder;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(OwnershipType.Unspecified)]
//        public OwnershipType ownershipType = OwnershipType.Unspecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("bankerDetails")]
//        public GeneralInformationBankerDetails[] bankerDetails;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("website", IsNullable = false)]
//        public Website[] websites;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("executive", IsNullable = false)]
//        public Person[] keyExecutives;

//        /// <remarks/>
//        public FamilyTreeInfo familyTreeInfo;

//        public LegalStatus legalStatus;

//        public string yearStarted;

//        public string employeesHere;

//        public RegistrationInfo registration;

//        public string inn;

//        public string okpo;
//    }

//    ///q307 Hoppenstedt q307 CredInform
//    /// [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://global.factiva.com/fvs/1.0")]
//    public class RegistrationInfo
//    {
//        /// <remarks/>
//        public string filingOfficeName;
//        /// <remarks/>
//        public string registrationId;
//        /// <remarks/>
//        public string description;
//        /// <remarks/>
//        public string eventDate;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("split", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Split
//    {
//        public FormattedDate splitDate;
//        public string splitValue;

//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("legalStatus", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class LegalStatus
//    {
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string scheme;
        
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
        
//        public string name;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyLocationInformation
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("address")]
//        public string[] address;

//        /// <remarks/>
//        public string city;

//        /// <remarks/>
//        public string state;

//        /// <remarks/>
//        public string zip;

//        /// <remarks/>
//        public string country;

//        /// <remarks/>
//        public string phone;

//        /// <remarks/>
//        public string phoneAreaCode;

//        /// <remarks/>
//        public string phoneCountryCode;

//        /// <remarks/>
//        public string fax;

//        /// <remarks/>
//        public string faxAreaCode;

//        /// <remarks/>
//        public string faxCountryCode;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("secondaryIndustryClassification", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class SecondaryIndustryClassification
//    {

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("sicIndustry")]
//        public Industry[] sicIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("naceIndustry")]
//        public Industry[] naceIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("naicsIndustry")]
//        public Industry[] naicsIndustry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("okvedIndustry")]
//        public Industry[] okvedIndustry;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GeneralInformationBankerDetails
//    {

//        /// <remarks/>
//        public string bankerNameAndAddress;

//        /// <remarks/>
//        public string bankerSortCode;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyRatiosReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public KeyRatiosReportData keyRatiosReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyRatiosReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioPanel", IsNullable = false)]
//        public KeyRatioPanel[] keyRatioPanels;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioPanel", IsNullable = false)]
//        public KeyRatioPanel[] financialHealthPanels;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioPanel", IsNullable = false)]
//        public KeyRatioPanel[] ratioComparisonPanels;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyRatioPanel
//    {
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string type;

//        /// <remarks/>
//        public string keyRatioName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioItem", IsNullable = false)]
//        public KeyRatioItem[] keyRatioItems;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyRatioItem
//    {


//        /// <remarks/>
//        public ItemDetails keyRatioItemDetails;

//        /// <remarks/>
//        public FormattedDate ratioDate;

//        /// <remarks/>
//        public Number rawData;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class IndexAggregatesReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public Industry500RatiosReportData industry500RatiosReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Industry500RatiosReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioPanel", IsNullable = false)]
//        public KeyRatioPanel[] ratioComparisonPanels;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class IndustryAggregatesReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public IndustryRatiosReportData industryRatiosReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class IndustryRatiosReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        ///q208
//        public string aggregatesDescriptor;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioPanel", IsNullable = false)]
//        public KeyRatioPanel[] ratioComparisonPanels;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class IndustrySectorAggregatesReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public IndustrySecRatiosReportData industrySecRatiosReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class IndustrySecRatiosReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        ///q208
//        public string aggregatesDescriptor;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("keyRatioPanel", IsNullable = false)]
//        public KeyRatioPanel[] ratioComparisonPanels;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SegmentReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public SegmentReportData segmentReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SegmentReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        /// <remarks/>
//        public SegmentFiscalPeriodDescriptions segmentFiscalPeriodDescriptions;

//        /// <remarks/>
//        public SegmentReportItems segmentReportItems;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SegmentFiscalPeriodDescriptions
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("fiscalPeriodDescription", IsNullable = false)]
//        public FiscalPeriodDescription[] annualFiscalPeriods;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("fiscalPeriodDescription", IsNullable = false)]
//        public FiscalPeriodDescription[] interimFiscalPeriods;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SegmentReportItems
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("categoryItem", IsNullable = false)]
//        public SegmentCategoryItem[] annualReportItems;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("categoryItem", IsNullable = false)]
//        public SegmentCategoryItem[] interimReportItems;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SegmentCategoryItem
//    {

//        /// <remarks/>
//        public SegmentCategoryItemSegment segment;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("reportItem", IsNullable = false)]
//        public ReportItem[] reportItems;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SegmentCategoryItemSegment
//    {

//        /// <remarks/>
//        public string descriptor;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string factivaCode;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportItem
//    {

//        /// <remarks/>
//        public ItemDetails itemDetails;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("fiscalPeriod", IsNullable = false)]
//        public FiscalPeriod[] fiscalPeriods;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class FiscalPeriod
//    {

//        /// <remarks/>
//        public FormattedDate endDate;

//        /// <remarks/>
//        public int fiscalYear;

//        /// <remarks/>
//        public Number rawData;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string type;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyCompetitorsReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public KeyCompetitorsReportData keyCompetitorsReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyCompetitorsReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("competitor", IsNullable = false)]
//        public Competitor[] competitors;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class BiographyReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public BiographyReportData biographyReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class BiographyReportData
//    {

//        /// <remarks/>
//        public Person generalInfo;

//        /// <remarks/>
//        public Biography biography;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("compensation")]
//        public Compensation[] compensation;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class Compensation
//    {

//        /// <remarks/>
//        public FormattedDate fiscalYearEnding;

//        /// <remarks/>
//        public Currency currency;

//        /// <remarks/>
//        public Number salary;

//        /// <remarks/>
//        public Number bonus;

//        /// <remarks/>
//        public Number totalShortTerm;

//        // mt 2008 - add totalLongTerm compensation
//        /// <remarks/>
//        public Number totalLongTerm;

//        /// <remarks/>
//        public Number otherShortTerm;

//        /// <remarks/>
//        public Number total;

//        /// <remarks/>
//        public Number longTermIncentive;

//        /// <remarks/>
//        public Number otherLongTerm;

//        /// <remarks/>
//        public Number numberOptionsExercised;

//        /// <remarks/>
//        public Number valueOptionsExercised;

//        /// <remarks/>
//        public Number numberOptionsExercisable;

//        /// <remarks/>
//        public Number valueOptionsExercisable;

//        /// <remarks/>
//        public Number numberOptionsUnexercised;

//        /// <remarks/>
//        public Number valueOptionsUnexercised;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GenericReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;


//        /// <remarks/>
//        public GenericReportData genericReportData;

//        /// <remarks/>
//        public string reportGroup;


//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GenericReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        /// <remarks/>
//        public string reportAccountingStandard;


//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("fiscalPeriodDescription", IsNullable = false)]
//        public FiscalPeriodDescription[] genericFiscalPeriodDescriptions;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("reportItem", IsNullable = false)]
//        public ReportItem[] reportItems;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UKRatiosReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public UKRatiosReportData ukRatiosReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UKRatiosReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("fiscalPeriodDescription", IsNullable = false)]
//        public FiscalPeriodDescription[] genericFiscalPeriodDescriptions;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("reportItem", IsNullable = false)]
//        public ReportItem[] reportItems;
//    }


//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class USRatiosReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public USRatiosReportData usRatiosReportData;
//    }


//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class USRatiosReportData
//    {
//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        public string reportCurrency;



//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("fiscalPeriodDescription", IsNullable = false)]
//        public FiscalPeriodDescription[] genericFiscalPeriodDescriptions;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("reportItem", IsNullable = false)]
//        public ReportItem[] reportItems;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("usRatioPanel", IsNullable = false)]
//        public USRatioPanel[] usRatioPanels;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class USRatioPanel
//    {
//        /// <remarks/>
//        public string usRatioName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("usRatioDataRow", IsNullable = false)]
//        public USRatioDataRow[] usRatioDataRows;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class USRatioDataRow
//    {
//        /// <remarks/>
//        public ItemDetails usRatioItemDetails;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("usRatioItem", IsNullable = false)]
//        public USRatioItem[] usRatioItems;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class USRatioItem
//    {
//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string type;

//        /// <remarks/>
//        public Number rawData;


//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class LongBusinessDescriptionReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public LongBusinessDescriptionReportLongBusinessDescriptionReportData longBusinessDescriptionReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class LongBusinessDescriptionReportLongBusinessDescriptionReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] businessDescription;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SwotAnalysisReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public SwotAnalysisReportSwotAnalysisReportData swotAnalysisReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SwotAnalysisReportSwotAnalysisReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] overview;

//        /// <remarks/>
//        public SummaryAndDetails strength;

//        /// <remarks/>
//        public SummaryAndDetails weakness;

//        /// <remarks/>
//        public SummaryAndDetails opportunity;

//        /// <remarks/>
//        public SummaryAndDetails threat;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class SummaryAndDetails
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] overview;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] details;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyStatementReport
//    {

//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        /// <remarks/>
//        public CompanyStatementReportCompanyStatementReportData companyStatementReportData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyStatementReportCompanyStatementReportData
//    {

//        /// <remarks/>
//        public string reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
//        public ReportParagraphPara[] companyStatement;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class FactivaPeerComparison
//    {

//        /// <remarks/>
//        public Competitors competitors;

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class FactivaCompany
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companies")]
//        public Company[] companies;

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class FactivaSubsidiaries
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companies")]
//        public Company[] companies;

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CodedNewsSearchReportResponse
//    {

//        /// <remarks/>
//        public CodedNewsType codedNewsType;

//        /// <remarks/>
//        public CodedNewsSearchResponse codedNewsSearchResponse;

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyDevHeadlinesSearchReportResponse
//    {

//        /// <remarks/>
//        public CodedNewsType codedNewsType;

//        /// <remarks/>
//        public HeadlineSearchResponse headlineSearchResponse;

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("articleResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ArticleResponse
//    {

//        /// <remarks/>
//        public ArticleResultSet articleResultSet;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool odeAdminBlocked = false;
//    }



//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("articleResultSet", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ArticleResultSet
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("article")]
//        public Article[] article;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int count;
//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getFolderInfoRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetFolderInfoRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("groupFolderIds")]
//        public string[] groupFolderIds;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("productId")]
//        public ProductId[] productId;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool inactiveFolder = false;

//        /// <remarks/>
//        public bool hitsCount;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("listFolderTypes")]
//        public ListFolderType[] listFolderTypes;



//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("codeDescription", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CodeDescription
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string lang;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlTextAttribute()]
//        public string Value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("addContinuousFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AddContinuousFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public Relevance relevance;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool relevanceSpecified = true;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public string userData;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DocumentFormat documentFormat;

//        /// <remarks/>
//        public DispositionType dispositionType;

//        /// <remarks/>
//        public DocumentType documentType;

//        /// <remarks/>
//        public PostMethod editorPostMethod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool editorPostMethodSpecified;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        /// <remarks/>
//        public string emailLanguage;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;



//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum Relevance
//    {
//        /// <remarks/>
//        Low,
//        /// <remarks/>
//        Medium,
//        /// <remarks/>
//        High,

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DocumentFormat
//    {

//        /// <remarks/>
//        Html,

//        /// <remarks/>
//        Plain,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DispositionType
//    {

//        /// <remarks/>
//        Attachment,

//        /// <remarks/>
//        Inline,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DocumentType
//    {

//        /// <remarks/>
//        FullTextDocument,

//        /// <remarks/>
//        FullTextWithIndexing,

//        /// <remarks/>
//        HeadlinesAndLeadSentences,
//        /// <summary>
//        /// ANN dotcom24
//        /// </summary>
//        HeadlineOnly,
//    }

   


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("capitalChangeReport", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CapitalChangeReport
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("capitalChange")]
//        public CapitalChange[] capitalChange;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reviseCompanyAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReviseCompanyAlertRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(FolderCategory.Prospect)]
//        public FolderCategory folderCategory = FolderCategory.Prospect;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubjects")]
//        public string[] newsSubjects;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companies")]
//        public string[] companies;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industries")]
//        public string[] industries;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("customTopics")]
//        public AlertCustomTopic[] customTopics;

//        /// <remarks/>
//        public bool allAnalystReports;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("regulatoryFillings")]
//        public RegulatoryFilling[] regulatoryFillings;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        ///<chg bucket="Q107" id="mobileheadlines" level="0"></chg>
//        public DocumentType documentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        public bool includePubAndWeb = true;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CompanyAlertSubject
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        ManagementMoves,

//        /// <remarks/>
//        NewProductsAndServices,

//        /// <remarks/>
//        ContractsAndOrders,

//        /// <remarks/>
//        EarningsReleases,

//        /// <remarks/>
//        MergersAndAcquisitions,

//        /// <remarks/>
//        Bankruptcy,

//        /// <remarks/>
//        LegalAndJudicial,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum RegulatoryFilling
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("10K")]
//        Item10K,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("10Q")]
//        Item10Q,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("8K")]
//        Item8K,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("4F")]
//        Item4F,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("20K")]
//        Item20K,

//        /// <remarks/>
//        Proxy,

//        /// <remarks/>
//        Insider,

//        /// <remarks/>
//        Institutional,

//        /// <remarks/>
//        Registrations,

//        /// <remarks/>
//        All,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DeliveryTime
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        AllDay,

//        /// <remarks/>
//        Morning,

//        /// <remarks/>
//        Afternoon,

//        /// <remarks/>
//        MorningAndAfternoon,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("moreLikeThisSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class MoreLikeThisSearchRequest
//    {

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public string searchFragment;

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        public bool includeSnippets;

//        /// <remarks/>
//        public DateFormat dateFormat;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool dateFormatSpecified;

//        /// <remarks/>
//        public string resultsHorizon;

//        /// <remarks/>
//        public string CodeScheme;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("dateFormat", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum DateFormat
//    {

//        /// <remarks/>
//        MMDDCCYY,

//        /// <remarks/>
//        DDMMCCYY,

//        /// <remarks/>
//        CCYYMMDD,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("userAuthorizationResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UserAuthorizationResponse
//    {

//        /// <remarks/>
//        public string accountId;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(AdministratorFlag.NotAdministrator)]
//        public AdministratorFlag administratorFlag = AdministratorFlag.NotAdministrator;

//        /// <remarks/>
//        public string productId;

//        /// <remarks/>
//        public string ruleSet;

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public UserType userType;

//        /// <remarks/>
//        public string sessionId;

//        /// <remarks/>
//        public string autoLoginToken;

//        /// <remarks/>
//        public AuthorizationMatrix authorizationMatrix;

//        public string lwrFlag;

//        public string emailLogin;

//        public int idleTimeout;

//        [XmlIgnore]
//        public bool idleTimeoutSpecified;

//        public int maxSession;

//        [XmlIgnore]
//        public bool maxSessionSpecified;

//        /// <remarks/>
//        public UserStatus userStatus;

//        public string erFlag;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum UserStatus
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Active,

//        /// <remarks/>
//        Inactive,

//        /// <remarks/>
//        Deleted,

//        /// <remarks/>
//        Suspended,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum AdministratorFlag
//    {

//        /// <remarks/>
//        AccountAdministrator,

//        /// <remarks/>
//        GroupAdministrator,

//        /// <remarks/>
//        NotAdministrator,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum UserType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Corporate,

//        /// <remarks/>
//        Individual,

//        /// <remarks/>
//        Creditcard,

//        /// <remarks/>
//        CustomerService,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class AuthorizationMatrix
//    {

//        /// <remarks/>
//        public MatrixArchiveService archive;

//        /// <remarks/>
//        public MatrixCibsService cibs;

//        /// <remarks/>
//        public MatrixTrackService track;

//        /// <remarks/>
//        public MatrixEmailService email;

//        /// <remarks/>
//        public MatrixIndexService index;

//        /// <remarks/>
//        public MatrixMdsService mds;

//        /// <remarks/>
//        public MatrixMembershipService membership;

//        /// <remarks/>
//        public MatrixNdsService nds;

//        /// <remarks/>
//        public MatrixSymbologyService symbology;

//        /// <remarks/>
//        public MatrixInterfaceService @interface;

//        /// <remarks/>
//        public MatrixUerService uer;

//        /// <remarks/>
//        public MatrixMigrationService migration;

//        /// <remarks/>
//        public MatrixRepmanService repman;

//        /// <remarks/>
//        public MatrixInsightService insight;

//        /// <remarks/>
//        public MatrixFSInterfaceService fsInterface;

//     }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixArchiveService : AuthorizationComponent
//    {

//        /// <remarks/>
//        public string company;

//        /// <remarks/>
//        public string region;

//        /// <remarks/>
//        public string industry;

//        /// <remarks/>
//        public string department;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixCibsService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixRepmanService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixEmailService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixIndexService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixMdsService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixTrackService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixArchiveService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixSymbologyService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixInterfaceService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixMigrationService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixMembershipService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixUerService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixNdsService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixInsightService))]
//    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MatrixFSInterfaceService))]
//    public class AuthorizationComponent
//    {

//        /// <remarks/>
//        public string ac1;

//        /// <remarks/>
//        public string ac2;

//        /// <remarks/>
//        public string ac3;

//        /// <remarks/>
//        public string ac4;

//        /// <remarks/>
//        public string ac5;

//        /// <remarks/>
//        public string ac6;

//        /// <remarks/>
//        public string ac7;

//        /// <remarks/>
//        public string ac8;

//        /// <remarks/>
//        public string ac9;

//        /// <remarks/>
//        public string ac10;

//        /// <remarks/>
//        public string da1;

//        /// <remarks/>
//        public string da2;

//        /// <remarks/>
//        public string da3;

//        /// <remarks/>
//        public string da5;

//        public string da7;

//        public string ads;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixCibsService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixRepmanService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixInsightService : AuthorizationComponent
//    {
//        public string dbId;
//        public string userId;
//        public string password;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixFSInterfaceService : AuthorizationComponent
//    {
       
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixEmailService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixIndexService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixMdsService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixTrackService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixSymbologyService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixInterfaceService : AuthorizationComponent
//    {
//        public string insider;
//        public string rCenter;
//        public string searchAssist;
//        public string salesworksPartner;
//        public string mcemail;/*sm _030909 for blocking mobile email cookie*/
//        public string duLinkBuilder;/*sm _030909 for blocking mobile email cookie*/
//        public string cvd;/* CVDLink */

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixMigrationService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixMembershipService : AuthorizationComponent
//    {
//        public string personalization;

//        public string sharing;

//        public string multimedia;

//        public string newsletterDA;

//        /// <summary>
//        /// added by SM - 1/26/09; for moving newsvliews link to dotcom and permission around it
//        /// </summary>
//        public string gripAdmin;

//        /// <summary>
//        /// added by SM - 1/26/09; for moving newsvliews link to dotcom and permission around it
//        /// </summary>
//        public string gripDefault;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixUerService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class MatrixNdsService : AuthorizationComponent
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getArticleRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetArticleRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("reference", IsNullable = false)]
//        public string[] references;

//        /// <remarks/>
//        public string responseDataSet;

//        /// <remarks/>
//        public string highlightString;

//        /// <remarks/>
//        public string usageAggregator;

//        /// <summary>
//        /// This flag is used to retreview websites and multimedia. 
//        /// </summary>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool websitesHeadlines = false;

//        public MetricDataContainer metricDataContainer;

//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIworksResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("company")]
//        public Company[] company;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("source")]
//        public Source[] source;

//        /// <remarks/>
//        public IworksResponseType iworksResponseType;

//        /// <remarks/>
//        public PersonalContentResponse personalContentResponse;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IworksResponseType
//    {

//        /// <remarks/>
//        Preferences,

//        /// <remarks/>
//        CompanyList,

//        /// <remarks/>
//        SourceList,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getChartDataRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetChartDataRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("instrument")]
//        public string[] instrument;

//        /// <remarks/>
//        public string instrumentType;

//        /// <remarks/>
//        public CodeType codeType;

//        /// <remarks/>
//        public Periodicity periodicity;

//        /// <remarks/>
//        public string usage;

//        /// <remarks/>
//        public string returnFormat;

//        /// <remarks/>
//        public string returnAttributes;

//        /// <remarks/>
//        public string adjust;

//        /// <remarks/>
//        public TimeRange timeRange;

//        /// <remarks/>
//        public string dateRange;

//        /// <remarks/>
//        public int numOfPrice;

//        /// <remarks/>
//        public DateRangeFormat dateRangeFormat;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CodeType
//    {

//        /// <remarks/>
//        DJ,

//        /// <remarks/>
//        FCODE,

//        /// <remarks/>
//        RIC,

//        /// <remarks/>
//        TLID,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TimeRange
//    {

//        /// <remarks/>
//        Past1Month,

//        /// <remarks/>
//        Past2Months,

//        /// <remarks/>
//        Past3Months,

//        /// <remarks/>
//        Past6Months,

//        /// <remarks/>
//        Past1Year,

//        /// <remarks/>
//        Past2Years,

//        /// <remarks/>
//        Past5Years,

//        /// <remarks/>
//        Past10Years,

//        /// <remarks/>
//        Past20Years,

//        /// <remarks/>
//        YearToDate,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DateRangeFormat
//    {

//        /// <remarks/>
//        YYYYMMDD,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("groupPermissionFlag", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum GroupPermissionFlag
//    {

//        /// <remarks/>
//        DefaultToParent,

//        /// <remarks/>
//        Block,

//        /// <remarks/>
//        Allow,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("companyScreeningResultSet", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CompanyScreeningResultSet
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companyScreeningResult")]
//        public CompanyScreeningResultType[] companyScreeningResult;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int total;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool contentRestricted = false;

//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public bool hasPremiumContent;
        
//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int first;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int last;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int count;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CompanyScreeningResultType
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Financials financials;

//        /// <remarks/>
//        public Instrument instrument;

//        [System.Xml.Serialization.XmlElementAttribute("newsData")]
//        public NewsDataType[] newsData;

//        [System.Xml.Serialization.XmlElementAttribute("bank")]
//        public Bank[] banks;

//    }


//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ExecutiveScreeningResultType
//    {

//        /// <remarks/>
//        public Person executive;

//        /// <remarks/>
//        public Financials financials;


//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("marketIndexList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class MarketIndexList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("instrument")]
//        public Instrument[] instrument;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("artWork", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class HighlightField
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("eLink", typeof(ELink))]
//        [System.Xml.Serialization.XmlElementAttribute("text", typeof(Text))]
//        [System.Xml.Serialization.XmlElementAttribute("hlt", typeof(HighlightedText))]
//        public object[] Items;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("GetIndustrySnapshot", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIndustrySnapshotRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(IndustryCodeScheme.FII)]
//        public IndustryCodeScheme industryCodeScheme = IndustryCodeScheme.FII;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(IndustryQueryType.Code)]
//        public IndustryQueryType queryType = IndustryQueryType.Code;

//        /// <remarks/>
//        public string queryString;

//        /// <remarks/>
//        public CodedNewsType newsType;

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool expandCodes = true;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("industryCodeScheme", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum IndustryCodeScheme
//    {

//        /// <remarks/>
//        FII,

//        /// <remarks/>
//        SIC,

//        /// <remarks/>
//        NACE,

//        /// <remarks/>
//        NAICS,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IndustryQueryType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Code,

//        /// <remarks/>
//        Name,

//        /// <remarks/>
//        Description,

//        /// <remarks/>
//        DescriptorAndAliases,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ScreenerResponse
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("region")]
//        public GenericScreeningList<Region> region;

//        [System.Xml.Serialization.XmlElementAttribute("newsSubject")]
//        public GenericScreeningList<NewsSubject> newsSubject;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("exchange")]
//        public Exchange[] exchange;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("instrument", IsNullable = false)]
//        public Instrument[] marketIndexList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("auditor", IsNullable = false)]
//        public Auditor[] auditorList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("region", IsNullable = false)]
//        public Region[] countryList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("region", IsNullable = false)]
//        public Region[] stateProvinceList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry", IsNullable = false)]
//        public GenericScreeningList<Industry> industry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("currency", IsNullable = false)]
//        public Currency[] currencyList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("managementLevel", IsNullable = false)]
//        public ManagementLevel[] managementLevelList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("degree", IsNullable = false)]
//        public Degree[] degreeList;
        
//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("department", IsNullable = false)]
//        public Department[] departmentList;

//        [System.Xml.Serialization.XmlArrayItemAttribute("locationType", IsNullable = false)]
//        public LocationType[] locationTypes;

//        /// <remarks/>
//        public PersonalContentResponse personalContentResponse;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("savedCompanyList", IsNullable = false)]
//        public SavedCompanyListListViewDTO companyLists;

//      [System.Xml.Serialization.XmlArrayItemAttribute("legalStatus", IsNullable = false)]
//        public LegalStatus[] legalStatusList;

//        [System.Xml.Serialization.XmlArrayItemAttribute("bank", IsNullable = false)]
//        public Bank[] bankList;

//        [System.Xml.Serialization.XmlElementAttribute("sharedCompanyList", IsNullable = false)]
//        public SharedCompanyListResponse sharedCompanyLists;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("lookupIndustries", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class LookupIndustries
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public Industry[] industry;

//        /// <remarks/>
//        public Status status;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("savedCompanyList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class SavedCompanyList
//    {

//        /// <remarks/>
//        public string name;

//        /// <remarks/>
//        public bool group;

//        /// <remarks/>
//        public long id;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("value")]
//        public string[] value;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("dateFormatPreference", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum DateFormatPreference
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        mdy,

//        /// <remarks/>
//        dmy,

//        /// <remarks/>
//        iso,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("emptyRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class EmptyRequest
//    {
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("sourceSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class SourceSearchRequest
//    {

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SourceResponseType.Brief)]
//        public SourceResponseType responseType = SourceResponseType.Brief;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SourceResponseType
//    {

//        /// <remarks/>
//        Brief,

//        /// <remarks/>
//        Verbose,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("routingSlip", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class RoutingSlip
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("route")]
//        public Route[] route;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum Route
//    {

//        /// <remarks/>
//        MainHeadline,

//        /// <remarks/>
//        DateFormatting,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("addOnlineFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AddOnlineFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public Relevance relevance;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool relevanceSpecified = true;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public string userData;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public PostMethod editorPostMethod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool editorPostMethodSpecified;

//        /// <remarks/>
//        public long selectId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool selectIdSpecified;

//        /// <remarks/>
//        public bool deliveryToAssociatedFeed;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool deliveryToAssociatedFeedSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetCompanyCustomReportRequest
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("reportTypes")]
//        public ReportTypes[] reportTypes;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsCodedNewsTypes")]
//        public CodedNewsType[] newsCodedNewsTypes;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("keyDevelopmentsCodedNewsTypes")]
//        public CodedNewsType[] keyDevelopmentsCodedNewsTypes;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool getKeyDevelopmentsArticle = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool getNewsArticles = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool getCompanyPeerComparison = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool getFactivaSubsidiaries = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(CompositeBillingReportType._Undefined)]
//        public CompositeBillingReportType metricsReportName = CompositeBillingReportType._Undefined;

//        [System.ComponentModel.DefaultValueAttribute(null)]
//        public string commonCompanyName;

//        public PreferencesDTO preferencesDTO;

//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool getExecutiveBriefing = false;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ReportTypes
//    {

//        /// <remarks/>
//        AnnualBalanceSheet,

//        /// <remarks/>
//        AnnualCashFlow,

//        /// <remarks/>
//        AnnualIncomeStatement,

//        /// <remarks/>
//        DNBPCIAnnualBalanceSheet,

//        /// <remarks/>
//        DNBPCIAnnualIncomeStatement,

//        /// <remarks/>
//        DNBPCIKeyRatios,

//        /// <remarks/>
//        DNBComparableFinancials,

//        /// <remarks/>
//        DNBPCIKeyFinancials,

//        /// <remarks/>
//        InterimBalanceSheet,

//        /// <remarks/>
//        InterimCashFlow,

//        /// <remarks/>
//        InterimIncomeStatemnet,

//        /// <remarks/>
//        GeographicSegmentBreakDown,

//        /// <remarks/>
//        BusinessSegmentBreakDown,

//        /// <remarks/>
//        OverviewAndHistory,

//        /// <remarks/>
//        KeyRatios,

//        /// <remarks/>
//        KeyFinancials,

//        /// <remarks/>
//        KeyFinancialsLastFiscalPeriod,

//        /// <remarks/>
//        DataMonitorLongBusinessDescription,

//        /// <remarks/>
//        DataMonitorCompetitionList,

//        /// <remarks/>
//        DataMonitorBusinessDescription,

//        /// <remarks/>
//        DataMonitorProductsAndServices,

//        /// <remarks/>
//        DataMonitorOfficersAndExecutives,

//        /// <remarks/>
//        DataMonitorOverviewAndHistory,

//        /// <remarks/>
//        ReutersCompetitionList,

//        /// <remarks/>
//        OfficersAndExecutives,

//        /// <remarks/>
//        ProductsAndServices,

//        /// <remarks/>
//        RegulatoryFilings,

//        /// <remarks/>
//        MajorCustomers,

//        /// <remarks/>
//        CompanyGeneralInfoPrimary,

//        /// <remarks/>
//        CompanyGeneralInfoSeconday,

//        /// <remarks/>
//        IndustryRatios,

//        /// <remarks/>
//        IndustrySecRatios,

//        /// <remarks/>
//        IndustrySandPRatios,

//        /// <remarks/>
//        CoporateEvents,

//        /// <remarks/>
//        KeySubsidiaries,

//        /// <remarks/>
//        UKKeyRatios,

//        /// <remarks/>
//        LongBusinessDescription,

//        /// <remarks/>
//        CompanyStatement,

//        /// <remarks/>
//        SwotAnalysis,

//        /// <remarks/>
//        TechnologyInformation,

//        ///<remarks/>
//        ZoomInfoBusinessDescription,

//        ///<remarks/> 
//        HoppfLongBusinessDescription,
//        ///<remarks>q307 Hoppenstedt/CredInform</remarks>
//        ///<remarks> Hoppenstedt Key Figures</remarks>
//        KeyFiguresHoppenstedt,
//        ///<remarks>Hoppenstedt fins</remarks>
//        AnnualBalanceSheetGermanConsolidated,
//        ///<remarks>Hoppenstedt fins</remarks>
//        AnnualBalanceSheetGermanUnconsolidated,
//        ///<remarks>Hoppenstedt fins</remarks>
//        AnnualIncomeStatementGermanConsolidated,
//        ///<remarks>Hoppenstedt fins</remarks>
//        AnnualIncomeStatementGermanUnconsolidated,
//        /// <remarks>Hoppenstedt fins</remarks>,
//        RatiosBGermanConsolidated,
//        /// <remarks>Hoppenstedt fins</remarks>,
//        RatiosBGermanUnconsolidated,
//        ///<remarks>q307 CredInform</remarks>
//        ImportExportTable,
//        ///<remarks>q307 CredInform</remarks>
//        RegistrationTable,
//        ///<remarks>q307 CredInform</remarks>
//        ShareholdersTable,
//        ///<remarks>q407 ExecBriefing</remarks>
//        ExecutiveBriefingReport,

//        // mt 08/2007 - add company brands
//        /// <remarks/>
//        CompanyBrands,
//        //os Q108 
//        DNBKeyFinancialsSecondary,

//        ///<remarks>Generate</remarks>
//        GenerateBusinessDescription
//    }
//    //q307 hoppenstetd

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CompositeBillingReportType
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        CompanySnapshot,

//        /// <remarks/>
//        IndustrySnapshot,

//        /// <remarks/>
//        CustomReport,

//        /// <remarks/>
//        FinancialHealth,

//        /// <remarks/>
//        UKFinancialHealth,

//        /// <remarks/>
//        DetailedCompanyProfileReport,

//        /// <remarks/>
//        DetailedCompanyProfileReportWithFinancials,

//        /// <remarks/>
//        KeyRatios,

//        /// <remarks/>
//        RatioComparisonReport,

//        /// <remarks/>
//        CorporateAffiliations,

//        ///<remarks/>//q407
//        ExecutiveBriefingReport,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("companyLists", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CompanyLists
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("savedCompanyList")]
//        public SavedCompanyList[] savedCompanyList;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("managementLevelList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ManagementLevelList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("managementLevel")]
//        public ManagementLevel[] managementLevel;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("iqRefineSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IqRefineSearchRequest
//    {

//        /// <remarks/>
//        public string iqTree;

//        /// <remarks/>
//        public string refineCode;

//        /// <remarks/>
//        public bool executeQuery;

//        /// <remarks/>
//        public int minimumHits;

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchSortOrder.Relevance)]
//        public SearchSortOrder sortOrder = SearchSortOrder.Relevance;

//        /// <remarks/>
//        public bool includeSnippets;

//        /// <remarks/>
//        public bool includeDescriptors;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchContentCategory.Publications)]
//        public SearchContentCategory channel = SearchContentCategory.Publications;

//        /// <remarks/>
//        public string contentLanguage;

//        /// <remarks/>
//        public bool restrictSources;

//        /// <remarks/>
//        public string sourceList;

//        /// <remarks/>
//        public string fcode;

//        /// <remarks/>
//        public string wordProximity;

//        /// <remarks/>
//        public bool alternateContext;

//        /// <remarks/>
//        public string rankingString;

//        /// <remarks/>
//        public string searchFragment;

//        /// <remarks/>
//        public string pictureSearchFragment;

//        /// <remarks/>
//        public string websiteSearchFragment;

//        /// <remarks/>
//        public string spinBss;

//        /// <remarks/>
//        public bool replaceCompanyName;

//        /// <remarks/>
//        public bool addCodeToBss;

//        /// <remarks/>
//        public bool keepDeletedCodes;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SearchSortOrder
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        PublicationDateMostRecentFirst,

//        /// <remarks/>
//        PublicationDateOldestFirst,

//        /// <remarks/>
//        Relevance,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SearchContentCategory
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Publications,

//        /// <remarks/>
//        Pictures,

//        /// <remarks/>
//        WebSites,

//        /// <remarks/>
//        Multimedia,

//        /// <remarks/>
//        Blogs,

//        /// <remarks/>
//        NewsSites,

//        /// <remarks/>
//        Video,

//        /// <remarks/>
//        Audio,

//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("peerComparisonResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PeerComparisonResponse
//    {

//        /// <remarks/>
//        public ReportResponse reportResponse;

//        /// <remarks/>
//        public Competitors competitorsResponse;

//        /// <remarks/>
//        public Industry industryResponse;

//        [System.Xml.Serialization.XmlElementAttribute("industryParents")]
//        public Industry[] industryParents;

//        [System.Xml.Serialization.XmlElementAttribute("prefSpreadsheetSaveAs")]
//        public SpreadsheetSaveAsPreferenceDTO prefSpreadsheetSaveAs;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("userAuthorizationRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UserAuthorizationRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("userLists")]
//        public UserLists[] userLists;

//        /// <remarks/>
//        public string serviceName;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class UserLists
//    {

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public string @namespace;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("interfaceLanguage", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum InterfaceLanguage
//    {

//        /// <remarks/>
//        Default,

//        /// <remarks/>
//        ChineseSimplified,

//        /// <remarks/>
//        ChineseTaiwan,

//        /// <remarks/>
//        English,

//        /// <remarks/>
//        French,

//        /// <remarks/>
//        German,

//        /// <remarks/>
//        Italian,

//        /// <remarks/>
//        Japaneese,

//        /// <remarks/>
//        Russian,

//        /// <remarks/>
//        Spanish,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("iqSearchContinuationRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IqSearchContinuationRequest
//    {

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        public bool includeSnippets;

//        /// <remarks/>
//        public string searchContext;

//        /// <remarks/>
//        public int firstResultToReturn;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("snippetDisplay", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum SnippetDisplay
//    {

//        /// <remarks/>
//        None,

//        /// <remarks/>
//        Inline,

//        /// <remarks/>
//        MouseOver,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("addIndustryAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AddIndustryAlertRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(FolderCategory.Other)]
//        public FolderCategory folderCategory = FolderCategory.Other;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubjects")]
//        public string[] newsSubjects;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("customTopics")]
//        public AlertCustomTopic[] customTopics;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industryReports")]
//        public IndustryAlertReport[] industryReports;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industryPublications")]
//        public IndustryAlertPublications[] industryPublications;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        ///<chg bucket="Q107" id="mobileheadlines"></chg>
//        public DocumentType documentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        public bool includePubAndWeb = true;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IndustryAlertSubject
//    {

//        /// <remarks/>
//        Undefined,

//        /// <remarks/>
//        LegalAndJudicial,

//        /// <remarks/>
//        RegulationAndGovernmentPolicy,

//        /// <remarks/>
//        StockListings,

//        /// <remarks/>
//        Performance,

//        /// <remarks/>
//        Bankruptcy,

//        /// <remarks/>
//        FundingAndCapital,

//        /// <remarks/>
//        OwnershipChanges,

//        /// <remarks/>
//        NewProductsAndServices,

//        /// <remarks/>
//        ContractsAndOrders,

//        /// <remarks/>
//        MonopoliesAndAntitrust,

//        /// <remarks/>
//        ManagementIssues,

//        /// <remarks/>
//        LaborAndPersonnelIssues,

//        /// <remarks/>
//        Marketing,

//        /// <remarks/>
//        Advertising,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IndustryAlertReport
//    {

//        /// <remarks/>
//        Undefined,

//        /// <remarks/>
//        Investext,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IndustryAlertPublications
//    {

//        /// <remarks/>
//        Undefined,

//        /// <remarks/>
//        SAndP,

//        /// <remarks/>
//        Mergent,

//        /// <remarks/>
//        Freedonia,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("addBatchFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AddBatchFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public Relevance relevance;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool relevanceSpecified = true;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public string userData;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DocumentFormat documentFormat;

//        /// <remarks/>
//        public DispositionType dispositionType;

//        /// <remarks/>
//        public DocumentType documentType;

//        /// <remarks/>
//        public PostMethod editorPostMethod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool editorPostMethodSpecified;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        /// <remarks/>
//        public string emailLanguage;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        public string timeZone;

//        /// <remarks/>
//        public bool adjustDaylight;

//        /// <remarks/>
//        public FilterBy filterBy;

//        /// <remarks/>
//        public MaxResultPerDelivery maxResultPerDelivery;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool maxResultPerDeliverySpecified = true;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

       
//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum FilterBy
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Relevance,

//        /// <remarks/>
//        Date,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum MaxResultPerDelivery
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("10")]
//        Item10,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("20")]
//        Item20,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("30")]
//        Item30,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("40")]
//        Item40,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("50")]
//        Item50,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum RemoveDuplicate
//    {

//        /// <remarks/>
//        None,

//        /// <remarks/>
//        High,

//        /// <remarks/>
//        Medium,

     
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("sourceResultSet", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class SourceResultSet
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("source")]
//        public Source[] source;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int count;
//    }


   

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("iqSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IqSearchRequest
//    {

//        /// <remarks/>
//        public string queryString;

//        /// <remarks/>
//        public bool executeQuery;

//        /// <remarks/>
//        public int minimumHits;

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchSortOrder.Relevance)]
//        public SearchSortOrder sortOrder = SearchSortOrder.Relevance;

//        /// <remarks/>
//        public bool includeSnippets;

//        /// <remarks/>
//        public bool includeDescriptors;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchContentCategory.Publications)]
//        public SearchContentCategory channel = SearchContentCategory.Publications;

//        /// <remarks/>
//        public string contentLanguage;

//        /// <remarks/>
//        public bool restrictSources;

//        /// <remarks/>
//        public string sourceList;

//        /// <remarks/>
//        public string fcode;

//        /// <remarks/>
//        public string wordProximity;

//        /// <remarks/>
//        public bool alternateContext;

//        /// <remarks/>
//        public string rankingString;

//        /// <remarks/>
//        public string searchFragment;

//        /// <remarks/>
//        public string pictureSearchFragment;

//        /// <remarks/>
//        public string websiteSearchFragment;

//        /// <remarks/>
//        public string spinBss;

//        /// <remarks/>
//        public bool replaceCompanyName;

//        /// <remarks/>
//        public bool addCodeToBss;

//        /// <remarks/>
//        public bool keepDeletedCodes;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("topIndustries", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class TopIndustries
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public Industry[] industry;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("countryList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CountryList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("region")]
//        public Region[] region;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("fvsFault", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FvsFault
//    {

//        /// <remarks/>
//        public string message;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("folderHeadlinesRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FolderHeadlinesRequest
//    {

//        /// <remarks/>
//        public HeadlineFormat headlineFormat;

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        public TrackSortBy sortBy;

//        /// <remarks/>
//        public TrackContentCategory contentCategory;

//        /// <remarks/>
//        public EditorPostStatus postMethod;

//        /// <remarks/>
//        public string bookmark;

//        /// <remarks/>
//        public string sessionmark;

//        /// <remarks/>
//        public FolderHeadlineViewType viewType;

//        /// <remarks/>
//        public bool resetSessionMark;

//        /// <remarks/>
//        public bool preview;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        public string searchQuery;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TrackSortBy
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        ArrivalTime,

//        /// <remarks/>
//        PublicationDateMostRecentFirst,

//        /// <remarks/>
//        PublicationDateOldestFirst,

//        /// <remarks/>
//        Relevance,

//        /// <remarks/>
//        Priority,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TrackContentCategory
//    {

//        /// <remarks/>
//        All,

//        /// <remarks/>
//        Internal,

//        /// <remarks/>
//        Publications,

//        /// <remarks/>
//        WebSites,

//        /// <remarks/>
//        CompanyReport,

//        /// <remarks/>
//        IndustryReport,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum EditorPostStatus
//    {

//        /// <remarks/>
//        Posted,

//        /// <remarks/>
//        Unposted,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum FolderHeadlineViewType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        All,

//        /// <remarks/>
//        New,

//        /// <remarks/>
//        Session,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("groupTrackEmail", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GroupTrackEmail
//    {

//        /// <remarks/>
//        public GroupTrackEmailSetting groupTrackEmailSetting;

//        /// <remarks/>
//        public GroupTrackDeliverySetup morningDeliveryOption;

//        /// <remarks/>
//        public GroupTrackDeliverySetup afternoonDeliveryOption;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GroupTrackEmailSetting
//    {

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public TimeZone timeZone;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DocumentType.HeadlinesAndLeadSentences)]
//        public DocumentType documentType = DocumentType.HeadlinesAndLeadSentences;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DocumentFormat.Plain)]
//        public DocumentFormat documentFormat = DocumentFormat.Plain;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(DispositionType.Inline)]
//        public DispositionType dispositionType = DispositionType.Inline;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(MaxHitsPerFolderDelivery.Item10)]
//        public MaxHitsPerFolderDelivery maxHitsPerFolderDelivery = MaxHitsPerFolderDelivery.Item10;

//        /// <remarks/>
//        public bool wirelessFriendly;

      

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TimeZone
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-12:00) International Date Line West")]
//        GMT1200EniwetokKwajalein,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-11:00) Midway Island, Samoa")]
//        GMT1100MidwayIslandSamoa,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-10:00) Hawaii")]
//        GMT1000Hawaii,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-09:00) Alaska")]
//        GMT0900Alaska,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-08:00) Pacific Time (US & Canada)")]
//        GMT0800PacificTimeUSCanadaTijuana,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-07:00) Arizona")]
//        GMT0700Arizona,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-07:00) Mountain Time (US & Canada)")]
//        GMT0700MountainTimeUSCanada,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-06:00) Central America")]
//        GMT0600CentralAmerica,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-06:00) Central Time (US & Canada)")]
//        GMT0600CentralTimeUSCanada,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-06:00) Guadalajara, Mexico City, Monterrey - New")]
//        GMT0600MexicoCity,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-06:00) Saskatchewan")]
//        GMT0600Saskatchewan,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-05:00) Bogota, Lima, Quito, Rio Branco")]
//        GMT0500BogotaLimaQuito,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-05:00) Eastern Time (US & Canada)")]
//        GMT0500EasternTimeUSCanada,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-05:00) Indiana (East)")]
//        GMT0500IndianaEast,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-04:00) Atlantic Time (Canada)")]
//        GMT0400AtlanticTimeCanada,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-04:00) Caracas, La Paz")]
//        GMT0400CaracasLaPaz,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-04:00) Santiago")]
//        GMT0400Santiago,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-03:30) Newfoundland")]
//        GMT0330Newfoundland,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-03:00) Brasilia")]
//        GMT0300Brasilia,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-03:00) Buenos Aires, Georgetown")]
//        GMT0300BuenosAiresGeorgetown,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-03:00) Greenland")]
//        GMT0300Greenland,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-02:00) Mid-Atlantic")]
//        GMT0200MidAtlantic,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-01:00) Azores")]
//        GMT0100Azores,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-01:00) Cape Verde Is.")]
//        GMT0100CapeVerdeIs,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT) Casablanca, Monrovia, Reykjavik")]
//        GMTCasablancaMonrovia,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London")]
//        GMTGreenwichMeanTimeDublinEdinburghLisbonLondon,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna")]
//        GMT0100AmsterdamBerlinBernRomeStockholmVienna,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague")]
//        GMT0100BelgradeBratislavaBudapestLjubljanaPrague,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+01:00) Brussels, Copenhagen, Madrid, Paris")]
//        GMT0100BrusselsCopenhagenMadridParis,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb")]
//        GMT0100SarajevoSkopjeSofijaVilniusWarsawZagreb,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+01:00) West Central Africa")]
//        GMT0100WestCentralAfrica,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Athens, Bucharest, Istanbul")]
//        GMT0200AthensIstanbulMinsk,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Minsk")]
//        GMT0200Bucharest,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Cairo")]
//        GMT0200Cairo,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Harare, Pretoria")]
//        GMT0200HararePretoria,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius")]
//        GMT0200HelsinkiRigaTallinn,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Jerusalem")]
//        GMT0200Jerusalem,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+03:00) Baghdad")]
//        GMT0300Baghdad,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+03:00) Kuwait, Riyadh")]
//        GMT0300KuwaitRiyadh,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+03:00) Moscow, St. Petersburg, Volgograd")]
//        GMT0300MoscowStPetersburgVolgograd,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+03:00) Nairobi")]
//        GMT0300Nairobi,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+03:30) Tehran")]
//        GMT0330Tehran,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+04:00) Abu Dhabi, Muscat")]
//        GMT0400AbuDhabiMuscat,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+04:00) Yerevan")]
//        GMT0400BakuTbilisiYerevan,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+04:30) Kabul")]
//        GMT0430Kabul,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+05:00) Ekaterinburg")]
//        GMT0500Ekaterinburg,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+05:00) Islamabad, Karachi, Tashkent")]
//        GMT0500IslamabadKarachiTashkent,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi")]
//        GMT0530CalcuttaChennaiMumbaiNewDelhi,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+05:45) Kathmandu")]
//        GMT0545Kathmandu,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+06:00) Almaty, Novosibirsk")]
//        GMT0600AlmatyNovosibirsk,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+06:00) Astana, Dhaka")]
//        GMT0600AstanaDhaka,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+05:30) Sri Jayawardenepura")]
//        GMT0600SriJayawardenepura,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+06:30) Yangon (Rangoon)")]
//        GMT0630Rangoon,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+07:00) Bangkok, Hanoi, Jakarta")]
//        GMT0700BangkokHanoiJakarta,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+07:00) Krasnoyarsk")]
//        GMT0700Krasnoyarsk,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi")]
//        GMT0800BeijingChongqingHongKongUrumqi,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+08:00) Irkutsk, Ulaan Bataar")]
//        GMT0800IrkutskUlaanBataar,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+08:00) Kuala Lumpur, Singapore")]
//        GMT0800KualaLumpurSingapore,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+08:00) Perth")]
//        GMT0800Perth,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+08:00) Taipei")]
//        GMT0800Taipei,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+09:00) Osaka, Sapporo, Tokyo")]
//        GMT0900OsakaSapporoTokyo,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+09:00) Seoul")]
//        GMT0900Seoul,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+09:00) Yakutsk")]
//        GMT0900Yakutsk,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+09:30) Adelaide")]
//        GMT0930Adelaide,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+09:30) Darwin")]
//        GMT0930Darwin,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+10:00) Brisbane")]
//        GMT1000Brisbane,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+10:00) Canberra, Melbourne, Sydney")]
//        GMT1000CanberraMelbourneSydney,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+10:00) Guam, Port Moresby")]
//        GMT1000GuamPortMoresby,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+10:00) Hobart")]
//        GMT1000Hobart,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+10:00) Vladivostok")]
//        GMT1000Vladivostok,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+11:00) Magadan, Solomon Is., New Caledonia")]
//        GMT1100MagadanSolomonIsNewCaledonia,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+12:00) Auckland, Wellington")]
//        GMT1200AucklandWellington,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+12:00) Fiji, Kamchatka, Marshall Is.")]
//        GMT1200FijiKamchatkaMarshallIs,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+13:00) Nuku\'alofa")]
//        GMT1300Nukualofa,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+04:00) Baku")]
//        GMT0400Baku,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-04:00) Manaus")]
//        GMT0400Manaus,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+03:00) Tbilisi")]
//        GMT0300Tbilisi,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Amman")]
//        GMT0200Amman,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Beirut")]
//        GMT0200Beirut,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-03:00) Montevideo")]
//        GMT0300Montevideo,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT+02:00) Windhoek")]
//        GMT0200Windhoek,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("(GMT-08:00) Tijuana, Baja California")]
//        GMT0800TijuanaBajaCalifornia,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum MaxHitsPerFolderDelivery
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("10")]
//        Item10,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("25")]
//        Item25,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("50")]
//        Item50,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("75")]
//        Item75,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GroupTrackDeliverySetup
//    {

//        /// <remarks/>
//        public string subject;

//        /// <remarks/>
//        public string setupId;

//        /// <remarks/>
//        public int deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("folderId")]
//        public string[] folderId;


//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("departmentList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DepartmentList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("department")]
//        public Department[] department;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("stateProvinceList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class StateProvinceList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("region")]
//        public Region[] region;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class PersonalContentRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(PersonalContentType._Unspecified)]
//        public PersonalContentType personalContentType = PersonalContentType._Unspecified;

//        [System.Xml.Serialization.XmlElementAttribute("companyCode")]
//        public string[] companyCode;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("references", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReferenceArray1To25
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("reference")]
//        public string[] reference;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("usageResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UsageResponse
//    {

//        /// <remarks/>
//        public UsageAccountInfo accountInfo;

//        /// <remarks/>
//        public UsageData usageData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIndustryListResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public Industry[] industry;

//        /// <remarks/>
//        public PersonalContentResponse personalContentResponse;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIworksRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(IworksRequestType.GetPreferences)]
//        public IworksRequestType iworksRequestType = IworksRequestType.GetPreferences;

//        /// <remarks/>
//        public string queryString;

//        /// <remarks/>
//        public CompanyQueryType companyQueryType;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(CompanySearchOperator.Contains)]
//        public CompanySearchOperator companySearchOperator = CompanySearchOperator.Contains;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(PersonalContentType._Unspecified)]
//        public PersonalContentType personalContentType = PersonalContentType._Unspecified;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public Source source;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IworksRequestType
//    {

//        /// <remarks/>
//        GetPreferences,

//        /// <remarks/>
//        LookupCompany,

//        /// <remarks/>
//        LookupSource,

//        /// <remarks/>
//        AddCompanyPreference,

//        /// <remarks/>
//        AddSourcePreference,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CompanyQueryType
//    {

//        /// <remarks/>
//        Default,

//        /// <remarks/>
//        DJ,

//        /// <remarks/>
//        RIC,

//        /// <remarks/>
//        DUNS,

//        /// <remarks/>
//        FCODE,

//        /// <remarks/>
//        HD,

//        /// <remarks/>
//        SmartSearch,

//        /// <remarks/>
//        RegistrationID,

//        /// <remarks/>
//        OrgCode
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CompanySearchOperator
//    {

//        /// <remarks/>
//        Equals,

//        /// <remarks/>
//        BeginsWith,

//        /// <remarks/>
//        Contains,

//        /// <remarks/>
//        IncludeAlias,

//        /// <remarks/>
//        CommonAliases,

//        // mt 2008 Q4 - misc - add option to screen companies by Trade Style Names
//        /// <remarks/>
//        IncludeAliasPlusTradeNames,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetQuoteRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("Symbol")]
//        public string[] Symbol;

//        /// <remarks/>
//        public bool QuickQuote;

//        /// <remarks/>
//        public CodeType CodeType;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetQuoteEXRequest
//    {

//        /// <remarks/>
//        [XmlElement("instruments")]
//        public InstrumentEX[] instruments;

//        /// <remarks/>
//        public bool QuickQuote;

       
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("GetIndustryList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIndustryListRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(IndustryListType.Root)]
//        public IndustryListType listType = IndustryListType.Root;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(IndustryCodeScheme.FII)]
//        public IndustryCodeScheme industryCodeScheme = IndustryCodeScheme.FII;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(PersonalContentType._Unspecified)]
//        public PersonalContentType personalContentType = PersonalContentType._Unspecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum IndustryListType
//    {

//        /// <remarks/>
//        All,

//        /// <remarks/>
//        Root,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("companyArchive", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CompanyArchive
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("company")]
//        public Company[] company;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("financials")]
//        public Financials[] financials;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIndustryReportRequest
//    {

//        /// <remarks/>
//        public Industry industry;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("clientCodeSpec", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ClientCodeSpec
//    {

//        /// <remarks/>
//        public string description;

//        /// <remarks/>
//        public string delimiter;

//        /// <remarks/>
//        public ClientCodeValidationType clientCodeValidationType;

//        /// <remarks/>
//        public ClientCodeInfoList clientCodeInfoList;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ClientCodeValidationType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Voluntary,

//        /// <remarks/>
//        Mandatory,

//        /// <remarks/>
//        Validated,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ClientCodeInfoList
//    {

//        /// <remarks/>
//        public ClientCodeInfo cc1;

//        /// <remarks/>
//        public ClientCodeInfo cc2;

//        /// <remarks/>
//        public ClientCodeInfo cc3;

//        /// <remarks/>
//        public ClientCodeInfo cc4;

//        /// <remarks/>
//        public ClientCodeInfo cc5;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ClientCodeInfo
//    {

//        /// <remarks/>
//        public string prompt;

//        /// <remarks/>
//        public string validationListName;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(ClientCodeSeverityFlag.Optional)]
//        public ClientCodeSeverityFlag severityFlag = ClientCodeSeverityFlag.Optional;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(ClientCodeRuleFlag.Other)]
//        public ClientCodeRuleFlag rule = ClientCodeRuleFlag.Other;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ClientCodeSeverityFlag
//    {

//        /// <remarks/>
//        Optional,

//        /// <remarks/>
//        Validated,

//        /// <remarks/>
//        Required,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ClientCodeRuleFlag
//    {

//        /// <remarks/>
//        Other,

//        /// <remarks/>
//        Alpha,

//        /// <remarks/>
//        Numeric,

//        /// <remarks/>
//        AlphaNumeric,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reviseOnlineFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReviseOnlineFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public Relevance relevance;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool relevanceSpecified = true;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public string userData;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public PostMethod editorPostMethod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool editorPostMethodSpecified;

//        /// <remarks/>
//        public long selectId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool selectIdSpecified;

//        /// <remarks/>
//        public bool deliveryToAssociatedFeed;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool deliveryToAssociatedFeedSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("auditorList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AuditorList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("auditor")]
//        public Auditor[] auditor;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetPartnerRedirectResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public string url;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("input")]
//        public GetPartnerRedirectResponseInput[] input;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GetPartnerRedirectResponseInput
//    {

//        /// <remarks/>
//        public string key;

//        /// <remarks/>
//        public string keyValue;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute("TailParagraphs", Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("tailParagraphs", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ArrayOfParagraph
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("paragraph")]
//        public Paragraph[] paragraph;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("degreeList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DegreeList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("degree")]
//        public Degree[] degree;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("deleteAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DeleteAlertRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetCompanyReportRequest
//    {

//        /// <remarks/>
//        public Company company;

//        public PreferencesDTO preferencesDTO;

//        /// <remarks/>
//        public ReportTypes reportType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool reportTypeSpecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("deleteFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class DeleteFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("indexSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IndexSearchRequest
//    {

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        public SearchSortOrder sortOrder;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool sortOrderSpecified;

//        /// <remarks/>
//        public bool includeSnippets;

//        /// <remarks/>
//        public DateFormat dateFormat;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool dateFormatSpecified;

//        /// <remarks/>
//        public string relevanceRank;

//        /// <remarks/>
//        public string headlineDiscriminator;

//        /// <remarks/>
//        public string countDiscriminator;

//        /// <remarks/>
//        public string resultsHorizon;

//        /// <remarks/>
//        public string CodeScheme;

//        /// <remarks/>
//        public string includeClipStatus;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("usageRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UsageRequest
//    {

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public string productId;

//        /// <remarks/>
//        public string accountId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime startDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime endDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime requestedDate;

//        /// <remarks/>
//        public string projectCode;

//        /// <remarks/>
//        public UsageType usageType;

//        /// <remarks/>
//        public int maxRecord;

//        /// <remarks/>
//        public int startRecord;

//        public int byMonthNum;
//    }


//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("headlineSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class HeadlineSearchRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(HeadlineSearchType._Unspecified)]
//        public HeadlineSearchType searchType = HeadlineSearchType._Unspecified;

//        /// <remarks/>
//        public string freeText;

//        /// <remarks/>
//        public string exactPhrase;

//        /// <remarks/>
//        public string allWords;

//        /// <remarks/>
//        public string orWords;

//        /// <remarks/>
//        public string notWords;

//        /// <remarks/>
//        public string searchContext;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public int headlineStart = 0;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(20)]
//        public int headlineCount = 20;

//        /// <remarks/>
//        public SearchContentCategory contentCategory;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchDateRange._Unspecified)]
//        public SearchDateRange dateRange = SearchDateRange._Unspecified;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchSortOrder.PublicationDateMostRecentFirst)]
//        public SearchSortOrder sortOrder = SearchSortOrder.PublicationDateMostRecentFirst;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(CodedNewsType._InvalidValue)]
//        public CodedNewsType newsType = CodedNewsType._InvalidValue;

//        /// <remarks/>
//        public string iqString;

//        /// <remarks/>
//        public string iqRefineCode;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(TruncationType.Large)]
//        public TruncationType truncationType = TruncationType.Large;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public int folderId = 0;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(PersonalContentType._Unspecified)]
//        public PersonalContentType personalContentType = PersonalContentType._Unspecified;

//        /// <remarks/>
//        public TrackContentCategory trackContentCategory;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("accessionNo")]
//        public string[] accessionNo;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("person")]
//        public Person[] person;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("source")]
//        public Source[] source;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("sourceExclude")]
//        public Source[] sourceExclude;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("sourceList")]
//        public PreferenceItem[] sourceList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("company")]
//        public Company[] company;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companyExclude")]
//        public Company[] companyExclude;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companyList")]
//        public PreferenceItem[] companyList;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companyListExclude")]
//        public PreferenceItem[] companyListExclude;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchOperator.And)]
//        public SearchOperator companyOperator = SearchOperator.And;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubject")]
//        public NewsSubject[] newsSubject;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubjectExclude")]
//        public NewsSubject[] newsSubjectExclude;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchOperator.And)]
//        public SearchOperator newsSubjectOperator = SearchOperator.And;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public Industry[] industry;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industryExclude")]
//        public Industry[] industryExclude;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchOperator.And)]
//        public SearchOperator industryOperator = SearchOperator.And;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("region")]
//        public Region[] region;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("regionExclude")]
//        public Region[] regionExclude;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(SearchOperator.And)]
//        public SearchOperator regionOperator = SearchOperator.And;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("contentLanguage")]
//        public ContentLanguage[] contentLanguage;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("searchFields")]
//        public string[] searchFields;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime fromDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool fromDateSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime toDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool toDateSpecified;

//        /// <remarks/>
//        public bool excludeRepublished;

//        /// <remarks/>
//        public bool excludeRecurring;

//        /// <remarks/>
//        public bool excludeObituaries;

      
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("accessionSearchRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AccessionSearchRequest
//    {
//        [System.Xml.Serialization.XmlElementAttribute("searchRequestItems")] 
//        public AccessionSearchRequestItem[] searchRequestItems;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(BriefcaseSortOrder.FIFO)]
//        public BriefcaseSortOrder sortOrder = BriefcaseSortOrder.FIFO;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(TruncationType.Large)]
//        public TruncationType truncationType = TruncationType.Large;

//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool returnHeadlines = true;

//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool includeSnippets;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool includeSnippetsSpecified;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("accessionSearchRequestItem", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AccessionSearchRequestItem
//    {
//        [System.Xml.Serialization.XmlElementAttribute("accessionNo")]
//        public string[] accessionNo;

//         /// <remarks/>
//        public SearchContentCategory contentCategory;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SearchDateRange
//    {

//        /// <remarks/>
//        _Unspecified,

//        /// <remarks/>
//        LastDay,

//        /// <remarks/>
//        LastWeek,

//        /// <remarks/>
//        LastMonth,

//        /// <remarks/>
//        Last3Months,

//        /// <remarks/>
//        Last6Months,

//        /// <remarks/>
//        LastYear,

//        /// <remarks/>
//        Last2Years,

//        /// <remarks/>
//        Custom,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TruncationType
//    {

//        /// <remarks/>
//        XSmall,

//        /// <remarks/>
//        Small,

//        /// <remarks/>
//        Medium,

//        /// <remarks/>
//        Large,

//        /// <remarks/>
//        None,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class PreferenceItem
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(typeof(long), "0")]
//        public long prefId = ((long)(0));

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute("")]
//        public string prefName = "";

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool isGroupPref = false;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SearchOperator
//    {

//        /// <remarks/>
//        And,

//        /// <remarks/>
//        Or,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("uerAuthorizeAndBillingRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UerAuthorizeAndBillingRequest
//    {

//        /// <remarks/>
//        public string authorizationCode;

//        /// <remarks/>
//        public string publicationCode;

//        /// <remarks/>
//        public string functionCode;

//        /// <remarks/>
//        public bool generateBillingRecord;

//        /// <remarks/>
//        public string responseId;

//        public string accessionNo;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("articlePopularityRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ArticlePopularityRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(TimeInterval.Day)]
//        public TimeInterval timeInterval = TimeInterval.Day;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(100)]
//        public int count = 100;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("docLang")]
//        public string[] docLang;

//        /// <remarks/>
//        public string docIndustry;

//        /// <remarks/>
//        public string docRegion;

//        /// <remarks/>
//        public string userIndustry;

//        /// <remarks/>
//        public string userCompany;

//        /// <remarks/>
//        public string userDept;

//        /// <remarks/>
//        public string userCountry;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum TimeInterval
//    {

//        /// <remarks/>
//        Day,

//        /// <remarks/>
//        Week,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetFolderRequest
//    {
//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool folderSubTypeSpecified = true;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(ProductId.Global)]
//        public ProductId productId = ProductId.Global;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool productIdSpecified = true;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum sourceDocument
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("10-K")]
//        Item10K,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("10-K405")]
//        Item10K405,

//        /// <remarks/>
//        ARS,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("currencyList", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CurrencyList
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("currency")]
//        public Currency[] currency;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ScreenerRequest
//    {

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(IndustryCodeScheme.FII)]
//        public IndustryCodeScheme industryCodeScheme = IndustryCodeScheme.FII;

//        /// <remarks/>
//        public IndustryQueryType industryQueryType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("code")]
//        public string[] code;

//        /// <remarks/>
//        public string queryString;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("screenerListTypes")]
//        public ScreenerListTypes[] screenerListTypes;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(PersonalContentType._Unspecified)]
//        public PersonalContentType personalContentType = PersonalContentType._Unspecified;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ScreenerListTypes
//    {

//        /// <remarks/>
//        Bank,

//        /// <remarks/>
//        RegionList,

//        /// <remarks/>
//        LookupRegion,

//        /// <remarks/>
//        LegalStatus,
 
//        /// <remarks/>
//        AuditorAccountant,

//        /// <remarks/>
//        ExchangeList,

//        /// <remarks/>
//        CountryList,

//        /// <remarks/>
//        CurrencyList,

//        /// <remarks/>
//        StateProvinceList,

//        /// <remarks/>
//        MarketIndexList,

//        MarketIndexFilteredList,

//        /// <remarks/>
//        TopIndustries,

//        /// <remarks/>
//        LookupIndustries,

//        /// <remarks/>
//        CompanyList,

//        /// <remarks/>
//        ManagementLevelList,

//        /// <remarks/>
//        DegreeList,

//        /// <remarks/>
//        DepartmentList,

//        /// <remarks/>
//        LocationTypes,

//        /// <remarks/>
//        NewsSubjects,

//        /// <remarks/>
//        SharedCompanyList,

//        // mt 2008 Q4 - misc - exception rule for index membership list
//        /// <remarks/>
//        MarketIndexAfterExceptionRuleList,

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reviseIndustryAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReviseIndustryAlertRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public Industry industry;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(FolderCategory.Other)]
//        public FolderCategory folderCategory = FolderCategory.Other;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubjects")]
//        public string[] newsSubjects;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("customTopics")]
//        public AlertCustomTopic[] customTopics;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industryReports")]
//        public IndustryAlertReport[] industryReports;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industryPublications")]
//        public IndustryAlertPublications[] industryPublications;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;


//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        ///<chg bucket="Q107" id="mobileheadlines" level="0"></chg>
//        public DocumentType documentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        public bool includePubAndWeb = true;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("stockPrice", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class StockPrice
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("instrument")]
//        public Instrument[] instrument;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string BOGUS;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("CodedNewsSearch", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CodedNewsSearchRequest
//    {

//        /// <remarks/>
//        public CodedNewsType newsType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companies")]
//        public Company[] companies;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industries")]
//        public Industry[] industries;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("executives")]
//        public Person[] executives;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("organization")]
//        public Organization[] organization;


//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("official")]
//        public GovernmentOfficial[] official;


//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(0)]
//        public int headlineStart = 0;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(20)]
//        public int headlineCount = 20;

//        /// <remarks/>
//        public string searchContext;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(TruncationType.Large)]
//        public TruncationType truncationType = TruncationType.Large;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool useAltQueries = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(1)]
//        public int scope = 1;

//        public string filterFragment;

//        public bool returnNavSet;

//        public SearchContentCategory[] contentCategory;

//        [System.ComponentModel.DefaultValueAttribute(ServerName.Index)]
//        public ServerName serverName = ServerName.Index;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("userRegistrationRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UserRegistrationRequest
//    {

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public string @namespace;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("sortOrder", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum SortOrder
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Ascending,

//        /// <remarks/>
//        Descending,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("timeFormatPreference", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public enum TimeFormatPreference
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        ampm,

//        /// <remarks/>
//        [System.Xml.Serialization.XmlEnumAttribute("24")]
//        Item24,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("indexSearchContinuationRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class IndexSearchContinuationRequest
//    {

//        /// <remarks/>
//        public int maxResultsToReturn;

//        /// <remarks/>
//        public bool includeSnippets;

//        /// <remarks/>
//        public string highlightString;

//        /// <remarks/>
//        public string searchContext;

//        /// <remarks/>
//        public int firstResultToReturn;

//        /// <remarks/>
//        public int contentServerAddress;

//        /// <remarks/>
//        public int contextId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reviseBatchFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReviseBatchFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public Relevance relevance;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool relevanceSpecified = true;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public string userData;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public string email;

//        public DocumentFormat documentFormat;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool documentFormatSpecified = true;


//        /// <remarks/>
//        public DispositionType dispositionType;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool dispositionTypeSpecified = true;


//        /// <remarks/>
//        public DocumentType documentType;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool documentTypeSpecified = true;

//        /// <remarks/>
//        public PostMethod editorPostMethod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool editorPostMethodSpecified;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        /// <remarks/>
//        public string emailLanguage;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;
//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool deliveryTimeSpecified = true;

//        /// <remarks/>
//        public string timeZone;

//        /// <remarks/>
//        public bool adjustDaylight;

//        // <remarks/>
//        public FilterBy filterBy;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool filterBySpecified = true;

//        /// <remarks/>
//        public MaxResultPerDelivery maxResultPerDelivery;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool maxResultPerDeliverySpecified = true;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("attribution", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Attribution
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public string code;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("timeZonePreference", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class TimeZonePreference
//    {

//        /// <remarks/>
//        public TimeZone timeZone;

//        /// <remarks/>
//        public bool convertToLocalTime;

//        /// <remarks/>
//        public bool useDaylightSavings;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetExecutiveDetailResponse : ResponseMessage
//    {

//        /// <remarks/>
//        public ExecutiveScreeningResultSet executiveScreeningResultSet;

//        /// <remarks/>
//        public CodedNewsSearchResponse codedNewsSearchResponse;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("reportResponse")]
//        public ReportResponse reportResponse;

//        public Person executive;

//        public bool hasMoreReports;


//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("executiveScreeningResultSet", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ExecutiveScreeningResultSet
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("executiveScreeningResult")]
//        public ExecutiveScreeningResultType[] executiveScreeningResult;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int total;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int first;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int last;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int count;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("pages", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Pages
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("page")]
//        public string[] page;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reviseExecutiveAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReviseExecutiveAlertRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("person")]
//        public Person[] person;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(FolderCategory.Other)]
//        public FolderCategory folderCategory = FolderCategory.Other;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        ///<chg bucket="Q107" id="mobileheadlines" level="0"></chg>
//        public DocumentType documentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getEncryptedTokenResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetEncryptedTokenResponse
//    {

//        /// <remarks/>
//        public string token;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetIndustrySnapshotResponse : ResponseMessage
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industry")]
//        public Industry[] industry;

//        /// <remarks/>
//        public StockPrice stockPrice;

//        /// <remarks/>
//        public Financials financials;

//        /// <remarks/>
//        public Competitors competitors;

//        /// <remarks/>
//        public CodedNewsSearchResponse codedNewsSearchResponse;

//        /// <remarks/>
//        public IndustrySalesByTier salesByTier;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("provider")]
//        public Provider[] provider;

//        public Factiva.FVS.MessageModel.Search2_0.SearchResponseContainer searchResponseContainer;

//        public InsightDocsData[] insightDocsData;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("corrections", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class Corrections
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("paragraph")]
//        public Paragraph[] paragraph;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("addExecutiveAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AddExecutiveAlertRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("person")]
//        public Person[] person;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(FolderCategory.Other)]
//        public FolderCategory folderCategory = FolderCategory.Other;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        ///<chg bucket="Q107" id="mobileheadlines" level="0"></chg>
//        public DocumentType documentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("userRegistrationResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UserRegistrationResponse
//    {

//        /// <remarks/>
//        public string title;

//        /// <remarks/>
//        public string firstName;

//        /// <remarks/>
//        public string lastName;

//        /// <remarks/>
//        public string suffix;

//        /// <remarks/>
//        public string jobTitle;

//        /// <remarks/>
//        public string industry;

//        /// <remarks/>
//        public string department;

//        /// <remarks/>
//        public string departmentDescription;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("address")]
//        public string[] address;

//        /// <remarks/>
//        public string city;

//        /// <remarks/>
//        public string province;

//        /// <remarks/>
//        public string state;

//        /// <remarks/>
//        public string zip;

//        /// <remarks/>
//        public string country;

//        /// <remarks/>
//        public string phone;

//        /// <remarks/>
//        public string phoneCountryCode;

//        /// <remarks/>
//        public string fax;

//        /// <remarks/>
//        public string faxCountryCode;

//        /// <remarks/>
//        public string companyName;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public bool excludeFromMailings;

//        /// <remarks/>
//        public bool dnbSubcriber;

//        /// <remarks/>
//        public UserType userType;

//        /// <remarks/>
//        public string accountId;

//        /// <remarks/>
//        public string securityWord;

//        /// <remarks/>
//        public string thirdPartyId;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(WsjUserType.Unspecified)]
//        public WsjUserType wsjType = WsjUserType.Unspecified;

//        ///
//        public string emailLogin;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum WsjUserType
//    {

//        /// <remarks/>
//        Unspecified,

//        /// <remarks/>
//        Seamless,

//        /// <remarks/>
//        Bulk,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("addCompanyAlertRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class AddCompanyAlertRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(FolderCategory.Prospect)]
//        public FolderCategory folderCategory = FolderCategory.Prospect;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("newsSubjects")]
//        public string[] newsSubjects;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("companies")]
//        public string[] companies;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("industries")]
//        public string[] industries;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("customTopics")]
//        public AlertCustomTopic[] customTopics;

//        /// <remarks/>
//        public bool allAnalystReports;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("regulatoryFillings")]
//        public RegulatoryFilling[] regulatoryFillings;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DeliveryTime deliveryTime;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        ///<chg bucket="Q107" id="mobileheadlines" ></chg>
//        public DocumentType documentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;

//        public bool includePubAndWeb = true;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("GetExecutiveDetail", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetExecutiveDetailRequest
//    {
//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool viewAllReports = false;

//        /// <remarks/>
//        public string companyQueryString;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(CompanyQueryType.HD)]
//        public CompanyQueryType companyQueryType = CompanyQueryType.HD;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewCompanyExecutiveTextOperator.Contains)]
//        public NewCompanyExecutiveTextOperator companySearchOperator = NewCompanyExecutiveTextOperator.Contains;

//        /// <remarks/>
//        public string firstName;

//        /// <remarks/>
//        public string lastName;

//        /// <remarks/>
//        public string fullName;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool includeNickNames = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewCompanyExecutiveTextOperator.Contains)]
//        public NewCompanyExecutiveTextOperator firstNameOperator = NewCompanyExecutiveTextOperator.Contains;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewCompanyExecutiveTextOperator.Contains)]
//        public NewCompanyExecutiveTextOperator lastNameOperator = NewCompanyExecutiveTextOperator.Contains;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewPersonFilter._Undefined)]
//        public NewPersonFilter filter = NewPersonFilter._Undefined;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool searchDisplayName = false;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("person")]
//        public Person person;

//        /// <remarks/>
//        public CodedNewsType newsType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool newsTypeSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("reportList")]
//        public string[] reportList;

//        [System.Xml.Serialization.XmlElementAttribute("newsFilters")]
//        public NewsFilters newsFilters;

//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("GetExecutiveSnapshotRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetExecutiveSnapshotRequest
//    {

//        /// <remarks/>
//        public string consolidatedId;

//        /// <remarks/>
//        public string companyQueryString;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(CompanyQueryType.HD)]
//        public CompanyQueryType companyQueryType = CompanyQueryType.HD;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewCompanyExecutiveTextOperator.Contains)]
//        public NewCompanyExecutiveTextOperator companySearchOperator = NewCompanyExecutiveTextOperator.Contains;

//        /// <remarks/>
//        public string firstName;

//        /// <remarks/>
//        public string lastName;

//        /// <remarks/>
//        public string fullName;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool includeNickNames = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewCompanyExecutiveTextOperator.Contains)]
//        public NewCompanyExecutiveTextOperator firstNameOperator = NewCompanyExecutiveTextOperator.Contains;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewCompanyExecutiveTextOperator.Contains)]
//        public NewCompanyExecutiveTextOperator lastNameOperator = NewCompanyExecutiveTextOperator.Contains;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(NewPersonFilter._Undefined)]
//        public NewPersonFilter filter = NewPersonFilter._Undefined;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool searchDisplayName = false;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(false)]
//        public bool viewAllReports = false;

//        /// <remarks/>
//        public CodedNewsType newsType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool newsTypeSpecified;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("reportList")]
//        public string[] reportList;


//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum NewCompanyExecutiveTextOperator
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        BeginsWith,

//        /// <remarks/>
//        Contains,

//        /// <remarks/>
//        Equals,

//        /// <remarks/>
//        IncludeAlias,

//        /// <remarks/>
//        CommonAliases,

//        // mt 2008 Q4 - misc - add option to screen companies by Trade Style Names
//        /// <remarks/>
//        IncludeAliasPlusTradeNames,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum NewPersonFilter
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        Company,

//        /// <remarks/>
//        BoardMembership,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("loginResponse", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class LoginResponse
//    {

//        /// <remarks/>
//        public ClientCodeValidationType clientCodeValidationType;

//        /// <remarks/>
//        public ClientCodeInfoList clientCodeInfoList;

//        /// <remarks/>
//        public string clientCodeRedirect;

//        /// <remarks/>
//        public string wirelessClientCodeRedirect;

//        /// <remarks/>
//        public string userStatus;

//        /// <remarks/>
//        public string wsjType;

//        /// <remarks/>
//        public bool reusedSession;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
//        public System.DateTime accountTerminationDate;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool accountTerminationDateSpecified;

//        /// <remarks/>
//        public string accountId;

//        /// <remarks/>
//        [System.ComponentModel.DefaultValueAttribute(AdministratorFlag.NotAdministrator)]
//        public AdministratorFlag administratorFlag = AdministratorFlag.NotAdministrator;

//        /// <remarks/>
//        public string productId;

//        /// <remarks/>
//        public string ruleSet;

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public UserType userType;

//        /// <remarks/>
//        public string sessionId;

//        /// <remarks/>
//        public string autoLoginToken;

//        /// <remarks/>
//        public AuthorizationMatrix authorizationMatrix;

//        public AccountAuthOption lwrFlag;

//        public UserEmailLoginStatus emailLogin;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("getCompanyGeneralInfoRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetCompanyGeneralInfoRequest
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlArrayItemAttribute("reference", IsNullable = false)]
//        public string[] references;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("GetPartnerRedirect", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class GetPartnerRedirectRequest
//    {

//        /// <remarks/>
//        public ThompsonData thompsonData;

//        /// <remarks/>
//        public DnbData dnbData;

//        /// <remarks/>
//        public BvdData bvdData;

//        /// <remarks/>
//        public Environment environment;

//        /// <remarks/>
//        public ZoomInfoData zoomInfoData;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ThompsonData
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public string reference;

//        /// <remarks/>
//        public DestinationFormat destinationFormat;

//        /// <remarks/>
//        public string userId;

//        /// <remarks/>
//        public string password;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DestinationFormat
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        RTF,

//        /// <remarks/>
//        HTML,

//        /// <remarks/>
//        PDF,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class DnbData
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public string companyName;

//        /// <remarks/>
//        public string accessLevel;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class BvdData
//    {

//        /// <remarks/>
//        public Company company;

//        /// <remarks/>
//        public string companyName;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum Environment
//    {

//        /// <remarks/>
//        _Undefined,

//        /// <remarks/>
//        Development,

//        /// <remarks/>
//        Integration,

//        /// <remarks/>
//        BusinessContinuity,

//        /// <remarks/>
//        Production,
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ZoomInfoData
//    {

//        /// <remarks/>
//        public string personId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("updateProfileRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class UpdateProfileRequest
//    {

//        /// <remarks/>
//        public string title;

//        /// <remarks/>
//        public string firstName;

//        /// <remarks/>
//        public string lastName;

//        /// <remarks/>
//        public string suffix;

//        /// <remarks/>
//        public string jobTitle;

//        /// <remarks/>
//        public string industry;

//        /// <remarks/>
//        public string department;

//        /// <remarks/>
//        public string departmentDescription;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("address")]
//        public string[] address;

//        /// <remarks/>
//        public string city;

//        /// <remarks/>
//        public string province;

//        /// <remarks/>
//        public string state;

//        /// <remarks/>
//        public string zip;

//        /// <remarks/>
//        public string country;

//        /// <remarks/>
//        public string phone;

//        /// <remarks/>
//        public string phoneCountryCode;

//        /// <remarks/>
//        public string fax;

//        /// <remarks/>
//        public string faxCountryCode;

//        /// <remarks/>
//        public string companyName;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public bool excludeFromMailings;

//        /// <remarks/>
//        public bool dnbSubcriber;

//        /// <remarks/>
//        public UserType userType;

//        /// <remarks/>
//        public string accountId;

//        /// <remarks/>
//        public string securityWord;

//        /// <remarks/>
//        public string thirdPartyId;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("executives", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class CompanyExecutives
//    {

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("person")]
//        public Person[] person;
//    }

//    /// <remarks/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRootAttribute("reviseContinuousFolderRequest", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class ReviseContinuousFolderRequest
//    {

//        /// <remarks/>
//        public string folderName;

//        /// <remarks/>
//        public string searchString;

//        /// <remarks/>
//        public Relevance relevance;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool relevanceSpecified = true;

//        /// <remarks/>
//        public FolderSubType folderSubType;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool folderSubTypeSpecified;

//        /// <remarks/>
//        public ProductId productId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool productIdSpecified;

//        /// <remarks/>
//        public string userData;

//        /// <remarks/>
//        public string contact;

//        /// <remarks/>
//        public RevisionPrivileges revisionPrivileges;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool revisionPrivilegesSpecified;

//        /// <remarks/>
//        public string email;

//        /// <remarks/>
//        public DocumentFormat documentFormat;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool documentFormatSpecified = true;


//        /// <remarks/>
//        public DispositionType dispositionType;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool dispositionTypeSpecified = true;


//        /// <remarks/>
//        public DocumentType documentType;

//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        [System.ComponentModel.DefaultValueAttribute(true)]
//        public bool documentTypeSpecified = true;

//        /// <remarks/>
//        public PostMethod editorPostMethod;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool editorPostMethodSpecified;

//        /// <remarks/>
//        public bool wirelessFriendly;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlIgnoreAttribute()]
//        public bool wirelessFriendlySpecified;

//        /// <remarks/>
//        public string emailLanguage;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute()]
//        public int folderId;

//        /// <remarks/>
//        [System.Xml.Serialization.XmlElementAttribute("delivContentType")]
//        public DeliveryContentType[] delivContentType;

//        [System.ComponentModel.DefaultValueAttribute(RemoveDuplicate.None)]
//        public RemoveDuplicate removeDuplicate = RemoveDuplicate.None;
//    }

//    [System.Xml.Serialization.XmlType(Namespace = "http://global.factiva.com/fvs/1.0")]
//    [System.Xml.Serialization.XmlRoot("familyTreeInfo", Namespace = "http://global.factiva.com/fvs/1.0", IsNullable = false)]
//    public class FamilyTreeInfo
//    {
//        public CompanyParent globalUltimateParent;

//        public CompanyParent domesticUltimateParent;

//        public CompanyParent headquaterParent;

//        [System.Xml.Serialization.XmlAttribute()]
//        public string code;

//        [System.Xml.Serialization.XmlAttribute()]
//        public string dunsNumber;

//        [System.Xml.Serialization.XmlAttribute()]
//        public string companyDesignation;

//        public Region parentLocation;

//        public Number familyMembers;

//        public Number branches;

//        public Number nonBranches;
//    }


//    /// <summary>
//    /// hoppenstedt keyfigures report provisional
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyFiguresReport
//    {
//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        public KeyFiguresReportData keyFiguresReportData;
//    }
//    /// <summary>
//    /// hoppenstedt keyfigures report provisional
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class KeyFiguresReportData
//    {
//        //public string tabularReportType;
//        [System.Xml.Serialization.XmlElementAttribute("reportTable")]
//        public ReportTable[] reportTables;
//    }
//    /// <summary>
//    /// hoppenstedt keyfigures report provisional
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportTable
//    {
//        [System.Xml.Serialization.XmlElementAttribute("reportTableType")]
//        public string reportTableType;
//        public string reportTableCurrency;
//        public string subtype;

//        public string fiscalYear;
//        public string currency;
//        public ReportTableColumn total;

//        [System.Xml.Serialization.XmlElementAttribute("reportTableHeader")]
//        public ReportTableHeader reportTableHeader;
//        [System.Xml.Serialization.XmlElementAttribute("reportTableRow")]
//        public ReportTableRow[] reportTableRows;

//    }
//    /// <summary>
//    /// hoppenstedt keyfigures report provisional
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportTableHeader
//    {
//        [System.Xml.Serialization.XmlElementAttribute("reportTableDescription")]
//        public string reportTableDescription;
//        [System.Xml.Serialization.XmlElementAttribute("headerColumn")]
//        public string[] headerColumns;


//    }
//    /// <summary>
//    /// hoppenstedt keyfigures report provisional
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportTableRow
//    {
//        [System.Xml.Serialization.XmlElementAttribute("reportTableDescription")]
//        public string reportTableDescription;
//        [System.Xml.Serialization.XmlElementAttribute("reportTableColumn")]
//        public ReportTableColumn[] reportTableColumns;
//        //[System.Xml.Serialization.XmlElementAttribute("reportTableColumn")]
//        //public string[] reportTableColumn;

//    }
//    /// <summary>
//    /// hoppenstedt keyfigures report 
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ReportTableColumn
//    {
//        //[System.Xml.Serialization.XmlElementAttribute("reportTableColumn")]
//        public Number rawData;
//        public string type;
//        public string rawString;
//        public string subtype;
//    }



//    /// <summary>
//    /// q307 CredInform importExportReport
//    /// </summary>

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ImportExportReport
//    {
//        /// <remarks/>
//        public ReportMetaData reportMetaData;

//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;

//        public ImportExportReportData importExportReportData;
//    }


//    /// <summary>
//    /// CredInform importExportReport
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ImportExportReportData
//    {

//        [System.Xml.Serialization.XmlElementAttribute("importTable")]
//        public ReportTable[] importTables;

//        [System.Xml.Serialization.XmlElementAttribute("exportTable")]
//        public ReportTable[] exportTables;
//    }

//    /// <summary>
//    /// q307 CredInform registrationReport 0
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class RegistrationReport
//    {
//        /// <remarks/>
//        public ReportMetaData reportMetaData;
//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;
//        public RegistrationReportData registrationReportData;
//    }

//    /// <summary>
//    /// q307 CredInform registrationReport 1
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class RegistrationReportData
//    {
//        [System.Xml.Serialization.XmlElementAttribute("registrationTable")]
//        public ReportTable[] registrationTables;
//    }



//    /// <summary>
//    /// q307 CredInform shareholdersReport 0
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class ShareholdersReport
//    {
//        /// <remarks/>
//        public ReportMetaData reportMetaData;
//        /// <remarks/>
//        public ReportReferenceType reportTypeCode;
//        public shareholdersReportData shareholdersReportData;
//    }

//    /// <summary>
//    /// q307 CredInform shareholdersReport 1
//    /// </summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class shareholdersReportData
//    {
//        [System.Xml.Serialization.XmlElementAttribute("shareholdersTable")]
//        public ReportTable[] shareholdersTables;
//    }

//    ///<summary>
//    ///q307 CredInform CredSubsidiariesAffiliatesReportData 0
//    ///</summary>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class CredSubsidiariesAffiliatesReportData
//    {
//        [System.Xml.Serialization.XmlElementAttribute("credSubsidiariesAffiliates")]
//        public CredSubsidiariesAffiliate[] credSubsidiariesAffiliates;
//    }

//    /// <summary>
//    /// q307 CredInform CredSubsidiariesAffiliatesReportData 0
//    /// </summary>
//    public class CredSubsidiariesAffiliate
//    {
//        public string code;
//        public string name;
//        public string country;
//        public ReportTableColumn percentDirectlyHeld;
//    }

//    /// <summary>
//    /// q108 Insight Documents List 
//    /// </summary>
//   [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class InsightDocsData
//    {
//        public string industryCode;
//        public string headline;
//        public string url;
//        public string title;
//    }
//    /// <summary>
//    /// q108 Insight Documents List 
//    /// </summary>
//     [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//     public class InsightDocsDataObject
//     {
//         public InsightDocsData[] insightDocsDatas;
//     }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum MetricsPPSDisplayFormat
//    {
//        _Undefined,
//        Print,
//        Save,
//        Email,
//        Briefcase,
//        Rtf,
//        Pdf,
//        Xml,
//    }

//    /// <summary/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum AccountAuthOption
//    {
//        /// <summary/>
//        Unspecified,
//        /// <summary/>
//        Convert,
//        /// <summary/>
//        ConvertPlusLwr,

//        Unknown,
//    }
//    /// <summary/>
//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum UserEmailLoginStatus
//    {
//        /// <summary/>
//        Disabled,
//        /// <summary/>
//        Pending,
//        /// <summary/>
//        Enabled
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ServerName
//    {
//        /// <summary/>
//        Unknown,
//        /// <summary/>
//        Search20,
//        /// <summary/>
//        Index
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SharingAssetType
//    {
//        /// <summary/>
//        Personal,
//        /// <summary/>
//        Shared,
//        /// <summary/>
//        Assigned,
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SharingAccessScope
//    {
//        /// <summary/>
//        Personal,
//        /// <summary/>
//        List,
//        /// <summary/>
//        Group,
//        /// <summary/>
//        Account,
//        /// <summary/>
//        Community,
//        /// <summary/>
//        Everyone,
//    }

//   [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum SharingAccessStatus
//    {
//        /// <summary/>
//       Off,
//       /// <summary/>
//        On,
//    }


//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum ListFolderType
//    {
//        /// <summary/>
//        Personal,
//        /// <summary/>
//        SubscribedPersonal,
//        /// <summary/>
//        AssignedPersonal,
//        /// <summary/>
//        All

//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DeliveryContentType
//    {
//        /// <summary/>
//        Article,
//        /// <summary/>
//        Report,
//        /// <summary/>
//        File,
//        /// <summary/>
//        Picture,
//        /// <summary/>
//        Inventory,
//        /// <summary/>
//        Coinfo,
//        /// <summary/>
//        Webpage,
//        /// <summary/>
//        Internal,
//        /// <summary/>
//        Publication,
//        /// <summary/>
//        All
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum CompanyListShareType
//    {
//        _Undefined = 0,
//        Personal,
//        Shared,
//        Proxy,
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GetUserAuthRequest
//    {
//        public GetNewsletterProxyToken token;
//        public GetUserAuthRequestType requestType;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public class GetUserAuthResponse : GetEncryptedTokenResponse
//    {
//        public bool isValidReader;
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum GetUserAuthRequestType
//    {
//        /// <remarks/>
//        GetTTLToken,
//        /// <summary/>
//        ValidateReader,

//        GetProxyCredentials,
//    }

//    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://global.factiva.com/fvs/1.0")]
//    public enum DisseminationOption
//    {
//        Internal,
//        External,
//        TTL,
//        ExternalReader
//    }

//}
