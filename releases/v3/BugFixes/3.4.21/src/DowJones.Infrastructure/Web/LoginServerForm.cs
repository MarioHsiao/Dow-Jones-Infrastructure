using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class LoginServerForm : IHtmlString
    {
        public static readonly IEnumerable<string> DefaultExcludedKeys = new [] {
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
            LoginServerUrl = DowJones.Properties.Settings.Default.LoginServerUrl;
            TargetSite = DowJones.Properties.Settings.Default.LoginConfigFile.ToAbsoluteUrl();
        }

        public LoginServerForm(HttpRequestBase request, string redirectUrl = null, IEnumerable<string> excludedKeys = null)
            : this()
        {
            Guard.IsNotNull(request, "request");

            InterfaceLanguage = request["interfacelanguage"];
            RedirectUrl = redirectUrl ?? request.Url.ToString();
            Referrer = request.ServerVariables["HTTP_REFERER"];
            RemoteAddress = request.ServerVariables["REMOTE_ADDR"];

            ImportParameters(request.QueryString, excludedKeys);
            ImportParameters(request.Form, excludedKeys);
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
                    .Where(key => !key.IsNullOrEmpty())
                    .Where(key => !nameValueCollection[key].IsNullOrEmpty())
                    .Where(key => !excludedKeys.Contains(key, true))
                    .Select(key => new KeyValuePair<string,string>(key, nameValueCollection[key]));

            AdditionalParameters.Merge(parameters, true);
        }
    }
}