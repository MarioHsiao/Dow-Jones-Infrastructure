using DowJones.Pages.Modules.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;

namespace DowJones.Web.Mvc.UI.Canvas.RavenDb.Tests
{
    [TestClass]
    public class RavenDbScriptModuleTemplateManagerTests
    {
        private RavenDbScriptModuleTemplateRepository _repository;
        private IDocumentSession _session;

        [TestInitialize]
        public void TestInitialize()
        {
            _session = RavenDbInstance.GetSession();
            _repository = new RavenDbScriptModuleTemplateRepository(_session);
        }

        [TestMethod]
        public void ShouldSaveTemplate()
        {
            var id = _repository.CreateTemplate(new ScriptModuleTemplate { Title = "Test Template"});
            _session.SaveChanges();

            var template = _session.Load<ScriptModuleTemplate>(id);

            Assert.IsNotNull(template);
            Assert.AreEqual("Test Template", template.Title);
        }
    }
}
