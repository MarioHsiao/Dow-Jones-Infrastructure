using System.Threading;
using System.Web.Mvc;
using DowJones.Core;

namespace DowJones.Web.Showcase.ActionFilters
{
    public class LanguageSetter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string language = filterContext.RequestContext.HttpContext.Request["lang"];
            var culture = CultureManager.GetCultureInfoFromInterfaceLanguage(language);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}