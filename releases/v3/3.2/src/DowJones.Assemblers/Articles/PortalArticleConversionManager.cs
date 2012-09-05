using System.Collections.Generic;
using System.Linq;
using DowJones.Ajax.Article;
using DowJones.Ajax.PortalArticle;
using DowJones.Articles;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Mapping;

namespace DowJones.Assemblers.Articles
{
	public class PortalArticleConversionManager : TypeMapper<ArticleResultset, PortalArticleResultSet>
	{
		IResourceTextManager _resourceTextManager;

		public PortalArticleConversionManager(IResourceTextManager resourceTextManager)
		{
			Guard.IsNotNull(resourceTextManager, "resourceTextManager");
			_resourceTextManager = resourceTextManager;
		}

		public override PortalArticleResultSet Map(ArticleResultset articleResultSet)
		{
			Guard.IsNotNull(articleResultSet, "articleResultSet");

			// create lighter payload by mapping only used fields

			var portalArticleResultSet = new PortalArticleResultSet
				{
					Status = articleResultSet.Status,
					AccessionNo = articleResultSet.AccessionNo,
					PublisherName = articleResultSet.PublisherName,
					PublicationDate = articleResultSet.PublicationDate,
					PublicationTime = articleResultSet.PublicationTime,
					WordCount = articleResultSet.WordCount,

					Html = EnsureCollection(articleResultSet.Html)
							.Where(c => c.ItemMarkUp == MarkUpType.Html).Select(c => c.ItemText),

					Pages = EnsureCollection(articleResultSet.Pages),

					CopyRights = EnsureCollection(articleResultSet.Copyright)
									.Where(c => c.ItemMarkUp == MarkUpType.Plain).Select(c => c.ItemText),

					Head = EnsureCollection(articleResultSet.Head)
									.Select(h => new LogoItem
										{
											Text = h.ItemText,
											Value = h.ItemValue,
											ImageSize = CalculatePictureSize(h.ItemMarkUp, articleResultSet.PictureSize),
										}),

					Headlines = EnsureCollection(articleResultSet.Headline)
									.Select(h => new HeadlineItem
										{
											Text = h.ItemText,
											IsHyperLink = h.ItemMarkUp == MarkUpType.Anchor
										}),

					Sources = EnsureCollection(articleResultSet.Source).Select(s => new SourceItem(s)),

					Credits = EnsureCollection(articleResultSet.Credit).Select(c => c.ItemText),

					Authors = EnsureCollection(articleResultSet.Authors)
									.Select(a => new AuthorItem
										{
											EntityData = a.ItemEntityData.ToJson().EscapeForHtml(),
											EntityName = a.ItemEntityData.Name
										}),

					IndexingCodeSets = EnsureCollection(articleResultSet.IndexingCodeSets)
											.Select(ics => new IndexingCodeSet
												{
													Code = ics.Key,
													Set = ics.Value
															.Select(c => string.Format("{0} : {1} | ", c.Key, c.Value))
															.Aggregate((cur, next) => cur + next)
															.TrimEnd("| ".ToCharArray())
												}),

					Language = articleResultSet.Language,
					LanguageCode = articleResultSet.LanguageCode,

					Ipcs = articleResultSet.Ipcs.ToArray(),
					Ipds = articleResultSet.Ipds.ToArray(),

					Corrections = GetParagraphs(articleResultSet.Correction),
					LeadParagraphs = GetParagraphs(articleResultSet.LeadParagraph),
					TailParagraphs = GetParagraphs(articleResultSet.TailParagraphs),
					Notes = GetParagraphs(articleResultSet.Notes),

					Contact = GetParagraph(articleResultSet.Contact),
					ArtWork = GetParagraph(articleResultSet.ArtWorks),

					Reference = new ArticleRef
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
						}.ToJson().EscapeForHtml()
				};


			

			var largeImageItem = portalArticleResultSet.Head.FirstOrDefault(h => h.ImageSize == ImageSize.Large);
			if (largeImageItem != null)
				portalArticleResultSet.LargeImageUrl = largeImageItem.Text;


			if (articleResultSet.Status != 0)
				portalArticleResultSet.StatusMessage = _resourceTextManager.GetErrorMessage(articleResultSet.Status.ToString());

			return portalArticleResultSet;
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

		private Paragraph GetParagraph(IEnumerable<RenderItem> sources, string tag = "p")
		{
			if (sources != null)
				return new Paragraph
				{
					Tag = tag,
					Items = sources.Select(r => new ParagraphItem(r))
				};

			return null;
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

		private IEnumerable<T> EnsureCollection<T>(IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}
	}
}
