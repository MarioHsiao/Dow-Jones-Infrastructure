using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Extensions;
using DowJones.Token;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public class PostProcessingOptionItem
	{
		ITokenRegistry _tokenRegistry;

		[JsonProperty("option")]
		[JsonConverter(typeof(StringEnumConverter))]
		public PostProcessingOptions Option { get; set; }

		[JsonProperty("token")]
		public string Token
		{
			get
			{
				return _tokenRegistry.Get(Option.GetAssignedToken());
			}
		}

		public PostProcessingOptionItem(ITokenRegistry tokenRegistry)
		{
			_tokenRegistry = tokenRegistry;
			Option = PostProcessingOptions.Unspecified;
		}

	}
}
