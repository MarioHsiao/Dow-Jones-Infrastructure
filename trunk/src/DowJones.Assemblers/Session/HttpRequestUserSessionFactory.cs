using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DowJones.Infrastructure;
using DowJones.Extensions;
using DowJones.Session;

namespace DowJones.Assemblers.Session
{
    public class HttpRequestUserSessionFactory : Factory<IUserSession>
    {
        private readonly HttpContextBase _httpContext;
        private const string UrlFormat = "http://{0}/api/1.0/Session/login/xml?userid={1}&password={2}&namespace={3}";
        private volatile static string loginServerHostName;
        private string _productId = "16";

        protected UserSession Session;

        public static string LoginServerHostName
        {
            get { return loginServerHostName ?? "api.int.dowjones.com"; }
            set { loginServerHostName = value; }
        }

        public string UserId
        {
            get { return GetRequestValue("userId", string.Empty); }
        }

        public string Password
        {
            get { return GetRequestValue("password", string.Empty); }
        }

        public string ProductId
        {
            get { return GetRequestValue("ProductId", _productId); }
            set { _productId = value; }
        }

        public string AccessPointCode
        {
            get { return GetRequestValue("apc", string.Empty); }
        }

        public string AccessPointCodeUsage
        {
            get { return GetRequestValue("apcu", string.Empty); }
        }

        public string ClientTypeCode
        {
            get { return GetRequestValue("ctc", string.Empty); }
        }

        public HttpRequestUserSessionFactory(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        private string GetRequestValue(string key, string defaultValue = null)
        {
            return string.IsNullOrWhiteSpace(_httpContext.Request[key])
                       ? defaultValue
                       : _httpContext.Request[key];
        }

        private string GenerateSessionID()
        {
            var url = string.Format(UrlFormat, LoginServerHostName, UserId, Password, ProductId);

            if (UserId.IsNullOrEmpty() || Password.IsNullOrEmpty() || ProductId.IsNullOrEmpty())
            {
                throw new ApplicationException("You need to provide a password, userId and productId to generate the appropriate UserSessionData");
            }

            XDocument response;

            try
            {
                response = XDocument.Load(url);
            }
            catch (Exception ex)
            {
                var message = string.Format("Couldn't automatically generate session ID from session server {0}- perhaps it's down?", LoginServerHostName);
                throw new ApplicationException(message, ex);
            }

            try
            {
                if (response.Root != null && response.Root.Name.LocalName.Contains("ErrorResponse"))
                    if (response.Document != null) throw new ApplicationException(response.Document.ToString());

                var sessionId =
                    response
                        .Descendants("SessionId")
                        .Select(x => x.Value)
                        .SingleOrDefault();

                return sessionId;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error parsing Session ID response from Session Server", ex);
            }
        }

        public override IUserSession Create()
        {

            if (!_httpContext.IsDebuggingEnabled)
            {
                throw new ApplicationException("Refusing to allow the DevelopmentSessionFactory outside of debug mode!");
            }

            // If a session id was passed in the request, use that
            var sessionId = GetRequestValue("session") ?? GetRequestValue("sessionid");
            if (sessionId != null)
                return new UserSession { UserId = UserId, SessionId = sessionId, ProductId = ProductId };

            // Otherwise, request a session directly

            var session = new UserSession
            {
                AccessPointCode = AccessPointCode,
                AccessPointCodeUsage = AccessPointCodeUsage,
                ClientTypeCode = ClientTypeCode,
                ProductId = ProductId,
                ProductPrefix = "GL",
                SessionId = GenerateSessionID(),
                UserId = UserId,
            };

            return session;
        }
    }
}