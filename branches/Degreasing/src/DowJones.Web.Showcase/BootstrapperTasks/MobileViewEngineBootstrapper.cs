using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Infrastructure;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Showcase.BootstrapperTasks
{
    public class MobileViewEngineBootstrapper : IBootstrapperTask
    {
        public void Execute()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new DowJonesRazorViewEngine());
        }
    }
}