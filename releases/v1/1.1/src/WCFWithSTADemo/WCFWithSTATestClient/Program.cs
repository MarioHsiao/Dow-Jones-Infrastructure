using System;
using System.ServiceModel;
using WCFWithSTADemo;


namespace WCFWithSTATestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<ITestService>(new BasicHttpBinding());
            var service = factory.CreateChannel(new EndpointAddress("http://localhost/WCFWithSTADemoService/TestService.svc"));
            int threadId, hashCode = 0;
            var apartmentType = service.GetApartmentTypeMTA(out threadId, out hashCode);

            Console.WriteLine("Thread's apartment type is {0}; Thread ID {1}; HashCode {2}", apartmentType, threadId, hashCode);

            apartmentType = service.GetApartmentTypeSTA(out threadId, out hashCode);
            Console.WriteLine("Thread's apartment type is {0}; Thread ID {1}; HashCode {2}", apartmentType, threadId, hashCode);

            factory.Close();
            Console.ReadLine();
        }
    }
}
