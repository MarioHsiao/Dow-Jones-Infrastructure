namespace DowJones.Security.Interfaces
{
    public interface IScreeningService
    {
        bool IsCompanyScreeningOn { get; }
        bool IsRadarOn { get; }
        bool IsExtendedScreeningOn { get; }
    }
}