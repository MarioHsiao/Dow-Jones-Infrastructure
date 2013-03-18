using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DowJones.Web.Mobile
{
    // Some statically-defined "known" mobile devices
    // TODO: Generate these from an external source (e.g. the browser info file)
    public partial class MobileDeviceInformation
    {
        public static MobileDeviceInformation Android = new MobileDeviceInformation
        {
            Category = "Android",
            Platform = "Android",
            UserAgentExpression = new Regex("Android"),
        };

        public static MobileDeviceInformation iPad = new MobileDeviceInformation
        {
            Category = "iPad",
            Platform = "iPad",
            UserAgentExpression = new Regex("iPad"),
        };

        public static MobileDeviceInformation iPhone = new MobileDeviceInformation
        {
            Category = "iPhone",
            Platform = "iPhone",
            UserAgentExpression = new Regex("iPhone"),
        };

        public static MobileDeviceInformation WindowsPhone = new MobileDeviceInformation
        {
            Category = "Windows Phone",
            Platform = "Windows Phone",
            UserAgentExpression = new Regex("Windows Phone"),
        };

     
        public static IEnumerable<MobileDeviceInformation> KnownDevices = new[]
            {
                Android, iPad, iPhone, WindowsPhone
            };
    }
}