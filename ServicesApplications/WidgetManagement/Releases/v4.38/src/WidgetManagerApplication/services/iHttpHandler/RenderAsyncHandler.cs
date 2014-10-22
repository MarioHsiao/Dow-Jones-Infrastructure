using System;
using System.Threading;
using System.Web;
using factiva.widgets.ui.syndication;

namespace factiva.widgets.ui.services
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderAsyncHandler : RenderHandler, IHttpAsyncHandler
    {
        #region IHttpAsyncHandler Members

        ///<summary>
        ///Initiates an asynchronous call to the HTTP handler.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.IAsyncResult"></see> that contains information about the status of the process.
        ///</returns>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        ///<param name="extraData">Any extra data needed to process the request. </param>
        ///<param name="cb">The <see cref="T:System.AsyncCallback"></see> to call when the asynchronous method call is complete. If cb is null, the delegate is not called. </param>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            context.Response.Write("<p>Begin IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");
            RenderAsyncOperation asynch = new RenderAsyncOperation(cb, context, extraData);
            asynch.StartAsyncWork();
            return asynch;
        }

        ///<summary>
        ///Provides an asynchronous process End method when the process ends.
        ///</summary>
        ///
        ///<param name="result">An <see cref="T:System.IAsyncResult"></see> that contains information about the status of the process. </param>
        public void EndProcessRequest(IAsyncResult result)
        {
        }

        #endregion
    }
}