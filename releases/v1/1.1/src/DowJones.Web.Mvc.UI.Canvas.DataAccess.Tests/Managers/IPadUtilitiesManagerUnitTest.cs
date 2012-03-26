using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class IPadUtilitiesManagerUnitTest : AbstractUnitTest
    {
        [TestMethod]
        public void GenerateMobileLoginToken()
        {
            var manager = new IPadUtilitiesManager(ControlData, new BasePreferences("en"));

            var temp = manager.GetMobileLoginToken();

            Console.WriteLine(temp);
        }
    }
}
