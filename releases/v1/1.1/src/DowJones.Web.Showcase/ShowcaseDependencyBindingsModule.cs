using System.Web;
using DowJones.DependencyInjection;
using DowJones.Managers.PAM;
using DowJones.Session;
using DowJones.Web.Navigation;
using DowJones.Web.Showcase.Mocks;

namespace DowJones.Web.Showcase
{
    public class ShowcaseDependencyBindingsModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Rebind<IPageAssetsManager>()
                .To<Mvc.UI.Canvas.DataAccess.Managers.Common.PageAssetsManager>();

            Bind<IControlData>()
                .ToMethod(x => ControlDataFactory.GetControlData(HttpContext.Current))
                .InRequestScope();

            Bind<IPreferences>()
                .ToConstant(new BasePreferences("en"));

            Bind<IMenuDataSource>().To<ShowcaseMenuDataSource>().InSingletonScope();
        }
    }
}
