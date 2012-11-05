using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Factiva.Currents.Models
{
	public class CurrentSourcesModel : Module
	{
		public IEnumerable<CurrentsHeadlineModel> CurrentsHeadlines { get; set; }
	}
}
