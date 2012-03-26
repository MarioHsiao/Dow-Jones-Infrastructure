using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DowJones.Extensions;
using DowJones.Web.Mvc.Extensions;
using DowJones.Web.Mvc.Properties;
using DowJones.Web.UI;

namespace DowJones.Web.Mvc.ActionResults
{
    public class AuthenticationServerLoginRedirectActionResult : ContentResult
    {
        public IDictionary<string, object> AdditionalParameters { get; set; }

        public IEnumerable<string> ExcludeKeyList
        {
            get { return _excludeKeyList ?? Enumerable.Empty<string>(); }
            set { _excludeKeyList = value; }
        }
        private IEnumerable<string> _excludeKeyList = new [] { "fcpil", "interfacelanguage", "targetsite", "landingpage", "http_referer", "remote_addr" };

        public string InterfaceLanguage { get; set; }

        public string LoginServerUrl
        {
            get { return _loginServerUrl; }
            set { _loginServerUrl = value; }
        }
        private string _loginServerUrl = Settings.Default.AuthenticationServiceLoginUrl;

        public string RedirectUrl { get; set; }

        public string Referrer { get; set; }

        public string RemoteAddress { get; set; }

        public string TargetSite { get; set; }



        public AuthenticationServerLoginRedirectActionResult(ControllerContext controllerContext, string redirectUrl)
        {
            var request = controllerContext.HttpContext.Request;

            AdditionalParameters = AdditionalParameters ?? new Dictionary<string, object>();
            ContentType = KnownMimeTypes.Html;
            InterfaceLanguage = request["interfacelanguage"];
            RedirectUrl = redirectUrl;
            Referrer = request.ServerVariables["HTTP_REFERER"];
            RemoteAddress = request.ServerVariables["REMOTE_ADDR"];
        }


        public override void ExecuteResult(ControllerContext context)
        {
            ImportAdditionalParameters(context.HttpContext.Request);

            Content = BuildContent();

            base.ExecuteResult(context);
        }

        private string BuildContent()
        {
            var contentBuilder = new StringBuilder();

            contentBuilder.AppendLine("<html><body>");

            contentBuilder.AppendFormat("<form name='loginRedirect' method='POST' action='{0}'>\r", LoginServerUrl);

                AppendHiddenField(contentBuilder, "interfacelanguage", InterfaceLanguage);
                AppendHiddenField(contentBuilder, "fcpil", InterfaceLanguage);
                AppendHiddenField(contentBuilder, "http_referer", Referrer);
                AppendHiddenField(contentBuilder, "remote_addr", RemoteAddress);
                AppendHiddenField(contentBuilder, "targetsite", TargetSite);
                AppendHiddenField(contentBuilder, "landingPage", RedirectUrl);

                foreach (var additionalParameter in AdditionalParameters.Where(x => x.Value != null))
                {
                    AppendHiddenField(contentBuilder, 
                        additionalParameter.Key, additionalParameter.Value.ToString());
                }

            contentBuilder.AppendLine("</form>");

            contentBuilder.AppendLine("<script language='javascript'>document.loginRedirect.submit();</script>");
            
            contentBuilder.AppendLine("</body></html>");

            return contentBuilder.ToString();
        }

        private static void AppendHiddenField(StringBuilder contentBuilder, string name, string value)
        {
            contentBuilder.AppendFormat("<input type='hidden' name='{0}' value='{1}'></input>", name, value);
        }

        private void ImportAdditionalParameters(HttpRequestBase request)
        {
            var queryStringParameters = RetrieveParameters(request.QueryString);
            var formParameters = RetrieveParameters(request.Form);
            AdditionalParameters.Merge(queryStringParameters, true);
            AdditionalParameters.Merge(formParameters, true);
        }

        private IDictionary<string, object> RetrieveParameters(NameValueCollection nameValueCollection)
        {
            IEqualityComparer<string> caseInsensitiveComparer = new CaseInsensitiveComparer();
            IEnumerable<KeyValuePair<string, string>> parameters = 
                nameValueCollection.Keys.Cast<string>()
                    .Where(key => (!key.IsNullOrEmpty()  && !nameValueCollection[key].IsNullOrEmpty() && !ExcludeKeyList.Contains(key, caseInsensitiveComparer)))
                    .Select(key => new KeyValuePair<string,string>(key, nameValueCollection[key]));

            return parameters.ToDictionary(x => x.Key, y => (object)y.Value);
        }
    }

    internal class CaseInsensitiveComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return String.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }
}
