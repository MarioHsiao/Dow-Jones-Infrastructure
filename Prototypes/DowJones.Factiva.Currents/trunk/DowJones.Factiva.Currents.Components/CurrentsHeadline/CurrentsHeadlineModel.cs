using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using DowJones.Infrastructure;

namespace DowJones.Factiva.Currents.Components.CurrentsHeadline
{
	public class CurrentsHeadlineModel : DowJones.Web.Mvc.UI.ViewComponentModel
	{
		private readonly PortalHeadlineListModel _portalHeadlineList;

		public IEnumerable<PortalHeadlineInfo> Headlines
		{
			get { return _portalHeadlineList.Result.ResultSet.Headlines; }
		}

		public int MaxNumHeadlinesToShow
		{
			get { return _portalHeadlineList.MaxNumHeadlinesToShow; }
		}

		public bool HasData
		{
			get { return Headlines.Any(); }
		}

		public string SelectedGuid { get; set; }
		public bool ShowSource { get; set; }
		public bool ShowPublicationDateTime { get; set; }

		public bool SourceClickable
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}

		public CurrentsHeadlineModel(PortalHeadlineListModel portalHeadlineList)
		{
			Guard.IsNotNull(portalHeadlineList, "portalHeadlineList");
			_portalHeadlineList = portalHeadlineList;
		}


		// view helpers
		public string GetSelectionStatus(PortalHeadlineInfo headline)
		{
			return SelectedGuid == headline.Reference.guid ? "dj_entry_selected" : string.Empty;
		}

		public bool ShouldShowSource (PortalHeadlineInfo headline)
		{
			return ShowSource 
					&& !headline.SourceCode.IsEmpty() 
					&& !headline.SourceDescriptor.IsEmpty();
		}

		public bool ShouldShowPublicationDateTime(PortalHeadlineInfo headline)
		{
			return ShowPublicationDateTime
					&& !headline.PublicationDateDescriptor.IsEmpty();
		}

		public string GetHeadlineUrl(PortalHeadlineInfo headline)
		{
            string headlineTitle = string.Empty;
            string accessNo = string.Empty;
            switch (headline.ContentCategoryDescriptor)
            {
                case "customerdoc":
                case "summary":
                    //Article.getParentArticle(headline.Reference.guid);
                    break;
                case "external":
                    if (headline.HeadlineUrl != string.Empty)
                    {
                        //DJGlobal.NewWindow({ url: headline.headlineUrl, windowName: "webArticleWin" });
                    }
                    break;
                case "multimedia":
                    {
                        string an = headline.Reference.guid;
                        headlineTitle = headline.Title;
                        //DJGlobal.getMultimediaVideos(an, title, container, thumbNail);
                        break;
                    }
                default:
                    accessNo = headline.Reference.guid;
                    var url = headline.HeadlineUrl;
                    headlineTitle = headline.Title;
                    //Article.processArticleBasedOnType(accessionNumber, dv, cc, sc, ref, mimeType, (popup || false), url, headlineTitle);
                    break;
            }
            string baseUrl = GetBaseUrlPath();
            baseUrl += @"\headlines\"+headlineTitle.Replace(' ','_')+"?an="+accessNo;
            return baseUrl;
		}

        /// <summary>
        /// gets the domain path
        /// </summary>
        /// <returns>returns the domain path</returns>
        public string GetBaseUrlPath()
        {
            WebPageContext context = WebPageContext.Current;

            if (context != null && context.Page != null && context.Page.Request != null)
            {
                return context.Page.Request.Url.Host;
            }
            return string.Empty; 
        }
	}
}