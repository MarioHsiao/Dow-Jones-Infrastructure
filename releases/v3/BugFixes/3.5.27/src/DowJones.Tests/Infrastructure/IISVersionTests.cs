using System.Web;
using DowJones.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure
{
    [TestClass]
    public class IISVersionTests : UnitTestFixture
    {
        [TestMethod]
        public void ShouldParseServerVersion()
        {
            var version = IISVersion.ParseServerVersion("Microsoft-IIS/5.1");

            Assert.AreEqual(5, version.Major);
        }

        [TestMethod]
        public void ShouldAssumeThatEmptyServerVersionIsCassini()
        {
            var version = IISVersion.ParseServerVersion(string.Empty);

            Assert.AreSame(IISVersion.Cassini, version);
        }

        [TestMethod, TestCategory("Integration"), Ignore]
        public void ShouldRetrieveFileVersionFromIISExecutableWhenRequestIsNull()
        {
            var version = ((HttpRequestBase)null).GetIISVersion();
            
            Assert.IsNotNull(version.Major);
            Assert.AreNotEqual(default(int), version.Major);
        }
    }
}