using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DowJones.Extensions;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.ActionResults
{
    public class AuthServerLoginRedirectActionResult : ContentResult
    {
        public IDictionary<string, object> AdditionalParameters { get; private set; }

        public IList<string> ExcludeKeyList
        {
            get { return _excludeKeyList ?? new List<string>(); }
            set { _excludeKeyList = value; }
        }
        private IList<string> _excludeKeyList = new List<string> {
                "fcpil", "interfacelanguage", "targetsite", "landingpage", "http_referer", "remote_addr"
            };

        public string InterfaceLanguage { get; set; }

        public string LoginServerUrl { get; set; }

        public string RedirectUrl { get; set; }

        public string Referrer { get; set; }

        public string RemoteAddress { get; set; }

        public string TargetSite { get; set; }


        public AuthServerLoginRedirectActionResult(ControllerContext controllerContext, string redirectUrl = null)
        {
            Guard.IsNotNull(controllerContext, "controllerContext");
            Guard.IsNotNull(controllerContext.HttpContext, "controllerContext.HttpContext");
            Guard.IsNotNull(controllerContext.HttpContext.Request, "controllerContext.HttpContext.Request");
            
            var request = controllerContext.HttpContext.Request;

            AdditionalParameters = AdditionalParameters ?? new Dictionary<string, object>();
            ContentType = KnownMimeTypes.Html;
            InterfaceLanguage = request["interfacelanguage"];
            LoginServerUrl = DowJones.Properties.Settings.Default.LoginServerUrl;
            RedirectUrl = redirectUrl ?? request.Url.ToString();
            Referrer = request.ServerVariables["HTTP_REFERER"];
            RemoteAddress = request.ServerVariables["REMOTE_ADDR"];
            TargetSite = GetTargetSite(controllerContext.HttpContext);
        }

        private static string GetTargetSite(HttpContextBase httpContext)
        {
            var absolutePath = VirtualPathUtility.ToAbsolute(DowJones.Properties.Settings.Default.LoginConfigFile);
            return string.Format("http://{0}{1}", httpContext.Request.Url.Authority, absolutePath);
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

        private void ImportAdditionalParameters(HttpRequestBase request)
        {
            var queryStringParameters = RetrieveParameters(request.QueryString);
            var formParameters = RetrieveParameters(request.Form);
            AdditionalParameters.Merge(queryStringParameters, true);
            AdditionalParameters.Merge(formParameters, true);
        }

        private IEnumerable<KeyValuePair<string, object>> RetrieveParameters(NameValueCollection nameValueCollection)
        {
            IEnumerable<KeyValuePair<string, string>> parameters = 
                nameValueCollection.Keys.Cast<string>()
                    .Where(key => !key.IsNullOrEmpty())
                    .Where(key => !nameValueCollection[key].IsNullOrEmpty())
                    .Where(key => !ExcludeKeyList.Contains(key, true))
                    .Select(key => new KeyValuePair<string,string>(key, nameValueCollection[key]));

            return parameters.ToDictionary(x => x.Key, y => (object)y.Value);
        }
    }
}
