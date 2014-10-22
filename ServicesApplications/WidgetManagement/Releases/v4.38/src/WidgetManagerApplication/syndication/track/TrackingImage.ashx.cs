using System;
using System.Web;
using factiva.nextgen.ui.formstate;
using EMG.widgets.ui.dto.request;

namespace EMG.widgets.ui.syndication.track
{
    /// <summary>
    /// 
    /// </summary>
    public class TrackingImage : IHttpHandler
    {
        private readonly bool isReusable = false;

        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            FormState formState = new FormState(string.Empty);
            RenderWidgetDTO renderWidgetDTO = (RenderWidgetDTO) formState.Accept(typeof (RenderWidgetDTO), false);
            String currentHost = (context.Request.UrlReferrer != null) ? context.Request.UrlReferrer.Host : context.Request.Url.Host;

            // Make sure this does not get cached.
            context.Response.AppendHeader("Pragma", "no-cache");
            context.Response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, private");
            context.Response.AppendHeader("Expires", "-1");
        }

        ///<summary>
        ///Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.
        ///</returns>
        ///
        public bool IsReusable
        {
            get { return isReusable; }
        }

        #endregion
    }
}