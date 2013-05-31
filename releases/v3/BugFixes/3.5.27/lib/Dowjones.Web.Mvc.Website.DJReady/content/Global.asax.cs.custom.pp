using System.Web.Mvc;
using System;
using System.Web;
using System.Web.Routing;
using $rootnamespace$.Controllers;

namespace $rootnamespace$
{
    public partial class MvcApplication : DowJones.Web.Mvc.HttpApplication
    {
        protected override void Application_Error()
        {

            if (HttpContext.Current.Request.Url.AbsolutePath.IndexOf(".aspx") > 0)
            {
                return;
            }

            Exception exception = Server.GetLastError();

            HttpContext.Current.Response.Clear();

            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                return;
            }
            else //It's an Http Exception, Let's handle it.
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        routeData.Values.Add("action", "NotFound404");
                        break;
                    case 500:
                        // Server error.
                        routeData.Values.Add("action", "InternalServer500");
                        break;
                    default:
                        routeData.Values.Add("action", "Generic");
                        break;
                }
            }

            Server.ClearError();

            // Avoid IIS7 Custom Error
            Response.TrySkipIisCustomErrors = true;

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }
    }
}