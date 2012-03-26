using DowJones.Globalization;
using DowJones.Security;

namespace DowJones.Session
{
    public interface IUserSession : ILoginCredentials
    {
        string AccessPointCode { get; }
        
        string AccountId { get; }

        string ClientTypeCode { get; }

        InterfaceLanguage InterfaceLanguage { get; }

        bool IsDebug { get; }

        bool IsProxySession { get; }
        
        string ProductId { get; }
        
        string ProductPrefix { get; }

        string SessionId { get; }
        
        string UserId { get; }
        
        bool Validate();
    }
}