using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DowJones.Extensions;

namespace DowJones.Ajax.PortalArticle
{
	public class PortalArticleResultSet
	{
		[JsonProperty("status")]
		public int Status { get; set; }

		[JsonProperty("statusMessage")]
		public string StatusMessage { get; set; }

		[JsonProperty("accessionNo")]
		public string AccessionNo { get; set; }

		[JsonProperty("reference")]
		public string Reference { get; set; }

		[JsonProperty("head")]
		public IEnumerable<LogoItem> Head { get; set; }

		[JsonProperty("headlines")]
		public IEnumerable<HeadlineItem> Headlines { get; set; }

		[JsonProperty("publicationDate")]
		public string PublicationDate { get; set; }

		[JsonProperty("publicationTime")]
		public string PublicationTime { get; set; }

		[JsonProperty("wordCount")]
		public int WordCount { get; set; }

		[JsonProperty("language")]
		public string Language { get; set; }

		[JsonProperty("html")]
		public IEnumerable<string> Html { get; set; }

		[JsonProperty("sources")]
		public IEnumerable<SourceItem> Sources { get; set; }

		[JsonProperty("sourceName")]
		public string SourceName { get; set; }

		[JsonProperty("authors")]
		public IEnumerable<AuthorItem> Authors { get; set; }

		[JsonProperty("credits")]
		public IEnumerable<CreditItem> Credits { get; set; }

		[JsonProperty("pages")]
		public IEnumerable<String> Pages { get; set; }

		[JsonProperty("corrections")]
		public IEnumerable<Paragraph> Corrections { get; set; }


		[JsonProperty("copyRights")]
		public IEnumerable<string> CopyRights { get; set; }

		[JsonProperty("leadParagraphs")]
		public IEnumerable<Paragraph> LeadParagraphs { get; set; }

		[JsonProperty("tailParagraphs")]
		public IEnumerable<Paragraph> TailParagraphs { get; set; }

		[JsonProperty("notes")]
		public IEnumerable<Paragraph> Notes { get; set; }
	}
}
