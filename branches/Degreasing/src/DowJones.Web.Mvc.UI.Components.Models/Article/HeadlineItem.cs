﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class HeadlineItem
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("isHyperLink")]
		public bool IsHyperLink { get; set; }
	}
}
