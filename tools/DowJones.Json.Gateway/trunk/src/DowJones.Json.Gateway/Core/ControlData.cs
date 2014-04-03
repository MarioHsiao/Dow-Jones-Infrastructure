using System.Runtime.Serialization;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class ControlData : AbstractJsonSerializable, IControlData
    {
        public ControlData()
        {
            //AuthorizationData = new AuthorizationData();
            //UserCommerceData = new UserCommerceData();
            //UserCredentialData = new UserCredentialData();
            //PlatformAdminData = new PlatformAdminData();
            // TransactionCacheData = new TransactionCacheData();
            //RoutingData = new RoutingData();
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

        public AuthorizationData AuthorizationData { get; set; }

        public bool IsValid()
        {
            return true;
        }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}