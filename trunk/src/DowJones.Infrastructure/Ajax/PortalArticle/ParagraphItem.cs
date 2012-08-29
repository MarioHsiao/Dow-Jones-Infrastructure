using System.Collections.Generic;
using DowJones.Ajax.Article;
using Newtonsoft.Json;
using DowJones.Extensions;
using System.Linq;

namespace DowJones.Ajax.PortalArticle
{
	public class ParagraphItem
	{
		[JsonProperty("markupType")]
		public MarkupType MarkupType { get; set; }

		string _text;
		[JsonProperty("text")]
		public string Text
		{
			get { return (_text ?? "").EscapeForHtml(); }
			set { _text = value; }
		}

		string _value;
		[JsonProperty("value")]
		public string Value
		{
			get { return (_value ?? "").EscapeForHtml(); }
			set { _value = value; }
		}

		[JsonProperty("enlargedImageUrl")]
		public string EnlargedImageUrl { get; set; }


		public ParagraphItem(IRenderItem r)
		{
			Title = r.Title;
			Text = r.ItemText;
			Value = r.ItemValue;
			EnlargedImageUrl = r.EnlargedImageUrl;
			Highlight = r.Highlight;
			Caption = r.Caption;
			Source = r.Source;
			Credit = r.Credit;
			MarkupType = Mapper.Map<MarkupType>(r.ItemMarkUp);

			if (r.ItemEntityData != null)
			{
				EntityName = r.ItemEntityData.Name;
				EntityCategory = r.ItemEntityData.Category;
				EntityData = r.ItemEntityData.ToJson();
			}

			ElinkItems = Enumerable.Empty<ElinkItem>();

			if (!r.ElinkItems.IsNullOrEmpty())
			{
				ElinkItems = r.ElinkItems.Select(e => new ElinkItem()
					{
						MarkupType = Mapper.Map<MarkupType>(e.ItemMarkUp),
						Text = e.ItemText,
						Value = e.ItemValue
					});
			}
		}

		string _title;
		[JsonProperty("title")]
		public string Title
		{
			get { return (_title ?? "").EscapeForHtml(); }
			set { _title = value; }
		}

		[JsonProperty("entityName")]
		public string EntityName { get; set; }

		[JsonProperty("entityCategory")]
		public string EntityCategory { get; set; }

		[JsonProperty("entityData")]
		public string EntityData { get; set; }

		[JsonProperty("elinkItems")]
		public IEnumerable<ElinkItem> ElinkItems { get; set; }

		[JsonProperty("highlight")]
		public bool Highlight { get; set; }

		[JsonProperty("caption")]
		public string Caption { get; set; }

		[JsonProperty("credit")]
		public string Credit { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("tag")]
		public string Tag { get; set; }

		[JsonProperty("postProcessingData")]
		public PostProcessingData PostProcessingData { get; set; }
	}
}
