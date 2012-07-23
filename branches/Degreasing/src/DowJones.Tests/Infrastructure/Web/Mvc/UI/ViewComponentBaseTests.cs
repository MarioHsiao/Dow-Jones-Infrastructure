using System.Linq;
using System.Web.UI;
using DowJones.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI
{
    [TestClass]
    public class ViewComponentBaseTests : UnitTestFixture
    {
        public const string SuperClassScriptUrl = "SuperClass.js";
        public const string SubClassScriptUrl = "SubClass.js";
        public const string SubSubClassScriptUrl = "SubSubClass.js";

        [TestMethod]
        public void ShouldDiscoverClientResourcesFromAllAnscestors()
        {
            var component = new MyImplemenetation();

            var clientResources = component.ClientResources;
            var superClassScript = clientResources.FirstOrDefault(x => x.Url == SuperClassScriptUrl);
            var subClassScript = clientResources.FirstOrDefault(x => x.Url == SubClassScriptUrl);
            var subSubClassScript = clientResources.FirstOrDefault(x => x.Url == SubSubClassScriptUrl);

            Assert.IsNotNull(superClassScript);
            Assert.IsNotNull(subClassScript);
            Assert.IsNotNull(subSubClassScript);
        }

        [TestMethod]
        public void ShouldDiscoverClientResourcesInOrderOfAncestry()
        {
            var component = new MyImplemenetation();

            var clientResources = component.ClientResources.Select(x => x.Url);

            var expectedOrder = new[] {SuperClassScriptUrl, SubClassScriptUrl, SubSubClassScriptUrl};

            Assert.IsTrue(clientResources.SequenceEqual(expectedOrder));
        }


        [ScriptResource("Super", Url = SuperClassScriptUrl)]
        internal class SomeSuperClass : ViewComponentBase
        {
            public override string ClientPluginName { get { return null; } }

            protected override void WriteContent(HtmlTextWriter writer)
            {
            }
        }

        [ScriptResource("SubClass", Url = SubClassScriptUrl)]
        internal class AGenericSubType<T> : SomeSuperClass
        {
        }

        [ScriptResource("ViewComponentSubSubClass", Url = SubSubClassScriptUrl)]
        internal class MyImplemenetation : AGenericSubType<object>
        {
        }
    }
}