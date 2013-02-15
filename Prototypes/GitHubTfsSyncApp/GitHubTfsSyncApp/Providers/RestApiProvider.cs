using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace GitHubTfsSyncApp.Providers
{
	public interface IRestApiProvider
	{
		T Post<T>(string resource, object body = null, IDictionary<string, string> headers = null)
			where T : new();

		T Get<T>(string resource, IDictionary<string, string> headers = null) where T : new();
		T Get<T>(Uri uri, IDictionary<string, string> headers = null) where T : new();
	}

	public class RestApiProvider : IRestApiProvider
	{
		private readonly string _url;

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

		public T Get<T>(string resource, IDictionary<string, string> headers = null) where T : new()
		{
			return Get<T>(new Uri(_url + resource));
		}

		T IRestApiProvider.Get<T>(Uri uri, IDictionary<string, string> headers)
		{
			return Get<T>(uri, headers);
		}

		public static T Get<T>(Uri uri, IDictionary<string, string> headers = null) where T : new()
		{
			var client = new RestClient(uri.GetBaseUrl());

			var request = new RestRequest(uri, Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			if (headers != null && headers.Any())
				foreach (var header in headers)
					request.AddHeader(header.Key, header.Value);

			var response = client.Execute<T>(request);

			return response.Data;

		}
	}
}