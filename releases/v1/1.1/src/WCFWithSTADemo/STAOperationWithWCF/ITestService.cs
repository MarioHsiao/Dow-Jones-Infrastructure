using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFWithSTADemo
{
    [ServiceContract]
    public interface ITestService
    {

        [OperationContract]
        string GetApartmentTypeMTA(out int threadId, out int hashCode);

        [OperationContract]
        string GetApartmentTypeSTA(out int threadId, out int hashCode);

    }
}