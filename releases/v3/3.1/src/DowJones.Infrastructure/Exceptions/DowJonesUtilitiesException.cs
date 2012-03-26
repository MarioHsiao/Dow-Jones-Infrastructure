// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DowJonesUtilitiesException.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using log4net;

namespace DowJones.Exceptions
{
    /// <summary>
    /// Description for DowJonesUtilitiesException.
    /// </summary>
    [DataContract(Name = "dowJonesUtilitiesException", Namespace = "")]
    [Serializable]
    public class DowJonesUtilitiesException : ApplicationException
    {
        #region Error codes
        public const long ErrorInvalidSessionLong = -2147176633;

        public const long BaseUtillityError = 585000;

        // CACHE MANAGER EXCEPTIONS Section 585001-585025
        public const long CacheManagerUnableToAddItem = 585001;
        public const long CacheManagerUnableToGetItem = 585002;
        public const long CacheManagerUnableToUpdateItem = 585003;

        // SEARCH MANAGER EXCEPTIONS Section 585026-585050
        public const long SearchManagerInvalidDto = 585026;
        public const long SearchManagerUnableToFindDocumentVector = 585027;
        public const long SearchManagerUnableToFindCodedNewsQuery = 585028;
        public const long CodedNewsSearchRequestIncomplete = 585029;

        // ENCRYPTION UTILITIES EXCEPTIONS Section 585051-585075
        public const long EncryptionUtilitiesEncryptingException = 585051;
        public const long EncryptionUtilitiesDecryptingException = 585052;
        public const long EncryptionUtilitiesValidationException = 585053;

        // ENCODING_UTILITIES EXCEPTIONS Section 585076-585100
        public const long EncodingUtilitiesEncodingException = 585076;
        public const long EncodingUtilitiesDecodingException = 585077;

        // RSS RETRIVAL EXCEPTIONS Section 585101-585125
        public const long RSSLanguageIsNotSupported = 585101;

        // SessionData Section 585126-585150
        public const long SessionDataAuthorizationError = 585126;

        // AUTO TRANSLATION Section 585151-585175
        public const long NotSupportedSourceLanguageException = 585151;
        public const long NotSupportedTargetLanguageException = 585152;
        public const long UnknownTranslationStatusException = 585153;
        public const long InvalidApiKeyException = 585154;
        public const long InvalidUserException = 585155;
        public const long MissingAuthorizationHttpHeaderexception = 585156;
        public const long AuthorizationHttpHeaderFormattingException = 585157;
        public const long AuthorizationSignatureMismatchException = 585158;
        public const long AuthorizationErrorException = 585159;
        public const long MissingLwDateHttpHeaderException = 585160;
        public const long MissingSourceTextException = 585161;
        public const long MissingSourceFileException = 585162;
        public const long MissingSourceUrlException = 585163;
        public const long InvalidSourceLanguageIDException = 585164;
        public const long InvalidTargetLanguageIDException = 585165;
        public const long LpidNotSpecifiedException = 585166;
        public const long LanguagePairNotFoundException = 585167;
        public const long UserNotAuthorizedForThisLanguagePairException = 585168;
        public const long InputSourceTextGreaterThanMaximumAllowableAmountException = 585169;
        public const long InvalidJobIDException = 585170;
        public const long NoSuchJobException = 585171;
        public const long InvalidApiKeyForSpecifiedJobIDException = 585172;
        public const long ConfigInvalidException = 585173;
        public const long UnknownTranslationProviderException = 585174;

        // Item Handler Section (585201 - 585225)
        public const long ItemHandlerError = 585201;
        public const long ItemHandlerParamInvalid = 585202;
        public const long ItemHandlerParamMissing = 585203;
        public const long ItemHandlerConfigInvalid = 585204;
        public const long ItemHandlerConfigMissing = 585205;
        public const long ItemHandlerUserAuthFailed = 585206;
        public const long ItemHandlerFileSizeExceeded = 585207;
        public const long ItemHandlerFileGetError = 585208;
        public const long ItemHandlerPamError = 585209;
        public const long ItemHandlerImageHeightExceeded = 585210;
        public const long ItemHandlerImageWidthExceeded = 585211;
        public const long ItemHandlerFileTypeInvalid = 585212;
        public const long ItemHandlerFileMimeTypeInvalid = 585213;
        public const long ItemHandlerFileNotFound = 585214;

        // DJInsider Handler Section (585226 - 585250)
        public const long DjindexHandlerError = 585226;
        public const long DjindexHandlerParamInvalid = 585227;
        public const long DjindexHandlerParamMissing = 585228;
        public const long DjindexHandlerUserAuthFailed = 585229;
        public const long DjindexHandlerAddDjiPreferenceFailed = 585230;
        public const long DjindexHandlerGetDjiPreferenceFailed = 585231;
        public const long DjindexHandlerUpdateDjiPreferenceFailed = 585232;

        // News Pages (585251-585275)
        public const long InvalidSearchContextString = 585251;
        public const long InvalidTopNewsPart = 585252;
        public const long CustomTopicTypeNotFound = 585253;
        public const long ModuleTypeNotFoundInDictionary = 585254;
        public const long InvalidCustomTopic = 585255;
        public const long InvalidCacheKey = 585256;
        public const long InvalidSyndicationItem = 585257;
        public const long MultimediaEpisodeGuidNotFound = 585258;
        public const long MultimediaRampFailed = 585259;
        public const long MultimediaMarketWatchFailed = 585260;
        public const long MultimediaHtmlUrlNotFound = 585261;
        public const long InvalidDeliveryMethod = 585262;
        public const long InvalidProductType = 585263;
        public const long InvalidShareAccessScope = 585264;
        public const long TrendingEntityTypeNotSupported = 585265;
        public const long ControlDataIsNull = 585266;
        public const long SearchContextTypeIsInvalid = 585267;

        // Page Asset Manager Section (585276 - 585300)
        public const long BasePageAssetsManagerError = 585275;
        public const long ModuleIsNull = 585276;
        public const long InvalidModuleData = 585277;
        public const long InvalidDataAccessRequest = 585278;
        public const long InvalidValidationRequest = 585279;
        public const long InvalidUpdateRequest = 585280;
        public const long InvalidUpdatePositionsRequest = 585281;
        public const long UnableToParseAlertId = 585282;
        public const long InvalidGetRequest = 585283;
        public const long InvalidModuleType = 585284;
        public const long UnableToCreatePage = 585285;
        public const long InvalidReplaceRequestRequest = 585286;
        public const long ModuleDoesNotExistOnPage = 585287;
        public const long PageIsNull = 585288;

        public const long EmptyAlertCollection = 585289;
        public const long EmptyFCodeCollection = 585290;
        public const long EmptyCustomTopicCollection = 585291;
        public const long EmptyNewsstandListCollection = 585292;
        public const long EmptyQueryEntityCollection = 585293;
        public const long EmptySourcesListCollection = 585294;
        public const long EmptySyndicationFeedIDCollection = 585295;

        public const long ModuleTypeNotExpected = 585296;

        public const long RegionalMapQueryEntityNotFound = 585297;

        public const long EmptyTopicsCollection = 585298;

        // Rendering Manager Section (585301 - 585325)
        public const long RenderManagerServerSideProcessingTimeout = 585301;


        //TODO: Get range for Infrastructure (585326 - 585350)
        public const long UninitializedPrincipleException = 585326;
        public const long OperationAbortedException = 585327;

        //TODO: Get range for Query Utility errors 585411 - 585499
        public const long QM_DATE_TYPE_NOT_SPECIFIED = 585411;
        public const long QM_ONE_OF_EACH_GROUP_ALLOWED = 585412;
        public const long QM_SEARCH_STRING_LENGTH_EXCEEDED = 585414;
        public const long QM_INVALID_QUERY = 585415;
        public const long QM_INVALID_FILTER_TYPE = 585416;
        public const long QM_FREETEXTFILTER_MULTIPLE_TEXT_NOT_ALLOWED = 585417;
        public const long QM_SEARCHADDITIONALFILTERS_MULTIPLE_SOURCE_FILTERS_NOT_ALLOWED = 585418;
        public const long QM_SEARCHADDITIONALFILTERS_KEYWORDS_NOT_SUPPORTED = 585419;
        public const long QM_STRUCTUREDSEARCH_IS_NULL = 585420;
        public const long QM_FILTER_OPERATOR_NOT_SUPPORTED = 585421;
        public const long QM_SEARCHFORMATCATEGORY_NOT_SUPPORTED = 585422;
        public const long QM_RESPONSE_IS_NULL = 585423;
        public const long QM_QUERY_TYPE_NOT_SUPPORTED = 585424;

         //TODO: Get range for Thunderball errors 585500 - 585525
        public const long ThunderballService_EmptyResponse = 585500;

        //Social Media backend error (585601 - 585610)

        //Social Media Backend Error - wrap different result status
        public const long SocialMediaBackendUnknownError = 585601;
        public const long SocialMediaBackendUserError = 585602;
        public const long SocialMediaBackendServerError = 585603;
        public const long SocialMediaBackendDeserializationError = 585604;

        #endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof(DowJonesUtilitiesException));
        private long returnCode = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesUtilitiesException"/> class.
        /// </summary>
        public DowJonesUtilitiesException()
        {
            LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesUtilitiesException"/> class.
        /// </summary>
        /// <param name="returnCodeFromFactivaService">The return code from factiva service.</param>
        public DowJonesUtilitiesException(long returnCodeFromFactivaService)
        {
            returnCode = returnCodeFromFactivaService;
            LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesUtilitiesException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="returnCodeFromFactivaService">The return code from factiva service.</param>
        public DowJonesUtilitiesException(string message, long returnCodeFromFactivaService) : base(message)
        {
            returnCode = returnCodeFromFactivaService;
            LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesUtilitiesException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DowJonesUtilitiesException(string message) : base(message)
        {
            LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesUtilitiesException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DowJonesUtilitiesException(string message, Exception innerException)
            : base(message, innerException)
        {
            LogException();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesUtilitiesException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="returnCodeFromFactivaService">The return code from factiva service.</param>
        public DowJonesUtilitiesException(Exception innerException, long returnCodeFromFactivaService)
            : base("", innerException)
        {
            returnCode = returnCodeFromFactivaService;

            LogException();
        }

        

        public virtual ILog Logger
        {
            get { return Log; }
        }

        public virtual long ReturnCode
        {
            get { return returnCode; }
        }

        protected virtual long ReturnCodeProtected
        {
            set { returnCode = value; }
        }

        public static DowJonesUtilitiesException ParseExceptionMessage(Exception ex)
        {
            long rc;
            return long.TryParse(ex.Message, out rc) ? new DowJonesUtilitiesException(rc) : new DowJonesUtilitiesException(ex, BaseUtillityError);
        }

        /// <summary>
        ///  Method that logs exception to <see cref="log4net"/>.
        /// </summary>
        protected void LogException()
        {
            if(ReturnCode == -1 || Logger.IsDebugEnabled)
            {
                // Always log -1 errors to ERROR log
                //var sb = new StringBuilder();
                //sb.AppendFormat("\nReturn code: {0} - Message: {1}", ReturnCode, Message);
                //GetInnerExceptionLog(sb, InnerException);
                //Logger.Error(sb.ToString());
                var stackTrace = StackTrace ?? new StackTrace().ToString();
                Logger.Error(string.Format("\nReturn code: {0} - Message: {1}\nStack Trace: {2}", ReturnCode, Message, stackTrace));
            }
            
        }

        static internal void GetInnerExceptionLog(StringBuilder sb, Exception innerException, int depth = 0)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (innerException == null)
                return;

            GetExceptionLog(sb, innerException, depth);

            GetInnerExceptionLog(sb, innerException.InnerException, depth + 1);
        }

        static internal void GetExceptionLog(StringBuilder sb, Exception exception, int depth)
        {
            sb.AppendFormat("\n==========Inner Exception level {0}==========", depth);
            sb.AppendFormat("\nInner Exception Type: {0}", exception.GetType().FullName);
            sb.AppendFormat("\nInner Exception Message: {0}", exception.Message);
            sb.AppendFormat("\nInner Exception Source: {0}", exception.Source);
            sb.AppendFormat("\nInner Exception StackTrace:\n{0}", exception.StackTrace);
        }
    }
}
