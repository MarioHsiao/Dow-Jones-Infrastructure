using System;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using log4net;
using RestSharp;
using Environment = DowJones.Json.Gateway.Interfaces.Environment;

namespace DowJones.Json.Gateway.Processors
{
    internal abstract class UrlBasedRestClientProcessor : RestClientProcessor
    {
        private ILog _log = LogManager.GetLogger(typeof(UrlBasedRestClientProcessor));

        protected abstract Method Method { get; }
        
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
            return restRequest.ControlData.RoutingData.Environment != Environment.Direct ? GetNonDevelopmentRestComposite(restRequest) : GetDevelopmentRestComposite(restRequest);
        }

        public RestComposite GetNonDevelopmentRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            //application/json, , text/json, text/x-json, text/javascript, 
            var client = new RestClient(restRequest.ControlData.RoutingData.ServerUri);
            client.ClearHandlers();
            
            var decorator = JsonSerializerFactory.Create(restRequest.ControlData.RoutingData.Serializer);
            var request = new RestRequest("", Method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = decorator,
            };
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);
            AddCommon(restRequest, request, decorator);
            
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
            client.ClearHandlers();

            var decorator = JsonSerializerFactory.Create(restRequest.ControlData.RoutingData.Serializer);
            var request = new RestRequest(GetRoutingUri(restRequest.Request), Method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = decorator,
            };
            
            AddCommon(restRequest, request, decorator);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }

        protected internal void AddCommon<T>(RestRequest<T> restRequest, IRestRequest request, DataConverterDecorator decorator)
            where T : IJsonRestRequest, new()
        {
             // add ControlData to header
            var jsonControlData = restRequest.ControlData.ToJson(ControlDataDataConverterSingleton.Instance);
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("ControlData(Json):\n{0}", jsonControlData);
            }

            request.AddHeader("ControlData", jsonControlData);
            GetQueryString(restRequest.Request, request, decorator);
        }
        
        protected internal void GetQueryString<TRequest>(TRequest request, IRestRequest restRequest, DataConverterDecorator decorator)
            where TRequest : IJsonRestRequest, new()
        {
            var properties = request.GetType().GetProperties();

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("Found {0} properties for {1}", properties.Count(), request.GetType().FullName);
            }

            foreach (var property in properties)
            {
                var dataMember = property.GetCustomAttributes(typeof(DataMemberAttribute), true).FirstOrDefault() as DataMemberAttribute;
                var name = dataMember != null ? dataMember.Name : property.Name;

                var val = property.GetValue(request, null);

                if (val == null)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Propery-Name: {0} is null", name);
                    }
                    continue;
                }

                var sourceType = property.PropertyType;
                if (sourceType.IsPrimitive || sourceType == typeof(string) || sourceType == typeof(Single))
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Propery-Name: {0}, Propery-Value: {1}", name, val);
                    }
                    restRequest.AddParameter(name, val, ParameterType.QueryString);
                }
                else if (sourceType.IsEnum || sourceType == typeof(DateTime) || sourceType.IsClass)
                {
                    var v = decorator.Serialize(val);
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Propery-Name: {0}, Propery-Value: {1}", name, v);
                    }
                    
                    restRequest.AddParameter(name, v);
                }
            }
        }
    }
}