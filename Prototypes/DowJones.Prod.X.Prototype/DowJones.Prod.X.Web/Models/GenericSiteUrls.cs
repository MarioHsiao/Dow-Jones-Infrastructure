using System;
using System.Web;
using System.Web.Mvc;
using DowJones.Prod.X.Common.Extentions;
using DowJones.Prod.X.Models.Site;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Models
{
    public class GenericSiteUrls : IGenericSiteUrls
    {
        private static readonly Lazy<string> YouTubeUrl = new Lazy<string>(() => Properties.Settings.Default.YouTubeUrl);
        private static readonly Lazy<string> FacebookUrl = new Lazy<string>(() => Properties.Settings.Default.FacebookUrl);
        private static readonly Lazy<string> TwitterUrl = new Lazy<string>(() => Properties.Settings.Default.TwitterUrl);
        private static readonly Lazy<string> BaseSiteHost = new Lazy<string>(() => X.Common.Properties.Settings.Default.BaseDotComHost);
        private static readonly Lazy<string> MarketingSiteUrl = new Lazy<string>(() => Properties.Settings.Default.MarketingSiteUrl);

        public GenericSiteUrls(IUserSession userSession, HttpContextBase httpContextBase, UrlHelper urlHelper)
        {
            if (httpContextBase == null || httpContextBase.Request == null || httpContextBase.Request.Url == null) return;

            PrivacyPolicy = string.Format(Properties.Settings.Default.PrivacyPolicyUrl,
                                          httpContextBase.Request.Url.Scheme,
                                          BaseSiteHost.Value,
                                          userSession.InterfaceLanguage,
                                          MapProduct(userSession.ProductPrefix));

            BaseDotcomUrl = string.Format("{0}://{1}", httpContextBase.Request.Url.Scheme, BaseSiteHost);
            BaseAppUrl = urlHelper.ContentAbsolute("~/");

            //Url for Live Help
            CustomerPortalUrl = String.Format("{0}/{1}/custsvc/webchat.aspx?SA_FROM={2}&amp;APC={3}", Properties.Settings.Default.CustomerPortalUrl, userSession.InterfaceLanguage, userSession.ProductPrefix, userSession.AccessPointCode);
            
        }

        public string BaseDotcomUrl { get; private set; }

        public string PrivacyPolicy { get; private set; }

        public string BaseAppUrl { get; private set; }

        public string CustomerPortalUrl { get; private set; }
        
        public string Marketing
        {
            get { return MarketingSiteUrl.Value; }
        }

        public string YouTube
        {
            get { return YouTubeUrl.Value; }
        }

        public string Facebook
        {
            get { return FacebookUrl.Value; }
        }

        public string Twitter
        {
            get { return TwitterUrl.Value; }
        }

        private static string MapProduct(string productPrefix)
        {
            switch (productPrefix.ToUpperInvariant())
            {
                case "IN":
                    return "insite";
                default:
                    return "global";
            }
        }

    }
}