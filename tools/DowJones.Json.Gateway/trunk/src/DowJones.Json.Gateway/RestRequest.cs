using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public class RestRequest<T> : IRestRequest<T> where T : new()
    {
        public IControlData ControlData { get; set; }

        public T Request { get; set; }
    }
}