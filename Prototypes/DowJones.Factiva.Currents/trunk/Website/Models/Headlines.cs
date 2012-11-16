using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;

namespace DowJones.Factiva.Currents.Website.Models
{
	public class Headlines
	{
		public string ViewAllSearchContext { get; set; }

		public CurrentsHeadlineModel CurrentsHeadline { get; set; }
	}
}