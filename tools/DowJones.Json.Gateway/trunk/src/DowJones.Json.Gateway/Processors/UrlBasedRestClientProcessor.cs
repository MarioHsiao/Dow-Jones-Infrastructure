using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using log4net;
using RestSharp;
using RestSharp.Contrib;
using Environment = DowJones.Json.Gateway.Common.Environment;

namespace DowJones.Json.Gateway.Processors
{
    internal abstract class UrlBasedRestClientProcessor : RestClientProcessor
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(UrlBasedRestClientProcessor));

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
            return restRequest.ControlData.RoutingData.Environment == Environment.Direct ? GetDevelopmentRestComposite(restRequest) : GetNonDevelopmentRestComposite(restRequest);
        }

        public RestComposite GetNonDevelopmentRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            //application/json, , text/json, text/x-json, text/javascript, 
            var client = new RestClient(restRequest.ControlData.RoutingData.ServerUri)
                {
                    FollowRedirects = false
                };
            client.ClearHandlers();
            
            var decorator = JsonSerializerFactory.Create(restRequest.ControlData.RoutingData.Serializer);
            var request = new RestRequest("", Method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = decorator,
            };

            //var uri = GetUri(GetRoutingUri(restRequest.Request), GetParams(restRequest.Request, decorator));
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);
            AddCommonHeaderParams(restRequest, request, decorator);
            UpateQueryStringParams(restRequest.Request, request, decorator);
            
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
            var client = new RestClient(restRequest.ControlData.RoutingData.ServiceUrl)
                {
                    FollowRedirects = false
                };
            client.ClearHandlers();

            var decorator = JsonSerializerFactory.Create(restRequest.ControlData.RoutingData.Serializer);
            var request = new RestRequest(GetRoutingUri(restRequest.Request), Method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = decorator,
            };
            
            AddCommonHeaderParams(restRequest, request, decorator);
            UpateQueryStringParams(restRequest.Request, request, decorator);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }

        protected internal void AddCommonHeaderParams<T>(RestRequest<T> restRequest, IRestRequest request, DataConverterDecorator decorator)
            where T : IJsonRestRequest, new()
        {
             // add ControlData to header
            var jsonControlData = restRequest.ControlData.ToJson(ControlDataDataConverterSingleton.Instance);
            if (_log.IsInfoEnabled)
            {
                _log.InfoFormat("ControlData(Json):\n{0}", jsonControlData);
            }

            request.AddHeader("ControlData", jsonControlData);
        }

        protected string GetUri(string baseUri, Dictionary<string, object> queryParams)
        {
            var pairs = queryParams.Select(s => string.Concat(s.Key, "=", HttpUtility.UrlEncode(s.Value.ToString()))).ToList();
            var qs = string.Join("&", pairs.ToArray());
            var partialUri = string.Concat(baseUri, "?", qs);

            return partialUri;
        }

        protected internal Dictionary<string, object> GetParams<TRequest>(TRequest request, DataConverterDecorator decorator)
            where TRequest : IJsonRestRequest, new()
        {
            var paramsDict = new Dictionary<string, object>();
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
                    paramsDict.Add(name, val);
                }
                else if (sourceType.IsEnum || sourceType == typeof(DateTime) || sourceType.IsClass)
                {
                    var v = decorator.Serialize(val);
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Propery-Name: {0}, Propery-Value: {1}", name, v);
                    }

                    paramsDict.Add(name, v);
                }
            }

            return paramsDict;
        }

        protected internal void UpateQueryStringParams<TRequest>(TRequest request, IRestRequest restRequest, DataConverterDecorator decorator)
            where TRequest : IJsonRestRequest, new()
        {
            var dict = GetParams(request, decorator);
            foreach (var item in dict)
            {
                restRequest.AddParameter(item.Key, item.Value, ParameterType.QueryString);
            }
        }
    }
}