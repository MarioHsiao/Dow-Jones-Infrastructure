namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IUERService
    {
        bool IsDNB { get; }
        bool IsDNBSnapshotOnly { get; }
        bool IsDNBReportsWithFactivaBill { get; }
        bool IsDNBReportsWithCreditcard { get; }
        bool IsSECFilings { get; }
        bool IsBvd { get; }
    }
}