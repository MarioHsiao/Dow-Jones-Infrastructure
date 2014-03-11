using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extentions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Properties;
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

    internal class RestComposite
    {
        public RestSharpRestClient Client { get; set; }

        public RestSharpRestRequest Request { get; set; }
    }

    public class RestManager
    {
        public RestManager()
        {
            ServiceType = "HTTP";
        }

        protected internal string ServerUri { get; set; }


        protected internal string ServiceType { get; set; }

        public RestResponse<TResponse> Execute<TRequest, TResponse>(RestRequest<TRequest> restRequest)
            where TRequest : IJsonRestRequest, new()
            where TResponse : IJsonRestResponse, new()
        {
            if (!restRequest.ControlData.IsValid())
            {
                throw new Exception("Invalid Control Data");
            }

            var composite = GetHttpProxy(restRequest);
            var response = composite.Client.Execute(composite.Request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new RestResponse<TResponse>
                       {
                           ReturnCode = 0,
                           ReponseControlData = restRequest.ControlData,
                           Data = JsonDataConverterDecoratorSingleton.Instance.Deserialize<TResponse>(response)
                       };
                default:
                    Console.Write(response.Content);
                    throw JsonGatewayException.ParseExceptionMessage(response.Content);
            }
        }

        internal RestComposite GetCompsiteByTransactionType<TRequest>(RestRequest<TRequest> restRequest)
            where TRequest : IJsonRestRequest, new()
        {
            return Settings.Default.TransportType == "RTS" ? 
                GetRtsProxy(restRequest) : 
                GetHttpProxy(restRequest);
        }

        internal RestComposite GetHttpProxy<TRequest>(RestRequest<TRequest> restRequest)
            where TRequest : IJsonRestRequest, new()
        {
            var client = new RestSharpRestClient(Settings.Default.ServerUri);
            var request = new RestSharpRestRequest("", restRequest.Method.ConvertTo<RestSharpMethod>())
                          {
                              RequestFormat = DataFormat.Json,
                              JsonSerializer = JsonDataConverterDecoratorSingleton.Instance,
                          };
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);

            // add controldata to header
            request.AddHeader("ControlData", restRequest.ControlData.ToJson());

            if (restRequest.Method == Method.GET)
            {
                // add url parameters
                GetQueryString(restRequest.Request, request);
            }
            else
            {
                request.AddBody(request);
            }
            
            return new RestComposite
                   {
                       Client = client,
                       Request = request
                   };
        }

        internal RestComposite GetRtsProxy<TRequest>(RestRequest<TRequest> restRequest)
            where TRequest : IJsonRestRequest, new()
        {
            var client = new RestSharpRestClient(Settings.Default.ServerUri);
            var request = new RestSharpRestRequest("", restRequest.Method.ConvertTo<RestSharpMethod>())
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = JsonDataConverterDecoratorSingleton.Instance,
            };
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }

        protected internal string GetRoutingUri<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new()
        {
            return string.Concat(request.GetServicePath(), "/", GetTransactionName(request));
        }

        protected internal string GetTransactionName<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new()
        {
            return request.GetType().Name;
        }

        protected internal void GetQueryString<TRequest>(TRequest request, RestSharpRestRequest restRequest)
            where TRequest : IJsonRestRequest, new()
        {
            var properties = request.GetType().GetProperties();

            foreach (var property in properties)
            {
                var dataMember = property.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute;
                var name =  dataMember != null ? dataMember.Name : property.Name ;

                var val = property.GetValue(request, null);
                if (val != null)
                {
                    restRequest.AddParameter(name, JsonDataConverterDecoratorSingleton.Instance.Serialize(val));
                }
            }
        }
    }
}