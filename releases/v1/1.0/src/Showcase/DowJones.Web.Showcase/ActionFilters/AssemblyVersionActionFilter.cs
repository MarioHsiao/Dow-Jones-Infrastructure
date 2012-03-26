using System.Reflection;
using System.Web.Mvc;

namespace DowJones.Web.Showcase.ActionFilters
{
    public class AssemblyVersionActionFilter : ActionFilterAttribute
    {
        private static readonly string ShowcaseAssemblyVersion =
            typeof (AssemblyVersionActionFilter).Assembly.GetName().Version.ToString();

        protected string InfrastructureAssemblyVersion
        {
            get
            {
                return _infrastructureAssemblyVersion =
                       _infrastructureAssemblyVersion 
                    ?? Assembly.Load("DowJones.Web.Mvc").GetName().Version.ToString(); 
            }
        }
        private static string _infrastructureAssemblyVersion;
            

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.InfrastructureAssemblyVersion = InfrastructureAssemblyVersion;
            filterContext.Controller.ViewBag.ShowcaseAssemblyVersion = ShowcaseAssemblyVersion;
        }

    }
}