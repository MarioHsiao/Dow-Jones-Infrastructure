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

		
	}
}