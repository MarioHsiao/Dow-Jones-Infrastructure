using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using DowJones.Web.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure
{
    [TestClass]
    public class ControllerRegistryTests : UnitTestFixtureBase<ControllerRegistry>
    {
        protected readonly Type TestControllerType = typeof(TestController);

        protected ControllerRegistry ControllerRegistry
        {
            get { return UnitUnderTest; }
        }


        [TestMethod]
        public void ShouldDiscoverControllersInAnAssembly()
        {
            Assert.IsTrue(ControllerRegistry.ControllerTypes.Contains(TestControllerType));
        }

        [TestMethod]
        public void ShouldDiscoverActionAttributess()
        {
            ControllerActionAttributeInfo testActionAttribute =
                ControllerRegistry.ControllerActionAttributes
                    .Where(attribute => attribute.Controller == TestControllerType)
                    .Where(attribute => attribute.Attribute is DisplayNameAttribute)
                    .Where(attribute => attribute.Action.Name == "TestAction")
                    .SingleOrDefault();

            Assert.IsNotNull(testActionAttribute);
            Assert.AreEqual(
                ((DisplayNameAttribute)testActionAttribute.Attribute).DisplayName,
                TestController.DisplayName);
        }

        protected override ControllerRegistry CreateUnitUnderTest()
        {
            ControllerRegistry unitUnderTest = new ControllerRegistry(new[] { TestControllerType });
            return unitUnderTest;
        }
    }


    #region Test Interface and Class Definitions

    public class TestController : Controller
    {
        public const string DisplayName = "Test Action";

        [DisplayName(DisplayName)]
        public ActionResult TestAction()
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}