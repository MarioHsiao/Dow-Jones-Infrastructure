using System.Linq;
using DowJones.Web.ClientResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones
{
    [TestClass]
    public class EmbeddedResourcesTests
    {
        [TestMethod]
        public void AllEmbeddedClientResourcesShouldBeAvailableAsAssemblyResources()
        {
            var validator = new EmbeddedClientResourceValidator();

            var assemblies = new[] {
                    typeof(DowJones.Web.Mvc.Resources.EmbeddedResources),
                    typeof(DowJones.Web.Mvc.UI.Components.Resources.EmbeddedResources),
                    typeof(DowJones.Web.Mvc.UI.Canvas.AbstractCanvas),
                    typeof(DowJones.Web.Mvc.Search.UI.Components.SearchNavigator.SearchNavigatorComponent),
                }.Select(x => x.Assembly).ToArray();

            var failures = validator.ValidateClientResources(assemblies).ToArray();

            if (failures.Any())
                Assert.Fail("\r\n" + string.Join("\r\n", failures));
        }
    }
}
