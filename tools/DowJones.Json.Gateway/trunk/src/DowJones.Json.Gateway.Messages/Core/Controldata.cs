using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public class ControlData : AbstractJsonSerializable, IControlData
    {
        public IRoutingData RoutingData { get; set; }
        public IUserCommerceData UserCommerceData { get; set; }
        public IUserCredentialData UserCredentialData { get; set; }
        public IPlatformAdminData PlatformAdminData { get; set; }
        public ITransactionCacheData TransactionCacheData { get; set; }
    }
}