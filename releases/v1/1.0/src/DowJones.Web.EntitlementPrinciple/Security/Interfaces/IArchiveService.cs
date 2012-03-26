namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IArchiveService
    {
        string ArchiveAC2 { get; }
        string ArchiveAC3 { get; }
        string ArchiveAC9 { get; }
        bool IsRICForHistoricalOn { get; }
    }
}