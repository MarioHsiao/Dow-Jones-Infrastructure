using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IJsonRestRequest
    {
        string ToJson(ISerialize decorator);
    }
}