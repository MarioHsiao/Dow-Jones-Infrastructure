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
    internal class GenericRestClientProcessor : RestClientProcessor
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
            request.AddBody(request);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }
    }
}