using System;
using System.IO;
using System.Linq;
using DowJones.Web.ClientResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones
{
    [TestClass]
    public class EmbeddedResourcesTests
    {
#pragma warning disable 169
        // HACK: Refer to the Components assembly so it's included in the output assemblies!
        private static readonly Type ArbitraryComponentTypeReference = typeof (DowJones.Web.Mvc.UI.Components.Menu.MenuExtensions);
#pragma warning restore 169


        [TestMethod]
        public void AllEmbeddedClientResourcesShouldBeAvailableAsAssemblyResources()
        {
            var validator = new EmbeddedClientResourceValidator();

            var failures = 
                validator.ValidateClientResources(
                    Path.GetDirectoryName(GetType().Assembly.Location), 
                    "DowJones.*.dll", 
                    @"\.Test"
                ).ToArray();

            if (failures.Any())
                Assert.Fail("\r\n" + string.Join("\r\n", failures));
        }
    }
}
