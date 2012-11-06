using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Factiva.Currents.Aggregrator
{
    public class Constants
    {
        public const string CallbackParam = "callback";
        public static string JsonContentWithCallbackType = "text/javascript; charset=utf-8";
        public static string JsonContentType = "application/json; charset=utf-8";
        public static string XmlContentType = "text/xml; charset=utf-8";
        public const string OverrideHttpStatus = "OverrideHttpStatus";
        public const char DefaultDelimiter = '|';
        public const string TransactionLog = "TransactionLog";
        public const string ApiVersion = "1.0";
        public const string XmlFormat = "xml";
        public const string JsonFormat = "json";
        public const string AuditLog = "AuditLog";
        public const string ARMValues = "ArmValues";
    }

    public static class ErrorConstants
    {
        public const long EnumConverstionFailed = 680011; // Enumeration Conversion Failed for one or more Enums in the response.
        public const long InvalidContentCategory = 684811;// Content Category is not valid. Possible values are Publications,Pictures,WebSites,Multimedia.
        public const long InvalidSourceCategory = 684802;
        public const long InvalidOffset = 686204;

        public const long InvalidSearchStringForFirstCharacterOnly = 684812; //Search String can only be one character long for Search Operator FirstCharacterOnly. Please limit your search to one character.
        public const long InvalidSearchOperator = 684813; //Please provide a valid SearchOperator. Possible values are CompleteTerm, FirstCharacterOnly.
        public const long InvalidChildrenType = 684814; //Please provide valid ChildrenType. Possible values are Group, Source.

        public const long InvalidSortBy = 680221; // new
        public const long InvalidSortOrder = 680009; // new

        public const long UnknownError = 680000;
        public const long InvalidFormat = 680002;
        public const long SessionIdIsNull = 680003;
        public const long NotAuthorized = 680005;
        public const long InvalidModuleId = 686201;
        public const long InvalidPageId = 686202;
        public const long InvalidRecords = 686203;
        //public const long InvalidFirstResultToReturn = 686204;
        public const long InvalidTimeFrame = 686205;
        public const long InvalidParts = 686206;
        //public const long InvalidFirstPartToReturn = 686207;
        public const long InvalidMaxPartsToReturn = 686208;
        public const long InvalidAccessQualifier = 686209;
        public const long InvalidFilterType = 686210;
        //public const long InvalidRegionCodes = 686211; // Added but not beeing used as of now
        public const long InvalidTrendType = 686212;
        public const long InvalidEntityType = 686213;
        public const long InvalidEditIn = 686214;
        public const long InvalidUseCustomDateRange = 686215;
        public const long InvalidUseStub = 686216;
        public const long InvalidSearchContextRef = 686217;
        public const long InvalidAccessionNo = 686218;
        public const long InvalidModuleType = 686219;
        public const long InvalidMaxEntitiesToReturn = 686220;
        public const long InvalidSummaryType = 686221;
        public const long InvalidTruncationType = 686222;
        public const long InvalidAccessScope = 686223;

        public const long InvalidGroupOperator = 686601;
        public const long InvalidFilterGroup = 686602;
        public const long SourceAlreadyExists = 686603;
        public const long InvalidAssetCode = 686604;
        public const long IntConversionFailed = 686605;
        public const long BooleanConversionFailed = 686606;

        public const long InvalidWorkspaceId = 686607;
        public const long InvalidDisplayFormat = 686608;


        public const long InvalidCodes = 684801;
        public const long CodesNotFound = 684804;
        public const long RootCantBeSearchedWithOtherCodes = 684815; //"Root" cannot be searched with other codes. Please remove other codes.


        //Private Simple Search Errors
        public const long InvalidDeDuplicaitonMode = 680409;

        public const long FailedToDeserializeHeader = 686224;

        public const long InvalidSourceStatus = 686225;


        #region Top Stories
        public const long InvalidQueryString = 686243;
        public const long InvalidSearchMode = 686244;

        public const long InvalidSymbol = 686245;
        public const long QuoteNotFound = 686246;
        public const long ChartNotFound = 686247;
        public const long InvalidArticleRef = 686248;

        public const long InvalidSymbology = 686249;

        public const long ArticleNotFound = 680415;
        //public const long NoCollectionFound = 680416;
        public const long NoCollectionFound = 680809;
        public const long NoSymbolsFound = 680810;
        public const long InvalidCollectionCode = 680811;
        public const long InvalidPictureSize = 680812;
        public const long InvalidCacheScope = 680813;
        public const long InvalidExpirationPolicy = 680814;
        #endregion

        #region Alerts
        //public const long InvalidAlertDeliveryMethod = 680202;
        public const long InvalidAlertProductType = 680203;
        //public const long InvalidAlertDeliveryTimes = 680204;
        // public const long InvalidAlertDocumentFormat = 680205;
        public const long InvalidAlertId = 680206;
        public const long InvalidAlertName = 680207;
        //public const long InvalidAlertDeduplicationLevel = 680208;
        public const long InvalidAlertDocumentType = 680209;
        public const long InvalidAlertSearchMode = 680210;
        public const long InvalidAlertSourceGenre = 680211;
        public const long InvalidAlertEmail = 680213;
        public const long NoAlertsFound = 680214;
        public const long InvalidAlertNameLength = 680216;
        public const long InvalidAlertQueryString = 680218;
        public const long InvalidAlertParts = 680220;
        public const long InvalidAlertSortBy = 680221;
        //public const long InvalidAlertDispositionType = 680222; not used anywhere
        public const long InvalidResultDisplayFormat = 680243;
        public const long InvalidResponseAssetType = 680244;
        public const long InvalidAlertDeliveryMethod = 680245;
        public const long InvalidAlertDeliveryTimes = 680246;
        public const long InvalidAlertDocumentFormat = 680247;
        public const long InvalidAlertDeduplicationLevel = 680248;
        public const long InvalidAlertUseAlertDedup = 680249;
        #endregion

        #region ManagedList
        public const long InvalidListId = 682404;
        #endregion

        #region SavedQuery
        public const long InvalidFilterOperator = 680223;
        public const long InvalidFilterTarget = 680224;
        public const long InvalidSearchLanguageCode = 680225;
        public const long InvalidFreeTextFilterType = 680226;
        public const long InvalidSearchFormatCategory = 680227;
        public const long InvalidSearchDeduplicationType = 680228;
        public const long InvalidSearchSection = 680229;
        public const long InvalidSourceEntityType = 680230;
        public const long InvalidSourceNewsQueryContentType = 680231;
        public const long InvalidNewsQueryScope = 680232;

        public const long InvalidFreeTextSearchMode = 680233;
        public const long InvalidAccessControlScope = 680234;
        public const long InvalidAssignedScope = 680235;
        public const long InvalidSharePromotion = 680236;
        public const long InvalidListingScope = 680237;
        public const long InvalidPreviousACScope = 680238;
        public const long InvalidRootAccessControlScope = 680239;
        public const long InvalidShareStatus = 680240;
        public const long InvalidSearchSetupScreen = 680241;
        public const long InvalidShareType = 680242;
        #endregion

        #region Content
        //public const long InvalidContentQueryString = 680417;
        public const long InvalidContentQueryString = 680443;
        public const long InvalidIsRedirect = 680432;
        public const long NoUrlFound = 680004;
        public const long InvalidAllDates = 680410;
        public const long InvalidDaysRange = 680412;
        //public const long InvalidDelayedContent = 680425;
        public const long InvalidDelayedContent = 680441;
        public const long InvalidStartDate = 682607;
        public const long InvalidEndDate = 682608;
        public const long InvalidFreshnessDate = 680433;
        public const long InvalidSnippetType = 680414;
        public const long InvalidMaxBuckets = 680405;
        public const long InvalidMaxKeywords = 680434;
        public const long InvalidDatesCombination = 680802;
        public const long InvalidBlackList = 680435;
        public const long InvalidTimeNavigatorPartDescriptor = 680436;
        public const long InvalidCompanies = 680437;
        public const long InvalidIndustries = 680438;
        public const long InvalidRegions = 680439;
        public const long InvalidSubjects = 680440;
        public const long InvalidArticleFormat = 680803;
        public const long InvalidArticleFormatType = 680442;
        public const long InvalidDateRangeType = 680444;
        public const long InvalidArtcileResponseSet = 680446;
        public const long InvalidMultimediaResponseSet = 680447;
        public const long InvalidBilldataContentCategory = 680430;
        public const long InvalidBilldata = 680424;
        public const long InvalidSearchParameters = 680448;// If no start date,end date, daterange and daterangetype is given in realtime search.
        public const long InvalidBilldataArticleParts = 680426;
        #endregion

        #region Newsstand
        public const long InvalidSourceType = 686501;
        public const long InvalidSourceCode = 686502;
        public const long InvalidSectionId = 686503;
        #endregion

        #region Signal
        public const long InvalidLanguageCode = 686401;
        public const long InvalidSignalType = 686402;
        public const long InvalidConfidenceRange = 686403;
        public const long InvalidResultType = 686404;
        public const long InvalidSearchType = 686405;
        public const long InvalidSignalId = 686406;
        public const long InvalidFromDate = 686407;
        public const long InvalidToDate = 686408;
        public const long InvalidIndustryCode = 686409;
        public const long InvalidOrganizationCode = 686410;
        public const long InvalidRegionCode = 686411;
        public const long InvalidSignalParts = 686412;
        public const long InvalidSignalDaysRange = 684213;
        #endregion

        #region Collection
        public const long InvalidCollectionParts = 686701;
        public const long InvalidCollectionName = 680805; // reusing existing collection error numbers from 1.0
        public const long InvalidCollectionId = 680804;
        public const long InvalidCollectionArticleRefs = 680806;
        public const long InvalidCollectionNameLength = 680808;
        public const long InvalidCharactersInCollectionName = 680807;
        public const long InvalidContentId = 680801;
        #endregion

        #region NewsLetters
        public const long InvalidEditionId = 686801;
        public const long InvalidTemplateId = 686802;
        public const long ArticleResponseSetIsNull = 686803;
        public const long MultimediaResponseIsNull = 686804;
        public const long InvalidAvailableWidth = 686805;
        #endregion

        #region Company
        public const long InvalidCompanyId = 680601;
        public const long InvalidCompanyParts = 680614;
        public const long InvalidCompanySourceGenre = 680608;
        public const long InvalidCompanyNewsType = 680609;
        //public const long InvalidSymbology = 682603;
        public const long InvalidCompanyInvestextReportId = 680610;
        public const long InvalidAssetReference = 680613;
        #endregion

        #region Session
        public const long InvaildSessionTimeout = 683602;
        public const long InvalidSessionNamespace = 683604;
        public const long InvalidEmailPattern = 683606;
        public const long InvalidSessionParts = 683607;
        public const long InvalidSessionIdCode = -2147176633;
        public const long InvalidCharsInSessionId = -2147172335;
        #endregion

    }
}
