using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Web.Mvc.Infrastructure;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI;
using Ninject.Web.Mvc;
using Ninject.Web.Mvc.Filter;
using Ninject.Web.Mvc.Validation;

namespace DowJones
{
    public class CoreMvcFrameworkBindingModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            // START Ninject MVC stuff
            Bind<IDependencyResolver>().To<NinjectDependencyResolver>().InTransientScope();
            Bind<IFilterProvider>().To<NinjectFilterAttributeFilterProvider>().InTransientScope();
            Bind<IFilterProvider>().To<NinjectFilterProvider>().InTransientScope();
            Bind<ModelValidatorProvider>().To<NinjectDataAnnotationsModelValidatorProvider>().InTransientScope();
            // END   Ninject MVC stuff

            Bind<IClientStateMapper>().To<ClientStateMapper>().InTransientScope();

            Bind<IControllerRegistry>().To<ControllerRegistry>().InSingletonScope();

            Bind<IRouteGenerator>().To<RouteGenerator>().InSingletonScope();

            Bind<StylesheetRegistryBuilder>().ToSelf().InRequestScope();

            Bind<IStylesheetRegistry>().To<StylesheetRegistry>().InRequestScope();

            Bind<ScriptRegistryBuilder>().ToSelf().InRequestScope();

            Bind<IScriptRegistry>().To<ScriptRegistry>().InRequestScope();

            Bind<IScriptRegistryWriter>().To<ScriptRegistryWriter>().InRequestScope();

            Bind<IViewComponentRegistry>().To<ViewComponentRegistry>().InRequestScope();

            Bind<ViewComponentFactory>().ToSelf().InRequestScope();
        }
    }
}
