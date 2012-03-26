namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IFSInterfaceService
    {
        bool IsDowJonesConsultantUser { get; }
        bool IsWMDJAPCO { get; }
        bool IsWMDJAPAE { get; }
        bool IsWMDJAPFA { get; }
        bool IsDJIB { get; }
    }
}