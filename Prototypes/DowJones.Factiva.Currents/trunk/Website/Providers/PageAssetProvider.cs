using System;
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
		private readonly IPageServiceResponseParser _pageServiceResponseParser;

		// TODO: Refactor to read from config
		private const string ServiceUrl = "http://fdevweb3/data.factiva.com/PageService.svc";

		public PageAssetProvider(IPageServiceResponseParser pageServiceResponseParser)
		{
			_pageServiceResponseParser = pageServiceResponseParser;
		}

		#region Implementation of IPageAssetProvider

		public PageServiceResponse GetPageByName(string pageName)
		{
			var pageId = MapPageNameToId(pageName);

			return GetPageById(pageId);

		}

		public PageServiceResponse GetPageById(string pageId)
		{
			var client = new RestClient(ServiceUrl);
			var request = new RestRequest("id/json", Method.GET);
			request.AddParameter("pageid", pageId);

			var rawResponse = client.Execute(request);

			var pageServiceResponse = _pageServiceResponseParser.Parse(rawResponse.Content);

			return pageServiceResponse;
		}

		#endregion

		protected string MapPageNameToId(string name)
		{
			var client = new RestClient(ServiceUrl);
			var request = new RestRequest("/json", Method.GET);
			var response = client.Execute<NewsPagesListServiceResult>(request);

			return response.Data.Package.NewsPages.First(p => p.Title.Equals(name, StringComparison.OrdinalIgnoreCase)).ID;
		}
	}
}