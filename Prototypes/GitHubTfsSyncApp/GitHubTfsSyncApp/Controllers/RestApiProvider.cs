using System;
using System.Collections.Generic;
using System.Linq;
using GitHubTfsSyncApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace GitHubTfsSyncApp.Controllers
{
	public interface IRestApiProvider
	{
		T Post<T>(string resource, object body = null, IDictionary<string, string> headers = null)
			where T : new();
	}

	public class RestApiProvider : IRestApiProvider
	{
		private readonly string _url;

		private readonly Lazy<JsonNetSerializer> _jsonSerializer = new Lazy<JsonNetSerializer>(() => new JsonNetSerializer());

		public RestApiProvider(string url)
		{
			_url = url;
		}

		public T Post<T>(string resource, object body = null, IDictionary<string, string> headers = null)
			where T : new()
		{
			var client = new RestClient(_url);
			var request = new RestRequest(resource, Method.POST)
				{
					RequestFormat = DataFormat.Json,				
				};

			if (body != null)
				request.AddBody(body);

			if (headers != null && headers.Any())
				foreach (var header in headers)
					request.AddHeader(header.Key, header.Value);

			var response = client.Execute<T>(request);

			return response.Data;
		}
	}
}