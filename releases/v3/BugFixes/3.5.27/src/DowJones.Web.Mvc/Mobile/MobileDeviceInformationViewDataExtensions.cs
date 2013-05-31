using System.Web.Mvc;
using DowJones.Web.Mobile;

namespace DowJones.Web.Mvc.Mobile.Extensions
{
    public static class MobileDeviceInformationViewDataExtensions
    {
        public static bool IsMobileDevice(this ViewDataDictionary viewData)
        {
            return viewData.GetMobileDevice() != null;
        }

        public static MobileDeviceInformation GetMobileDevice(this ViewDataDictionary viewData)
        {
            var mobileDeviceInfo = viewData[MobileDeviceInformation.ViewDataKey] as MobileDeviceInformation;
            return mobileDeviceInfo;
        }
    }
}