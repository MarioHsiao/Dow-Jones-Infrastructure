using System.Collections.Generic;
using DowJones.Managers.Search;
using DowJones.Session;
using DowJones.Token;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Infrastructure.Web
{
    [TestClass]
    public class ProductSourceGroupConfigurationManagerTest : UnitTestFixtureBase<ProductSourceGroupConfigurationManager>
    {
        private readonly IControlData _controlData = new ControlData {UserID = "joyful", UserPassword = "joyful", ProductID = "16"};
        private readonly ITokenRegistry _tokenRegistry = new Mock<ITokenRegistry>().Object;

        protected ProductSourceGroupConfigurationManager Provider
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void CommunicatorPsts()
        {
//            IEnumerable<string> response = Provider.PrimarySourceTypes("communicator");
//            Assert.IsNotNull(response);
//            Assert.IsTrue(new List<string>(response).Count > 50);
        }

        [TestMethod]
        public void CommunicatorSourceGroup()
        {
//            IEnumerable<SourceGroup> response = Provider.SourceGroups("communicator");
//            Assert.IsNotNull(response);
//            Assert.IsTrue(new List<SourceGroup>().Count <= 5);
        }

        protected override ProductSourceGroupConfigurationManager CreateUnitUnderTest()
        {
            return new ProductSourceGroupConfigurationManager(_controlData, _tokenRegistry);
        }
    }
}