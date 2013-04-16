using System.Configuration;
using DowJones.DegreasedDashboards.Website.BootstrapperTasks;
using DowJones.Infrastructure.Common;
using DowJones.Pages;
using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Raven.Client;
using Raven.Client.Embedded;

namespace DowJones.DegreasedDashboards.Website
{
    public class Dependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IControlData>().ToConstant(new ControlData());
            Bind<IUserSession>().ToConstant(new UserSession { SessionId = "1234567890" });
            Bind<IPreferences>().ToMethod(x => new Preferences.Preferences("en"));
            Bind<IPrinciple>().ToConstant(new Principle());
            Bind<Product>().ToConstant(new GlobalProduct());
            Bind<IPageSubscriptionManager>().To<InMemoryPageSubscriptionManager>().InSingletonScope();

            if (bool.Parse(ConfigurationManager.AppSettings["RavenDb.Embedded"] ?? "false"))
            {
                InitializeEmbeddedRavenDb();
            }
        }

        private void InitializeEmbeddedRavenDb()
        {
            var documentStore = 
                new EmbeddableDocumentStore {
                    DataDirectory = ConfigurationManager.AppSettings["RavenDb.DataDirectory"],
                    RunInMemory = bool.Parse(ConfigurationManager.AppSettings["RavenDb.Embedded.RunInMemory"] ?? "false"),
                };

            Rebind<IDocumentStore>()
                .ToConstant(documentStore)
                .InSingletonScope()
                .OnActivation(x => x.Initialize());

            InitializeTestData.ShouldExecute = documentStore.RunInMemory;
        }

        public class Principle : IPrinciple
        {
            public IUserSubPrinciple UserServices { get; private set; }
            public ICoreServicesSubPrinciple CoreServices { get; private set; }
            public IRuleSet RuleSet { get; private set; }
        }
    }
}