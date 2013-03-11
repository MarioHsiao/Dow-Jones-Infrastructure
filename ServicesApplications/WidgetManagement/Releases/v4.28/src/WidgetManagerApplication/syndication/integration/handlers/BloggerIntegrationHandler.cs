using System;
using System.Text;
using System.Web;
using System.Web.UI;
using EMG.Utility.Handlers;
using EMG.widgets.ui.syndication.integration.portalEndPoints;
using factiva.nextgen.ui.formstate;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.syndication.integration.alertWidget;
using EMG.widgets.ui.syndication.subscriber;
using EMG.widgets.ui.utility;

namespace EMG.widgets.ui.syndication.integration.handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class BloggerIntegrationHandler : Page, IHttpAsyncHandler
    {
        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

            var handlerType = typeof(BaseBloggerIntegrationHandler);
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
    /// 
    /// </summary>
    public class BaseBloggerIntegrationHandler : IHttpHandler
    {
        private const bool m_IsReuseable = true;
        private const string m_Favicon = "http://preview.factiva.com/m_Favicon.ico";
        private const string m_TemplateFileLocation = "~/syndication/integration/templates/bloggerintegration.xml";

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            var formState = new FormState(string.Empty);
            var renderWidgetDTO = (RenderWidgetDTO)formState.Accept(typeof(RenderWidgetDTO), false);
            var currentHost = (context.Request.UrlReferrer != null) ? context.Request.UrlReferrer.Host : context.Request.Url.Host;
            
            if (renderWidgetDTO.IsValid())
            {
                renderWidgetDTO.showTitle = true;
                var bloggerWidget = new BloggerWidget(renderWidgetDTO);
                context.Response.ContentType = utility.Utility.GetMimeType(MimeType.HTML);
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write(string.Format(HTMLResponseTemplate, BaseGetIntegrationCode.GetWidgetTitle(renderWidgetDTO, true), bloggerWidget.GetIntegrationCode(), m_Favicon));
                context.Response.End();
            }
            else throw new HttpException(400, "Invalid Request");
            return;
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
            get { return m_IsReuseable; }
        }


        /// <summary>
        /// Gets the HTML response template.
        /// </summary>
        /// <value>The HTML response template.</value>
        private string HTMLResponseTemplate
        {
            // 0 - Title
            // 1 - Integration Script Code
            // 2 - logo Url -- http://preview.factiva.com/m_Favicon.ico
            get
            {
                return TemplateManager.Instance.GetTemplate(GetType(), m_TemplateFileLocation);
            }
        }
    }
}
