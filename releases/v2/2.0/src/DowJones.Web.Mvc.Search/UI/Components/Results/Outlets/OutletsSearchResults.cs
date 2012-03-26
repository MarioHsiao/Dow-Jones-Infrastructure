using System.Collections.Generic;
using DowJones.Search.Navigation;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Mvc.UI.Components.Models.RelatedConcepts;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Search.UI.Components.Results.Outlets
{
	public class OutletsSearchResults : SearchResults
	{
		const int PAGE_SIZE_DEFAULT_VALUE = 25;

		public CompositeOutletModel Outlets { get; set; }
		public RelatedConceptsComponentModel RelatedConcepts { get; set; }
		public bool HideRelatedConcepts { get; set; }

		private int pageSize = 0;

		[ClientProperty("pageSize")]
		public int PageSize
		{
			get
			{
				if (this.pageSize == 0)
				{
					this.pageSize = PAGE_SIZE_DEFAULT_VALUE;
				}

				return this.pageSize;
			}
			set
			{
				this.pageSize = value;
			}

		}


		public OutletsSearchResults()
		{
			this.Outlets = new CompositeOutletModel();
		}

		public override bool HasResults
		{
			get { return true; }
		}
	}
}