namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IPlatformAssetManagementService
    {
        bool HasNewspages { get; }
        bool IsNewspagesSubscribeOnly { get; }
        int NumberOfPersonalNewspages { get; }
    }
}