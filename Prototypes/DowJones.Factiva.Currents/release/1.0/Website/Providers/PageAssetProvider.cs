using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using DowJones.Extensions;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;
using RestSharp;
using System.Linq;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class PageAssetProvider : IPageAssetProvider
	{
		private readonly IPageServiceResponseParser _pageServiceResponseParser;

		private readonly string _serviceUrl;

		public PageAssetProvider(IPageServiceResponseParser pageServiceResponseParser)
		{
			_pageServiceResponseParser = pageServiceResponseParser;
			_serviceUrl = "{0}/{1}".FormatWith(
							ConfigurationManager.AppSettings.Get("DataServiceUrl").Trim('/'), 
							ConfigurationManager.AppSettings.Get("PageServiceEndpoint"));
		}

		#region Implementation of IPageAssetProvider

		public virtual PageServiceResponse GetPageByName(string pageName)
		{
			var pageId = MapPageNameToId(pageName);

			return GetPageById(pageId);

		}

		public virtual PageServiceResponse GetPageById(string pageId)
		{
			var client = new RestClient(_serviceUrl);
			var request = new RestRequest("id/json", Method.GET);
			request.AddParameter("pageid", pageId);

			var rawResponse = client.Execute(request);

			if (rawResponse.StatusCode != HttpStatusCode.OK)
				throw new ApplicationException("Unable to retrieve page details", rawResponse.ErrorException);


			var pageServiceResponse = _pageServiceResponseParser.Parse(rawResponse.Content);

			return pageServiceResponse;
		}

		public virtual IEnumerable<PageListModel> GetPages()
		{
			var client = new RestClient(_serviceUrl);
			var request = new RestRequest("/json", Method.GET);
			var response = client.Execute<NewsPagesListServiceResult>(request);

			if(response.StatusCode != HttpStatusCode.OK || response.Data == null)
				throw new ApplicationException("Unable to retrieve list of pages", response.ErrorException);

			return  response.Data.Package.NewsPages.Select(Mapper.Map<PageListModel>);
		}

		#endregion

		protected string MapPageNameToId(string name)
		{
			var pageList = GetPages();
			return pageList.First(p => p.Title.Equals(name, StringComparison.OrdinalIgnoreCase)).Id;
		}
	}
}