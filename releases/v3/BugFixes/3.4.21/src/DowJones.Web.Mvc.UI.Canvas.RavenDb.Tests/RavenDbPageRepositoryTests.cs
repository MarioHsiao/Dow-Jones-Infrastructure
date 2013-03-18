using System.Linq;
using DowJones.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;

namespace DowJones.Web.Mvc.UI.Canvas.RavenDb.Tests
{
    [TestClass]
    public class RavenDbPageRepositoryTests
    {
        private RavenDbPageRepository _repository;
        private IDocumentSession _session;

        [TestInitialize]
        public void TestInitialize()
        {
            _session = RavenDbInstance.GetSession();
            _repository = new RavenDbPageRepository(_session);
        }

        [TestMethod]
        public void ShouldGetPage()
        {
            var id = _repository.CreatePage(new Page());
            var page = _repository.GetPage(id);

            Assert.IsNotNull(page);
            Assert.AreEqual(id.Value, page.ID);
        }

        [TestMethod]
        public void ShouldSavePage()
        {
            var id = _repository.CreatePage(new Page());

            var pages = _session.Query<Page>().ToArray();

            Assert.IsTrue(pages.Count() > 0);

            var page = pages.Single(x => x.ID == id.Value);

            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ID);
        }

        [TestMethod]
        public void ShouldRetrieveByUsername()
        {
            _repository.CreatePage(new Page { OwnerUserId = "FrankSinatra"});
            var page = _repository.GetPages().FirstOrDefault(x => x.OwnerUserId == "FrankSinatra");
            Assert.IsNotNull(page);
        }
    }
}
