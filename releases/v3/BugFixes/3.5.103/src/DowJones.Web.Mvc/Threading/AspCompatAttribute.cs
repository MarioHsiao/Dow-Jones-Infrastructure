using System;
using DowJones.Web.Mvc.Routing;

namespace DowJones.Web.Mvc.Threading
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AspCompatAttribute : RouteAttribute
    {
        public AspCompatAttribute() : base(null)
        {
        }

        public AspCompatAttribute(string url) : base(url)
        {
        }
    }
}