namespace DowJones.Json.Gateway.Interfaces
{
    public interface IPlatformAdminData : IJsonSerializable
    {
        int? TransactionTimeout { get; set; }
    }
}