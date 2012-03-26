using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.CacheKey
{
    [TestClass]
    public class CacheKeysUnitTest : AbstractUnitTest
    {

        [TestMethod]
        public void TestPageListCacheKeyGenerator()
        {
            var generator = new PageListCacheKeyGenerator();
            Console.WriteLine(@"PageListCacheKeyGenerator.ToCacheKey::->{0}", generator.ToCacheKey());
        }

        [TestMethod]
        public void TestGetPageById()
        {
            var test = "{\"pid\":\"13794\",\"rid\":\"13794\",\"racs\":\"Personal\",\"pacs\":\"Personal\",\"paq\":\"User\",\"p\":\"NP\",\"n\":\"Page\"}";
            var generator = new PageCacheKeyGenerator(test);
            Console.WriteLine(generator.ToReference());
            Assert.IsTrue(test == generator.ToCacheKey());

            var manager = new PageListManager(ControlData, Preferences);
            manager.GetPage(generator.ToReference(), true);
        }
    }
}
