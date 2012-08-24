using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class SourceItem
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("entityData")]
		public string EntityData { get; set; }

		[JsonProperty("entityName")]
		public string EntityName { get; set; }

		[JsonProperty("isEntityLink")]
		public bool IsEntityLink { get; protected set; }

		public SourceItem(DowJones.Ajax.Article.RenderItem item)
		{
			if (item.ItemMarkUp == DowJones.Infrastructure.MarkUpType.EntityLink)
			{
				IsEntityLink = true;
				EntityData = item.ItemEntityData.ToJson().EscapeForHtml();
				EntityName = item.ItemEntityData.Name;
			}
			else if (item.ItemMarkUp == DowJones.Infrastructure.MarkUpType.Span)
			{
				Text = item.ItemText;
			}

			// do nothing if markup type is not one of the above
		}
	}
}
