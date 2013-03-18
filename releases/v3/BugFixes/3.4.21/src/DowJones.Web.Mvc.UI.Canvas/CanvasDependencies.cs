using DowJones.Web.ClientResources;
using DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule;
using DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.ClientResources;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class CanvasDependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IClientResourceProcessor>().To<ScriptModuleClientResourcePopulator>().InRequestScope();
            Bind<IClientResourceRepository>().To<ScriptModuleResourceRepository>().InRequestScope();
        }
    }
}