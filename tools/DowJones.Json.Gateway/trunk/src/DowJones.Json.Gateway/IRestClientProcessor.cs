using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extentions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Properties;
using RestSharp;

namespace DowJones.Json.Gateway
{


    internal interface IRestClientProcessor
    {
        RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new();

        RestResponse<TResponse> GenerateErrorResponse<TResponse>(long code, string message)
            where TResponse : IJsonRestResponse, new();

        string GetRoutingUri<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new();
    }

    public abstract class RestClientProcessor : IRestClientProcessor
    {
        public abstract RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest) 
            where TReq : IJsonRestRequest, new() 
            where TRes : IJsonRestResponse, new();
        
        public RestResponse<TResponse> GenerateErrorResponse<TResponse>(long code, string message)
            where TResponse : IJsonRestResponse, new()
        {
            return new RestResponse<TResponse>
            {
                ReturnCode = code,
                Error = new Error()
                {
                    Code = code,
                    Message = message,
                }
            };
        }


        public string GetRoutingUri<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new()
        {
            return string.Concat(request.GetServicePath(), "/", GetTransactionName(request));
        }
        
        protected internal string GetTransactionName<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new()
        {
            return request.GetType().Name;
        }
    } 
}