using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public class RestResponse<TResponse>
        where TResponse : new()
    {
        public IControlData ReponseControlData { get; set; }

        public int ReturnCode { get; set; }

        public TResponse Data { get; set; }
    }
}