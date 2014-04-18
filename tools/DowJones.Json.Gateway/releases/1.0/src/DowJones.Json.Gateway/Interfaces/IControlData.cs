using DowJones.Json.Gateway.Messages.Core;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IControlData : IJsonSerializable, IJsonRestRequest
    {
        RoutingData RoutingData { get; set; }

        UserCommerceData UserCommerceData { get; set; }

        UserCredentialData UserCredentialData { get; set; }

        PlatformAdminData PlatformAdminData { get; set; }

        TransactionCacheData TransactionCacheData { get; set; }
        
        AuthorizationData AuthorizationData { get; set; }

        bool IsValid();
    }
} 