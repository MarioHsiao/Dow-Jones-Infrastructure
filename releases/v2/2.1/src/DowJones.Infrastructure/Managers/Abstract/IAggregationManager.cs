using DowJones.Session;

namespace DowJones.Managers.Abstract
{
    public interface IAggregationManager
    {
        IControlData LastTransactionControlData { get; }
        string LastRawResponse { get; }
    }
}
