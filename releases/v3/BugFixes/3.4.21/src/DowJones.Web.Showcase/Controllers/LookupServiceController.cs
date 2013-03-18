using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DowJones.GeoLocation;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class LookupServiceController : ControllerBase
    {
        private readonly LookupService _lookupService;

        public LookupServiceController(LookupService lookupService)
        {
            _lookupService = lookupService;
        }

        public ActionResult Index(string address)
        {
            var ip = address ?? GetIPAddress(Request);
            ViewBag.IpAddress = ip;
            ViewBag.CountryName = _lookupService.GetCountry(ip).Name;
            return View("Index");
        }

        public static string GetIPAddress(HttpRequestBase request)
        {
            var remoteAddr = request.UserHostAddress;
            var forwardedFor = request.ServerVariables["X_FORWARDED_FOR"];
            string ip;

            if (forwardedFor == null)
            {
                ip = remoteAddr;
            }
            else
            {
                ip = forwardedFor;
                if (ip.IndexOf(",", System.StringComparison.Ordinal) > 0)
                {
                    var arIPs = ip.Split(',');

                    foreach (var item in arIPs.Where(item => !IsPrivateIpAddress(item)))
                    {
                        return item;
                    }
                }
            }
            return ip;
        }

        private static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

    }
}
