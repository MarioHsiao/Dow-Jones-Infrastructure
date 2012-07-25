using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DowJones.Documentation.Website.Models
{
	public class RelatedTopicViewModel
	{
		public Name Name { get; private set; }

		public RelatedTopicViewModel(RelatedTopic relatedTopic)
		{
			Name = relatedTopic.Page;
			Page = relatedTopic.Page;
			Category = relatedTopic.Category;
		}

		public RouteValueDictionary GetRouteValues()
		{
			return new RouteValueDictionary{ { "category", Category }, { "page", Page } };
		}

		public string Category { get; private set; }
		public string Page { get; private set; }
	}
}