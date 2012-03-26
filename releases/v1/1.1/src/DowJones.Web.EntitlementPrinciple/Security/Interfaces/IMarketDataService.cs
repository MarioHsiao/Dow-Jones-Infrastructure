namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IMarketDataService
    {
        bool IsCurrentQuote { get; }
        bool IsQuickQuote { get; }
        bool IsFullQuote { get; }
        bool IsHistoricalQuote { get; }
        bool IsTreadlineQuote { get; }
        bool IsReuterInvestorQuote { get; }
        bool IsBackgroundDataFromReuters { get; }
    }
}