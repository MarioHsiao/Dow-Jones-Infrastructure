using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class Paragraph
	{
		[JsonProperty("items")]
		public IEnumerable<ParagraphItem> Items { get; set; }

		[JsonProperty("tag")]
		public string Tag { get; set; }
	}
}
