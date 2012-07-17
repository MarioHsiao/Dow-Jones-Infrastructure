using DowJones.Web.Mvc.UI.Components.CompositeOutlet;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.RelatedConcepts;

namespace DowJones.Web.Mvc.Search.UI.Components.Results.Outlets
{
	public class OutletsSearchResults : SearchResults
	{
		const int PAGE_SIZE_DEFAULT_VALUE = 20;

		public CompositeOutletModel Outlets { get; set; }
		public RelatedConceptsModel RelatedConcepts { get; set; }
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