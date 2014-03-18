using System;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Properties;
using RestSharp;
using Environment = DowJones.Json.Gateway.Interfaces.Environment;

namespace DowJones.Json.Gateway.Processors
{
    internal class GenericRestClientProcessor : RestClientProcessor
    {
        private readonly Method _method;
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

        public RestComposite GetRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {
            return restRequest.ControlData.RoutingData.Environment != Environment.Development ? GetNonDevelopmentRestComposite(restRequest) : GetDevelopmentRestComposite(restRequest);
        }

        public RestComposite GetNonDevelopmentRestComposite<T>(RestRequest<T> restRequest)
            where T : IJsonRestRequest, new()
        {

            var client = new RestClient(Settings.Default.ServerUri);
            client.RemoveHandler("application/xml");
            client.RemoveHandler("text/xml");

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

            var client = new RestClient(restRequest.ControlData.RoutingData.ServerUri);
            client.RemoveHandler("application/xml");
            client.RemoveHandler("text/xml");

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
            request.AddHeader("ControlData", restRequest.ControlData.ToJson());
            request.AddParameter("application/json", restRequest.Request.ToJson(decorator), ParameterType.RequestBody);
        }
    }
}