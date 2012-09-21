using System.Threading;
using DowJones.Dash.Website.DataSources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SplunkDataSourceTests
    {
        [TestMethod]
        public void ShouldRetrieveMobileData()
        {
            DataReceivedEventArgs args = null;

            var dataSource = new BrowserDataSource();
            dataSource.DataReceived += (o, x) => args = x;
            dataSource.Start();

            while(args == null)
                Thread.Sleep(1000);

            Assert.IsTrue(true);
        }
    }
}
