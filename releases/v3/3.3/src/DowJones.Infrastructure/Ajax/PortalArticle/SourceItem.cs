﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using DowJones.Extensions;
using DowJones.Ajax.Article;
using DowJones.Infrastructure;

namespace DowJones.Ajax.PortalArticle
{
	public class SourceItem
	{
		[JsonProperty("entityData")]
		public string EntityData { get; set; }

		[JsonProperty("entityName")]
		public string EntityName { get; set; }

		[JsonProperty("entityCode")]
		public string EntityCode { get; set; }

		[JsonProperty("isEntityLink")]
		public bool IsEntityLink { get; protected set; }

		public SourceItem(RenderItem item)
		{
			if (item.ItemMarkUp == MarkUpType.EntityLink)
			{
				IsEntityLink = true;
				EntityData = item.ItemEntityData.ToJson().EscapeForHtml();
				EntityName = item.ItemEntityData.Name;
				EntityCode = item.ItemEntityData.Code;
			}
			else if (item.ItemMarkUp == MarkUpType.Span)
			{
				EntityName = item.ItemText;
			}

			// do nothing if markup type is not one of the above
		}


		
	}
}