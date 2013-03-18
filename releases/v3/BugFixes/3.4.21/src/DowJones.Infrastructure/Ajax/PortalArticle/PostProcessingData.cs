using DowJones.Ajax.Article;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Ajax.PortalArticle
{
	public class PostProcessingData
	{
		[JsonProperty("type")]
		public PostProcessing Type { get; set; }

		[JsonProperty("elinkValue")]
		public string ElinkValue { get; set; }

		[JsonProperty("elinkText")]
		public string ElinkText { get; set; }

		public PostProcessingData()
		{
			Type = PostProcessing.UnSpecified;
		}

		public PostProcessingData(PostProcessData source)
		{
			Type = source.Type;
			ElinkText = source.ElinkText;
			ElinkValue = source.ElinkValue;
		}
	}

	
}
