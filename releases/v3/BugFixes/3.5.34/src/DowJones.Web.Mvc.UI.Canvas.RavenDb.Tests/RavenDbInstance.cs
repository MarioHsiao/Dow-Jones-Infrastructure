using Raven.Client;
using Raven.Client.Embedded;

namespace DowJones.Web.Mvc.UI.Canvas.RavenDb.Tests
{
    public class RavenDbInstance
    {
        public static IDocumentSession GetSession()
        {
            return GetDocumentStore().OpenSession();
        }

        private static IDocumentStore GetDocumentStore()
        {
            var documentStore = new EmbeddableDocumentStore { RunInMemory = true };

            documentStore.Initialize();

            return documentStore;
        }
    }
}