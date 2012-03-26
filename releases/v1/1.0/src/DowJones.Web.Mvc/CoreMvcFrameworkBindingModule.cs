using System.Web;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Web.Mvc.Infrastructure;
using DowJones.Web.Mvc.Properties;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI;
using Ninject;

namespace DowJones.Web.Mvc
{
    public class CoreMvcFrameworkBindingModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IClientStateMapper>().To<ClientStateMapper>();

            Bind<IControllerRegistry>().To<ControllerRegistry>().InSingletonScope();

            Bind<IRouteGenerator>().To<RouteGenerator>().InSingletonScope();

            Bind<StylesheetRegistryBuilder>().ToSelf().InRequestScope();
            Bind<IStylesheetRegistry>().To<StylesheetRegistry>().InRequestScope();

            Bind<ScriptRegistryBuilder>().ToSelf().InRequestScope();
            Bind<IScriptRegistry>().To<ScriptRegistry>().InRequestScope();

            Bind<IScriptRegistryWriter>()
                .ToMethod(x => {
                    IKernel kernel = x.Kernel;
                    var request = kernel.Get<HttpRequestBase>();
                    if(Settings.Default.HeadJsEnabled && !request.IsAjaxRequest() && !string.IsNullOrWhiteSpace(request["headjs"]))
                        return kernel.Get<HeadJsScriptRegistryWriter>();
                    else
                        return kernel.Get<ScriptRegistryWriter>();
                })
                .InRequestScope();

            Bind<IViewComponentRegistry>().To<ViewComponentRegistry>().InRequestScope();

            Bind<ViewComponentFactory>().ToSelf().InRequestScope()
                .OnActivation(factory => factory.Initialize());
        }
    }
}
