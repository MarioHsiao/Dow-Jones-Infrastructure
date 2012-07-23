namespace DowJones.Security.Interfaces
{
    public interface ISnapshotService
    {
        bool IsCompanySnapshotOn { get; }
        bool IsIndustrySnapshotOn { get; }
        bool IsExecutiveSnapshotOn { get; }
        bool IsGovernmetSnapshotOn { get; }
        bool IsCorporateFamilyOn { get; }
        bool IsPremiumSnapshotOn { get; }
    }
}