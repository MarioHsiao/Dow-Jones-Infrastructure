using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;
using DowJones.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MarkupType
	{
		Anchor,
		ArticleElink,
		ArticleElinkHighlight,
		ArticleHighlight,
		EntityLink,
		Html,
		Image,
		ImageFigure,
		Plain,
		PostProcessing,
		SpanAnchor,
		Unknown,
	}

	public class MarkupTypeViewModelMapper : TypeMapper<MarkUpType, MarkupType>
	{
		public override MarkupType Map(MarkUpType source)
		{
			// source has more types but we don't care about those in case of para items
			switch (source)
			{
				case MarkUpType.Image: return MarkupType.Image;
				case MarkUpType.EntityLink: return MarkupType.EntityLink;
				case MarkUpType.Anchor: return MarkupType.Anchor;
				case MarkUpType.Plain: return MarkupType.Plain;
				case MarkUpType.PostProcessing: return MarkupType.PostProcessing;
				case MarkUpType.SpanAnchor: return MarkupType.SpanAnchor;
				case MarkUpType.ArticleHighlight: return MarkupType.ArticleHighlight;
				case MarkUpType.ArticleElink: return MarkupType.ArticleElink;
				case MarkUpType.ArticleElinkHighlight: return MarkupType.ArticleElinkHighlight;
				case MarkUpType.Html: return MarkupType.Html;
				case MarkUpType.ImageFigure: return MarkupType.ImageFigure;
				default:
					return MarkupType.Unknown;
			}
		}
	}
}
