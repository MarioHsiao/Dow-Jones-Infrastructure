using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Ajax.PortalArticle
{
	public class IndexingCodeSet
	{
		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("set")]
		public string Set { get; set; }
	}
}
