using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public class RestResponse<TResponse>
        where TResponse : new()
    {
        public IControlData ReponseControlData { get; set; }

        public Error Error { get; set; }

        public long ReturnCode { get; set; }

        public TResponse Data { get; set; }
    }
}