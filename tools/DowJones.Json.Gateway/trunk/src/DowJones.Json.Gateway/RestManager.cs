using System;
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

    public class RestManager
    {
        public RestResponse<TResponse> Execute<TRequest, TResponse>(RestRequest<TRequest> restRequest) 
            where TRequest : new() 
            where TResponse: new()
        {
            if (!restRequest.ControlData.IsValid())
            {
                throw new Exception("Invalid Control Data");
            }

            var client = new RestSharpRestClient(restRequest.ServerUri);
            var request = new RestSharpRestRequest(restRequest.ResourcePath, restRequest.Method.ConvertTo<RestSharpMethod>())
                          {
                              RequestFormat = DataFormat.Json,
                              JsonSerializer = JsonDataConverterDecoratorSingleton.Instance,
                          };

            var response = client.Execute(request);

            return new RestResponse<TResponse>
                    {
                        ReturnCode = 0,
                        ReponseControlData = restRequest.ControlData,
                        Data = JsonDataConverterDecoratorSingleton.Instance.Deserialize<TResponse>(response)
                    };
        }
    }
}
