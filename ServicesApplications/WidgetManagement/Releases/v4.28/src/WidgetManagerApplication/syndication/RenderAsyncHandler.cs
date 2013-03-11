using System;
using System.Web;
using System.Web.UI;

namespace EMG.widgets.ui.syndication
{
    /// <summary>
    /// </summary>
    public class RenderAsyncHandler : Page, IHttpAsyncHandler
    {
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extradata)
        {
            return AspCompatBeginProcessRequest(context, cb, extradata);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }

        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

            var handlerType = typeof(RenderHandler);
            IHttpHandler handler;
            try
            {
                // Create the handler by calling class abc or class xyz.
                handler = (IHttpHandler)Activator.CreateInstance(handlerType, true);
            }
            catch (Exception ex)
            {
                throw new HttpException("Unable to create handler", ex);
            }

            if (handler != null)
            {
                handler.ProcessRequest(Context);
            }

            Context.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>
    /// </summary>
    internal class BaseRenderAsyncHandler : RenderHandler, IHttpAsyncHandler
    {
        #region IHttpAsyncHandler Members

        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="cb">The <see cref="T:System.AsyncCallback"></see> to call when the asynchronous method call is complete. If cb is null, the delegate is not called.</param>
        /// <param name="extraData">Any extra data needed to process the request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"></see> that contains information about the status of the process.
        /// </returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var asynch = new RenderAsyncOperation(cb, context, extraData);
            asynch.StartAsyncWork();
            return asynch;
        }

        /// <summary>
        /// Provides an asynchronous process End method when the process ends.
        /// </summary>
        /// <param name="result">An <see cref="T:System.IAsyncResult"></see> that contains information about the status of the process.</param>
        public void EndProcessRequest(IAsyncResult result)
        {
        }

        #endregion
    }
}