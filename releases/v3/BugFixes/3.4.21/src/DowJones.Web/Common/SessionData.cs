// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionData.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the SessionData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using DowJones.Exceptions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Numerical;
using DowJones.Globalization;
using DowJones.Loggers;
using DowJones.Managers.Abstract;
using DowJones.Properties;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Profile.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Session
{
    /// <summary>
    /// </summary>
    public class SessionData
    {
        public const string GLOBAL_SESS_COOKIE_KEY = "GSLogin";
        public const string GLOBAL_PERM_COOKIE_KEY = "GPLogin";
        public const string CONTEXT_KEY_CURRENT_SESSION_DATA_OBJECT = "CURRENT_UTILITY_SESSION_DATA_OBJECT";
        public const string EMG_ACCESS_OBJECT_COOKIE_NAME = "Entitlements_{0}_{1}_{2}";
        public const string EMG_LANGUAGE_COOKIE_KEY = "persistent";
        public const string EMG_LANGUAGE_SUBCOOKIE_KEY = "lang";

        public static readonly string DEFAULT_ACCESS_POINT_CODE = Settings.Default.DefaultAccessPointCode;
        public static readonly string DEFAULT_PRODUCT_PREFIX = Settings.Default.DefaultProductPrefix;
        public static readonly string DEFAULT_CLIENT_CODE_TYPE = Settings.Default.DefaultClientCodeType;
        public static readonly string DEFAULT_INTERFACE_LANGUAGE = Settings.Default.DefaultInterfaceLanguage;
        public static readonly string DEFAULT_FACTIVA_PREFIX = Settings.Default.DefaultFactivaPrefix;
        //// public readonly static bool DEFAULT_BYPASS_CLIENTBILLING = Settings.Default.DefaultByPassClientBilling;

        //// sm-111708 for having client type set as per the product

        /// <summary>
        /// The client type code that is used for the external reader site type user.
        /// </summary>
        public static readonly string DEFAULT_EXTERNAL_READER_CLIENTTYPECODE = "X";

        /// <summary>
        /// The client type code that is used for the internal reader site type user.
        /// </summary>
        public static readonly string DEFAULT_INTERNAL_READER_CLIENTTYPECODE = "R";

        /// <summary>
        /// The client type code that is used for the iWorks site type user.
        /// </summary>
        public static readonly string DEFAULT_IWORKS_CLIENTTYPECODE = "K";

        private readonly ILog log = LogManager.GetLogger(typeof(SessionData));
        private readonly string accessPointCode = DEFAULT_ACCESS_POINT_CODE;
        private readonly int _debugLevel;
        private readonly bool _initializeEntitlementsObject = true;
        private static string _version;
        private string _clientTypeCode = DEFAULT_CLIENT_CODE_TYPE;
        private string _productPrefix = DEFAULT_PRODUCT_PREFIX;
        private string _interfaceLanguage = DEFAULT_INTERFACE_LANGUAGE;
        private string _cultureInfoCode = DEFAULT_INTERFACE_LANGUAGE;

        //// private bool _bypassClientBilling = DEFAULT_BYPASS_CLIENTBILLING;

        //// sm -091508 for the SAFROM_PU and PN cookies that login server will set when it know this is proxy access
        //// this is for View All and reader LWS/Proxy logins that is implemented in Q4 2008
        //// this comes in from newsletters and widgets;
        private string _proxyUserId;
        private string _proxyProductId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData"/> class.
        /// </summary>
        public SessionData()
            : this(null, null, 0, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData"/> class.
        /// </summary>
        /// <param name="accessPointCode">The access point code.</param>
        public SessionData(string accessPointCode)
            : this(accessPointCode, null, 0, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData"/> class.
        /// </summary>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public SessionData(string accessPointCode, string interfaceLanguage)
            : this(accessPointCode, interfaceLanguage, 0, true)
        {
        }

        public SessionData(string proxyUserId, string proxyNameSpace, string accessPointCode, string interfaceLanguage, int debugLevel, bool initializeFactivaAccessObject, string overridenPrefix, string overridenAPC)
            : this(accessPointCode, interfaceLanguage, debugLevel, initializeFactivaAccessObject, overridenPrefix, overridenAPC)
        {
            _proxyUserId = proxyUserId;
            _proxyProductId = proxyNameSpace;

            // Read it in from a cookie.
            UserId = _proxyUserId;
            ProductId = _proxyProductId;

            //// BypassClientBilling = _bypassClientBilling;
            if (String.IsNullOrEmpty(UserId) || String.IsNullOrEmpty(ProductId))
            {
                return;
            }

            _initializeEntitlementsObject = initializeFactivaAccessObject;

            var factivaAccessObject = ReadBaseEntitlementsObjectFromCookie();
            if (factivaAccessObject != null)
            {
                BaseEntitlementsObject = factivaAccessObject;
            }
        }

        public SessionData(string accessPointCode, string interfaceLanguage, int debugLevel, bool initializeFactivaAccessObject, string overridenPrefix, string overridenAPC)
        {
            // override the following defaults based on passed parameters
            this.accessPointCode = !string.IsNullOrEmpty(overridenAPC) ? overridenAPC : accessPointCode;
            _interfaceLanguage = interfaceLanguage;
            _debugLevel = debugLevel;
            _initializeEntitlementsObject = initializeFactivaAccessObject;

            // override ProductPrefix and clientTypeCode based on accessPointCode
            SetVariablesBasedOnAccessPointCode();

            // Read login server cookies
            if (!string.IsNullOrEmpty(overridenPrefix))
            {
                _productPrefix = overridenPrefix;
            }

            ReadGlobalCookies(_productPrefix);

            // Set the UI Culture
            SetUICulture(_interfaceLanguage);

            // SetFormatters(_interfaceLanguage);
            SetFormatters(_cultureInfoCode);

            // Initialize the BaseEntitlementsObject
            if (initializeFactivaAccessObject)
            {
                // Read it in from a cookie.
                var baseEntitlementsObject = ReadBaseEntitlementsObjectFromCookie();
                if (baseEntitlementsObject != null)
                {
                    BaseEntitlementsObject = baseEntitlementsObject;
                }
            }

            // Save session data instance to HttpContext
            HttpContext.Current.Items[CONTEXT_KEY_CURRENT_SESSION_DATA_OBJECT] = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData"/> class.
        /// </summary>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="debugLevel">The debug level.</param>
        /// <param name="initializeFactivaAccessObject">if set to <c>true</c> [initialize factiva access object].</param>
        public SessionData(string accessPointCode, string interfaceLanguage, int debugLevel, bool initializeFactivaAccessObject)
        {
            // override the following defaults based on passed parameters
            this.accessPointCode = accessPointCode;
            _interfaceLanguage = interfaceLanguage;
            _debugLevel = debugLevel;

            // override ProductPrefix and clientTypeCode based on accessPointCode
            SetVariablesBasedOnAccessPointCode();

            // Read login server cookies
            ReadGlobalCookies(_productPrefix);

            // Set the UI Culture
            SetUICulture(_interfaceLanguage);

            // SetFormatters(_interfaceLanguage);
            SetFormatters(_cultureInfoCode);

            // Initialize the BaseEntitlementsObject
            if (initializeFactivaAccessObject)
            {
                // Read it in from a cookie.
                var baseEntitlements = ReadBaseEntitlementsObjectFromCookie();
                if (baseEntitlements != null)
                {
                    BaseEntitlementsObject = baseEntitlements;
                }
            }

            // Save session data instance to HttpContext
            HttpContext.Current.Items[CONTEXT_KEY_CURRENT_SESSION_DATA_OBJECT] = this;
        }

        /// <summary>
        /// Gets the session based control data.
        /// </summary>
        /// <value>The session based control data.</value>
        public IControlData SessionBasedControlData
        {
            get
            {
                return IsLoginServerCookieIndicatingProxyUserAccess()
                           ? GetInternalReaderLightWeightUserWithProxyCredentials(_proxyUserId, _proxyProductId)
                           : ControlDataManager.GetSessionUserControlData(SessionId, _clientTypeCode, accessPointCode);
            }
        }

        /// <summary>
        /// Gets the number formatter.
        /// </summary>
        /// <value>The number formatter.</value>
        public NumberFormatter NumberFormatter { get; private set; }

        /// <summary>
        /// Gets the date time formatter.
        /// </summary>
        /// <value>The date time formatter.</value>
        public DateTimeFormatter DateTimeFormatter { get; private set; }

        /// <summary>
        /// Gets or sets the base entitlements object.
        /// </summary>
        /// <value>The base entitlements object.</value>
        public BaseEntitlements BaseEntitlementsObject { get; set; }

        /// <summary>
        /// Gets or sets the client type code.
        /// </summary>
        /// <value>The client type code.</value>
        public string ClientTypeCode
        {
            get { return _clientTypeCode; }
            set { _clientTypeCode = value; }
        }

        /// <summary>
        /// Gets or sets the culture language code.
        /// </summary>
        /// <value>The culture language code.</value>
        public string CultureLanguageCode
        {
            get { return _cultureInfoCode; }
            set { _cultureInfoCode = value; }
        }

        /// <summary>
        /// Gets the access point code.
        /// </summary>
        /// <value>The access point code.</value>
        public string AccessPointCode
        {
            get { return accessPointCode; }
        }

        /// <summary>
        /// Gets the product prefix.
        /// </summary>
        /// <value>The product prefix.</value>
        public string ProductPrefix
        {
            get { return _productPrefix; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; private set; }

        /// <summary>
        /// Gets the product id.
        /// </summary>
        /// <value>The product id.</value>
        public string ProductId { get; private set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <value>The session id.</value>
        public string SessionId { get; private set; }

        /// <summary>
        /// Gets the account id.
        /// </summary>
        /// <value>The account id.</value>
        public string AccountId { get; private set; }

        /// <summary>
        /// Gets the interface language.
        /// </summary>
        /// <value>The interface language.</value>
        public string InterfaceLanguage
        {
            get { return _cultureInfoCode; }
        }

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        /// <value>The rule set.</value>
        public RuleSet RuleSet { get; private set; }

        /// <summary>
        /// Gets the debug level.
        /// </summary>
        /// <value>The debug level.</value>
        public int DebugLevel
        {
            get { return _debugLevel; }
        }

        /// <summary>
        /// Gets or sets the proxy user id.
        /// </summary>
        /// <value>The proxy user id.</value>
        public string ProxyUserId
        {
            get { return _proxyUserId; }
            set { _proxyUserId = value; }
        }

        /// <summary>
        /// Gets or sets the proxy product id.
        /// </summary>
        /// <value>The proxy product id.</value>
        public string ProxyProductId
        {
            get { return _proxyProductId; }
            set { _proxyProductId = value; }
        }

        public static SessionData Instance()
        {
            var sessionData = (SessionData)HttpContext.Current.Items[CONTEXT_KEY_CURRENT_SESSION_DATA_OBJECT];
            return sessionData;
        }

        private void SetFormatters(string interfacelanguage)
        {
            NumberFormatter = new NumberFormatter();
            DateTimeFormatter = new DateTimeFormatter(interfacelanguage);
        }

        private void SetUICulture(string interfaceLanguage)
        {
            // Set interface language
            _interfaceLanguage = LanguageUtilityManager.ValidateLanguageCodeWithTemplate(interfaceLanguage);

            _cultureInfoCode = LanguageUtilityManager.GetCultureInfoLanguageCode(_interfaceLanguage, true);

            try
            {
                switch (_interfaceLanguage)
                {
                    default:
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_cultureInfoCode);
                        break;
                    case "zh":
                    case "zh-cn":
                    case "zhcn":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-cn");
                        break;
                    case "zh-tw":
                    case "zhtw":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-tw");
                        break;

                    case "template":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        _interfaceLanguage = "en";
                        _cultureInfoCode = "en";
                        break;
                }
            }
            catch
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("Unable to set content-language");
                }
            }

            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private BaseEntitlements DeserializeEntitlementsObject(string compressedStr)
        {
            if (string.IsNullOrEmpty(compressedStr) || string.IsNullOrEmpty(compressedStr.Trim()))
            {
                return null;
            }

            var utfEncoder = new UTF8Encoding();
            try
            {
                // Decompress the compressed string.
                var byteB = Convert.FromBase64String(compressedStr);
                var temp = utfEncoder.GetString(byteB);

                // De-Serialize Decompressed string int FactivaAcessObject
                using (var reader = new StringReader(temp))
                {
                    var xmlSerializer = new XmlSerializer(typeof(BaseEntitlements));
                    return (BaseEntitlements)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return null;
        }

        /// <summary>
        /// Gets the internal reader light weight user with proxy credentials.
        /// Pass in a controlData object and allowable proxy user-id, ns;
        /// Only certain transactions are allowed for proxy usage. Failure might indicate a non-allowed transaction
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public static IControlData GetInternalReaderLightWeightUserWithProxyCredentials(string userId, string productId)
        {
            var lightWeightUser = Settings.Default.InternalReaderLightweightUser;

            var controlData = ControlDataManager.GetLightWeightUserControlData(lightWeightUser.userId, lightWeightUser.userPassword, lightWeightUser.productId);
            controlData.ProxyUserId = userId;
            controlData.ProxyProductId = productId;
            return controlData;
        }

        public BaseEntitlements GetBaseEntitlementsObject()
        {
            GetUserProfileResponse getUserProfileResp = null;
            NewsViewsPermissions newsViewsPermissions = null;
            GetUserAuthorizationsNoCacheResponse authorizationsNoCacheResponse = null;

            var profileUserType = UserType.Unspecified;
            var profileManager = new ProfileManager(SessionBasedControlData, new BasicTransactionTimer());

            if ((!string.IsNullOrEmpty(SessionId) &&
                 !string.IsNullOrEmpty(_clientTypeCode) &&
                 !string.IsNullOrEmpty(accessPointCode)) ||
                (!string.IsNullOrEmpty(_proxyUserId) && !string.IsNullOrEmpty(_proxyProductId)))
            {

                getUserProfileResp = profileManager.GetUserProfileResponseStub();
                RuleSet = new RuleSet(getUserProfileResp.ruleSet);

                ////  get the user type from the user profile too...
                if (getUserProfileResp.userType != null)
                {
                    // M (Media)
                    // G (Government)
                    // R (Regular)
                    // A (Academic)
                    // c (Customer Service)
                    switch (getUserProfileResp.userType.ToUpper())
                    {
                        case "M":
                            profileUserType = UserType.Media;
                            break;
                        case "G":
                            profileUserType = UserType.Government;
                            break;
                        case "R":
                            profileUserType = UserType.Regular;
                            break;
                        case "A":
                            profileUserType = UserType.Academic;
                            break;
                        case "C":
                            profileUserType = UserType.CustomerService;
                            break;
                    }
                }

                //// meaning with proxy..
                if (IsLoginServerCookieIndicatingProxyUserAccess())
                {
                    //// since this transaction cannot impersonation and the user is passed in payload...
                    var ctrlData = SessionBasedControlData;
                    ctrlData.ProxyUserId = string.Empty;
                    ctrlData.ProxyProductId = string.Empty;
                    authorizationsNoCacheResponse = profileManager.GetUserAuthorizationsNoCacheResponse(_proxyUserId, _proxyProductId, ctrlData);

                    if (String.IsNullOrEmpty(AccountId) && !String.IsNullOrEmpty(authorizationsNoCacheResponse.AccountId))
                    {
                        AccountId = authorizationsNoCacheResponse.AccountId;
                    }

                    newsViewsPermissions = profileManager.GetNewsViewsPermissions(authorizationsNoCacheResponse);
                }
                else
                {
                    authorizationsNoCacheResponse =
                        profileManager.GetUserAuthorizationsNoCacheResponse(UserId, ProductId);
                    newsViewsPermissions = profileManager.GetNewsViewsPermissions(authorizationsNoCacheResponse);
                }
            }

            if (RuleSet != null && newsViewsPermissions != null && authorizationsNoCacheResponse != null)
            {
                var entitlements = new BaseEntitlements
                {
                    IsDotComDisplayServiceOn = RuleSet.IsDotComDisplayServiceOn,
                    IsTrackCoreServiceOn = RuleSet.IsTrackCoreServiceOn,
                    IsFcpDisplayServiceOn = RuleSet.IsFcpCompanyTabDisplayServiceOn,
                    IsFcpCompanyTabDisplayServiceOn = RuleSet.IsFcpCompanyTabDisplayServiceOn,
                    IsFcpIndustryTabDisplayServiceOn = RuleSet.IsFcpIndustryTabDisplayServiceOn,
                    IsDotComCompanyQuickSearchDisplayServiceOn = RuleSet.IsDotComCompanyQuickSearchDisplayServiceOn,
                    IsDotComQuoteDisplayServiceOn = RuleSet.IsDotComQuoteDisplayServiceOn,
                    IsDotComWorkspaceDisplayServiceOn = RuleSet.IsDotComBriefcaseDisplayServiceOn,
                    IsNewsViewsAdministratorOn = newsViewsPermissions.IsNewsViewsAdministratorOn,
                    CurrentUserType = authorizationsNoCacheResponse.UserType,
                    RuleSet = getUserProfileResp.ruleSet,
                    IsDotComTrackDisplayServiceOn = RuleSet.IsDotComTrackDisplayServiceOn
                };

                IsLoginServerCookieIndicatingProxyUserAccess();

                // Get the users Registration Industry code and add it BaseEntitlementsObject
                // This will be used while building the default page to get the default Editors Choice widget code.
                if (getUserProfileResp.industryCode != null)
                {
                    entitlements.UserRegistrationIndustryCode = getUserProfileResp.industryCode;
                }

                // Update the authorization access for newsletters
                entitlements.IsDotComNewsletterDisplayServiceOn = true;
                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac5 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac5.Count == 1 &&
                    !string.IsNullOrEmpty(authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac5[0]) &&
                    !string.IsNullOrEmpty(authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac5[0].Trim()))
                {
                    // Check the Interface/AC8 to see if Package is turned on
                    switch (authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac5[0].ToUpperInvariant())
                    {
                        case "OFF":
                            entitlements.IsDotComNewsletterDisplayServiceOn = false;
                            break;
                    }
                }

                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Membership != null)
                {
                    entitlements.IsAllowedToSetExternalReaderProfiles = authorizationsNoCacheResponse.externalReaderFlag;

                    entitlements.IsAllowedToSetTTLProxyCredentials = authorizationsNoCacheResponse.AuthorizationMatrix.Membership.newsletterDA != null && authorizationsNoCacheResponse.AuthorizationMatrix.Membership.newsletterDA.ToUpper() == "TTLT";
                    if (authorizationsNoCacheResponse.AuthorizationMatrix.@Interface != null)
                    {
                        entitlements.IsAllowedToRetriveSharePointWebPart = authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.allowSharePointWidget != null
                                                                            && authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.allowSharePointWidget.ToUpper() == "ON";
                    }

                    if (authorizationsNoCacheResponse.administratorFlag == AdministratorFlag.AccountAdministrator)
                    {
                        entitlements.IsAccountAdministrator = true;
                    }

                    var isAllowedToPersonalize = (authorizationsNoCacheResponse.AuthorizationMatrix.Membership.personalization == null ||
                                                  authorizationsNoCacheResponse.AuthorizationMatrix.Membership.personalization != "OFF") &&
                                                 profileUserType != UserType.Academic;

                    // This is irrespective of what is set in membership
                    if (isAllowedToPersonalize || entitlements.IsAccountAdministrator)
                    {
                        entitlements.IsPersonalizationOn = true;
                    }

                    //// if nor personalization and not sharind DA, then off.. else true
                    if (entitlements.IsPersonalizationOn &&
                        (authorizationsNoCacheResponse.AuthorizationMatrix.Membership.sharingDA == null ||
                         authorizationsNoCacheResponse.AuthorizationMatrix.Membership.sharingDA != "OFF"))
                    {
                        entitlements.IsSharingOn = true;
                    }
                }

                // mt 2008 Q2 - DJI access
                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.insider != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.insider == "OFF")
                {
                    entitlements.IsDowJonesInsider = false;
                }

                if (!(authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                      authorizationsNoCacheResponse.AuthorizationMatrix.@Interface != null &&
                      authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.IsMyDJFactivaEnabled))
                {
                    entitlements.IsMyDJFactivaOn = false;
                }

                //// mt 2008 Q2 - DJI access
                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.projectVisibleAds != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.projectVisibleAds == "OFF")
                {
                    entitlements.IsProjectVisibleAdsOn = false;
                }


                //// sm_3/1907 - > for userallowed to save cookie and save password
                entitlements.IsUserAllowedToChangePassword = profileManager.IsUserAllowedToChangePassword();
                entitlements.IsUserAllowedToSavePersistentCookie = profileManager.IsUserAllowedToSavePersistentCookie();

                //// sm_3/1907 - > for EmailState, and EmailConversion
                entitlements.UserEmailLoginConversionState = authorizationsNoCacheResponse.emailLoginConversionAllowed;
                entitlements.UserEmailLoginState = authorizationsNoCacheResponse.emailLoginState;

                //// sm_05/14/07 - > for WSJ.com link
                entitlements.IsUserAllowedToViewWSJCOMLink = getUserProfileResp.allowedWSJAccess;

                //// factivaAccessObject.isAdminBlockingAlertEmailDelivery =
                ////  profileManager.IsAdminBlockingAlertEmailDelivery();

                //// mt 2008 Q4 - RIC Replacement
                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac9 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac9.Count == 1 &&
                    !string.IsNullOrEmpty(authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac9[0]) &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac9[0].ToUpperInvariant() == "RIC=ON")
                {
                    entitlements.IsGrandFatheredUser = true;
                }

                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac6 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac6.Count == 1 &&
                    !string.IsNullOrEmpty(authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac6[0]) &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac6[0].Contains("ADVISOR"))
                {
                    entitlements.IsSegmentationBrandOverride = true;
                    entitlements.SegmentationPrdType = authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac6[0];
                }

                // sm 2009 Q2 - reader flex.. here;s the deal; if we allow any user to coming in here even if not reader
                // then we need to know if the user is reader when we show the Upgrade link
                // the siteusertype will be 'reader' but the user might not have ac8 ='prv'
                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac8 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac8.Count == 1)
                {
                    entitlements.InterfaceAc8 = authorizationsNoCacheResponse.AuthorizationMatrix.@Interface.ac8[0];
                }

                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Index != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Index.ac1 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Index.ac1.Count == 1)
                {
                    entitlements.IndexAC1 = authorizationsNoCacheResponse.AuthorizationMatrix.Index.ac1[0];
                }

                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Index != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Index.ac2 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Index.ac2.Count == 1)
                {
                    entitlements.IndexAC2 = authorizationsNoCacheResponse.AuthorizationMatrix.Index.ac2[0];
                }

                if (authorizationsNoCacheResponse.AuthorizationMatrix != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac2 != null &&
                    authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac2.Count == 1)
                {
                    entitlements.ArchiveAC2 = authorizationsNoCacheResponse.AuthorizationMatrix.Archive.ac2[0];
                }

                return entitlements;
            }

            return null;
        }

        public static string GetVersion()
        {
            if (string.IsNullOrEmpty(_version))
            {
                _version = Assembly.GetAssembly(typeof(SessionData)).GetName().Version.ToString();
            }

            return _version;
        }

        private static string GetCurrentApplicationPath()
        {
            //// Return variable declaration
            string appPath;

            //// Getting the current context of HTTP request
            var context = HttpContext.Current;

            //// Checking the current context content
            if (context != null)
            {
                //// Formatting the fully qualified website url/name
                appPath = string.Format(
                                        "{0}://{1}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host
                                        );
                /* context.Request.Url.Port == 80
                ? string.Empty : ":" + context.Request.Url.Port,
              context.Request.ApplicationPath*/

                return appPath.ToLower();
            }

            /*if (!appPath.EndsWith("/"))
                appPath += "/";*/
            return string.Empty;
        }

        private static string GetCookieDomain()
        {
            var validDomains = new List<string> { ".factiva.com", ".dojones.com" };
            var hostName = GetCurrentApplicationPath();

            foreach (var validDomain in validDomains.Where(hostName.Contains))
            {
                return validDomain;
            }

            return string.Empty;
        }

        public void UpdateEntitlementsObjectCookie(BaseEntitlements baseEntitlements)
        {
            if (baseEntitlements == null)
                return;
            var update = true;
            var cookieName = string.Format(EMG_ACCESS_OBJECT_COOKIE_NAME, UserId, ProductId, GetVersion());

            var factivaAccessObjectCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (factivaAccessObjectCookie == null)
            {
                // Add cookie
                factivaAccessObjectCookie = new HttpCookie(cookieName) { Domain = GetCookieDomain(), Path = Settings.Default.EntitlementsCookiePath, Expires = DateTime.Now.AddMinutes(20) };
                update = false;
            }
            else
            {
                // just update the time
                factivaAccessObjectCookie.Expires = DateTime.Now.AddMinutes(20);
            }

            var utfEncoder = new UTF8Encoding();
            try
            {
                using (var sw = new StringWriter(new StringBuilder()))
                {
                    // Faking the existence of custom namespaces has a nice side effect          
                    // of leaving namespaces out entirely.   
                    var faker = new XmlSerializerNamespaces();
                    faker.Add(string.Empty, null);
                    var writer = new XmlTextWriter(sw);
                    var xmlSerializer = new XmlSerializer(typeof(BaseEntitlements), string.Empty);
                    writer.WriteRaw(string.Empty);
                    xmlSerializer.Serialize(writer, baseEntitlements, faker);
                    writer.Flush();
                    var byteA = utfEncoder.GetBytes(sw.ToString());
                    factivaAccessObjectCookie.Value = HttpUtility.UrlEncode(Convert.ToBase64String(byteA));
                    if (update)
                        HttpContext.Current.Response.Cookies.Set(factivaAccessObjectCookie);
                    else
                        HttpContext.Current.Response.Cookies.Add(factivaAccessObjectCookie);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        private BaseEntitlements ReadBaseEntitlementsObjectFromCookie()
        {
            var baseEntitlementsObjectCookie = HttpContext.Current.Request.Cookies[String.Format(EMG_ACCESS_OBJECT_COOKIE_NAME, UserId, ProductId, GetVersion())];
            BaseEntitlements baseEntitlementsObject;

            if (_initializeEntitlementsObject &&
                baseEntitlementsObjectCookie != null &&
                !string.IsNullOrEmpty(baseEntitlementsObjectCookie.Value))
            {
                baseEntitlementsObject = DeserializeEntitlementsObject(HttpUtility.UrlDecode(baseEntitlementsObjectCookie.Value.Trim()));
                if (baseEntitlementsObject == null)
                {
                    baseEntitlementsObject = GetBaseEntitlementsObject();
                    UpdateEntitlementsObjectCookie(baseEntitlementsObject);
                    return baseEntitlementsObject;
                }

                RuleSet = new RuleSet(baseEntitlementsObject.RuleSet);
                return baseEntitlementsObject;
            }
            baseEntitlementsObject = GetBaseEntitlementsObject();
            if (_initializeEntitlementsObject)
            {
                UpdateEntitlementsObjectCookie(baseEntitlementsObject);
            }

            return baseEntitlementsObject;
        }

        public static string GetLanguageFromCookie()
        {
            var retVal = SessionCookieManager.GetCookieValue(EMG_LANGUAGE_COOKIE_KEY, EMG_LANGUAGE_SUBCOOKIE_KEY);
            return !string.IsNullOrEmpty(retVal) ? retVal : "";
        }

        /// <summary>
        /// Determines whether [is login server cookie indicating proxy user access].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is login server cookie indicating proxy user access]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLoginServerCookieIndicatingProxyUserAccess()
        {
            var sid = SessionCookieManager.GetCookieValue(GLOBAL_SESS_COOKIE_KEY, "IF_S");
            if (!String.IsNullOrEmpty(sid))
            {
                return false;
            }

            var proxyUId = SessionCookieManager.GetCookieValue(GLOBAL_SESS_COOKIE_KEY, "IF_PU");
            var proxyPId = SessionCookieManager.GetCookieValue(GLOBAL_SESS_COOKIE_KEY, "IF_PN");

            return !String.IsNullOrEmpty(proxyUId) && !String.IsNullOrEmpty(proxyPId);
        }

        protected static void CreateLanguageCookie(string initialInterfaceLanguage)
        {
            SessionCookieManager.WritePersistentFactivaCookie(EMG_LANGUAGE_COOKIE_KEY, EMG_LANGUAGE_SUBCOOKIE_KEY, initialInterfaceLanguage);
        }

        public static bool SetLanguageCookie(string newInterfaceLanguage)
        {
            SessionCookieManager.WritePersistentFactivaCookie(EMG_LANGUAGE_COOKIE_KEY, EMG_LANGUAGE_SUBCOOKIE_KEY, newInterfaceLanguage);
            return GetLanguageFromCookie() == newInterfaceLanguage;
        }

        private void ReadGlobalCookies(string productPrefix)
        {
            var sessionCookie = (HttpContext.Current != null) ? HttpContext.Current.Request.Cookies[GLOBAL_SESS_COOKIE_KEY] : null;

            if (sessionCookie != null)
            {
                SessionId = sessionCookie.Values[productPrefix + "%5FS"] ?? sessionCookie.Values[productPrefix + "_S"];
                SessionId = HttpUtility.UrlDecode(SessionId);
            }

            if (SessionId == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ErrorInvalidSessionLong);
            }

            if (sessionCookie == null)
            {
                return;
            }

            ////  Note: _userId is UrlDecode due to the fact it may contain characters like[.]
            UserId = HttpUtility.UrlDecode(sessionCookie.Values[productPrefix + "%5FU"]) ??
                     HttpUtility.UrlDecode(sessionCookie.Values[productPrefix + "_U"]);

            //// 9/15/08 sm TO get the Proxy credentials if set by the login server; look for notes above where these variables are decl.
            _proxyUserId = SessionCookieManager.GetCookieValue(GLOBAL_SESS_COOKIE_KEY, "IF_PU");
            _proxyProductId = SessionCookieManager.GetCookieValue(GLOBAL_SESS_COOKIE_KEY, "IF_PN");

            var permanentCookie = HttpContext.Current.Request.Cookies[GLOBAL_PERM_COOKIE_KEY];

            if (UserId != null)
            {
                ProductId = sessionCookie.Values[productPrefix + "%5FN"] ??
                            sessionCookie.Values[productPrefix + "_N"];

                AccountId = sessionCookie.Values[productPrefix + "%5FA"] ??
                            sessionCookie.Values[productPrefix + "_A"];
            }
            else if (permanentCookie != null)
            {
                // Note: _userId is UrlDecode due to the fact it may contain characters like[.]
                UserId = HttpUtility.UrlDecode(permanentCookie.Values[productPrefix + "%5FU"]) ??
                         HttpUtility.UrlDecode(permanentCookie.Values[productPrefix + "_U"]);

                ProductId = HttpUtility.UrlDecode(permanentCookie.Values[productPrefix + "%5FN"]) ??
                            HttpUtility.UrlDecode(permanentCookie.Values[productPrefix + "_N"]);

                AccountId = HttpUtility.UrlDecode(permanentCookie.Values[productPrefix + "%5FA"]) ??
                            HttpUtility.UrlDecode(permanentCookie.Values[productPrefix + "_A"]);
            }
        }


        private void SetVariablesBasedOnAccessPointCode()
        {
            switch (accessPointCode)
            {
                case "A":
                case "C":
                case "G":
                case "J":
                case "K":
                case "P":
                case "Q":
                case "S":
                case "T":
                case "9":
                case "s":
                case "5": // Toolbar
                case "B":
                case "Y":
                case "E":
                case "R":
                case "z":
                case "w": // GRIPS/newsviews uses this
                case "p": // for now, dotcom and preview are using the same prefix
                case "2": // for the newsletter napc value...new from May 2007 bucket onwards
                case "i": // for the insight napc value...new from July 10th 2007 onwards
                    _clientTypeCode = "D";
                    _productPrefix = "IF";
                    break;

                case "7":
                    _clientTypeCode = "D";
                    _productPrefix = "GL";
                    break;
                case "WN": // FOR  WIDGET NEWSLETTER ENTRY -  SM Q1 2009
                case "WW": // FOR  WIDGET NEWSLETTER ENTRY -  SM Q1 2009
                case "RN": // FOR  WIDGET NEWSLETTER ENTRY -  SM Q1 2009
                case "RW": // FOR  WIDGET NEWSLETTER ENTRY -  SM Q1 2009
                case "NW": // FOR  WIDGET NEWSLETTER ENTRY -  SM Q1 2009
                case "PW": // FOR  WIDGET NEWSLETTER ENTRY -  SM Q1 2009
                    _clientTypeCode = "D";
                    _productPrefix = "IF";
                    break;
                case "l":
                case "h": // IWE (NonSales)
                case "a": // IWE (Sales)
                case "m": // Company screening
                case "d": // Industry snapshot
                case "x": // Executive snapshot/screening
                    _clientTypeCode = "S";
                    _productPrefix = "FC";
                    break;
                case "g":
                    _clientTypeCode = "M";
                    _productPrefix = "GE";
                    break;
                case "3": // Toolbar (Modules)
                    _clientTypeCode = "M";
                    _productPrefix = "GE";
                    break;
                case "u": // I-works (Modules)
                    _clientTypeCode = "M";
                    _productPrefix = "MO";
                    break;
                case "1": // Office
                case "f": // Office
                    _clientTypeCode = "M";
                    _productPrefix = "OF";
                    break;
                case "j": // WSJ
                    _clientTypeCode = "M";
                    _productPrefix = "WS";
                    break;
                case "e": // Reuters
                    _clientTypeCode = "M";
                    _productPrefix = "RE";
                    break;
                case "b": // SM-4/16/07 CHANGED FOR WIDGETS AS WIDGETS WIL USE THIS.
                    _clientTypeCode = "D";
                    _productPrefix = "GL";
                    break;
                case "v": // Mike hack #2395
                    _clientTypeCode = "M";
                    _productPrefix = "FU";
                    break;
                case "V": // Mike hack #2395
                    _clientTypeCode = "M";
                    _productPrefix = "MO";
                    break;
                case "DC":
                    _clientTypeCode = "G";
                    _productPrefix = "DC";
                    break;
                case "MR":
                    _clientTypeCode = "M";
                    _productPrefix = "MR";
                    break;
            }
        }

        /// <summary>
        /// Deletes the cookies set by Search 2.0 as apart of logout processe.
        /// 1. Clipping cookies
        /// 2. Factiva AccessObject cooies
        /// 3. newsview Admin cookie
        /// 4. ?? anything else missing?
        /// </summary>
        public void DeleteSetCookies(string userId, string productId)
        {
            var entitlementsCookieName = string.Format(EMG_ACCESS_OBJECT_COOKIE_NAME, userId, productId, GetVersion());
            SessionCookieManager.DeleteCookie(entitlementsCookieName);
        }

        public class BaseEntitlements
        {
            public string RuleSet;
            public string UserRegistrationIndustryCode = string.Empty;

            public bool IsAccountAdministrator;
            public bool IsDotComDisplayServiceOn;
            public bool IsNewsViewsAdministratorOn;
            public bool IsPersonalizationOn;
            public bool IsTrackCoreServiceOn;

            // mt 2008 Q2 Company occurence coding
            public bool IsFcpDisplayServiceOn;
            public bool IsFcpCompanyTabDisplayServiceOn;
            public bool IsFcpIndustryTabDisplayServiceOn;
            public bool IsDotComCompanyQuickSearchDisplayServiceOn;
            public bool IsDotComWorkspaceDisplayServiceOn;
            public bool IsDotComNewsletterDisplayServiceOn;
            public bool IsDotComQuoteDisplayServiceOn;

            //removing blocking 6/2/08 part of Q3 2008
            //public bool IsMultimediaContentOn;
            public bool IsSharingOn;
            public bool IsUserAllowedToSavePersistentCookie; //sm_3/19/07
            public bool IsUserAllowedToChangePassword;//sm_3/19/07
            public bool IsUserAllowedToViewWSJCOMLink; // sm_05/14/07 for wsj link.

            // public EmailValidationLevel UserEmailValidationLevel = EmailValidationLevel.NoEmailAddressHasBeenStoredInPrefereneces;
            public UserType CurrentUserType = UserType.Unspecified;
            public EmailLoginConversionAllowed UserEmailLoginConversionState = EmailLoginConversionAllowed.NotAllowed; //sm_3/19/07
            public EmailLoginState UserEmailLoginState = EmailLoginState.Unspecified; //sm_3/19/07

            public bool IsDotComTrackDisplayServiceOn;
            //public bool isAdminBlockingAlertEmailDelivery; //for Q4 2007

            // mt 2008 Q2 - DJI Access
            public bool IsDowJonesInsider = true;
            //public bool IsNttXtractionPOCOn;
            public bool IsProjectVisibleAdsOn = true;

            // mt 2008 Q4 - RIC Replacement
            public bool IsGrandFatheredUser;

            // dd 2008 Q4 - Widgets TTL/Proxy and ExternalReader
            public bool IsAllowedToSetExternalReaderProfiles;
            public bool IsAllowedToSetTTLProxyCredentials;
            public bool IsAllowedToRetriveSharePointWebPart;

            /*03202009 - to user interface/ac6 to decide the branding override for dotocm and newsletter builder */
            public bool IsSegmentationBrandOverride;
            public string SegmentationPrdType = "";//making this is a string as I am not sure what all are possible.ADVISOR-for WM..

            public string InterfaceAc8;

            public bool IsMyDJFactivaOn = true;

            public string IndexAC1 = string.Empty;
            public string IndexAC2 = string.Empty;
            public string ArchiveAC2 = string.Empty;

        }

        /// <summary>
        /// Summary description for ProfileManager.
        /// </summary>
        public class ProfileManager : AbstractAggregationManager
        {
            private static readonly ILog _log = LogManager.GetLogger(typeof(ProfileManager));
            private GetUserProfileResponse _response;

            /// <summary>
            /// Gets the log.
            /// </summary>
            /// <value>The log.</value>
            protected override ILog Log
            {
                get { return _log; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ProfileManager"/> class.
            /// </summary>
            /// <param name="controlData">The control data.</param>
            public ProfileManager(IControlData controlData, ITransactionTimer transactionTimer)
                : base(controlData, transactionTimer)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ProfileManager"/> class.
            /// </summary>
            /// <param name="sessionID">The session ID.</param>
            /// <param name="clientTypeCode">The client type code.</param>
            /// <param name="accessPointCode">The access point code.</param>
            public ProfileManager(string sessionID, string clientTypeCode, string accessPointCode, ITransactionTimer transactionTimer)
                : base(sessionID, clientTypeCode, accessPointCode, transactionTimer)
            {
            }

            /// <summary>
            /// retrieves user's stored email address
            /// </summary>
            /// <returns></returns>
            public string GetEmailAddress()
            {
                try
                {
                    if (_response != null)
                    {
                        return _response.email;
                    }
                    var request = new GetUserProfileRequest();
                    var response = Process<GetUserProfileResponse>(request);
                    _response = response;
                    return response == null ? null : response.email;
                }
                catch
                {
                    return null;
                }
            }



            /// <summary>
            /// Gets the rule set.
            /// </summary>
            /// <returns></returns>
            public RuleSet GetRuleSet()
            {
                try
                {
                    if (_response != null)
                    {
                        return new RuleSet(_response.ruleSet);
                    }
                    var request = new GetUserProfileRequest();
                    var response = Process<GetUserProfileResponse>(request);
                    _response = response;
                    return response == null ? null : new RuleSet(response.ruleSet);

                }
                catch
                {
                    return null;
                }
            }

            public GetUserProfileResponse GetUserProfileResponseStub()
            {

                if (_response != null)
                {
                    return _response;
                }
                var request = new GetUserProfileRequest();
                var response = Process<GetUserProfileResponse>(request);
                _response = response;
                return _response;

            }

            /// <summary>
            /// Checks if the user is Academic Uer type
            /// </summary>
            /// <returns>True if academic else false..</returns>
            public bool IsAcademicUserType()
            {
                if (_response != null)
                {
                    return _response.userType.ToUpper().Equals("A");
                }
                var request = new GetUserProfileRequest();
                var response = Process<GetUserProfileResponse>(request);
                _response = response;
                return _response.userType.ToUpper().Equals("A");
            }

            /// <summary>
            /// Gets the news views permissions.
            /// </summary>
            /// <param name="userId">The user id.</param>
            /// <param name="namespace">The @namespace.</param>
            /// <returns></returns>
            public NewsViewsPermissions GetNewsViewsPermissions(string userId, string @namespace)
            {
                GetUserAuthorizationsNoCacheResponse authResp = GetUserAuthorizationsNoCacheResponse(userId, @namespace);
                return new NewsViewsPermissions(authResp);
            }

            /// <summary>
            /// Gets the news views permissions.
            /// </summary>
            /// <param name="authResp">The auth resp.</param>
            /// <returns></returns>
            public NewsViewsPermissions GetNewsViewsPermissions(GetUserAuthorizationsNoCacheResponse authResp)
            {
                return new NewsViewsPermissions(authResp);
            }

            /// <summary>
            /// Gets the get user authorizations no cache response.
            /// </summary>
            /// <param name="userId">The user id.</param>
            /// <param name="namespace">The @namespace.</param>
            /// <returns></returns>
            public GetUserAuthorizationsNoCacheResponse GetUserAuthorizationsNoCacheResponse(string userId, string @namespace)
            {
                var auth = new GetUserAuthorizationsNoCacheRequest { userId = userId };
                return Process<GetUserAuthorizationsNoCacheResponse>(auth);
            }

            /// <summary>
            /// Gets the get user authorizations no cache response.
            /// </summary>
            /// <param name="userId">The user id.</param>
            /// <param name="namespace">The @namespace.</param>
            /// <param name="overrideControlData">The overriden control data.</param>
            /// <returns></returns>
            public GetUserAuthorizationsNoCacheResponse GetUserAuthorizationsNoCacheResponse(string userId, string @namespace, IControlData overrideControlData)
            {
                var auth = new GetUserAuthorizationsNoCacheRequest { userId = userId, productId = @namespace };

                ServiceResponse resp = Invoke<GetUserAuthorizationsNoCacheRequest>(auth, overrideControlData);

                Object objResponse;
                resp.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

                return (GetUserAuthorizationsNoCacheResponse)objResponse;
            }

            public bool IsUserAllowedToChangePassword()
            {
                return false;
            }

            public bool IsUserAllowedToSavePersistentCookie()
            {
                return false;
            }

        }

    }
}