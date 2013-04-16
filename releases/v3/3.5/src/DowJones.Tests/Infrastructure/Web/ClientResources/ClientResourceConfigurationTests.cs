using System.Linq;
using System.Text;
using System.Xml.Linq;
using DowJones.Exceptions;
using DowJones.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Web.ClientResources
{
    [TestClass]
    public class ClientResourceConfigurationTests : UnitTestFixture
    {
        [TestMethod]
        public void ShouldLoadClientResourceAliases()
        {
            const string jqueryAlias = "jquery", jqueryName = "DowJones.Web.Mvc.Resources.js.jquery.js";
            const string commonAlias = "common", commonName = "DowJones.Web.Mvc.Resources.js.common.js";

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", jqueryAlias, jqueryName);
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", commonAlias, commonName);
            xmlBuilder.Append("</ClientResources>");

            var config = XDocument.Parse(xmlBuilder.ToString());
            var aliases = new ClientResourceConfiguration(config).Aliases.ToArray();

            Assert.AreEqual(2, aliases.Count());
            Assert.AreEqual(jqueryName, aliases.Single(x => x.Alias == jqueryAlias).Name);
            Assert.AreEqual(commonName, aliases.Single(x => x.Alias == commonAlias).Name);
        }

        [TestMethod]
        public void ShouldThrowExceptionForInvalidClientResourceAliasMappingAlias()
        {
            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", string.Empty, "RESOURCE_NAME");
            xmlBuilder.Append("</ClientResources>");

            var config = XDocument.Parse(xmlBuilder.ToString());

            try
            {
                var aliases = new ClientResourceConfiguration(config).Aliases;
            }
            catch(DowJonesUtilitiesException)
            {
                return;
            }

            Assert.Fail("Exception not thrown!");
        }

        [TestMethod]
        public void ShouldThrowExceptionForInvalidClientResourceAliasMappingName()
        {
            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", "RESOURCE_ALIAS", string.Empty);
            xmlBuilder.Append("</ClientResources>");

            var config = XDocument.Parse(xmlBuilder.ToString());

            try
            {
                var aliases = new ClientResourceConfiguration(config).Aliases;
            }
            catch(DowJonesUtilitiesException)
            {
                return;
            }

            Assert.Fail("Exception not thrown!");
        }
    }
}