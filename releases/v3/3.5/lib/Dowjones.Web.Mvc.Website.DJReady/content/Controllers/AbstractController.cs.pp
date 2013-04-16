using DowJones.Web.Mvc.Threading;
using $rootnamespace$.Filters;
using DowJones.Web.Mvc.ActionFilters;
namespace $rootnamespace$.Controllers
{
	[SessionTimeoutExceptionFilter(redirectUrl: "~/Home/Index", Order = 1)]
    [HandleException(Order = 0)]
    [AspCompat]
    public abstract class AbstractController : DowJones.Web.Mvc.ControllerBase
    {
    }
}