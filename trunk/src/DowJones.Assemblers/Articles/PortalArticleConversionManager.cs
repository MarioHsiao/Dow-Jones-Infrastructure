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

			var portalArticleResultSet = new PortalArticleResultSet();

			portalArticleResultSet.Status = articleResultSet.Status;
			if (articleResultSet.Status != 0)
				portalArticleResultSet.StatusMessage = _resourceTextManager.GetErrorMessage(articleResultSet.Status.ToString());

			portalArticleResultSet.AccessionNo = articleResultSet.AccessionNo;
			portalArticleResultSet.PublicationDate = articleResultSet.PublicationDate;
			portalArticleResultSet.PublicationTime = articleResultSet.PublicationTime;
			portalArticleResultSet.WordCount = articleResultSet.WordCount;
			portalArticleResultSet.Html = (articleResultSet.Html ?? Enumerable.Empty<RenderItem>()).Where(c => c.ItemMarkUp == MarkUpType.Html).Select(c => c.ItemText);
			portalArticleResultSet.Pages = articleResultSet.Pages ?? Enumerable.Empty<string>();
			portalArticleResultSet.CopyRights = (articleResultSet.Copyright ?? Enumerable.Empty<RenderItem>()).Where(c => c.ItemMarkUp == MarkUpType.Plain).Select(c => c.ItemText);
			portalArticleResultSet.Language = articleResultSet.Language;

			portalArticleResultSet.Reference = new ArticleRef
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
			}.ToJson().EscapeForHtml();

			// create lighter payloads

			var head = articleResultSet.Head ?? Enumerable.Empty<RenderItem>();
			portalArticleResultSet.Head = head.Select(h => new LogoItem
			{
				Text = h.ItemText,
				Value = h.ItemValue,
				ImageSize = CalculatePictureSize(h.ItemMarkUp, articleResultSet.PictureSize),
			});

			portalArticleResultSet.LargeImageUrl = head.Where(h => h.ItemMarkUp == MarkUpType.HeadImageLarge).First().ItemText;

			var headlines = articleResultSet.Headline ?? Enumerable.Empty<RenderItem>();
			portalArticleResultSet.Headlines = headlines.Select(h => new HeadlineItem
			{
				Text = h.ItemText,
				IsHyperLink = h.ItemMarkUp == MarkUpType.Anchor
			});

			var sources = articleResultSet.Source ?? Enumerable.Empty<RenderItem>();
			portalArticleResultSet.Sources = sources.Select(s => new SourceItem(s));

			var authors = articleResultSet.Authors ?? Enumerable.Empty<RenderItem>();
			portalArticleResultSet.Authors = authors.Select(a => new AuthorItem
			{
				EntityData = a.ItemEntityData.ToJson().EscapeForHtml(),
				EntityName = a.ItemEntityData.Name
			});

			var credits = articleResultSet.Credit ?? Enumerable.Empty<RenderItem>();
			portalArticleResultSet.Credits = credits.Select(c => new CreditItem
			{
				Text = c.ItemText
			});



			portalArticleResultSet.Corrections = GetParagraphs(articleResultSet.Correction);
			portalArticleResultSet.LeadParagraphs = GetParagraphs(articleResultSet.LeadParagraph);
			portalArticleResultSet.TailParagraphs = GetParagraphs(articleResultSet.TailParagraphs);
			portalArticleResultSet.Notes = GetParagraphs(articleResultSet.Notes);

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
