using System.Security.Cryptography.X509Certificates;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IJsonSerializable
    {
        string ToJson();
    }
}