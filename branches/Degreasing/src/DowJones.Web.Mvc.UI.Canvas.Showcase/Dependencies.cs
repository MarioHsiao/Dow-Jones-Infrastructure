using System;
using System.Configuration;
using DowJones.Infrastructure.Common;
using DowJones.Pages;
using DowJones.Pages.Modules.Templates;
using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.RavenDb;
using Ninject;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace DowJones.DegreasedDashboards.Website
{
    public class Dependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IControlData>().ToConstant(new ControlData());
            Bind<IUserSession>().ToConstant(new UserSession { SessionId = "1234567890" });
            Bind<IPreferences>().ToMethod(x => new DowJones.Preferences.Preferences("en"));
            Bind<IPrinciple>().ToConstant(new Principle());
            Bind<Product>().ToConstant(new GlobalProduct());

            IDocumentStore documentStore;

            if ("true".Equals(ConfigurationManager.AppSettings["RavenDb.Embedded"]))
            {
                documentStore = new EmbeddableDocumentStore {
                        DataDirectory = ConfigurationManager.AppSettings["RavenDb.DataDirectory"],
                        RunInMemory = "true".Equals(ConfigurationManager.AppSettings["RavenDb.RunInMemory"]),
                    };
            }
            else
            {
                documentStore = new DocumentStore { ConnectionStringName = "RavenDb" };
            }

            Bind<IDocumentStore>()
                .ToConstant(documentStore)
                .InSingletonScope()
                .OnActivation(x => x.Initialize());

            Bind<IDocumentSession>()
                .ToMethod(x => x.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation(x => x.Dispose());

            Bind<IPageRepository>().To<RavenDbPageRepository>().InRequestScope();
            Bind<IScriptModuleTemplateManager>().To<RavenDbScriptModuleTemplateRepository>().InRequestScope();

//            Bind<IPageManager>().To<InMemoryPageManager>().InSingletonScope();
            Bind<IPageSubscriptionManager>().To<InMemoryPageSubscriptionManager>().InSingletonScope();
//            Bind<IScriptModuleTemplateManager>().To<InMemoryScriptModuleTemplateManager>().InSingletonScope();
        }

        public class Principle : IPrinciple
        {
            public IUserSubPrinciple UserServices { get; private set; }
            public ICoreServicesSubPrinciple CoreServices { get; private set; }
            public IRuleSet RuleSet { get; private set; }
        }
    }
}