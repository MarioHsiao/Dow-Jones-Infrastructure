using System;
using DowJones.Json.Gateway.Extentions;
using DowJones.Json.Gateway.Interfaces;
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

        protected internal string ServerUri { get; set; }


        protected internal string ServiceType { get; set; }

        public RestResponse<TRes> Execute<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new()
        {
            if (!restRequest.ControlData.IsValid())
            {
                throw new Exception("Invalid Control Data");
            }

            var method = GetMethod(restRequest.Request);

            switch (method)
            {
                case Method.GET:
                    return new GetRestClientProcessor().Process<TReq, TRes>(restRequest);
            }

            return null;
        }
        

        private Method GetMethod<T>(T request)
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

       /* internal RestComposite GetRtsProxy<T>(RestRequest<T> restRequest)
            where T : IGetJsonRestRequest, IPostJsonRestRequest, IPutJsonRestRequest, IDeleteJsonRestRequest, new()

        {
            var client = new RestSharpRestClient(Settings.Default.ServerUri);
            var request = new RestSharpRestRequest("", restRequest.Method.ConvertTo<Method>())
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = JsonDataConverterDecoratorSingleton.Instance,
            };
            request.AddParameter("uri", GetRoutingUri(restRequest.Request), ParameterType.QueryString);

            return new RestComposite
            {
                Client = client,
                Request = request
            };
        }*/

    }
}