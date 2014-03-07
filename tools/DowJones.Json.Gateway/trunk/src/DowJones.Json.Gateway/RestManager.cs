using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Core;
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
        public IJsonConverter JsonConverter { get; set; }
        
        public RestManager(IJsonConverter converter = null)
        {
            JsonConverter = converter;
        }

        public void Execute(RestRequest restRequest)
        {
            var client = new RestSharpRestClient(restRequest.ServerUri);
            var request = new RestSharpRestRequest(restRequest.ResourcePath, restRequest.Method.ConvertTo<RestSharpMethod>())
                          {
                              RequestFormat = DataFormat.Json, 
                              JsonSerializer = JsonConverter ?? new JsonDotNetJsonConverter(),
                          };

            //client.ExecuteAsync();

        }
    }
}
