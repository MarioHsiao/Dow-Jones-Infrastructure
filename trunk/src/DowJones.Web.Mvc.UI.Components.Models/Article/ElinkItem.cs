using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class ElinkItem
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("markupType")]
		public MarkupType MarkupType { get; set; }
	}
}
