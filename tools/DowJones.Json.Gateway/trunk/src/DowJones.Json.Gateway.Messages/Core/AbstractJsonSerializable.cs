using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    [KnownType(typeof(PlatformAdminData))]
    [KnownType(typeof(RoutingData))]
    [KnownType(typeof(TransactionCacheData))]
    [KnownType(typeof(UserCommerceData))]
    [KnownType(typeof(UserCredentialData))]
    public abstract class AbstractJsonSerializable:  IJsonSerializable
    {
        public string ToJson()
        {
            return JsonDataConverterSingleton.Instance.Serialize(this);
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }

        public ControlData GetClone()
        {
            return (ControlData)MemberwiseClone();
        }
    }
}