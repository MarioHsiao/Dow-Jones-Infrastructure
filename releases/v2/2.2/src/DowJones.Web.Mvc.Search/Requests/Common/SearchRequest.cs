using System.Collections.Generic;
using DowJones.Globalization;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.Search.UI.Components.Results.Headlines;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.Search.UI.Components.Results;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.Requests
{
	[SearchRequestModelBinder]
	public abstract class SearchRequest
	{
		public string FreeText { get; set; }

		public string Languages { get; set; }

		public uint Server { get; set; }

		public string ContextId { get; set; }

		public SearchDateRange? DateRange { get; set; }

		public string StartDate { get; set; }

		public string EndDate { get; set; }

		public SortOrder? Sort { get; set; }
															
		public ShowDuplicates? ShowDuplicates { get; set; }
															 
		public QueryFilters Filters { get; set; }

		public string PrimaryGroup { get; set; }

		public string SecondaryGroup { get; set; }

		/// <summary>
		/// Pagination Start Index
		/// </summary>
		public int? Start { get; set; }

		/// <summary>
		/// Pagination Max Results Count
		/// </summary>
		public int? PageSize { get; set; }

		public IEnumerable<string> RankUp { get; set; }
		public IEnumerable<string> RankDown { get; set; }

		public string StartIndexReference { get; set; }

		public bool HideDYM { get; set; }

		public bool HideRelatedConcepts { get; set; }

		public bool HideNewsVolume { get; set; }

		public AuthorOutletNavigator EntityNavigator { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<InclusionFilter> Inclusions { get; set; }
        
	}
}