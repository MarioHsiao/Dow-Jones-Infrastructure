using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Processors;
using log4net;
using RestSharp;
using RestSharpRestClient = RestSharp.RestClient;
using RestSharpRestRequest = RestSharp.RestRequest;

namespace DowJones.Json.Gateway
{
    public class RestComposite
    {
        public RestSharpRestClient Client { get; set; }

        public RestSharpRestRequest Request { get; set; }
    }

    public class RestManager
    {
        public RestManager()
        {
            ServiceType = "HTTP";
        }

        private readonly ILog _log = LogManager.GetLogger(typeof(RestManager));
        

        protected internal string ServerUri { get; set; }


        protected internal string ServiceType { get; set; }

        public RestResponse<TRes> Execute<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new()
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info("Start-Execute");
                _log.Info(restRequest.ControlData.ToJson());
            }

            if (!restRequest.ControlData.IsNotNullAndValid())
            {
                throw new JsonGatewayException(JsonGatewayException.ControlDataIsNotValid, "Invalid Control Data");
            }

            var method = GetMethod(restRequest.Request);
            return new RestClientProcessorFactory(method).Process<TReq, TRes>(restRequest);
        }
        
        private static Method GetMethod<T>(T request)
            where T : IJsonRestRequest, new()
        {
            var postRequest = request as IPostJsonRestRequest;
            if (postRequest != null)
            {
                return Method.POST;
            }

            var updateRequest = request as IPutJsonRestRequest;
            if (updateRequest != null)
            {
                return Method.PUT;
            }

            var deleteRequest = request as IDeleteJsonRestRequest;
            return deleteRequest != null ? Method.DELETE : Method.GET;
        }
    }
}