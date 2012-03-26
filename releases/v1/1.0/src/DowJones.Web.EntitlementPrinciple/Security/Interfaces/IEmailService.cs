namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IEmailService
    {
        int MaximumFoldersPerEmailSetup { get; }
        bool IsNewsDigest { get; }
    }
}