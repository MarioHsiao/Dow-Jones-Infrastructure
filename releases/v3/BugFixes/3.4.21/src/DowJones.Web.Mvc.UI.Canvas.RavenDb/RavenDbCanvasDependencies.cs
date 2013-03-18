using DowJones.Pages;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc.UI.Canvas.RavenDb;
using Ninject;
using Raven.Client;
using Raven.Client.Document;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class RavenDbCanvasDependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            BindIfAbsent<IDocumentStore>(binding => binding
                .ToConstant(new DocumentStore { ConnectionStringName = "RavenDb" })
                .InSingletonScope()
                .OnActivation(x => x.Initialize())
            );

            BindIfAbsent<IDocumentSession>(binding => binding
                .ToMethod(x => x.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation(x => x.Dispose())
            );

            BindIfAbsent<IPageRepository>(binding => binding
                .To<RavenDbPageRepository>()
                .InRequestScope()
            );

            BindIfAbsent<IScriptModuleTemplateManager>(binding => binding
                .To<RavenDbScriptModuleTemplateRepository>()
                .InRequestScope()
            );
        }
    }
}