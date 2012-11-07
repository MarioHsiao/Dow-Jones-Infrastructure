namespace DowJones.Factiva.Currents.Website.Models.PageService
{
    public interface IServicePartResult<TPackage>  where TPackage : IPackage
    {
        string Identifier
        {
            get;
            set;
        }

        long ReturnCode
        {
            get;
            set;
        }

        long ElapsedTime
        {
            get;
            set;
        }

        string StatusMessage
        {
            get;
            set;
        }

        string PackageType
        {
            get; 
            set;
        }

        TPackage Package
        {
            get;
            set;
        }
    }
}