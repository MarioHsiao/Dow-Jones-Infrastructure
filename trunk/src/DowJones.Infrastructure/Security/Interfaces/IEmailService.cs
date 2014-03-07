namespace DowJones.Security.Interfaces
{
    public interface IEmailService
    {
        int MaximumFoldersPerEmailSetup { get; }
        bool IsNewsDigest { get; }
        bool IsFullTextODEAllowed { get; }
    }
}