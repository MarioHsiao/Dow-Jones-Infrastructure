using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public interface IRestResponse<T> where T : IJsonRestResponse ,new()
    {
        IControlData ResponseControlData { get; set; }
        Error Error { get; set; }
        long ReturnCode { get; set; }
        T Data { get; set; }
    }
}