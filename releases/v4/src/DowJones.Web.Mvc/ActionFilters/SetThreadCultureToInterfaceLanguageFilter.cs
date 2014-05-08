using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Globalization;
using DowJones.Session;

namespace DowJones.Web.Mvc.ActionFilters
{
    public class SetThreadCultureToInterfaceLanguageFilter : IActionFilter
    {
        protected internal IUserSession Session
        {
            get { return _session ?? ServiceLocator.Resolve<IUserSession>(); }
            set { _session = value; }
        }
        private IUserSession _session;


        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var interfaceLanguage = Session.InterfaceLanguage;

            var culture = CultureManager.GetCultureInfoFromInterfaceLanguage(interfaceLanguage);

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}