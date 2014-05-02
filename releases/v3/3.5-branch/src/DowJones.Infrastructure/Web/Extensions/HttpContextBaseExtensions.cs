using System;
using System.Web;
using System.Web.Routing;

namespace DowJones.Extensions.Web
{
    /// <summary>
    /// Contains extension methods of <see cref="HttpContextBase"/>.
    /// </summary>
    public static class HttpContextBaseExtensions
    {
        public static HttpContextBase ToHttpContextBase(this HttpContext context)
        {
            return new HttpContextWrapper(context ?? HttpContext.Current);
        }

        public static bool DebugEnabled(this HttpContextBase context, bool value)
        {
            context = context ?? new HttpContextWrapper(HttpContext.Current);

            context.Items["Debug"] = value;

            return DebugEnabled(context);
        }

        public static bool DebugEnabled(this HttpContextBase context)
        {
            context = context ?? new HttpContextWrapper(HttpContext.Current);

            bool? debug = context.Items["Debug"] as bool?;

            // Then check the request
            if (debug == null && !string.IsNullOrWhiteSpace(context.Request["debug"]))
            {
                debug = "true".Equals(context.Request["debug"], StringComparison.OrdinalIgnoreCase);
                context.Items["Debug"] = debug;
            }

            // Return the value or fall back to the ASP.NET debug mode value
            return debug.GetValueOrDefault(context.IsDebuggingEnabled);
        }

        /// <summary>
        /// Requests the context.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static RequestContext RequestContext(this HttpContextBase instance)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(instance) ?? new RouteData();
            RequestContext requestContext = new RequestContext(instance, routeData);

            return requestContext;
        }

        /// <summary>
        /// Gets a value indicating whether we're running under Mono.
        /// </summary>
        /// <value><c>true</c> if Mono; otherwise, <c>false</c>.</value>
        public static bool IsMono(this HttpContextBase instance)
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        /// <summary>
        /// Gets a value indicating whether we're running under Linux or a Unix variant.
        /// </summary>
        /// <value><c>true</c> if Linux/Unix; otherwise, <c>false</c>.</value>
        public static bool IsLinux(this HttpContextBase instance)
        {
            int p = (int)Environment.OSVersion.Platform;
            return ((p == 4) || (p == 128));
        }

        public static string GetExternalUrl(this HttpContextBase context, string url)
        {
            var request = context.Request;

            string rootUrl =
                string.Format(@"{0}://{1}",
                              request.Url.Scheme,
                              request.Url.Authority
                    );

            var externalUrl = rootUrl + VirtualPathUtility.ToAbsolute(url);
            
            return externalUrl;
        }
    }
}