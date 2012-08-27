using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class PostProcessingData
	{
		[JsonProperty("type")]
		public DowJones.Infrastructure.PostProcessing Type { get; set; }

		[JsonProperty("elinkValue")]
		public string ElinkValue { get; set; }

		[JsonProperty("elinkText")]
		public string ElinkText { get; set; }

		public PostProcessingData()
		{

		}

		public PostProcessingData(PostProcessData source)
		{
			Type = source.Type;
			ElinkText = source.ElinkText;
			ElinkValue = source.ElinkValue;
		}
	}

	
}
