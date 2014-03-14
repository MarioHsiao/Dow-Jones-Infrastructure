using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Properties;
using RestSharp;

namespace DowJones.Json.Gateway.Processors
{
    internal class GetRestClientProcessor : RestClientProcessor
    {
        public override RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
        {
            var composite = GetHttpProxy(restRequest);
            try
            {
                var response = composite.Client.Execute(composite.Request);
                return ProcessStatus<TReq, TRes>(restRequest, response);
            }
            catch (Exception ex)
            {
                return GenerateGenericError<TRes>(ex);
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

            // add ControlData to header
            request.AddHeader("ControlData", restRequest.ControlData.ToJson());
            GetQueryString(restRequest.Request, request);

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