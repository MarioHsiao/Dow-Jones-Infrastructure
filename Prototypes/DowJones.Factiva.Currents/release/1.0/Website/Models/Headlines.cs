using System.Collections.Generic;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Extensions;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;

namespace DowJones.Factiva.Currents.Website.Models
{
	public class HeadlinePreview
	{
		public PortalHeadlineInfo Headline { get; set; }

        public string FormattedDateTime(PortalHeadlineInfo headline)
        {
            if (headline.PublicationTimeDescriptor.HasValue())
                return headline.PublicationDateTime.ToLocalTime().ToString("HH:mm") + "," + headline.PublicationDateDescriptor;
            else
                return headline.PublicationDateTimeDescriptor;
        }
	}

	// TODO: figure out a better name
	public class HeadlineList
	{
		public string ViewAllSearchContext { get; set; }

		public CurrentsHeadlineModel Headlines { get; set; }
	}
}