using System.Configuration;
using DowJones.Infrastructure;
using Raven.Client;
using Raven.Client.Document;

namespace DowJones.Web.Mvc.UI.Canvas.RavenDb
{
    public class RavenDbDocumentStoreFactory : Factory<IDocumentStore>
    {
        public override IDocumentStore Create()
        {
            IDocumentStore documentStore;

            if ("true".Equals(ConfigurationManager.AppSettings["RavenDb.Embedded"]))
            {
                documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore
                    {
                        DataDirectory = ConfigurationManager.AppSettings["RavenDb.DataDirectory"],
                        RunInMemory = "true".Equals(ConfigurationManager.AppSettings["RavenDb.Embedded.RunInMemory"]),
                    };
            }
            else
            {
                documentStore = new DocumentStore {ConnectionStringName = "RavenDb"};
            }

            documentStore.Conventions.AllowQueriesOnId = true;
            documentStore.Conventions.SaveEnumsAsIntegers = true;

            return documentStore;
        }
    }

}
