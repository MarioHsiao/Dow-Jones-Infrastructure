using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using DowJones.DependencyInjection;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess
{
    public class CoreDataAccessFrameworkBindingModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<TaskFactory>().To<TaskFactory>().InSingletonScope().WithConstructorArgument("numberOfThreads", 200);
        }
    }
}
