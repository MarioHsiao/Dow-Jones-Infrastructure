using System.Collections.Generic;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;

namespace DowJones.Factiva.Currents.Website.Models
{
	public class SourcesViewModel
	{
		public IEnumerable<CurrentsHeadlineModel> CurrentsHeadlines { get; set; }
	}
}