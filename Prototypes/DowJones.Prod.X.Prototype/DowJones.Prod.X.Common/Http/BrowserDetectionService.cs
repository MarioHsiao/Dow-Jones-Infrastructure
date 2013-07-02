using System;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Extensions;
using DowJones.Managers.Abstract;

namespace DowJones.Prod.X.Common.Http
{
    public class BrowserDetectionService : IService, IBrowserDetectionService
    {
        private const string Pattern = "up.browser|up.link|windows ce|iphone|ipod|iemobile|mini|mmp|symbian|midp|wap|phone|pocket|mobile|pda|psp";
        private const string PopUserAgent = "|acs-|alav|alca|amoi|audi|aste|avan|benq|bird|blac|blaz|brew|cell|cldc|cmd-|dang|doco|eric|hipt|inno|ipaq|java|jigs|kddi|keji|leno|lg-c|lg-d|lg-g|lge-|maui|maxo|midp|mits|mmef|mobi|mot-|moto|mwbp|nec-|newt|noki|opwv|palm|pana|pant|pdxg|phil|play|pluc|port|prox|qtek|qwap|sage|sams|sany|sch-|sec-|send|seri|sgh-|shar|sie-|siem|smal|smar|sony|sph-|symb|t-mo|teli|tim-|tosh|tsm-|upg1|upsi|vk-v|voda|w3c |wap-|wapa|wapi|wapp|wapr|webc|winw|winw|xda|xda-|";
        private readonly HttpContextBase _contextBase;

        public BrowserDetectionService(HttpContextBase contextBase)
        {
            _contextBase = contextBase;
            if (_contextBase == null)
            {
                return;
            }

            UserAgent = _contextBase.Request.ServerVariables["HTTP_USER_AGENT"];
            if (UserAgent.IsNullOrEmpty())
            {
                return;
            }

            UserAgent = UserAgent.ToLower();
            DeviceType = GetDeviceType();
            IsTelevision = DeviceType == DeviceType.Television;
            IsDesktop = DeviceType == DeviceType.Desktop;
            UpdateSpecificTypes();
        }

        public bool IsMobileSafari { get; private set; }
        public bool IsTabletSafari { get; private set; }
        public bool IsMobileAndroid { get; private set; }
        public bool IsTelevision { get; private set; }
        public bool IsDesktop { get; private set; }
        public string UserAgent { get; private set; }
        public DeviceType DeviceType { get; private set; }

        public DeviceType GetDeviceType()
        {
            // Check if user agent is a smart TV - http://goo.gl/FocDk
            if (Regex.IsMatch(UserAgent, @"GoogleTV|SmartTV|Internet.TV|NetCast|NETTV|AppleTV|boxee|Kylo|Roku|DLNADOC|CE\-HTML",
                              RegexOptions.IgnoreCase))
            {
                return DeviceType.Television;
            }

            // Check if user agent is a TV Based Gaming Console
            if (Regex.IsMatch(UserAgent, "Xbox|PLAYSTATION.3|Wii", RegexOptions.IgnoreCase))
            {
                return DeviceType.Television;
            }

            // Check if user agent is a Tablet
            if ((Regex.IsMatch(UserAgent, "iP(a|ro)d", RegexOptions.IgnoreCase) ||
                 (Regex.IsMatch(UserAgent, "tablet", RegexOptions.IgnoreCase)) &&
                 (!Regex.IsMatch(UserAgent, "RX-34", RegexOptions.IgnoreCase)) ||
                 (Regex.IsMatch(UserAgent, "FOLIO", RegexOptions.IgnoreCase))))
            {
                return DeviceType.Tablet;
            }

            // Check if user agent is an Android Tablet
            if ((Regex.IsMatch(UserAgent, "Linux", RegexOptions.IgnoreCase)) &&
                (Regex.IsMatch(UserAgent, "Android", RegexOptions.IgnoreCase)) &&
                (!Regex.IsMatch(UserAgent, "Fennec|mobi|HTC.Magic|HTCX06HT|Nexus.One|SC-02B|fone.945",
                                RegexOptions.IgnoreCase)))
            {
                return DeviceType.Tablet;
            }

            // Check if user agent is a Kindle or Kindle Fire
            if ((Regex.IsMatch(UserAgent, "Kindle", RegexOptions.IgnoreCase)) ||
                (Regex.IsMatch(UserAgent, "Mac.OS", RegexOptions.IgnoreCase)) &&
                (Regex.IsMatch(UserAgent, "Silk", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Tablet;
            }

            // Check if user agent is a pre Android 3.0 Tablet
            if (
                (Regex.IsMatch(UserAgent,
                               @"GT-P10|SC-01C|SHW-M180S|SGH-T849|SCH-I800|SHW-M180L|SPH-P100|SGH-I987|zt180|HTC(.Flyer|\\_Flyer)|Sprint.ATP51|ViewPad7|pandigital(sprnova|nova)|Ideos.S7|Dell.Streak.7|Advent.Vega|A101IT|A70BHT|MID7015|Next2|nook",
                               RegexOptions.IgnoreCase)) ||
                (Regex.IsMatch(UserAgent, "MB511", RegexOptions.IgnoreCase)) &&
                (Regex.IsMatch(UserAgent, "RUTEM", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Tablet;
            }

            if (IsMobileDevice())
            {
                return DeviceType.Mobile;
            }

            // Check if user agent is unique Mobile User Agent
            if (
                (Regex.IsMatch(UserAgent,
                               "BOLT|Fennec|Iris|Maemo|Minimo|Mobi|mowser|NetFront|Novarra|Prism|RX-34|Skyfire|Tear|XV6875|XV6975|Google.Wireless.Transcoder",
                               RegexOptions.IgnoreCase)))
            {
                return DeviceType.Mobile;
            }

            // Check if user agent is an odd Opera User Agent - http://goo.gl/nK90K
            if ((Regex.IsMatch(UserAgent, "Opera", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(UserAgent, "Windows.NT.5", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(UserAgent, @"HTC|Xda|Mini|Vario|SAMSUNG\-GT\-i8000|SAMSUNG\-SGH\-i9",
                                    RegexOptions.IgnoreCase)))
            {
                return DeviceType.Mobile;
            }

            // Check if user agent is Windows Desktop
            if ((Regex.IsMatch(UserAgent, "Windows.(NT|XP|ME|9)")) &&
                     (!Regex.IsMatch(UserAgent, "Phone", RegexOptions.IgnoreCase)) ||
                     (Regex.IsMatch(UserAgent, "Win(9|.9|NT)", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Desktop;
            }

            // Check if agent is Mac Desktop
            if ((Regex.IsMatch(UserAgent, "Macintosh|PowerPC", RegexOptions.IgnoreCase)) &&
                     (!Regex.IsMatch(UserAgent, "Silk", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Desktop;
            }

            // Check if user agent is a Linux Desktop
            if ((Regex.IsMatch(UserAgent, "Linux", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(UserAgent, "X11", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Desktop;
            }

            // Check if user agent is a Solaris, SunOS, BSD Desktop
            if (
                (Regex.IsMatch(UserAgent, "Solaris|SunOS|BSD", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Desktop;
            }

            // Check if user agent is a Desktop BOT/Crawler/Spider
            if (
                (Regex.IsMatch(UserAgent,
                               "Bot|Crawler|Spider|Yahoo|ia_archiver|Covario-IDS|findlinks|DataparkSearch|larbin|Mediapartners-Google|NG-Search|Snappy|Teoma|Jeeves|TinEye",
                               RegexOptions.IgnoreCase)) &&
                (!Regex.IsMatch(UserAgent, "Mobile", RegexOptions.IgnoreCase)))
            {
                return DeviceType.Desktop;
            }

            // Otherwise assume it is a Unknown Device
            return DeviceType.Unknown;
        }

        private bool IsMobileDevice()
        {
            // Checks the user-agent  
            // Checks if its a Windows browser but not a Windows Mobile browser  
            if (UserAgent.Contains("windows") && !UserAgent.Contains("windows ce"))
            {
                return false;
            }

            // Checks if it is a mobile browser  

            var mc = Regex.Matches(UserAgent, Pattern, RegexOptions.IgnoreCase);
            if (mc.Count > 0)
            {
                return true;
            }

            // Checks if the 4 first chars of the user-agent match any of the most popular user-agents  
            if (PopUserAgent.Contains("|" + UserAgent.Substring(0, 4) + "|"))
            {
                return true;
            }

            // Checks the accept header for wap.wml or wap.xhtml support  
            var accept = _contextBase.Request.ServerVariables["HTTP_ACCEPT"];
            if (accept != null)
            {
                if (accept.Contains("text/vnd.wap.wml") || accept.Contains("application/vnd.wap.xhtml+xml"))
                {
                    return true;
                }
            }

            // Checks if it has any mobile HTTP headers  
            var xWapProfile = _contextBase.Request.ServerVariables["HTTP_X_WAP_PROFILE"];
            var profile = _contextBase.Request.ServerVariables["HTTP_PROFILE"];
            var opera = _contextBase.Request.Headers["HTTP_X_OPERAMINI_PHONE_UA"];

            return xWapProfile != null || profile != null || opera != null;
        }

        private void UpdateSpecificTypes()
        {
            var ipodIndex = "iPod".IndexOf(UserAgent, StringComparison.Ordinal);
            var iphoneIndex = "iPhone".IndexOf(UserAgent, StringComparison.Ordinal);
            var ipadIndex = "iPad".IndexOf(UserAgent, StringComparison.Ordinal);
            var androidIndex = "Android".IndexOf(UserAgent, StringComparison.Ordinal);

            if (iphoneIndex + ipodIndex > -1) IsMobileSafari = true;
            if (ipadIndex > -1) IsTabletSafari = true;
            if (androidIndex > -1) IsMobileAndroid = true;
        }
    }
}
