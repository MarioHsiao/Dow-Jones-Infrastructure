using System;
using System.Net;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using RestSharp;

namespace DowJones.Json.Gateway.Processors
{
    public abstract class RestClientProcessor : IRestClientProcessor
    {
        public abstract RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new();

        public RestResponse<TRes> GenerateErrorResponse<TRes>(long code, string message)
            where TRes : IJsonRestResponse, new()
        {
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
            switch (response.StatusCode)
            {
                case HttpStatusCode.NonAuthoritativeInformation:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK: // Request succeeded process body of code
                    return new RestResponse<TRes>
                           {
                               ReturnCode = 0,
                               ResponseControlData = restRequest.ControlData,
                               Data = DataContractConverterDecoratorSingleton.Instance.Deserialize<TRes>(response)
                           };

                case HttpStatusCode.BadRequest:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.BadRequest, 
                        "Equivalent to HTTP status 400. " +
                        "BadRequest indicates that the request could not be understood by the server. BadRequest is sent when no other error is applicable, or if the exact error is unknown or does not have its own error code.");

                case HttpStatusCode.GatewayTimeout:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.GatewayTimeout, 
                        "Equivalent to HTTP status 504. " +
                        "GatewayTimeout indicates that an intermediate proxy server timed out while waiting for a response from another proxy or the origin server.");

                case HttpStatusCode.RequestUriTooLong:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.RequestUriTooLong, 
                        "Equivalent to HTTP status 414. " +
                        "RequestUriTooLong indicates that the URI is too long.");

                case HttpStatusCode.MethodNotAllowed:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.MethodNotAllowed, 
                        "Equivalent to HTTP status 405. " +
                        "MethodNotAllowed indicates that the request method (POST or GET) is not allowed on the requested resource.");

                case HttpStatusCode.NotAcceptable:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.NotAcceptable, 
                        "Equivalent to HTTP status 406. " +
                        "NotAcceptable indicates that the client has indicated with Accept headers that it will not accept any of the available representations of the resource.");

                case HttpStatusCode.NotFound:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.NotFound,
                        "Equivalent to HTTP status 404. " +
                        "NotFound indicates that the requested resource does not exist on the server.");

                case HttpStatusCode.NotImplemented:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.NotImplemented, 
                        ".");

                case HttpStatusCode.RequestTimeout:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.RequestTimeout,
                        "Equivalent to HTTP status 501. " +
                        "NotImplemented indicates that the server does not support the requested function.");

                case HttpStatusCode.Unauthorized:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.Unauthorized,
                        "Equivalent to HTTP status 401. " +
                        "Unauthorized indicates that the requested resource requires authentication. The WWW-Authenticate header contains the details of how to perform the authentication.");

                case HttpStatusCode.Forbidden:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.Forbidden,
                        "Equivalent to HTTP status 403. " +
                        "Forbidden indicates that the server refuses to fulfill the request.");

                case HttpStatusCode.RequestEntityTooLarge:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.RequestEntityTooLarge,
                        "Equivalent to HTTP status 413. " +
                        "RequestEntityTooLarge indicates that the request is too large for the server to process.");

                case HttpStatusCode.ServiceUnavailable:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.ServiceUnavailable,
                        "Equivalent to HTTP status 503. " +
                        "ServiceUnavailable indicates that the server is temporarily unavailable, usually due to high load or maintenance.");

                case HttpStatusCode.InternalServerError:
                    // Equivalent to HTTP status 500. 
                    // InternalServerError indicates that a generic error has occurred on the server.
                    var error = JsonGatewayError.Parse(response.Content).Error;
                    return GenerateErrorResponse<TRes>(error.Code, error.Message);

                default:
                    return GenerateErrorResponse<TRes>(JsonGatewayException.GenericError, "Generic Error.");
            }
        }
    }
}