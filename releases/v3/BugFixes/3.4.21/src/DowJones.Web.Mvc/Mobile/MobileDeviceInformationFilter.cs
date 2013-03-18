using System.Linq;
using System.Web.Mvc;
using DowJones.Web.Mobile;

namespace DowJones.Web.Mvc.Mobile
{
    public class MobileDeviceInformationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var browser = request.Browser;

            if (!browser.IsMobileDevice) 
                return;

            MobileDeviceInformation mobileInfo =
                MobileDeviceInformation.KnownDevices.FirstOrDefault(
                    knownDevice => knownDevice.MatchesUserAgent(request.UserAgent));

            mobileInfo = mobileInfo ?? new MobileDeviceInformation(browser.Platform);

            filterContext.Controller.ViewData[MobileDeviceInformation.ViewDataKey] = mobileInfo;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            /* NOOP */
        }
    }
}
