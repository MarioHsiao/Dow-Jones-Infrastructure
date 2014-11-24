using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class ControlData : AbstractJsonSerializable, IControlData
    {
        public ControlData()
        {
        }

        [DataMember]
        public RoutingData RoutingData { get; set; } 

        [DataMember]
        public UserCommerceData UserCommerceData { get; set; }

        [DataMember]
        public UserCredentialData UserCredentialData { get; set; }

        [DataMember]
        public PlatformAdminData PlatformAdminData { get; set; }

        [DataMember]
        public TransactionCacheData TransactionCacheData { get; set; }

        [DataMember]
        public AuthorizationData AuthorizationData { get; set; }

        public bool IsValid()
        {
            return true;
        }

        public string ToJson(ISerialize decorator)
        {
            try
            {
                return decorator.Serialize(this);
            }
            catch
            {
                throw new JsonGatewayException(JsonGatewayException.ControlDataSerializationError, "Unable to serialize the 'ControlData' object."); 
            }
        }
    }
}