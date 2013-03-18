using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Ajax.PortalArticle
{
	public class Paragraph
	{
		[JsonProperty("items")]
		public IEnumerable<ParagraphItem> Items { get; set; }

		[JsonProperty("tag")]
		public string Tag { get; set; }
	}
}
