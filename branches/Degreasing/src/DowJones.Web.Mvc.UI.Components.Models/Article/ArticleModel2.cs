﻿using System;
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
				return Status != 0 ? TokenRegistry.GetErrorMessage(Status) : string.Empty;
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
		public bool HasHeadline { get; protected set; }

		[ClientProperty("renderDefaultPostProcessing")]
		[JsonProperty("renderDefaultPostProcessing")]
		public bool RenderDefaultPostProcessing
		{
			get
			{
				return ShowSourceLinks && PostProcessing == DowJones.Infrastructure.PostProcessing.UnSpecified;
			}
		}


		public bool ShowSourceLinks { get; set; }

		public bool ShowAuthorLinks { get; set; }

		/// <summary>
		/// Gets or sets PostProcessing.
		/// </summary>
		public DowJones.Infrastructure.PostProcessing PostProcessing { get; set; }

		/// <summary>
		/// Gets or sets the post processing options.
		/// </summary>
		/// <value>
		/// The post processing options.
		/// </value>
		public IEnumerable<PostProcessingOptions> PostProcessingOptions { get; set; }

		[ClientProperty("sources")]
		[JsonProperty("sources")]
		public IEnumerable<SourceItem> Sources { get; set; }

		[ClientProperty("sourceName")]
		[JsonProperty("sourceName")]
		public string SourceName { get; set; }

		[ClientProperty("authors")]
		[JsonProperty("authors")]
		public IEnumerable<AuthorItem> Authors { get; set; }

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
		public IEnumerable<CreditItem> Credits { get; set; }

		[ClientProperty("renderCredits")]
		[JsonProperty("renderCredits")]
		public bool RenderCredits
		{
			get
			{
				return Credits.Any();
			}
		}

		[ClientProperty("correction")]
		[JsonProperty("correction")]
		public IEnumerable<Paragraph> Corrections { get; set; }

		[ClientProperty("hasCorrections")]
		[JsonProperty("hasCorrections")]
		public bool HasCorrections { get { return Corrections.Any(); } }

		[ClientProperty("leadParagraphs")]
		[JsonProperty("leadParagraphs")]
		public IEnumerable<Paragraph> LeadParagraphs { get; set; }

		[ClientProperty("hasLeadParagraphs")]
		[JsonProperty("hasLeadParagraphs")]
		public bool HasLeadParagraphs { get { return LeadParagraphs.Any(); } }


		[ClientProperty("tailParagraphs")]
		[JsonProperty("tailParagraphs")]
		public IEnumerable<Paragraph> TailParagraphs { get; set; }

		[ClientProperty("renderTailParagraphs")]
		[JsonProperty("renderTailParagraphs")]
		public bool RenderTailParagraphs { get { return TailParagraphs.Any() && ArticleDisplayOptions != DisplayOptions.Headline; } }

		[ClientProperty("notes")]
		[JsonProperty("notes")]
		public IEnumerable<Paragraph> Notes { get; set; }

		[ClientProperty("hasNotes")]
		[JsonProperty("hasNotes")]
		public bool HasNotes { get { return Notes.Any(); } }


		private ArticleRef _articleRef;

		[Inject("Until we figure out a better way")]
		protected ITokenRegistry TokenRegistry { get; set; }


		public ArticleModel2(ArticleResultset articleDataSet)
		{
			Status = articleDataSet.Status;
			AccessionNo = articleDataSet.AccessionNo;
			HasHeadline = !articleDataSet.Headline.IsNullOrEmpty();

			_articleRef = new ArticleRef
			{
				AccessionNo = articleDataSet.AccessionNo,
				ContentCategory = articleDataSet.ContentCategory,
				ContentCategoryDescriptor = articleDataSet.ContentCategoryDescriptor,
				ContentSubCategory = articleDataSet.ContentSubCategory,
				ContentSubCategoryDescriptor = articleDataSet.ContentSubCategoryDescriptor,
				OriginalContentCategory = articleDataSet.OriginalContentCategory,
				ExternalUri = articleDataSet.ExternalUri,
				MimeType = articleDataSet.MimeType,
				@ref = articleDataSet.Ref,
				SubType = articleDataSet.SubType
			};

			// create a lighter (payload of) head
			var head = articleDataSet.Head ?? Enumerable.Empty<RenderItem>();
			Head = head.Select(h => new LogoItem
			{
				Text = h.ItemText,
				Value = h.ItemValue,
				ImageSize = CalculatePictureSize(h.ItemMarkUp, articleDataSet.PictureSize)
			});

			var headlines = articleDataSet.Headline ?? Enumerable.Empty<RenderItem>();
			Headlines = headlines.Select(h => new HeadlineItem
			{
				Text = h.ItemText,
				IsHyperLink = h.ItemMarkUp == MarkUpType.Anchor
			});

			var sources = articleDataSet.Source ?? Enumerable.Empty<RenderItem>();
			Sources = sources.Select(s => new SourceItem(s));

			var authors = articleDataSet.Authors ?? Enumerable.Empty<RenderItem>();
			Authors = authors.Select(a => new AuthorItem
			{
				EntityData = a.ItemEntityData.ToJson().EscapeForHtml(),
				EntityName = a.ItemEntityData.Name
			});

			var credits = articleDataSet.Credit ?? Enumerable.Empty<RenderItem>();
			Credits = credits.Select(c => new CreditItem
			{
				Text = c.ItemText
			});


			Corrections = GetParagraphs(articleDataSet.Correction);
			LeadParagraphs = GetParagraphs(articleDataSet.LeadParagraph);
			TailParagraphs = GetParagraphs(articleDataSet.TailParagraphs);
			Notes = GetParagraphs(articleDataSet.Notes);
			
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