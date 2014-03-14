namespace DowJones.Json.Gateway.Interfaces
{
    public interface IControlData : IJsonSerializable
    {
        IRoutingData RoutingData { get; set; }

        IUserCommerceData UserCommerceData { get; set; }

        IUserCredentialData UserCredentialData { get; set; }

        IPlatformAdminData PlatformAdminData { get; set; }

        ITransactionCacheData TransactionCacheData { get; set; }

        bool IsValid();
    }
} 