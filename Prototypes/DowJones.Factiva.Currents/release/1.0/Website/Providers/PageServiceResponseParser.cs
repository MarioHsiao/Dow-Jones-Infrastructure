using System.Collections.Generic;
using System.Reflection;
using DowJones.Factiva.Currents.ServiceModels;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class PageServiceResponseParser : IPageServiceResponseParser
	{
		private readonly IEnumerable<Assembly> _searchAssemblies;

		public PageServiceResponseParser(IEnumerable<Assembly> assemblies)
		{
			_searchAssemblies = assemblies;
		}

		#region Implementation of IPageServiceResponseParser

		public PageServiceResponse Parse(string rawContent)
		{
			var jsonNetFriendlyContent = ConvertToJsonNetFriendlyContent(rawContent);
			var response = JsonConvert
							.DeserializeObject<PageServiceResponse>(jsonNetFriendlyContent, 
								new JsonSerializerSettings
								{
									TypeNameHandling = TypeNameHandling.Auto,
									Binder = new TypeNameSerializationBinder(_searchAssemblies),
								});

			return response;
		}

		private string ConvertToJsonNetFriendlyContent(string rawContent)
		{
			return rawContent.Replace("__type", "$type");
		}

		#endregion
	}
}