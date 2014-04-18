using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Processors
{
    internal interface IRestClientProcessor
    {
        RestResponse<TRes> Process<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new();
    }
}