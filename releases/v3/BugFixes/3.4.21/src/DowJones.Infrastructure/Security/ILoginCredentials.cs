namespace DowJones.Security
{
    public interface ILoginCredentials
    {
        string LightWeightLoginToken { get; }
        string ProxyNamespace { get; }

        string ProxyUserId { get; }

        void SetProxyUser(string proxyUserId, string proxyNamespace, string proxyAccountId);
    }
}