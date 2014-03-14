using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public class RestResponse<T> : IRestResponse<T> where T : new()
    {
        public IControlData ResponseControlData { get; set; }

        public Error Error { get; set; }

        public long ReturnCode { get; set; }

        public T Data { get; set; }
    }
}