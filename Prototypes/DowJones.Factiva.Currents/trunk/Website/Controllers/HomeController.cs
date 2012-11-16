using System.Web.Mvc;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Factiva.Currents.Website.Controllers
{
    using System.Collections.Generic;
    using System.Xml;
    using Contracts;
    using DowJones.Factiva.Currents.Website.Models;

	public class HomeController : ControllerBase
	{
		private readonly IPageAssetProvider _pageAssetProvider;

		public HomeController(IPageAssetProvider pageAssetProvider)
		{
			_pageAssetProvider = pageAssetProvider;
		}

		public ActionResult Index(string type, string name)
		{
			var pages = _pageAssetProvider.GetPages();
			return View(pages);
		}

        public ActionResult SiteMap(string type, string name)
        {
            var pages = _pageAssetProvider.GetPages();
            Response.ContentType = "text/xml";
            return this.Content(GenerateXml(pages), "text/xml");
        }

        private string GenerateXml(IEnumerable<PageListModel> pages)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("urlset");
            xmlDoc.AppendChild(root);
            UrlHelper urlHelper = new UrlHelper(Request.RequestContext);
           
            IEnumerator<PageListModel> model = pages.GetEnumerator();
            while (model.MoveNext())
            {
                var pageListModel = model.Current;
                XmlElement url = xmlDoc.CreateElement("url");
                XmlElement loc = xmlDoc.CreateElement("loc");
                if (!string.IsNullOrEmpty(Request.Url.OriginalString) && pageListModel != null)
                {
                    var protocol =  HttpContext.Request.Url.Scheme;
                    loc.InnerText = urlHelper.Action(pageListModel.FriendlyTitle, "Pages", null, protocol);
                }
                url.AppendChild(loc);

                XmlElement lastMod = xmlDoc.CreateElement("lastmod");
                lastMod.InnerText = System.DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                url.AppendChild(lastMod);
                XmlElement changeFreq = xmlDoc.CreateElement("changefreq");
                changeFreq.InnerText = "hourly";
                url.AppendChild(changeFreq);
                XmlElement priority = xmlDoc.CreateElement("priority");
                priority.InnerText = "0.9";
                url.AppendChild(priority);
                root.AppendChild(url);
            }

            return xmlDoc.OuterXml;
        }

       
	}
}
