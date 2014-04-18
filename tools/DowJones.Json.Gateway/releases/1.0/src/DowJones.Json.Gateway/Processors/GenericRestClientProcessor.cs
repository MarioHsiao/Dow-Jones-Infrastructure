using System;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Properties;
using log4net;
using log4net.Core;
using RestSharp;
using Environment = DowJones.Json.Gateway.Common.Environment;

namespace DowJones.Json.Gateway.Processors
{
    internal class GenericRestClientProcessor : RestClientProcessor
    {
        private readonly Method _method;
        private ILog _log = LogManager.GetLogger(typeof (GenericRestClientProcessor));

        public GenericRestClientProcessor(Method method)
        {
            _method = method;
        }

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

        internal override ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public RestComposite GetRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            return restRequest.ControlData.RoutingData.Environment != Environment.Direct ? GetNonDevelopmentRestComposite(restRequest) : GetDevelopmentRestComposite(restRequest);
        }

        public RestComposite GetNonDevelopmentRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {

            var client = new RestClient(restRequest.ControlData.RoutingData.ServerUri);
            client.ClearHandlers();

            var decorator = JsonSerializerFactory.Create(restRequest.ControlData.RoutingData.Serializer);
            var request = new RestRequest("", _method)
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

            var client = new RestClient(restRequest.ControlData.RoutingData.HttpEndPoint);
            client.ClearHandlers();
            
            var decorator = JsonSerializerFactory.Create(restRequest.ControlData.RoutingData.Serializer);
            var request = new RestRequest(GetRoutingUri(restRequest.Request), _method)
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
            var jsonRequest = restRequest.Request.ToJson(decorator);
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("ControlData(Json):\n{0}", jsonControlData);
                _log.DebugFormat("Request(Json):\n{0}", jsonRequest);
            }
            request.AddHeader("ControlData", jsonControlData);
            request.AddParameter("application/json", jsonRequest, ParameterType.RequestBody);
        }
    }
}