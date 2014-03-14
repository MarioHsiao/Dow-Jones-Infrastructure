using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests.Pam
{
    [TestClass]
    public class ListServiceUnitTestses : AbstractUnitTests
    {
        [TestMethod]
        public void GetListById()
        {
            var r = new RestRequest<GetListById>
                    {
                        Request = new GetListById
                                  {
                                      Id = 10,
                                  },
                        ControlData = GetControlData()
                    };

            var rm = new RestManager();
            var t = rm.Execute<GetListById, GetListByIdResponse>(r);
        }
    }
}