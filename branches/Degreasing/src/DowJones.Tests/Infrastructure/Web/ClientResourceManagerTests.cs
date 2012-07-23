using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web
{
    [TestClass]
    public class ClientResourceManagerTests : UnitTestFixture
    {

        [TestMethod]
        public void ShouldAliasAClientResourceName()
        {
            const string ExpectedAlias = "jquery";
            const string ExpectedResourceName = "DowJones.Web.Mvc.UI.Resources.jquery.js";

            var aliases = new[] {new ClientResourceAlias
                                     {
                                         Alias = ExpectedAlias, 
                                         Name = ExpectedResourceName
                                     }};

            var manager = NewClientResourceManager(aliases: aliases);

            var alias = manager.Alias(ExpectedResourceName);

            Assert.AreEqual(ExpectedAlias, alias);
        }

        [TestMethod]
        public void ShouldAliasMultipleLevelsOfAliases()
        {
            const string ExpectedAlias = "jquery";
            const string IntermediateAlias = "jquery.js";
            const string ExpectedResourceName = "DowJones.Web.Mvc.UI.Resources.jquery.js";

            var aliases = new[] {
                new ClientResourceAlias { Alias = ExpectedAlias, Name = IntermediateAlias },
                new ClientResourceAlias { Alias = IntermediateAlias, Name = ExpectedResourceName },
            };

            var manager = NewClientResourceManager(aliases: aliases);

            var alias = manager.Alias(ExpectedResourceName);

            Assert.AreEqual(ExpectedAlias, alias);
        }

        [TestMethod]
        public void ShouldDealiasAClientResourceName()
        {
            const string ExpectedAlias = "jquery";
            const string ExpectedResourceName = "DowJones.Web.Mvc.UI.Resources.jquery.js";

            var aliases = new[] {new ClientResourceAlias
                                     {
                                         Alias = ExpectedAlias, 
                                         Name = ExpectedResourceName
                                     }};

            var manager = NewClientResourceManager(aliases: aliases);

            var resourceName = manager.Dealias(ExpectedAlias);

            Assert.AreEqual(ExpectedResourceName, resourceName);
        }

        [TestMethod]
        public void ShouldDealiasMultipleLevelsOfAliases()
        {
            const string ExpectedAlias = "jquery";
            const string IntermediateAlias = "jquery.js";
            const string ExpectedResourceName = "DowJones.Web.Mvc.UI.Resources.jquery.js";

            var aliases = new[] {
                new ClientResourceAlias { Alias = ExpectedAlias, Name = IntermediateAlias },
                new ClientResourceAlias { Alias = IntermediateAlias, Name = ExpectedResourceName },
            };

            var manager = NewClientResourceManager(aliases: aliases);

            var resourceName = manager.Dealias(ExpectedAlias);

            Assert.AreEqual(ExpectedResourceName, resourceName);
        }


        [TestMethod]
        public void ShouldGetAggregateClientResourceGroups()
        {
            var jquery = new ClientResource {Name = "DowJones.Web.Mvc.UI.Resources.jquery.js", Url="~/jquery.js"};
            var jqueryUi = new ClientResource {Name = "DowJones.Web.Mvc.UI.Resources.jquery-ui.js", Url="~/jquery-ui.js"};

            var jqueryAlias = new ClientResourceAlias { Alias = "jquery", Name = jquery.Name };
            var jqueryUiAlias = new ClientResourceAlias { Alias = "jquery-ui", Name = jqueryUi.Name };
            var aggregate = new ClientResourceAlias
                                {
                                    Alias = "aggregate", 
                                    Name = string.Join(ClientResourceManager.ResourceNameDelimiter, new[] { jquery.Name, jqueryUi.Name })
                                };

            var manager = NewClientResourceManager(
                new [] { jquery, jqueryUi}, 
                new [] { jqueryAlias, jqueryUiAlias, aggregate });

            var resources = manager.GetClientResources(aggregate.Alias).ToList();
            Assert.AreEqual(2, resources.Count);
            CollectionAssert.Contains(resources, jquery);
            CollectionAssert.Contains(resources, jqueryUi);
        }


        [TestMethod]
        public void ShouldGetClientResourceByName()
        {
            const string ExpectedResourceName = "DowJones.Web.Mvc.UI.Resources.jquery.js";
            var expectedResource = new ClientResource { Name = ExpectedResourceName, Url = "~/jquery.js" };

            var clientResources = new[]
                                      {
                                          expectedResource, 
                                          // Add a "dummy" resource to try to trip up the Manager
                                          new ClientResource { Name = "DUMMY", Url = "~/dummy.js" }
                                      };

            var manager = NewClientResourceManager(clientResources);

            var actualResources = manager.GetClientResources(new[] { ExpectedResourceName });

            Assert.AreEqual(expectedResource, actualResources.SingleOrDefault());
        }

        [TestMethod]
        public void ShouldGetClientResourceByAlias()
        {
            const string ExpectedAlias = "jquery";
            const string ExpectedResourceName = "DowJones.Web.Mvc.UI.Resources.jquery.js";

            var expectedResource = new ClientResource { Name = ExpectedResourceName, Url = "~/jquery.js" };

            var clientResources = new[]
                                      {
                                          expectedResource, 
                                          // Add a "dummy" resource to try to trip up the Manager
                                          new ClientResource { Name = "DUMMY", Url = "~/dummy.js" }
                                      };

            var aliases = new[] {new ClientResourceAlias
                                     {
                                         Alias = ExpectedAlias, 
                                         Name = expectedResource.Name
                                     }};

            var manager = NewClientResourceManager(clientResources, aliases);

            var actualResources = manager.GetClientResources(new[] { ExpectedAlias });

            Assert.AreEqual(expectedResource, actualResources.SingleOrDefault());
        }


        [TestMethod]
        public void ShouldGenerateUrlWithMultipleResourcesCombined()
        {
            var resources = new []  {
                    new ClientResource { Name = "jquery", Url = "~/jquery.js" },
                    new ClientResource { Name = "jquery-ui", Url = "~/jquery.ui.js" },
                    new ClientResource { Name = "common", Url = "~/common.js" },
                };

            var manager = NewClientResourceManager();
            manager.UrlResolver = (resource, culture) => string.Join(";", resource);

            var url = manager.GenerateUrl(resources, CultureInfo.CurrentCulture);

            Assert.IsTrue(Regex.IsMatch(url, "jquery[^-]"));
            Assert.IsTrue(Regex.IsMatch(url, "jquery-ui"));
            Assert.IsTrue(Regex.IsMatch(url, "common"));
        }

        [TestMethod]
        public void ShouldGenerateUrlWithAliasesAndMultipleResourcesCombined()
        {
            var resources = new []  {
                    new ClientResource { Name = "jquery.js", Url = "~/jquery.js" },
                    new ClientResource { Name = "jquery-ui.js", Url = "~/jquery.ui.js" },
                    new ClientResource { Name = "common.js", Url = "~/common.js" },
                };

            var aliases =
                from resource in resources
                let alias = resource.Name.Replace(".js", string.Empty)
                select new ClientResourceAlias { Alias = alias, Name = resource.Name };

            var manager = NewClientResourceManager(aliases: aliases);
            manager.UrlResolver = (resource, culture) => string.Join(";", resource);

            var url = manager.GenerateUrl(resources, CultureInfo.CurrentCulture);

            Assert.IsTrue(Regex.IsMatch(url, "jquery[^-]"));
            Assert.IsTrue(Regex.IsMatch(url, "jquery-ui"));
            Assert.IsTrue(Regex.IsMatch(url, "common"));
        }


        [TestMethod]
        public void ShouldBeAbleToTakeRoundTripFromGeneratedUrlBackToResources()
        {
            var expectedResources = new []  {
                    new ClientResource { Name = "jquery", Url = "~/jquery.js" },
                    new ClientResource { Name = "jquery-ui", Url = "~/jquery.ui.js" },
                    new ClientResource { Name = "common", Url = "~/common.js" },
                };

            var manager = NewClientResourceManager(expectedResources);
            manager.UrlResolver = (resource, culture) => string.Join(";", resource);

            var url = manager.GenerateUrl(expectedResources, CultureInfo.CurrentCulture);

            var actualResources = manager.GetClientResources(url).ToList();

            Assert.AreEqual(expectedResources.Count(), actualResources.Count);

            foreach(var expectedResource in expectedResources)
                CollectionAssert.Contains(actualResources, expectedResource);
        }

        [TestMethod]
        public void ShouldBeAbleToTakeRoundTripFromGeneratedUrlBackToResourcesWithAliases()
        {
            var expectedResources = new[]  {
                    new ClientResource { Name = "jquery.js", Url = "~/jquery.js" },
                    new ClientResource { Name = "jquery-ui.js", Url = "~/jquery.ui.js" },
                    new ClientResource { Name = "common.js", Url = "~/common.js" },
                };

            var aliases =
                from resource in expectedResources
                let alias = resource.Name.Replace(".js", string.Empty)
                select new ClientResourceAlias { Alias = alias, Name = resource.Name };


            var manager = NewClientResourceManager(expectedResources, aliases);
            manager.UrlResolver = (resource, culture) => string.Join(";", resource);

            var url = manager.GenerateUrl(expectedResources, CultureInfo.CurrentCulture);

            var actualResources = manager.GetClientResources(url).ToList();

            Assert.AreEqual(expectedResources.Count(), actualResources.Count);

            foreach(var expectedResource in expectedResources)
                CollectionAssert.Contains(actualResources, expectedResource);
        }

        [TestMethod]
        public void ShouldDeserializeSerializedClientResourceNames()
        {
            var resources = new[]  {
                    new ClientResource { Name = "jquery.js", Url = "~/jquery.js" },
                    new ClientResource { Name = "jquery-ui.js", Url = "~/jquery.ui.js" },
                    new ClientResource { Name = "common.js", Url = "~/common.js" },
                };

            var serializedNames = string.Join(
                ClientResourceManager.ResourceNameDelimiter, 
                resources.Select(x => x.Name));

            var manager = NewClientResourceManager(resources);

            var resourceNames = manager.ParseClientResourceNames(serializedNames).ToList();

            CollectionAssert.AreEquivalent(resourceNames, resources.Select(x => x.Name).ToList());
        }
    
        [TestMethod]
        public void ShouldDealiasSerializedClientResourceNames()
        {
            var resources = new[]  {
                    new ClientResource { Name = "jquery.js", Url = "~/jquery.js" },
                    new ClientResource { Name = "jquery-ui.js", Url = "~/jquery.ui.js" },
                    new ClientResource { Name = "common.js", Url = "~/common.js" },
                };

            var aliases =
                from resource in resources
                let alias = resource.Name.Replace(".js", string.Empty)
                select new ClientResourceAlias { Alias = alias, Name = resource.Name };

            var serializedAliases = string.Join(
                ClientResourceManager.ResourceNameDelimiter,
                aliases.Select(x => x.Alias));

            var manager = NewClientResourceManager(resources, aliases);

            var resourceNames = manager.ParseClientResourceNames(serializedAliases).ToList();

            CollectionAssert.AreEquivalent(resourceNames, resources.Select(x => x.Name).ToList());
        }

        [TestMethod]
        public void ShouldReturnEmptyListWhenSerializedResourceNamesAreEmpty()
        {
            var resources = new[]  {
                    new ClientResource { Name = "jquery.js", Url = "~/jquery.js" },
                };

            var manager = NewClientResourceManager(resources);

            var resourceNames = manager.ParseClientResourceNames(string.Empty);

            Assert.AreEqual(0, resourceNames.Count());
        }


        protected ClientResourceManager NewClientResourceManager(
            IEnumerable<ClientResource> resources = null,
            IEnumerable<ClientResourceAlias> aliases = null
        )
        {
            return new ClientResourceManager(
                resources ?? Enumerable.Empty<ClientResource>(),
                aliases ?? Enumerable.Empty<ClientResourceAlias>()
            );
        }
    }
}