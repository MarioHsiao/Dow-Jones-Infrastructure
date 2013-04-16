using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc
{
    [TestClass]
    public class DowJonesControllerBaseTests : UnitTestFixtureBase<ControllerStub>
    {

        [TestMethod]
        public void ShouldProvideDummyLoggerWhenNoneIsInjected()
        {
            Assert.IsNotNull(UnitUnderTest.Logger, "Logger should have been injected but is null");
        }


        protected override ControllerStub CreateUnitUnderTest()
        {
            return new ControllerStub();
        }
    }

    /// <summary>
    /// Empty implementation stub for testing
    /// </summary>
    public class ControllerStub : ControllerBase
    {
        public ILog Logger { get { return Log; } }
    }
}