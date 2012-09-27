using System.Collections.Generic;
using DowJones.Dash.Caching;
using DowJones.Dash.DataSources;
using DowJones.Dash.Website.Hubs;
using DowJones.Dash.Website.RavenDB;
using DowJones.Infrastructure.Common;
using DowJones.Pages;
using DowJones.Pages.Modules;
using DowJones.Pages.Modules.Templates;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.RavenDb;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Ninject;
using Raven.Client;

namespace DowJones.Dash.Website
{
    public class Dependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IControlData>().ToMethod(x => new ControlData()).InRequestScope();
            Bind<IUserSession>().ToMethod(x => new UserSession { SessionId = "12345" }).InRequestScope();
            Bind<IPreferences>().ToMethod(x => new DowJones.Preferences.Preferences("en")).InRequestScope();
            Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse())).InRequestScope();
            Bind<Product>().ToConstant(new GlobalProduct()).InSingletonScope();
            Bind<IPageSubscriptionManager>().To<PageSubscriptionManagerStub>();
            Bind<IDataSourceRepository>().To<DataSourceRepository>().InSingletonScope();

            BindToFactory<IDocumentStore, RavenDbDocumentStoreFactory>()
                .InSingletonScope()
                .OnActivation(x => x.Initialize());

            Bind<IDocumentSession>()
                .ToMethod(x => x.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation(x => x.Dispose());

            Bind<IPageRepository>().To<RavenDbPageRepository>().InRequestScope();
            Bind<IScriptModuleTemplateManager>().To<RavenDbScriptModuleTemplateRepository>().InRequestScope();
            Bind<IDashboardMessageCache>().To<DashboardMessageCache>().InSingletonScope();
        }
    }

    class PageSubscriptionManagerStub : IPageSubscriptionManager
    {
        public void PrivatizeModules(IEnumerable<Module> modules)
        {
            throw new System.NotImplementedException();
        }

        public void PublicizeModules(IEnumerable<Module> modules)
        {
            throw new System.NotImplementedException();
        }

        public void EnablePage(PageReference pageRef, bool enabled = true)
        {
            throw new System.NotImplementedException();
        }

        public void PublishPage(PageReference pageRef, params int[] personalAlertIds)
        {
            throw new System.NotImplementedException();
        }

        public string SubscribeToPage(PageReference pageRef)
        {
            throw new System.NotImplementedException();
        }

        public string SubscribeToPage(PageReference pageRef, int position)
        {
            throw new System.NotImplementedException();
        }

        public void UnpublishPage(PageReference pageRef)
        {
            throw new System.NotImplementedException();
        }

        public void UnsubscribeToPage(PageReference pageRef)
        {
            throw new System.NotImplementedException();
        }
    }
}