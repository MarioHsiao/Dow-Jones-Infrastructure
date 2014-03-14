using System;
using DowJones.Json.Gateway.Interfaces;
using RestSharp;

namespace DowJones.Json.Gateway.Processors
{
    internal class RestClientProcessorFactory : IRestClientProcessor
    {
        protected IRestClientProcessor Processor { get; private set; }

        protected internal RestClientProcessorFactory(Method method)
        {
            switch (method)
            {
                case Method.GET:
                    Processor = new GetRestClientProcessor();
                    break;
                default:
                    Processor = new GenericRestClientProcessor();
                    break;
            }
        }

        public RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest) 
            where TReq : IJsonRestRequest, 
            new() where TRes : IJsonRestResponse, new()
        {
            return Processor.Process<TReq, TRes>(restRequest);
        }
    }
}
