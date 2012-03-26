using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc
{
    [TestClass]
    public class DowJonesControllerBaseTests : UnitTestFixtureBase<DowJonesControllerStub>
    {

        [TestMethod]
        public void ShouldProvideDummyLoggerWhenNoneIsInjected()
        {
            Assert.IsNotNull(UnitUnderTest.Logger, "Logger should have been injected but is null");
        }


        protected override DowJonesControllerStub CreateUnitUnderTest()
        {
            return new DowJonesControllerStub();
        }
    }

    /// <summary>
    /// Empty implementation stub for testing
    /// </summary>
    public class DowJonesControllerStub : DowJonesControllerBase
    {
        public ILog Logger { get { return Log; } }
    }
}