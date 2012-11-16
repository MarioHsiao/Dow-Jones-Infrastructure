using System.Configuration;
using DowJones.Extensions;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using RestSharp;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Website.Providers
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

		public Headlines GetHeadlines(string searchContext)
		{
			var client = new RestClient(_serviceUrl);
			var request = new RestRequest("headlines/json", Method.GET);
			request.AddParameter("searchContextRef", searchContext);

			var response = client.Execute(request).Content;

			var portalHeadlineListResult = JsonConvert.DeserializeObject<PortalHeadlinesServiceResult>(response);
			return new Headlines
				{
					ViewAllSearchContext = portalHeadlineListResult.Package.ViewAllSearchContextRef,
					CurrentsHeadline = new CurrentsHeadlineModel(new PortalHeadlineListModel(portalHeadlineListResult.Package.Result)),
				};
		}

		#endregion
	}
}