
namespace DowJones.Prod.X.Core.Interfaces
{
    public interface IMembershipService
    {
        bool IsSessionValid();
        long LastError { get; set; }
    }
}
