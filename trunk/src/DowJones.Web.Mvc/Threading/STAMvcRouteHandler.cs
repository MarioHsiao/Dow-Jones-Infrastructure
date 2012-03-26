using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Threading
{
    public class STAMvcRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new STAMvcHandler(requestContext);
        }
    }
}