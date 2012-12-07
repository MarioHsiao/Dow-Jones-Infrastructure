using System.Collections.Generic;
using System.Configuration;
//using DowJones.Extensions;
using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DowJones.Ajax.PortalHeadlineList;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using DowJones.Extensions;

namespace DowJones.Factiva.Currents.Website.Providers
{
    public class ContentProvider : IContentProvider
	{
		private readonly string _serviceUrl;

        public ContentProvider()
		{
            _serviceUrl = "{0}/{1}".FormatWith(
                                ConfigurationManager.AppSettings.Get("DataServiceUrl").Trim('/'),
                                ConfigurationManager.AppSettings.Get("ContentServiceEndpoint"));
            
		}

		#region Implementation of ISearchContext

		public PortalHeadlinesServiceResult GetHeadlines(string searchContext)
		{
			var client = new RestClient(_serviceUrl);
			var request = new RestRequest("headlines/json", Method.GET);
			request.AddParameter("searchContextRef", searchContext);

			var response = client.Execute(request).Content;
			return JsonConvert.DeserializeObject<PortalHeadlinesServiceResult>(response);
		}

	    public PortalHeadlineInfo GetHeadlineByAccessionNumber(string accessionNumber)
        {
            var client = new RestClient(_serviceUrl);
            var request = new RestRequest("headlines/an/json", Method.GET);
            request.AddParameter("an", accessionNumber);

            var response = client.Execute(request).Content;

            var portalHeadlineObject = (JObject) JsonConvert.DeserializeObject(response);

            return portalHeadlineObject.GetValue("portalHeadlineListDataResult")
					.ToObject<PortalHeadlineListDataResult>()
					.ResultSet
					.Headlines
					.FirstOrDefault();
            
        }

		#endregion
	}
}