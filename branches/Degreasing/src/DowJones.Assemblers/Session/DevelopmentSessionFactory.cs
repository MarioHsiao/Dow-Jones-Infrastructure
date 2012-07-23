using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DowJones.Session;
using DowJones.Web;

namespace DowJones.Assemblers.Session
{
    /// <summary>
    /// Session Factory implementation that accepts a URL-based session id
    /// and/or retrieves one automatically from the session server
    /// </summary>
    public class DevelopmentSessionFactory : UserSessionFactory
    {
        private const string UrlFormat =
            "http://{0}/api/1.0/Session/login/xml?userid={1}&password={2}&namespace={3}";

        public static string LoginServerHostName
        {
            get { return loginServerHostName ?? "api.int.dowjones.com"; }
            set { loginServerHostName = value; }
        }
        private volatile static string loginServerHostName;

        private readonly HttpContextBase _httpContext;

        public string UserId
        {
            get { return GetRequestValue("UserId", _username); }
            set { _username = value; }
        }
        private string _username = "dacostad";

        public string Password
        {
            get { return GetRequestValue("Password", _password); }
            set { _password = value; }
        }
        private string _password = "vader";

        public string ProductId
        {
            get { return GetRequestValue("ProductId", _productId); }
            set { _productId = value; }
        }
        private string _productId = "16";


        public DevelopmentSessionFactory(HttpContextBase httpContext, HttpCookieManager cookieManager, ReferringProduct metaData)
            : base(httpContext, cookieManager, metaData)
        {
            _httpContext = httpContext;
        }


        public override IUserSession Create()
        {
            // If a session id was passed in the request, use that
            var sessionId = GetRequestValue("session") ?? GetRequestValue("sessionid");
            if (sessionId != null)
                return new UserSession { UserId = UserId, SessionId = sessionId, ProductId = ProductId };

            // See if the standard cookie-based approach works...
            var session = (UserSession)base.Create();

            if (string.IsNullOrWhiteSpace(session.UserId))
                session.UserId = UserId;
            
            if(session.IsValid())
                return session;

            // Otherwise, request a session directly
            session.SessionId = GenerateSessionID();

            return session;
        }

        private string GetRequestValue(string key, string defaultValue = null)
        {
            return string.IsNullOrWhiteSpace(_httpContext.Request[key]) 
                       ? defaultValue 
                       : _httpContext.Request[key];
        }

        private string GenerateSessionID()
        {
            string url = string.Format(UrlFormat, LoginServerHostName, UserId, Password, ProductId);

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
                if (response.Root.Name.LocalName.Contains("ErrorResponse"))
                    throw new ApplicationException(response.Document.ToString());

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
    }
}