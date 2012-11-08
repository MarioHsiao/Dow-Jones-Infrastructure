using System.Collections.Generic;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Web.Mvc.UI.Canvas;
using RestSharp;
using System.Linq;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class PageAssetProvider : IPageAssetProvider
	{
		// TODO: Refactor to read from config
		private const string ServiceUrl = "http://fdevweb3/data.factiva.com/PageService.svc";

		#region Implementation of IPageAssetProvider

		protected string MapPageNameToId(string name)
		{
			var client = new RestClient(ServiceUrl);
			var request = new RestRequest("/json", Method.GET);
			var response = client.Execute<NewsPagesListServiceResult>(request);

			return response.Data.Package.NewsPages.First(p => p.Title == name).ID;
		}

		public IEnumerable<IModule> GetModulesForPage(string pageName)
		{
			var pageId = MapPageNameToId(pageName);

			var client = new RestClient(ServiceUrl);
			var request = new RestRequest("id/json", Method.GET);
			request.AddParameter("pageid", pageId);

			//var response = client.Execute<SourcesNewsPageModuleServiceResult>(request);

			return null;

		}

		#endregion
	}
}