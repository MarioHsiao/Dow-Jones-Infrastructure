using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.UI;

namespace DowJones.Web.Mvc.Threading
{
    public class STAMvcHandler : Page, IHttpAsyncHandler, IRequiresSessionState
    {
        public RequestContext RequestContext { get; set; }


        public STAMvcHandler(RequestContext requestContext)
        {
            if (requestContext == null)
                throw new ArgumentNullException("requestContext");

            RequestContext = requestContext;
        }


        protected override void OnInit(EventArgs e)
        {
            var controllerBuilder = ControllerBuilder.Current;
            var requiredString = RequestContext.RouteData.GetRequiredString("controller");
            var factory = controllerBuilder.GetControllerFactory();
            var controller = factory.CreateController(RequestContext, requiredString);
            
            if (controller == null)
            {
                throw new InvalidOperationException("Could not find controller: " + requiredString);
            }

            RemoveOptionalRoutingParameters();

            try
            {
                controller.Execute(RequestContext);
            }
            finally
            {
                factory.ReleaseController(controller);
            }

            Context.ApplicationInstance.CompleteRequest();
        }

        public override void ProcessRequest(HttpContext httpContext)
        {
            throw new NotSupportedException("This should not get called for an STA");
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return AspCompatBeginProcessRequest(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }


        void IHttpHandler.ProcessRequest(HttpContext httpContext)
        {
            ProcessRequest(httpContext);
        }

        private void RemoveOptionalRoutingParameters()
        {
            var rvd = RequestContext.RouteData.Values;

            // Get all keys for which the corresponding value is 'Optional'.
            // ToArray() necessary so that we don't manipulate the dictionary while enumerating.
            var matchingKeys = (from entry in rvd
                                     where entry.Value == UrlParameter.Optional
                                     select entry.Key).ToArray();

            foreach (var key in matchingKeys)
            {
                rvd.Remove(key);
            }
        }

    }
}