namespace DowJones.OperationalData
{
    internal class ODSConstants
    {
        //ALERT
        public static readonly string KEY_ALERT_ID = "fid";
        public static readonly string KEY_ALERT_NAME = "fn";

        public static readonly string KEY_ALERT_COUNT = "fcnt";
        public static readonly string KEY_ALERT_TYPE = "ft";

        //RSS
        public static readonly string KEY_COMPANY_NAME = "comp";

        // Edtion
        //public static readonly string KEY_DOWNLOAD_DATE = "Dwnld_Date";
        public static readonly string KEY_EDITION_ID = "EditionId";
        public static readonly string KEY_EDITION_ACTION = "EDAction";
        public static readonly string KEY_DISSEMINATION_FORMAT = "DissFmt";
        public static readonly string KEY_SAVE_PDF_IN_INSIGHT = "SavePDFInInsight";
        public static readonly string KEY_CONTAINER_RESULTS_FORMAT = "ContRsltsFmt";

        //Newsletter
        public static readonly string KEY_CUSTOM_CSS = "cstm";
        public static readonly string KEY_DELIVERY_TYPE = "dt";
        public static readonly string KEY_DISPOSITION = "dsp";

        public static readonly string KEY_DOMAIN = "dmn";
        public static readonly string KEY_ED_NAME = "edn";
        public static readonly string KEY_EID = "eid";
        public static readonly string KEY_EMAIL = "email";
        public static readonly string KEY_FEED_NAME = "feed";
        public static readonly string KEY_FIRST_NAME = "first";
        public static readonly string KEY_FORMAT = "fmt";
        public static readonly string KEY_HEADLINE_VIEW = "view";
        public static readonly string KEY_HOMEPAGE = "HOMEPAGE";
        public static readonly string KEY_LAST_NAME = "last";

        public static readonly string KEY_LIST_ID = "slid";
        public static readonly string KEY_LIST_TYPE = "lt";
        //public static readonly string KEY_NAMESPACE = "ns";   //Dave Dacosta changed. Consolidate to KEY_USER_NAMESPACE
        public static readonly string KEY_NL_NAME = "nln";
        public static readonly string KEY_NLID = "nlid";
        public static readonly string KEY_NUMBER_OF_ITEMS = "cnt";
        public static readonly string KEY_RADAR_ALERT_ID = "raid";
        public static readonly string KEY_RSS_TYPE = "type";
        public static readonly string KEY_TMPL = "tmpl";
        public static readonly string KEY_USER_ID = "uid";
        public static readonly string KEY_USER_NAMESPACE = "ns";
        public static readonly string KEY_VIEW_ID = "vwid";

        //Widget
        public static readonly string KEY_WIDGET_ID = "wid";
        public static readonly string KEY_WIDGET_NAME = "wn";

        public static readonly string ODS_PREFIX_FOR_KEY = "FCS_OD_";

        //for Asset activity tracking - sm 09/08/08

        /// <summary>
        /// The Asset ID in string
        /// </summary>
        public static readonly string KEY_VIEW_ASSET_ID = "ASSETID";

        /// <summary>
        /// The Asset Type in string; Possible values for Q42008 release: PNP
        /// </summary>
        public static readonly string KEY_VIEW_ASSET_TYPE = "ASTTYPE";

        /// <summary>
        /// The Action taken on the asset; Possible values: View, Published, Used
        /// For Q42008, the only valid action from UI is "View"
        /// </summary>
        public static readonly string KEY_VIEW_ASSET_ACTION = "ASTACTION";

        /// <summary>
        /// Publishing Domain
        /// </summary>
        public static readonly string KEY_VIEW_PUBLISHING_DOMAIN = "DOMAIN";

        // Article Asset
        public static readonly string KEY_ASSET_ID = "aid";
        public static readonly string KEY_ASSET_NAME = "aname";
        public static readonly string KEY_ASSET_TYPE = "atype";
        public static readonly string KEY_DISSEMINATION_METHOD = "dmd";
        public static readonly string KEY_LINK_TYPE = "lnk";
        public static readonly string KEY_AUDIENCE_OPTION = "aopt";

        #region SearchRegion
        public static readonly string KEY_SEARCH_PAGE = "SRCH_Page";
        public static readonly string KEY_SEARCH_TYPE = "Cont_Ind";
        public static readonly string KEY_SAVED_SEARCH = "Saved_Form";
        public static readonly string KEY_FREE_TEXT_INCLUDED = "FreeTxt_Incl";
        public static readonly string KEY_SEARCH_DATE_RANGE = "Srch_DtRng";
        public static readonly string KEY_DEDUPLICATION_SETTING = "Dedup_Stng";
        public static readonly string KEY_RESULT_SORT_ORDER = "RsltSortOrder";
        public static readonly string KEY_NUMBER_OF_HEADLINE_DISPLAY = "HdlnDisp_Req";
        public static readonly string KEY_IS_SUCCESSFUL = "Success_Ind";
        public static readonly string KEY_ERROR_CODE = "ErrorCode";
        public static readonly string KEY_TOTAL_HEADLINE_FOUND = "TotalHdlnsFnd";
        public static readonly string KEY_NUMBER_OF_UNIQUE_HEADLINES = "UnqHdlnVwd";

        public static readonly string KEY_SOURCES_INCLUDED = "Srcs_Incl";
        public static readonly string KEY_COMPANIES_INCLUDED = "Comps_Incl";
        public static readonly string KEY_SUBJECT_INCLUDED = "Subs_Incl";
        public static readonly string KEY_INDUSTRY_INCLUDED = "Inds_Incl";
        public static readonly string KEY_REGION_INCLUDED = "Regns_Incl";
        public static readonly string KEY_LANGUAGE_INCLUDED = "Lngs_Incl";
        public static readonly string KEY_AUTHOR_INCLUDED = "AuthLkp_Incl";
        public static readonly string KEY_USER_CONSULTANT_LENS = "UseConsLens";
        public static readonly string KEY_LEAD_SENTENCE_DISPLAY = "LeadSntncDisp";
        public static readonly string KEY_DISCOVERY_FILTER_DATE = "DFilt_Date";
        public static readonly string KEY_DISCOVERY_FILTER_COMPANY = "DFilt_Comps";
        public static readonly string KEY_DISCOVERY_FILTER_SOURCE = "DFilt_Srcs";
        public static readonly string KEY_DISCOVERY_FILTER_SUBJECT = "DFilt_Subs";
        public static readonly string KEY_DISCOVERY_FILTER_INDUSTRY = "DFilt_Inds";
        public static readonly string KEY_DISCOVERY_FILTER_NEWSCLUSTER = "DFilt_NwsClst";
        public static readonly string KEY_DISCOVERY_FILTER_KEYWORD = "DFilt_KWrd";
        public static readonly string KEY_DISCOVERY_FILTER_EXECUTIVE = "DFilt_Exec";
        public static readonly string KEY_DISCOVERY_FILTER_AUTHOR = "DFilt_Auth";
        public static readonly string KEY_NUMBER_OF_DUPLICATE_HEADLINES = "DupHdln";

        public static readonly string KEY_SEARCH_FREE_TEXT = "SrchFreeTxtIn";
        public static readonly string KEY_REPUBLISHED_NEWS_EXCLUDED = "RepubNws_Exl";
        public static readonly string KEY_RECURRING_PRICING_MARKET_DATA_EXCLUDED = "RecPrMktData_Exl";
        public static readonly string KEY_OBITUARIES_SPORT_CALENDAR_EXCLUDED = "OrbSprtCal_Exl";

        public static readonly string KEY_CONCEPT_EXPLORER_INCLUDED = "CncptExp_Incl";
        public static readonly string KEY_PERSONAL_OR_GROUP = "PersOrGrp";
        public static readonly string KEY_SHARE_TYPE = "ShareType";
        
        #endregion

        #region ArticleRegion
        public static readonly string KEY_ORIGIN = "AVO";
        public static readonly string KEY_ORIGIN_ADDITIONAL = "AddnlOriginData";
        public static readonly string KEY_POST_PROCESSING = "PostProcess";
        public static readonly string KEY_POST_PROCESSING_ADDITIONAL = "AddnlPostProcess";
        public static readonly string KEY_DISPLAY_FORMAT = "DispFmt";
        public static readonly string KEY_SOURCE_CODE = "SrcCd";

        public static readonly string KEY_SOURCE_NAME = "SrcName";
        public static readonly string KEY_PUBLISHER_NAME = "PubName";
        public static readonly string KEY_CONTENT_LANGUAGE = "ContLang";
        public static readonly string KEY_ARTICLE_CATEGORY = "ArtCat";
        public static readonly string KEY_ARTICLE_CATEGORY_ADDITIONAL = "AddnlArtCat";

        public static readonly string KEY_DEST_DETAIL = "AddnlDestDtl";

        public static readonly string KEY_VIEW_DESTINATION = "AVDest";
        public static readonly string KEY_VIEW_AUTO_COMPLETED_TERM = "AutoCompletedTerm";

        #endregion
        public static readonly string KEY_ERROR_DESC = "ErrorDesc";
        public static readonly string KEY_STACK_TRACE = "StackTrace";
        public static readonly string KEY_OCP_FUNCTION_CODE = "OCPFunctionCode";
        public static readonly string KEY_APP_FILE_NAME = "AppFileName";
        public static readonly string KEY_APP_FUNCTION_NAME = "AppFunctionName";
        public static readonly string KEY_APP_FILE_LINE_NUMBER = "AppFileLineNum";
        public static readonly string KEY_REQUESTOR_IP = "RequestorIP";
        public static readonly string KEY_WEB_SERVER_NAME = "WebServerName";
        public static readonly string KEY_REQ_DATE_TIME = "ReqDateTime";
        public static readonly string KEY_RESP_DATE_TIME = "RespDateTime";
        public static readonly string KEY_ERROR_LOG_TIME = "ErrorLogTime";
        public static readonly string KEY_BACK_END_TRAN_NAME = "BackEndTranName";
        public static readonly string KEY_GTW_TRAN_NAME = "GTWTranName";
        public static readonly string KEY_GTW_TRAN_NAMESPACE = "GTWTranNameSpace";
        public static readonly string KEY_ADDNL_INFO = "AddnlInfo";
        public static readonly string KEY_USER_ACCT_NUM = "UserAcctNum";
        public static readonly string REQ_COOKIES_CNT = "ReqCookiesCnt";
        public static readonly string REQ_QUERY_STRING_CNT = "ReqQueryStringCnt";
        public static readonly string FORM_CNT = "FormCount";
        public static readonly string FCS_ODS_TRANS_NAME = "ODSTranName";
        /*Start: Infosys*/
        /*05/03/2010- Added FormMethod parameter*/
        public static readonly string KEY_ODS_FORM_METHOD = "FormMethod";
        /*End: Infosys*/
        public static readonly string KEY_ODS_TRAN_NAME = "ODSTranName";

        #region Dashboard View
        public static readonly string KEY_DASHBOARD = "Dashboard";
        #endregion

        #region Snapshot
        public static readonly string KEY_SNAPSHOT_TYPE = "SnapshotType";
        #endregion

        #region RelationshipMapping
        public static readonly string KEY_RELATION_FROM = "RelationFrom";
        public static readonly string KEY_RELATION_TO = "RelationTo";
        #endregion

        #region BriefingBookCreation

        public static readonly string FCS_OD_PDFDownload = "PDFDownload";
        #endregion

        #region CompanySnapshotview
        public static readonly string FCS_OD_COMPANY_NAME = "CompName";
        public static readonly string FCS_OD_COMPANY_TYPE = "CompType";
        public static readonly string FCS_OD_FROM_EXTENDED_UNIVERSE = "FromExtUniv";
        public static readonly string FCS_OD_REQUESTOR_IP = "RequestorIP";
        #endregion
        #region IndustrySnapshotView

        public static readonly string FCS_OD_INDUSTRY_NAME = "IndustryName";
        #endregion

        #region EntitySearch

        public static readonly string FCS_OD_ENTITY_TYPE = "EntityType";
        public static readonly string FCS_OD_SEARCH_TYPE = "SearchType";
        #endregion

        #region AutoCompleteUsage
        public static readonly string FCS_OD_SOURCE_PAGE = "SourcePage";
        public static readonly string FCS_OD_SOURCE_BROWSER_CATEGORY = "SrcBrwsrCategory";

        #endregion
        #region SimpleSearchPreference
        public static readonly string FCS_OD_PREFERENCE_VALUE = "PreferenceValue";
        #endregion
        #region Error Data

        #endregion

    }
}