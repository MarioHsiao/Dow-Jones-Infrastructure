using DowJones.Session;

namespace DowJones.Managers.Abstract
{
    public interface IAggregationManager : IService
    {
        IControlData LastTransactionControlData { get; }
        string LastRawResponse { get; }
    }

    public interface IService
    {
        
    }
}
