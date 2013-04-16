using DowJones.Session;

namespace DowJones.Managers.Abstract
{
    public interface IAggregationManager : IService
    {
        IControlData LastTransactionControlData { get; }
        string LastRawResponse { get; }
    }

    public interface IExternalService : IService
    {
        
    }

    public interface IService
    {
        
    }
}
