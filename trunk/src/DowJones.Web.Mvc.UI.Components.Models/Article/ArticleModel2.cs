using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Ajax.Article;
using DowJones.Articles;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Token;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class ArticleModel2
	{
		[ClientProperty("showIndexing")]
		[JsonProperty("showIndexing")]
		public bool ShowIndexing
		{
			get
			{
				return ArticleDisplayOptions == DisplayOptions.Indexing
					   || ArticleDisplayOptions == DisplayOptions.Headline;
			}
		}

		[ClientProperty("status")]
		[JsonProperty("status")]
		public int Status { get; set; }

		[ClientProperty("statusMessage")]
		[JsonProperty("statusMessage")]
		public string StatusMessage
		{
			get
			{
				return Status != 0 ? _tokenRegistry.GetErrorMessage(Status) : string.Empty;
			}
		}

		[ClientProperty("accessionNo")]
		[JsonProperty("accessionNo")]
		public string AccessionNo { get; set; }

		[ClientProperty("articleDisplayOption")]
		[JsonProperty("articleDisplayOption")]
		public string ArticleDisplayOption { get { return ArticleDisplayOptions.ToString(); } }

		public DisplayOptions ArticleDisplayOptions { get; set; }

		[ClientProperty("reference")]
		[JsonProperty("reference")]
		public string Reference
		{
			get
			{
				return _articleRef.ToJson().EscapeForHtml();
			}
		}

		[ClientProperty("head")]
		[JsonProperty("head")]
		public IEnumerable<LogoItem> Head { get; set; }

		[ClientProperty("headlines")]
		[JsonProperty("headlines")]
		public IEnumerable<HeadlineItem> Headlines { get; set; }

		[ClientProperty("hasHeadline")]
		[JsonProperty("hasHeadline")]
		public bool HasHeadline { get { return Headlines.Any(); } }

		[ClientProperty("renderDefaultPostProcessing")]
		[JsonProperty("renderDefaultPostProcessing")]
		public bool RenderDefaultPostProcessing
		{
			get
			{
				return ShowSourceLinks && PostProcessing == DowJones.Infrastructure.PostProcessing.UnSpecified;
			}
		}


		/// <summary>
		/// Gets or sets PostProcessing.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public DowJones.Infrastructure.PostProcessing PostProcessing { get; set; }


		private IEnumerable<Components.PostProcessing.PostProcessingOptions> _postProcessingOptions;
		/// <summary>
		/// Gets or sets the post processing options.
		/// </summary>
		/// <value>
		/// The post processing options.
		/// </value>
		public IEnumerable<PostProcessingOptions> PostProcessingOptions {
			get { return _postProcessingOptions;  } 
			set 
			{ 
				_postProcessingOptions = value;
				PostProcessingOptionsWithToken = PostProcessingOptions.Select(p => new PostProcessingOptionItem(_tokenRegistry) { Option = p });
			}
		}

		[ClientProperty("postProcessingOptionsWithToken")]
		[JsonProperty("postProcessingOptionsWithToken")]
		public IEnumerable<PostProcessingOptionItem> PostProcessingOptionsWithToken { get; set; }

		[ClientProperty("showSourceLinks")]
		[JsonProperty("showSourceLinks")]
		public bool ShowSourceLinks { get; set; }

		[ClientProperty("showAuthorLinks")]
		[JsonProperty("showAuthorLinks")]
		public bool ShowAuthorLinks { get; set; }

		[ClientProperty("showSocialButtons")]
		[JsonProperty("showSocialButtons")]
		public bool ShowSocialButtons { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether ShowTranslator.
		/// </summary>
		[ClientProperty("showTranslator")]
		[JsonProperty("showTranslator")]
		public bool ShowTranslator { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether ShowPostProcessing.
		/// </summary>
		[ClientProperty("showPostProcessing")]
		[JsonProperty("showPostProcessing")]
		public bool ShowPostProcessing { get; set; }

		[ClientProperty("sources")]
		[JsonProperty("sources")]
		public IEnumerable<SourceItem> Sources { get; private set; }

		[ClientProperty("hasSources")]
		[JsonProperty("hasSources")]
		public bool HasSources { get { return Sources.Any(); } }

		[ClientProperty("sourceName")]
		[JsonProperty("sourceName")]
		public string SourceName { get; set; }

		[ClientProperty("authors")]
		[JsonProperty("authors")]
		public IEnumerable<AuthorItem> Authors { get; private set; }

		[ClientProperty("renderAuthors")]
		[JsonProperty("renderAuthors")]
		public bool RenderAuthors
		{
			get
			{
				return ShowAuthorLinks
						&& Authors.Any()
						&& PostProcessing == DowJones.Infrastructure.PostProcessing.UnSpecified;
			}
		}

		[ClientProperty("credits")]
		[JsonProperty("credits")]
		public IEnumerable<CreditItem> Credits { get; private set; }

		[ClientProperty("hasCredits")]
		[JsonProperty("hasCredits")]
		public bool HasCredits { get { return Credits.Any(); } }

		[ClientProperty("pages")]
		[JsonProperty("pages")]
		public IEnumerable<String> Pages { get; private set; }

		[ClientProperty("hasPages")]
		[JsonProperty("hasPages")]
		public bool HasPages { get { return Pages.Any(); } }

		[ClientProperty("corrections")]
		[JsonProperty("corrections")]
		public IEnumerable<Paragraph> Corrections { get; set; }

		[ClientProperty("hasCorrections")]
		[JsonProperty("hasCorrections")]
		public bool HasCorrections { get { return Corrections.Any(); } }

		[ClientProperty("copyRights")]
		[JsonProperty("copyRights")]
		public IEnumerable<string> CopyRights { get; private set; }

		[ClientProperty("hasCopyRights")]
		[JsonProperty("hasCopyRights")]
		public bool HasCopyRights { get { return CopyRights.Any(); } }

		[ClientProperty("leadParagraphs")]
		[JsonProperty("leadParagraphs")]
		public IEnumerable<Paragraph> LeadParagraphs { get; private set; }

		[ClientProperty("hasLeadParagraphs")]
		[JsonProperty("hasLeadParagraphs")]
		public bool HasLeadParagraphs { get { return LeadParagraphs.Any(); } }


		[ClientProperty("tailParagraphs")]
		[JsonProperty("tailParagraphs")]
		public IEnumerable<Paragraph> TailParagraphs { get; private set; }

		[ClientProperty("renderTailParagraphs")]
		[JsonProperty("renderTailParagraphs")]
		public bool RenderTailParagraphs { get { return TailParagraphs.Any() && ArticleDisplayOptions != DisplayOptions.Headline; } }

		[ClientProperty("notes")]
		[JsonProperty("notes")]
		public IEnumerable<Paragraph> Notes { get; private set; }

		[ClientProperty("hasNotes")]
		[JsonProperty("hasNotes")]
		public bool HasNotes { get { return Notes.Any(); } }

		[ClientProperty("publicationDate")]
		[JsonProperty("publicationDate")]
		public string PublicationDate { get; private set; }

		[ClientProperty("publicationTime")]
		[JsonProperty("publicationTime")]
		public string PublicationTime { get; private set; }

		[ClientProperty("wordCount")]
		[JsonProperty("wordCount")]
		public int WordCount { get; set; }

		[ClientProperty("renderWordCount")]
		[JsonProperty("renderWordCount")]
		public bool RenderWordCount { get { return WordCount > 0 && !Html.Any(); } }

		[ClientProperty("language")]
		[JsonProperty("language")]
		public string Language { get; private set; }


		public IEnumerable<string> Html { get; private set; }

		private ArticleRef _articleRef;


		ITokenRegistry _tokenRegistry;


		public ArticleModel2(ArticleResultset articleResultSet, ITokenRegistry tokenRegistry)
		{
			Guard.IsNotNull(articleResultSet, "articleResultSet");
			Guard.IsNotNull(tokenRegistry, "tokenRegistry");

			_tokenRegistry = tokenRegistry;

			Status = articleResultSet.Status;
			AccessionNo = articleResultSet.AccessionNo;
			PublicationDate = articleResultSet.PublicationDate;
			PublicationTime = articleResultSet.PublicationTime;
			WordCount = articleResultSet.WordCount;
			Html = (articleResultSet.Html ?? Enumerable.Empty<RenderItem>()).Where(c => c.ItemMarkUp == MarkUpType.Html).Select(c => c.ItemText);
			Pages = articleResultSet.Pages ?? Enumerable.Empty<string>();
			CopyRights = (articleResultSet.Copyright ?? Enumerable.Empty<RenderItem>()).Where(c => c.ItemMarkUp == MarkUpType.Plain).Select(c => c.ItemText);
			Language = articleResultSet.Language;

			_articleRef = new ArticleRef
			{
				AccessionNo = articleResultSet.AccessionNo,
				ContentCategory = articleResultSet.ContentCategory,
				ContentCategoryDescriptor = articleResultSet.ContentCategoryDescriptor,
				ContentSubCategory = articleResultSet.ContentSubCategory,
				ContentSubCategoryDescriptor = articleResultSet.ContentSubCategoryDescriptor,
				OriginalContentCategory = articleResultSet.OriginalContentCategory,
				ExternalUri = articleResultSet.ExternalUri,
				MimeType = articleResultSet.MimeType,
				@ref = articleResultSet.Ref,
				SubType = articleResultSet.SubType
			};

			// create lighter payloads

			var head = articleResultSet.Head ?? Enumerable.Empty<RenderItem>();
			Head = head.Select(h => new LogoItem
			{
				Text = h.ItemText,
				Value = h.ItemValue,
				ImageSize = CalculatePictureSize(h.ItemMarkUp, articleResultSet.PictureSize)
			});

			var headlines = articleResultSet.Headline ?? Enumerable.Empty<RenderItem>();
			Headlines = headlines.Select(h => new HeadlineItem
			{
				Text = h.ItemText,
				IsHyperLink = h.ItemMarkUp == MarkUpType.Anchor
			});

			var sources = articleResultSet.Source ?? Enumerable.Empty<RenderItem>();
			Sources = sources.Select(s => new SourceItem(s));

			var authors = articleResultSet.Authors ?? Enumerable.Empty<RenderItem>();
			Authors = authors.Select(a => new AuthorItem
			{
				EntityData = a.ItemEntityData.ToJson().EscapeForHtml(),
				EntityName = a.ItemEntityData.Name
			});

			var credits = articleResultSet.Credit ?? Enumerable.Empty<RenderItem>();
			Credits = credits.Select(c => new CreditItem
			{
				Text = c.ItemText
			});

			

			Corrections = GetParagraphs(articleResultSet.Correction);
			LeadParagraphs = GetParagraphs(articleResultSet.LeadParagraph);
			TailParagraphs = GetParagraphs(articleResultSet.TailParagraphs);
			Notes = GetParagraphs(articleResultSet.Notes);

		}


		private IEnumerable<Paragraph> GetParagraphs(IEnumerable<Dictionary<string, List<RenderItem>>> sources)
		{
			if (sources != null)
				return sources
					.SelectMany(s => s)
					.Select(kvc => new Paragraph
					{
						Tag = kvc.Key.ToLower() == "pre" ? "pre" : "p",
						Items = kvc.Value.Select(r => new ParagraphItem(r))
					}).ToArray();

			return Enumerable.Empty<Paragraph>();

		}

		/// <summary>
		/// Calculates the size of the picture based on rules and requested size.
		/// </summary>
		/// <param name="markUpType">Size of actual image as present in the item.</param>
		/// <param name="pictureSize">Requested Size of the picture.</param>
		/// <returns>The calculated Image Size</returns>
		private ImageSize CalculatePictureSize(MarkUpType markUpType, PictureSize pictureSize)
		{
			if (markUpType == MarkUpType.HeadLogo)
				return ImageSize.Logo;

			// next see if size matches with requested size (PictureSize)
			if (markUpType == MarkUpType.HeadImageLarge
				&& pictureSize == PictureSize.Large)
				return ImageSize.Large;

			if (markUpType == MarkUpType.HeadImageSmall
				&& pictureSize == PictureSize.Small)
				return ImageSize.Small;


			if (markUpType == MarkUpType.HeadImageXSmall
				&& pictureSize == PictureSize.XSmall)
				return ImageSize.XSmall;

			return ImageSize.Unknown;

		}

		
	}
}
