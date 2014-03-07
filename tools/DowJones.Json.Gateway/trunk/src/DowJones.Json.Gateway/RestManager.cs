using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Extentions;
using RestSharp;
using RestSharpRestClient = RestSharp.RestClient;
using RestSharpRestRequest = RestSharp.RestRequest;
using RestSharpMethod = RestSharp.Method;

namespace DowJones.Json.Gateway
{
    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class RestRequest
    {
        public string ServerUri { get; set; } 

        public string ResourcePath { get; set; }

        public Method Method { get; set; }
    }

    public class RestManager
    {
        public T Execute<T>(RestRequest restRequest) where T : new()
        {
            var client = new RestSharpRestClient(restRequest.ServerUri);
            var request = new RestSharpRestRequest(restRequest.ResourcePath, restRequest.Method.ConvertTo<RestSharpMethod>())
                          {
                              RequestFormat = DataFormat.Json,
                              JsonSerializer = JsonDataConverterDecoratorSingleton.Instance,
                          };

            var response = client.Execute<T>(request);
            return response.Data;

            //client.ExecuteAsync();

        }
    }
}
