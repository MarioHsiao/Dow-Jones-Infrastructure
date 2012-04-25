using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using DowJones.Infrastructure;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Web;

namespace DowJones.Assemblers.Session
{
    [Obsolete("Use UserSessionFactory instead (class renamed)")]
    public class CookieBasedUserSessionFactory : UserSessionFactory
    {
        public CookieBasedUserSessionFactory(HttpContextBase httpContext, HttpCookieManager cookieManager, ReferringProduct referringProduct) 
            : base(httpContext, cookieManager, referringProduct)
        {
        }
    }

    public class UserSessionFactory : Factory<IUserSession>
    {
        protected internal const string DefaultProductId = "16";
        protected internal const string LoginCookieName = "login";
        protected internal const string LoginCookieDebugLevelKey = "dg";
        protected internal const string RequestDebugLevelKey = "debugLevel";
        protected internal const string LanguageKey = "fcpil";

        private readonly HttpCookieManager _cookieManager;
        private readonly ReferringProduct _referringProduct;
        private readonly HttpContextBase _httpContext;
        private UserSession _session;

        public UserSessionFactory(HttpContextBase httpContext, HttpCookieManager cookieManager, ReferringProduct referringProduct)
        {
            _httpContext = httpContext;
            _cookieManager = cookieManager;
            _referringProduct = referringProduct;
        }

        public string ProductPrefix
        {
            get
            {
                return _referringProduct.ProductPrefix 
                    ?? Settings.Default.DefaultProductPrefix;
            }
        }

        public string ClientTypeCode
        {
            get
            {
                return _referringProduct.ClientTypeCode 
                    ?? Settings.Default.DefaultClientCodeType;
            }
        }

        public string AccessPointCode
        {
            get
            {
                return _referringProduct.AccessPointCode 
                    ?? Settings.Default.DefaultAccessPointCode;
            }
        }

        public override IUserSession Create()
        {
            _session = new UserSession
                           {
                               AccessPointCode = AccessPointCode,
                               AccountId = _cookieManager.GetSessionValue(ProductPrefix + "_A"),
                               ClientTypeCode = ClientTypeCode,
                               ProductId = _cookieManager.GetSessionValue(ProductPrefix + "_N"),
                               ProductPrefix = ProductPrefix,
                               SessionId = DecodeSessionId(_cookieManager.GetSessionValue(ProductPrefix + "_S")),
                               UserId = DecodeUserId(_cookieManager.GetSessionValue(ProductPrefix + "_U")),
                           };

            var language = _httpContext.Request[LanguageKey];

            if (string.IsNullOrEmpty(language))
                language = _cookieManager.GetLocalPermanentValue("lang");

            if (!string.IsNullOrEmpty(language))
                _session.SetInterfaceLanguage(language);

            var hasSessionCookie = _cookieManager.HasGlobalSessionCookie;
            if (!hasSessionCookie)
            {
                _session.AccountId = _cookieManager.GetPermanentValue(ProductPrefix + "_A");
                _session.ProductId = _cookieManager.GetPermanentValue(ProductPrefix + "_N");
                _session.UserId = _cookieManager.GetPermanentValue(ProductPrefix + "_U");
            }

            SetDebug();

            PopulateProxyInfo(_session);

            if (string.IsNullOrEmpty(_session.ProductId))
                _session.ProductId = DefaultProductId;

            return _session;
        }

        private void SetDebug()
        {
            int debugLevel;
            if (!int.TryParse(_cookieManager.GetCookieValue(LoginCookieName, LoginCookieDebugLevelKey), out debugLevel))
                int.TryParse(_httpContext.Request[RequestDebugLevelKey], out debugLevel);

            _session.IsDebug = debugLevel > 0;
        }

        private static string DecodeUserId(string userId)
        {
            if (userId == null)
                return null;

            return HttpCookieManager.DecodeAspValueString(userId);
        }

        private static string DecodeSessionId(string sid)
        {
            if (sid == null)
                return null;
            sid = sid.Replace("%5F", "_");
            sid = sid.Replace("%2F", "/");
            sid = sid.Replace("%3D", "=");
            return sid;
        }

        private void PopulateProxyInfo(UserSession userSession)
        {
            if (userSession.SessionId == null)
            {
                userSession.ProxyUserId = _cookieManager.GetSessionValue(ProductPrefix + "_PU");
                userSession.ProxyNamespace = _cookieManager.GetSessionValue(ProductPrefix + "_PN");
            }

            PopulateProxyInfoFromCredentials(userSession, _httpContext.Request.Headers["credentials"]);
        }

        /// <summary>
        /// Parses user credentials and proxy credentials passed in request header. Accepts XML and JSON formats.
        /// TODO: 
        ///     1. It must have all required fields to initialize session data, can not read some value from cookies
        ///
        /// <example>
        /// <b>XML Format</b>
        /// <![CDATA[
        ///     <credentials> 
        ///         <accessPointCode>NP</accessPointCode>
        ///         <accessPointCodeUsage>NP</accessPointCodeUsage>
        ///         <cacheKey></cacheKey> 
        ///         <clientCode></clientCode> 
        ///         <token>27137ZzZINAUQT2CAAAGUAIAAAAAAYU2AAAAAABSGAYTCMBXGEZTCMBSGIZTCMJZ</token>
        ///         <credentialType>sessionId</credentialType>
        ///         <proxyUserId></proxyUserId>
        ///         <proxyUserNamespace></proxyUserNamespace> 
        ///         <remoteAddress>172.25.247.244</remoteAddress>
        ///     </credentials>
        /// ]]>
        /// <b>JSON Format</b>
        /// <c>
        /// {
        ///     "accessPointCode":null,
        ///     "accessPointCodeUsage":null,
        ///     "remoteAddress":"127.0.0.1",
        ///     "token":"27137ZzZKJHEQUSCAAAGUAIAAAAADCGRAAAAAABSGAYTCMBZGA4DCOBTGMZDKNZQ",
        ///     "credentialType":0
        /// }
        /// </c>
        /// </example>
        /// </summary>
        protected internal void PopulateProxyInfoFromCredentials(UserSession userSession, string credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials))
                return;

            bool? success = TryPopulateProxyInfoFromCredentialsJson(userSession, credentials);

            if (success.HasValue && !success.Value)
                success = TryPopulateProxyInfoFromCredentialsXml(userSession, credentials);

            if (success.HasValue && !success.Value)
                throw new ArgumentException("Credentials header could not be parsed. Please ensure that it is valid JSON or XML");
        }

        protected internal bool? TryPopulateProxyInfoFromCredentialsJson(UserSession userSession, string credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials))
                return null;

            try
            {
                var ser = new JavaScriptSerializer();
                var cred = ser.Deserialize<Credentials>(credentials);
                userSession.ProxyUserId = cred.ProxyUserId;
                userSession.ProxyNamespace = cred.ProxyUserNamespace;
                return true;
            }
            catch
            {
                return false;
            }
        }


        protected internal bool? TryPopulateProxyInfoFromCredentialsXml(UserSession userSession, string credentialsXml)
        {
            if (string.IsNullOrWhiteSpace(credentialsXml))
                return null;

            try
            {
                XDocument doc = XDocument.Parse(credentialsXml);

                if (doc.Root == null) return false;

                IEnumerable<XElement> populatedNodes =
                    doc.Root.Descendants().Where(x => !string.IsNullOrWhiteSpace(x.Value));

                userSession.ProxyUserId =
                    populatedNodes.Where(x => x.Name == "proxyUserId")
                        .Select(x => x.Value)
                        .FirstOrDefault() ?? userSession.ProxyUserId;

                userSession.ProxyNamespace =
                    populatedNodes.Where(x => x.Name == "proxyUserNamespace")
                        .Select(x => x.Value)
                        .FirstOrDefault() ?? userSession.ProxyNamespace;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}