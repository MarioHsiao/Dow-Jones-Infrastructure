using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace DowJones.Orchard.SingleSignOn
{
    public class LoginServerForm : IHtmlString
    {
        public static readonly IEnumerable<string> DefaultExcludedKeys = new[] {
                "fcpil", "interfacelanguage", "targetsite", 
                "landingpage", "http_referer", "remote_addr",
            };

        public IDictionary<string, object> AdditionalParameters { get; private set; }

        public string InterfaceLanguage { get; set; }

        public string LoginServerUrl { get; set; }

        public string RedirectUrl { get; set; }

        public string Referrer { get; set; }

        public string RemoteAddress { get; set; }

        public string TargetSite { get; set; }


        public LoginServerForm()
        {
            AdditionalParameters = new Dictionary<string, object>();
            InterfaceLanguage = "en";
            LoginServerUrl = ConfigurationManager.AppSettings["LoginServerUrl"];
        }

        public LoginServerForm(HttpRequestBase request, string redirectUrl = null, IEnumerable<string> excludedKeys = null)
            : this()
        {
            InterfaceLanguage = request["interfacelanguage"];
            RedirectUrl = redirectUrl ?? GetRedirectUrl(request);
            Referrer = request.ServerVariables["HTTP_REFERER"];
            RemoteAddress = request.ServerVariables["REMOTE_ADDR"];
            TargetSite = GetTargetSite(request);

            ImportParameters(request.QueryString, excludedKeys);
            ImportParameters(request.Form, excludedKeys);
        }

        private static string GetRedirectUrl(HttpRequestBase request)
        {
            string redirectUrl = request.QueryString["redirectUrl"];

            if(string.IsNullOrWhiteSpace(redirectUrl))
                redirectUrl = VirtualPathUtility.ToAbsolute("~/");

            if(redirectUrl.StartsWith("http") == false)
            {
                redirectUrl = string.Format("{0}{1}",
                        GetSchemeAndAuthority(request),
                        redirectUrl
                    );
            }

            return redirectUrl;
        }

        private static string GetTargetSite(HttpRequestBase request)
        {
            var targetSite = ConfigurationManager.AppSettings["LoginServerTargetSite"];

            if (string.IsNullOrWhiteSpace(targetSite))
            {
                targetSite = string.Format("{0}{1}",
                        GetSchemeAndAuthority(request),
                        VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["LoginConfigFile"])
                    );
            }

            return targetSite;
        }

        private static string GetSchemeAndAuthority(HttpRequestBase request)
        {
            return string.Format("{0}://{1}",
                                 request.Url.Scheme,
                                 request.Url.Authority);
        }

        public string ToHtmlString()
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
                AppendHiddenField(
                    contentBuilder,
                    additionalParameter.Key,
                    additionalParameter.Value.ToString()
                    );
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

        public void ImportParameters(NameValueCollection nameValueCollection, IEnumerable<string> excludedKeys)
        {
            excludedKeys = excludedKeys ?? DefaultExcludedKeys;

            IEnumerable<KeyValuePair<string, string>> parameters =
                nameValueCollection.Keys.Cast<string>()
                    .Where(key => !string.IsNullOrWhiteSpace(key))
                    .Where(key => !string.IsNullOrWhiteSpace(nameValueCollection[key]))
                    .Where(key => !excludedKeys.Contains(key))
                    .Select(key => new KeyValuePair<string, string>(key, nameValueCollection[key]));

            foreach (var parameter in parameters)
                AdditionalParameters[parameter.Key] = parameter.Value;
        }
    }
}