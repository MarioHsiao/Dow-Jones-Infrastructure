using System;
using System.Configuration;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Extensions;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using RestSharp;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public class SearchContextManager : ISearchContext
	{
		private readonly string _serviceUrl;

		public SearchContextManager()
		{
			_serviceUrl = "{0}/{1}".FormatWith(
								ConfigurationManager.AppSettings.Get("DataServiceUrl").Trim('/'),
								ConfigurationManager.AppSettings.Get("PageServiceEndpoint"));	
		}

		#region Implementation of ISearchContext

		public PortalHeadlineListDataResult GetHeadlines(string searchContext)
		{
			var client = new RestClient(_serviceUrl);
			var request = new RestRequest("headlines/json", Method.GET);
			request.AddParameter("searchContextRef", searchContext);

			var response = client.Execute(request).Content;

			var portalHeadlineListDataResult = JsonConvert
													.DeserializeObject<PortalHeadlinesServiceResult>(response)
													.Package
													.Result;
			return portalHeadlineListDataResult;
		}

		#endregion
	}
}