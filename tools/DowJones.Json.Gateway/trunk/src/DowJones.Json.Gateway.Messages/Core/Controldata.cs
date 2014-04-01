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
        public IRoutingData RoutingData { get; set; }

        [DataMember]
        public IUserCommerceData UserCommerceData { get; set; }

        [DataMember]
        public IUserCredentialData UserCredentialData { get; set; }

        [DataMember]
        public IPlatformAdminData PlatformAdminData { get; set; }

        [DataMember]
        public ITransactionCacheData TransactionCacheData { get; set; }

        public IAuthorizationData AuthorizationData { get; set; }

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