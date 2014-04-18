using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway
{
    public interface IRestRequest<T> where T : new()
    {
        IControlData ControlData { get; set; }
        T Request { get; set; }
    }
}