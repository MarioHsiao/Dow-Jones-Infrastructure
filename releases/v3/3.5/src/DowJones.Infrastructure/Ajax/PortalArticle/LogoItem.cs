using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Ajax.PortalArticle
{
	public class LogoItem
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("imageSize")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ImageSize ImageSize { get; set; }

		[JsonProperty("hasLargeImage")]
		public bool HasLargeImage
		{
			get
			{
				return ImageSize == ImageSize.Small || ImageSize == ImageSize.XSmall;
			}
		}
		
	}
}
