using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IPlatformAdminData : IJsonSerializable
    {
        int? TransactionTimeout { get; set; }
    }
}