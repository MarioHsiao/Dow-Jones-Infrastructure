using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IGetJsonRestRequest : IJsonRestRequest
    {
    }

    public interface IPostJsonRestRequest : IJsonRestRequest
    {
    }

    public interface IPutJsonRestRequest : IJsonRestRequest
    {
    }

    public interface IDeleteJsonRestRequest : IJsonRestRequest
    {
    }

    public interface IJsonRestRequest
    {
        string ToJson(ISerialize decorator);
    }

    public interface IJsonRestResponse
    {
    }
}