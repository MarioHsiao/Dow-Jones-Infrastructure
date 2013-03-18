using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.Search.UI.Components.Results
{
	public class AuthorOutletNavigator
	{
		public int PageSize { get; set; }
		public int FirstIndex { get; set; }
		public string SortBy { get; set; }
		public string SortOrder { get; set; }
		public string SelectedEntityIds { get; set; }
	}
}
