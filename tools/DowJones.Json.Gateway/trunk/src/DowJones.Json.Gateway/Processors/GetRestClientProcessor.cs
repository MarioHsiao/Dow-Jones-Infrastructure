using System;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using RestSharp;
using Environment = DowJones.Json.Gateway.Interfaces.Environment;

namespace DowJones.Json.Gateway.Processors
{
    internal class GetRestClientProcessor : RestClientProcessor
    {
        public override RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
        {
            var composite = GetRestComposite(restRequest);
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
        
        public RestComposite GetRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            return restRequest.ControlData.RoutingData.Environment != Environment.Development ? GetNonDevelopmentRestComposite(restRequest) : GetDevelopmentRestComposite(restRequest);
        }

        public RestComposite GetNonDevelopmentRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            //application/json, , text/json, text/x-json, text/javascript, 
            var client = new RestClient(restRequest.ControlData.RoutingData.ServerUri);
            client.RemoveHandler("application/xml");
            client.RemoveHandler("text/xml");

            var request = new RestRequest("", Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = DataConverterDecoratorSingleton.Instance,
            };
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);
            AddCommon(restRequest, request);
            
            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }

        public RestComposite GetDevelopmentRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            //application/json, , text/json, text/x-json, text/javascript, 
            var client = new RestClient(restRequest.ControlData.RoutingData.ServerUri);
            client.RemoveHandler("application/xml");
            client.RemoveHandler("text/xml");
            
            var request = new RestRequest( GetRoutingUri(restRequest.Request), Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = DataConverterDecoratorSingleton.Instance,
            };
            
            AddCommon(restRequest, request);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }


        protected internal void AddCommon<T>(RestRequest<T> restRequest, IRestRequest request)
            where T : IJsonRestRequest, new()
        {
             // add ControlData to header
            request.AddHeader("ControlData", restRequest.ControlData.ToJson());
            GetQueryString(restRequest.Request, request);
        }

        protected internal void GetQueryString<TRequest>(TRequest request, IRestRequest restRequest)
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
                    restRequest.AddParameter(name, DataConverterDecoratorSingleton.Instance.Serialize(val));
                }
            }
        }
    }
}