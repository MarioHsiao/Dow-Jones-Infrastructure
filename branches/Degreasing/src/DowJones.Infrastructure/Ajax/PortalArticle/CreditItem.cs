using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Ajax.PortalArticle
{
	public class CreditItem
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}
