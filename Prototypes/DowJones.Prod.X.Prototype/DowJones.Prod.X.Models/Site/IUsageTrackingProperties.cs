namespace DowJones.Prod.X.Models.Site
{
    public interface IUsageTrackingProperties
    {
        bool LogUsageTracking { get; }
        string UsageTrackingAccount { get; }
        string SessionId { get; }
        string UserIdNs { get; }
        string AccountId { get; }
        string AccessCode { get; }
        string FullUrl { get; }
        string InterfaceLanguage { get; }
        string PageName { get; set; }
        string Events { get; }
        string PageChannel { get; }
        string Source { get; }
        string Section { get; }
    }
}