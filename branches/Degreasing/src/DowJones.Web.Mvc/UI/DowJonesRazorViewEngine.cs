using System.Web;
using System.Web.Mvc;
using DowJones.Web.Mobile;
using DowJones.Web.Mvc.Mobile.Extensions;

namespace DowJones.Web.Mvc.UI
{
    public class DowJonesRazorViewEngine : RazorViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = GetMobileView(controllerContext, viewName, masterName, useCache);

            // If we didn't create a mobile view (either because this is not a mobile request
            // or no appropriate view was found), fall back to the "regular" view
            if (NoValidViewWasFound(result))
                result = base.FindView(controllerContext, viewName, masterName, useCache);

            return result;
        }

        private ViewEngineResult GetMobileView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = null;
            
            HttpBrowserCapabilitiesBase browser = controllerContext.HttpContext.Request.Browser;

            if(browser.IsMobileDevice)
            {
                // Get the Mobile Device information from the ViewData 
                // (which should have been set via the MobileDeviceInformationFilter)
                var mobileDevice = controllerContext.Controller.ViewData.GetMobileDevice();

                if(mobileDevice != null)
                {
                    string mobileViewName = string.Format("{0}/{1}", mobileDevice.FolderName, viewName);
                    result = base.FindView(controllerContext, mobileViewName, masterName, useCache);
                }

                if (NoValidViewWasFound(result))
                    result = base.FindView(controllerContext, MobileDeviceInformation.GenericFolderName + viewName, masterName, useCache);
            }

            return result;
        }

        private static bool NoValidViewWasFound(ViewEngineResult result)
        {
            return result == null || result.View == null;
        }
    }
}