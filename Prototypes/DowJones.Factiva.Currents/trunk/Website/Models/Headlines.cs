using System.Collections.Generic;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;

namespace DowJones.Factiva.Currents.Website.Models
{
	public class HeadlinePreview
	{
		public PortalHeadlineInfo Headline { get; set; }
	}

	// TODO: figure out a better name
	public class HeadlineList
	{
		public string ViewAllSearchContext { get; set; }

		public CurrentsHeadlineModel Headlines { get; set; }
	}
}