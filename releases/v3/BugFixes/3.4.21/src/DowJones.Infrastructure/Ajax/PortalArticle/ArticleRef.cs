using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Ajax.PortalArticle
{
	public class ArticleRef
	{
		[JsonProperty("guid")]
		public string AccessionNo { get; set; }

		[JsonProperty("contentCategory")]
		public ContentCategory ContentCategory { get; set; }

		[JsonProperty("contentSubCategory")]
		public ContentSubCategory ContentSubCategory { get; set; }

		[JsonProperty("contentCategoryDescriptor")]
		public string ContentCategoryDescriptor { get; set; }

		[JsonProperty("contentSubCategoryDescriptor")]
		public string ContentSubCategoryDescriptor { get; set; }

		[JsonProperty("originalContentCategory")]
		public string OriginalContentCategory { get; set; }

		[JsonProperty("externalUri")]
		public string ExternalUri { get; set; }

		[JsonProperty("mimetype")]
		public string MimeType { get; set; }

		[JsonProperty("ref")]
		public string @ref { get; set; }

		[JsonProperty("subType")]
		public string SubType { get; set; }
	}
}
