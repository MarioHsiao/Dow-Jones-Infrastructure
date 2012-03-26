using System;
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

            this.RequestContext = requestContext;
        }


        protected override void OnInit(EventArgs e)
        {
            ControllerBuilder controllerBuilder = ControllerBuilder.Current;

            string requiredString = this.RequestContext.RouteData.GetRequiredString("controller");
            IControllerFactory factory = controllerBuilder.GetControllerFactory();
            IController controller = factory.CreateController(this.RequestContext, requiredString);
            
            if (controller == null)
                throw new InvalidOperationException("Could not find controller: " + requiredString);

            try
            {
                controller.Execute(this.RequestContext);
            }
            finally
            {
                factory.ReleaseController(controller);
            }

            this.Context.ApplicationInstance.CompleteRequest();
        }

        public override void ProcessRequest(HttpContext httpContext)
        {
            throw new NotSupportedException("This should not get called for an STA");
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return this.AspCompatBeginProcessRequest(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            this.AspCompatEndProcessRequest(result);
        }


        void IHttpHandler.ProcessRequest(HttpContext httpContext)
        {
            this.ProcessRequest(httpContext);
        }
    }
}