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
        string ToJson(ISerialize decorator);
    }

    [DataContract]
    public abstract class GetJsonRestRequest : JsonRestRequest, IGetJsonRestRequest
    {
    }

    [DataContract]
    public abstract class PostJsonRestRequest : JsonRestRequest, IPostJsonRestRequest
    {
    }

    [DataContract]
    public abstract class PutJsonRestRequest : JsonRestRequest, IPutJsonRestRequest
    {
    }

    [DataContract]
    public abstract class DeleteJsonRestRequest : JsonRestRequest, IDeleteJsonRestRequest
    {
    }

    [DataContract]
    public abstract class JsonRestResponse : IJsonRestResponse
    {
        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }


    [DataContract()]
    public abstract class JsonRestRequest : IJsonRestRequest
    {
        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}