using System;

namespace DowJones.Web.Mvc.Routing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        public string Constraints { get; set; }

        public string Defaults { get; set; }

        public string Url { get; set; }


        public RouteAttribute(string url)
        {
            Url = url;
        }
    }
}