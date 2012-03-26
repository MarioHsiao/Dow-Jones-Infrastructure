using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class AddPageByIdServiceResultTest : AbstractUnitTest
    {
        [TestMethod]
        public void AddPageByIdTest()
        {
            var result = new AddPageByIdServiceResult();
            var request = new AddPageByIdRequest
                              {
                                  PageId = "12673"
                              };

            Preferences.InterfaceLanguage = "de";
            result.AddPageById(ControlData, request, Preferences);
        }
    }
}
