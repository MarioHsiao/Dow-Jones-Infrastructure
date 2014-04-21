using System;
using System.Linq;
using System.Net;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Core;
using log4net;
using RestSharp;
using JsonSerializer = DowJones.Json.Gateway.Common.JsonSerializer;

namespace DowJones.Json.Gateway.Processors
{
    public abstract class RestClientProcessor : IRestClientProcessor
    {
        internal abstract ILog Log { get; set; }

        public abstract RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new();

        public RestResponse<TRes> GenerateErrorResponse<TRes>(string content, long code, string message)
            where TRes : IJsonRestResponse, new()
        {
            if (content.IsNullOrEmpty())
                return new RestResponse<TRes>
                       {
                           ReturnCode = code,
                           Error = new Error
                                   {
                                       Code = code,
                                       Message = message,
                                   }
                       };

            var error = JsonGatewayException.Parse(content);
            if (error.ReturnCode != JsonGatewayException.GenericError)
            {
                return new RestResponse<TRes>
                       {
                           ReturnCode = error.ReturnCode,
                           Error = new Error
                                   {
                                       Code = error.ReturnCode,
                                       Message =  error.Message
                                   }
                       };
            }

            return new RestResponse<TRes>
                   {
                       ReturnCode = code,
                       Error = new Error
                               {
                                   Code = code,
                                   Message = message,
                               }
                   };
        }

        protected internal void LogBodyContent(string content)
        {
            if (!Log.IsDebugEnabled)
            {
                return;
            }

            Log.DebugFormat("Raw Response Content:\n {0}", content);
        }

        protected internal string GetRoutingUri<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new()
        {
            return string.Concat(request.GetServicePath(), "/", GetTransactionName(request));
        }

        protected internal string GetTransactionName<TRequest>(TRequest request)
            where TRequest : IJsonRestRequest, new()
        {
            var contract = request.GetDataContract();
            if (contract != null && !string.IsNullOrEmpty(contract.Name))
            {
                return contract.Name;
            }
            return request.GetType().Name;
        }

        protected internal RestResponse<TRes> GenerateGenericError<TRes>(Exception ex)
            where TRes : IJsonRestResponse, new()
        {
            return new RestResponse<TRes>
                   {
                       ReturnCode = JsonGatewayException.GenericError,
                       Error = new Error
                               {
                                   Code = JsonGatewayException.GenericError,
                                   Message = ex.Message
                               }
                   };
        }

        protected internal RestResponse<TRes> ProcessStatus<TReq, TRes>(RestRequest<TReq> restRequest, IRestResponse response)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new()
        {
            var jsonDecorator = restRequest.ControlData.RoutingData.Serializer == JsonSerializer.DataContract ?
                DataContractConverterDecoratorSingleton.Instance :
                JsonDotNetConverterDecoratorSingleton.Instance;

            LogBodyContent(response.Content);
            switch (response.StatusCode)
            {
                case HttpStatusCode.NonAuthoritativeInformation:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK: // Request succeeded process body of code
                    var cd = restRequest.ControlData;
                    if (response.Headers.Count > 0)
                    {
                        var controlDataParam = response.Headers.SingleOrDefault(parameter => String.Equals(parameter.Name, "ControlData", StringComparison.InvariantCultureIgnoreCase));

                        try
                        {
                            if (controlDataParam != null)
                            {
                                cd = JsonDotNetDataConverterSingleton.Instance.Deserialize<ControlData>(controlDataParam.Value.ToString());
                            }
                        }
                        catch
                        {
                            return GenerateErrorResponse<TRes>(
                                null, 
                                JsonGatewayException.GenericError, 
                                "Equivalent to HTTP status 500. " +
                                "Unable to deserialize control-data object.");
                        }
                    }
                    return new RestResponse<TRes>
                           {
                               ReturnCode = 0,
                               ResponseControlData = cd,
                               Data = jsonDecorator.Deserialize<TRes>(response.Content)
                           };

                case HttpStatusCode.BadRequest:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.BadRequest,
                        "Equivalent to HTTP status 400. " +
                        "BadRequest indicates that the request could not be understood by the server. BadRequest is sent when no other error is applicable, or if the exact error is unknown or does not have its own error code.");

                case HttpStatusCode.GatewayTimeout:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.GatewayTimeout,
                        "Equivalent to HTTP status 504. " +
                        "GatewayTimeout indicates that an intermediate proxy server timed out while waiting for a response from another proxy or the origin server.");

                case HttpStatusCode.RequestUriTooLong:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.RequestUriTooLong,
                        "Equivalent to HTTP status 414. " +
                        "RequestUriTooLong indicates that the URI is too long.");

                case HttpStatusCode.MethodNotAllowed:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.MethodNotAllowed,
                        "Equivalent to HTTP status 405. " +
                        "MethodNotAllowed indicates that the request method (POST or GET) is not allowed on the requested resource.");

                case HttpStatusCode.NotAcceptable:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.NotAcceptable,
                        "Equivalent to HTTP status 406. " +
                        "NotAcceptable indicates that the client has indicated with Accept headers that it will not accept any of the available representations of the resource.");

                case HttpStatusCode.NotFound:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.NotFound,
                        "Equivalent to HTTP status 404. " +
                        "NotFound indicates that the requested resource does not exist on the server.");

                case HttpStatusCode.NotImplemented:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.NotImplemented,
                        "Equivalent to HTTP status 501. " +
                        "NotImplemented indicates that the server does not support the requested function.");

                case HttpStatusCode.RequestTimeout:
                    return GenerateErrorResponse<TRes>(
                        response.Content, 
                        JsonGatewayException.RequestTimeout,
                        "Equivalent to HTTP status 408. " +
                        "RequestTimeout indicates that the client did not send a request within the time the server was expecting the request.")
                    ;

                case HttpStatusCode.Unauthorized:
                    return GenerateErrorResponse<TRes>(
                        response.Content, JsonGatewayException.Unauthorized,
                        "Equivalent to HTTP status 401. " +
                        "Unauthorized indicates that the requested resource requires authentication. The WWW-Authenticate header contains the details of how to perform the authentication.");

                case HttpStatusCode.Forbidden:
                    return GenerateErrorResponse<TRes>(
                        response.Content, 
                        JsonGatewayException.Forbidden,
                        "Equivalent to HTTP status 403. " +
                        "Forbidden indicates that the server refuses to fulfill the request.");

                case HttpStatusCode.RequestEntityTooLarge:
                    return GenerateErrorResponse<TRes>(
                        response.Content, 
                        JsonGatewayException.RequestEntityTooLarge,
                        "Equivalent to HTTP status 413. " +
                        "RequestEntityTooLarge indicates that the request is too large for the server to process.");

                case HttpStatusCode.ServiceUnavailable:
                    return GenerateErrorResponse<TRes>(
                        response.Content,
                        JsonGatewayException.ServiceUnavailable,
                        "Equivalent to HTTP status 503. " +
                        "ServiceUnavailable indicates that the server is temporarily unavailable, usually due to high load or maintenance.");

                // ReSharper disable RedundantCaseLabel
                case HttpStatusCode.InternalServerError:
                // ReSharper restore RedundantCaseLabel
                default:
                    // Equivalent to HTTP status 500. 
                    // InternalServerError indicates that a generic error has occurred on the server.
                    return GenerateErrorResponse<TRes>(
                        response.Content, 
                        JsonGatewayException.GenericError, 
                        "Equivalent to HTTP status 500. " +
                        "InternalServerError indicates that a generic error has occurred on the server.");
            }
        }
    }
}