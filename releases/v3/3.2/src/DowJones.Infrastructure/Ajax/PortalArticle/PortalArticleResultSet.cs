using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Ajax.PortalArticle
{
	public class PortalArticleResultSet
	{
		[JsonProperty("status")]
		public int Status { get; set; }

		[JsonProperty("statusMessage", NullValueHandling = NullValueHandling.Ignore)]
		public string StatusMessage { get; set; }

		[JsonProperty("accessionNo")]
		public string AccessionNo { get; set; }

		[JsonProperty("largeImageUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string LargeImageUrl { get; set; }

		[JsonProperty("reference", NullValueHandling = NullValueHandling.Ignore)]
		public string Reference { get; set; }

		[JsonProperty("head")]
		public IEnumerable<LogoItem> Head { get; set; }

		[JsonProperty("headlines")]
		public IEnumerable<HeadlineItem> Headlines { get; set; }

		[JsonProperty("publisherName", NullValueHandling = NullValueHandling.Ignore)]
		public string PublisherName { get; set; }

		[JsonProperty("publicationDate", NullValueHandling = NullValueHandling.Ignore)]
		public string PublicationDate { get; set; }

		[JsonProperty("publicationTime", NullValueHandling = NullValueHandling.Ignore)]
		public string PublicationTime { get; set; }

		[JsonProperty("wordCount")]
		public int WordCount { get; set; }

		[JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
		public string Language { get; set; }

		[JsonProperty("languageCode", NullValueHandling = NullValueHandling.Ignore)]
		public string LanguageCode { get; set; }

		[JsonProperty("html")]
		public IEnumerable<string> Html { get; set; }

		[JsonProperty("sources")]
		public IEnumerable<SourceItem> Sources { get; set; }

		[JsonProperty("sourceName", NullValueHandling = NullValueHandling.Ignore)]
		public string SourceName { get; set; }

		[JsonProperty("authors")]
		public IEnumerable<AuthorItem> Authors { get; set; }

		[JsonProperty("credits")]
		public IEnumerable<string> Credits { get; set; }

		[JsonProperty("contact")]
		public Paragraph Contact { get; set; }

		[JsonProperty("artWork")]
		public Paragraph ArtWork { get; set; }

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

		[JsonProperty("indexingCodeSets")]
		public IEnumerable<IndexingCodeSet> IndexingCodeSets { get; set; }

		[JsonProperty("ics")]
		public IEnumerable<string> Ipcs { get; set; }

		[JsonProperty("ipds")]
		public IEnumerable<string> Ipds { get; set; }

		// alien property. REST API populates it, not sure why or how to map it internally.
		[JsonProperty("externalUri")]
		public string ExternalUri { get; set; }
	}
}
