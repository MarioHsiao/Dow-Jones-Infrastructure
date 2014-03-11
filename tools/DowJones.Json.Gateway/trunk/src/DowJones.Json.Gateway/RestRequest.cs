using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public class RestRequest<TRequest> 
        where TRequest : new()
    {
        public IControlData ControlData { get; set; }

        public Method Method { get; set; }

        public TRequest Request { get; set; }
    }
}