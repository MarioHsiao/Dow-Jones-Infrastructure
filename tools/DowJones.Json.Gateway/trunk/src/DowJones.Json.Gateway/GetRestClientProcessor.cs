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

namespace DowJones.Json.Gateway
{
    internal class GetRestClientProcessor : RestClientProcessor
    {
        public override RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
        {
            var composite = GetHttpProxy(restRequest);
            try
            {
                var response = composite.Client.Execute(composite.Request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK: // Request succeeded process body of code
                        return new RestResponse<TRes>
                               {
                                   ReturnCode = 0,
                                   ReponseControlData = restRequest.ControlData,
                                   Data = JsonDataConverterDecoratorSingleton.Instance.Deserialize<TRes>(response)
                               };
                    case HttpStatusCode.BadRequest:
                        return GenerateErrorResponse<TRes>(JsonGatewayException.BadRequest, "Equivalent to HTTP status 400. BadRequest indicates that the request could not be understood by the server. BadRequest is sent when no other error is applicable, or if the exact error is unknown or does not have its own error code.");

                    case HttpStatusCode.InternalServerError:
                    default:
                        var error = JsonGatewayError.Parse(response.Content).Error;
                        return GenerateErrorResponse<TRes>(error.Code, error.Message);
                }
            }
            catch (Exception ex)
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
        }


        public RestComposite GetHttpProxy<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {

            var client = new RestClient(Settings.Default.ServerUri);
            var request = new RestRequest("", Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = JsonDataConverterDecoratorSingleton.Instance,
            };
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);

            // add controldata to header
            request.AddHeader("ControlData", restRequest.ControlData.ToJson());
            GetQueryString(restRequest.Request, request);
            request.AddBody(request);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }

        protected internal void GetQueryString<TRequest>(TRequest request, RestRequest restRequest)
            where TRequest : IJsonRestRequest, new()
        {
            var properties = request.GetType().GetProperties();

            foreach (var property in properties)
            {
                var dataMember = property.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute;
                var name = dataMember != null ? dataMember.Name : property.Name;

                var val = property.GetValue(request, null);

                if (val == null)
                {
                    continue;
                }

                var sourceType = property.PropertyType;
                if (sourceType.IsPrimitive || sourceType == typeof(string) || sourceType == typeof(Single))
                {
                    restRequest.AddParameter(name, val, ParameterType.QueryString);
                }
                else if (sourceType.IsEnum)
                {
                    restRequest.AddParameter(name, val.ToString(), ParameterType.QueryString);
                }
                else if (sourceType == typeof(DateTime))
                {
                    restRequest.AddParameter(name, ((DateTime)val).Ticks, ParameterType.QueryString);
                }
                else if (sourceType.IsClass)
                {
                    restRequest.AddParameter(name, JsonDataConverterDecoratorSingleton.Instance.Serialize(val));
                }
            }
        }
    }
}