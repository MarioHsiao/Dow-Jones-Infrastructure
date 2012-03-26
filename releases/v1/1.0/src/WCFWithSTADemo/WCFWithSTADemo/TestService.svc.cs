using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace WCFWithSTADemo
{
    public class TestService : ITestService
    {
        public string GetApartmentTypeMTA(out int threadId, out int hashCode)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            hashCode = Thread.CurrentThread.GetHashCode();
            return Thread.CurrentThread.GetApartmentState().ToString();
        }


        [STAOperationBehavior]
        public string GetApartmentTypeSTA(out int threadId, out int hashCode)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            hashCode = Thread.CurrentThread.GetHashCode();
            return Thread.CurrentThread.GetApartmentState().ToString();
        }

    }
}
