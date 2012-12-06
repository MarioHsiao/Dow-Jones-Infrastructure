using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.WebPages;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Extensions;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using DowJones.Infrastructure;

namespace DowJones.Factiva.Currents.Components.CurrentsHeadline
{
	public class CurrentsHeadlineModel : Web.Mvc.UI.ViewComponentModel
	{
		private readonly PortalHeadlineListModel _portalHeadlineList;

		public IEnumerable<PortalHeadlineInfo> Headlines
		{
			get { return _portalHeadlineList
							.Result.ResultSet.Headlines.Take(MaxNumHeadlinesToShow); }
		}

		public int MaxNumHeadlinesToShow { get; set; }

		public bool HasData
		{
			get { return Headlines.Any(); }
		}

		public string SelectedGuid { get; set; }
		public bool ShowSource { get; set; }
		public bool ShowPublicationDateTime { get; set; }
        public bool ShowAuthor { get; set; }

		public bool SourceClickable { get; set; }
        public bool AuthorClickable { get; set; }
        public bool MultimediaMode { get; set; }
       
        public bool ShowSnippet { get; set; }

		public CurrentsHeadlineModel(PortalHeadlineListModel portalHeadlineList)
		{
			Guard.IsNotNull(portalHeadlineList, "portalHeadlineList");
			_portalHeadlineList = portalHeadlineList;
			MaxNumHeadlinesToShow = _portalHeadlineList.MaxNumHeadlinesToShow;
			ShowSource = _portalHeadlineList.ShowSource;
			ShowPublicationDateTime = _portalHeadlineList.ShowPublicationDateTime;
            ShowAuthor = _portalHeadlineList.ShowAuthor;
            MultimediaMode = true;//_portalHeadlineList.MultimediaMode;
		}

        public bool ShouldShowSnippet(PortalHeadlineInfo headline)
        {
            return ShowSnippet 
                && headline.Snippets != null 
                && headline.Snippets.Count > 0 
                && !headline.Snippets.First().IsEmpty();
        }

		// view helpers
		public string GetSelectionStatus(PortalHeadlineInfo headline)
		{
			return SelectedGuid == headline.Reference.guid ? "dj_entry_selected" : string.Empty;
		}

		public bool ShouldShowSource(PortalHeadlineInfo headline)
		{
			return ShowSource
					&& !headline.SourceCode.IsEmpty()
					&& !headline.SourceDescriptor.IsEmpty();
		}

        public bool ShouldShowAuthor(PortalHeadlineInfo headline)
        {
            return ShowAuthor
                    && headline.Authors != null
                    && headline.Authors.Count > 0;
            
        }

        public bool IsMultimediaContent(PortalHeadlineInfo headline)
        {
            return MultimediaMode
                    && !string.IsNullOrEmpty(headline.ContentCategoryDescriptor)
                    && headline.ContentCategoryDescriptor.Equals("multimedia",StringComparison.CurrentCultureIgnoreCase);
        }

		public bool ShouldShowPublicationDateTime(PortalHeadlineInfo headline)
		{
            if(headline.PublicationDateDescriptor != null)
			    return ShowPublicationDateTime
                        && !headline.PublicationDateDescriptor.IsEmpty();
            else
                return ShowPublicationDateTime
                     && !headline.PublicationDateTimeDescriptor.IsEmpty();
		}

		public string GetHeadlineUrl(PortalHeadlineInfo headline, UrlHelper urlHelper)
		{
			// TODO: Refactor to remove hardcoded controller name from the Url

			if (!headline.ContentCategoryDescriptor.Equals("external"))
				return urlHelper.Content("~/article/{0}".FormatWith(headline.GenerateSeoUrl()));

			return headline.HeadlineUrl;
		}
	}

	public static class PortalHeadlineInfoExtensions
	{
		static readonly Regex WhiteListRegex = new Regex(@"[^\w\d\s]");
		static readonly Regex NormalizeSpacesRegex = new Regex(@"\s+");

		public static string GenerateSeoUrl(this PortalHeadlineInfo headline)
		{
			const string format = "{0}/{1}/{2}/{3}/{4}";

			return format
				.FormatWith(
					headline.PublicationDateTime.Year,
					headline.PublicationDateTime.Month,
					headline.PublicationDateTime.Day,
					Canonicalize(Sanitize(headline.Title)), 
					headline.Reference.guid
				);
		}

		private static string Sanitize(string input)
		{
			return WhiteListRegex.Replace(input, string.Empty);
		}

		private static string Canonicalize(string input)
		{
			return Normalize(input).Replace(" ", "-").ToLowerInvariant();
		}

		private static string Normalize(string input)
		{
			return NormalizeSpacesRegex.Replace(input, " ").Trim();
		}
	}
}