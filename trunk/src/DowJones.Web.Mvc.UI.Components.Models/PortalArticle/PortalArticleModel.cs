using System.Collections.Generic;
using System.Linq;
using DowJones.Ajax.PortalArticle;
using DowJones.Token;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.UI.Components.PortalArticle
{
	public class PortalArticleModel : ViewComponentModel
	{
		[ClientProperty("showIndexing")]
		public bool ShowIndexing
		{
			get
			{
				return ArticleDisplayOption == DisplayOptions.Indexing
					   || ArticleDisplayOption == DisplayOptions.Headline;
			}
		}

		[ClientProperty("articleDisplayOption")]
		[JsonConverter(typeof(StringEnumConverter))]
		public DisplayOptions ArticleDisplayOption { get; set; }

		[ClientProperty("postProcessingOptionsWithToken")]
		public IEnumerable<PostProcessingOptionItem> PostProcessingOptionsWithToken { get; set; }

		[ClientProperty("showSourceLinks")]
		public bool ShowSourceLinks { get; set; }

		[ClientProperty("showAuthorLinks")]
		public bool ShowAuthorLinks { get; set; }

		[ClientProperty("showSocialButtons")]
		public bool ShowSocialButtons { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether ShowTranslator.
		/// </summary>
		[ClientProperty("showTranslator")]
		public bool ShowTranslator { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether ShowPostProcessing.
		/// </summary>
		[ClientProperty("showPostProcessing")]
		public bool ShowPostProcessing { get; set; }
		
		/// <summary>
		/// Gets or sets PostProcessing.
		/// </summary>
		[ClientProperty("postProcessing")]
		[JsonConverter(typeof(StringEnumConverter))]
		public DowJones.Infrastructure.PostProcessing PostProcessing { get; set; }

		[ClientData]
		public PortalArticleResultSet Result { get; set; }


		// view helpers
		public bool HasHeadline { get { return !Result.Headlines.IsNullOrEmpty(); } }

		public bool RenderDefaultPostProcessing
		{
			get
			{
				return ShowSourceLinks && PostProcessing == DowJones.Infrastructure.PostProcessing.UnSpecified;
			}
		}

		private IEnumerable<Components.PostProcessing.PostProcessingOptions> _postProcessingOptions;

		/// <summary>
		/// Gets or sets the post processing options.
		/// </summary>
		/// <value>
		/// The post processing options.
		/// </value>
		public IEnumerable<PostProcessingOptions> PostProcessingOptions
		{
			get { return _postProcessingOptions; }
			set
			{
				_postProcessingOptions = value;
				PostProcessingOptionsWithToken = PostProcessingOptions.Select(p => new PostProcessingOptionItem(_tokenRegistry) { Option = p });
			}
		}

		public bool HasSources { get { return !Result.Sources.IsNullOrEmpty(); } }

		public bool RenderAuthors
		{
			get
			{
				return ShowAuthorLinks
						&& !Result.Authors.IsNullOrEmpty()
						&& PostProcessing == DowJones.Infrastructure.PostProcessing.UnSpecified;
			}
		}

		public bool HasCredits { get { return !Result.Credits.IsNullOrEmpty(); } }

		public bool HasPages { get { return !Result.Pages.IsNullOrEmpty(); } }

		public bool HasCorrections { get { return !Result.Corrections.IsNullOrEmpty(); } }

		public bool HasCopyRights { get { return !Result.CopyRights.IsNullOrEmpty(); } }

		public bool HasLeadParagraphs { get { return !Result.LeadParagraphs.IsNullOrEmpty(); } }

		public bool RenderTailParagraphs { get { return !Result.TailParagraphs.IsNullOrEmpty() && ArticleDisplayOption != DisplayOptions.Headline; } }

		public bool HasNotes { get { return !Result.Notes.IsNullOrEmpty(); } }

		public bool RenderWordCount { get { return Result.WordCount > 0 && !Result.Html.IsNullOrEmpty(); } }

		ITokenRegistry _tokenRegistry;
		public PortalArticleModel(ITokenRegistry tokenRegistry)
		{
			_tokenRegistry = tokenRegistry;
			Result = new PortalArticleResultSet();
		}

	}
}
