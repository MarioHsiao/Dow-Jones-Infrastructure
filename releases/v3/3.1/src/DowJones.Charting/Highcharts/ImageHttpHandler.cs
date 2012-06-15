using System;
using System.Web.UI;
using DowJones.Web.Handlers.Proxy.Core;

namespace DowJones.Charting.Highcharts
{
  using System.Web;

    /// <summary>
    /// 
    /// </summary>
    public class ImageHttpHandler : Page, IHttpAsyncHandler
    {
        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

            var handlerType = typeof(BaseImageHttpHandler);
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

            handler.ProcessRequest(Context);
            Context.ApplicationInstance.CompleteRequest();
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extradata)
        {
            return AspCompatBeginProcessRequest(context, cb, extradata);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }

    }


  /// <summary>
  /// ASP.NET HttpHandler for exporting Highcharts JS JavaScript charts.
  /// </summary>
  public class BaseImageHttpHandler : IHttpHandler
  {
    /// <summary>
    /// Gets a value indicating whether another request can use the 
    /// IHttpHandler instance.
    /// </summary>
    public bool IsReusable
    {
      get { return true; }
    }

    /// <summary>
    /// Processes HTTP web requests directed to this HttpHandler.
    /// </summary>
    /// <param name="context">An HttpContext object that provides references 
    /// to the intrinsic server objects (for example, Request, Response, 
    /// Session, and Server) used to service HTTP requests.</param>
    public void ProcessRequest(HttpContext context)
    {
        using (new TimedLog("BaseImageHttpHandler\tTotal read and write"))
        {
            // Process the request to export chart.
            ExportChart.ProcessImageRequest(context);
        }
    }
  }
}