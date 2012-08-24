using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class AuthorItem
	{
		[JsonProperty("entityData")]
		public string EntityData { get; set; }

		[JsonProperty("entityName")]
		public string EntityName { get; set; }
	}
}
