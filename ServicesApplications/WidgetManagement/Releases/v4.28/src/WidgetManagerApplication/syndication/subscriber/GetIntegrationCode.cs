using System;
using System.Text;
using System.Web;
using System.Web.UI;
using EMG.Utility.Handlers;
using EMG.widgets.ui.syndication.integration.portalEndPoints;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;
using factiva.nextgen.ui;
using factiva.nextgen.ui.formstate;
using EMG.widgets.Managers;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.syndication.integration;
using EMG.widgets.ui.utility;
using EMGUtility=EMG.widgets.ui.utility.Utility;

namespace EMG.widgets.ui.syndication.subscriber
{
    /// <summary>
    /// 
    /// </summary>
    public class GetIntegrationCode : Page, IHttpAsyncHandler
    {
        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

            var handlerType = typeof(BaseGetIntegrationCode);
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
    public class BaseGetIntegrationCode : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var formState = new FormState(string.Empty);
            var renderWidgetDTO = (RenderWidgetDTO) formState.Accept(typeof (RenderWidgetDTO), false);

            if (renderWidgetDTO != null && !renderWidgetDTO.IsValid())
            {
                // Note: my.live.com adds the following to the end of the url &nocache=<<someValue>>
                var encryptedRenderWidgetRequestDTO = new EncryptedRenderWidgetDTO
                                                          {
                                                              compositeToken = ProcessQueryString(context.Request.QueryString.ToString())
                                                          };
                renderWidgetDTO = encryptedRenderWidgetRequestDTO.Deserialize();
            }

            if (renderWidgetDTO != null && renderWidgetDTO.IsValid())
            {
                switch (renderWidgetDTO.integrationTarget)
                {
                    case IntegrationTarget.IGoogle:
                        Process(new IGoogleGadget(renderWidgetDTO),context);
                        break;
                    case IntegrationTarget.PageFlakes:
                        Process(new PageFlakesFlake(renderWidgetDTO), context);
                        break;
                    case IntegrationTarget.Netvibes:
                        Process(new NetvibesModule(renderWidgetDTO), context);
                        break;
                    case IntegrationTarget.LiveDotCom:
                        Process(new LiveDotComGadget(renderWidgetDTO), context);
                        break;
                    case IntegrationTarget.LiveSpaces:
                        Process(new LiveSpacesGadget(renderWidgetDTO), context);
                        break;
                    case IntegrationTarget.SharePointWebPart:
                        var part = new SharePointWebPart(renderWidgetDTO);
                        Process(part, context, RemoveInvalidChars(part.GetWidgetTitle()));
                        break;
                    default:

                        // Render out a plain page with base integration code on it.
                        context.Response.Expires = 30;
                        context.Response.ContentType = EMGUtility.GetMimeType(MimeType.HTML);
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.Write(string.Format(GetBasicXHTMLPageTemplate, GetWidgetTitle(renderWidgetDTO,false), CodeSnippetManager.GetBaseJavaScriptWidgetCodeSnippet(renderWidgetDTO.token, renderWidgetDTO.type, renderWidgetDTO.showTitle)));
                        context.Response.End();
                        break;
                }
            }
            else throw new HttpException(400, "Invalid Request: Unable to decrypt token");
            return;
        }

        private static string ProcessQueryString(string querystring)
        {
            if (string.IsNullOrEmpty(querystring))
            {
                return querystring;
            }
            var index = querystring.IndexOf('&');
            return index > 0 ? querystring.Substring(0, index) : querystring;
        }

        /// <summary>
        /// Removes the invalid chars.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private static string RemoveInvalidChars(string filename)
        {
            var invalid = new []{"\\", "/", ":", ";", "*", "?", "\"", "<", ">", "|", "#", "{", "}", "%", "~", "&"};
            foreach(var s in invalid)
            {
                filename = filename.Replace(s, string.Empty);
            }
            return filename;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <param name="escape">if set to <c>true</c> [escape].</param>
        /// <returns></returns>
        public static string GetWidgetTitle(RenderWidgetDTO renderWidgetDTO, bool escape)
        {
            var tokenProperties = new WidgetTokenProperties(renderWidgetDTO.token);
            var userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetRssFeed1LightWeightUser(), tokenProperties.UserId, tokenProperties.NameSpace);

            var widgetManager = new WidgetManager(userControlData, renderWidgetDTO.language);
            var widgetName = ResourceText.GetInstance.GetString("factivaWidgetName");
            try
            {
                var widget = widgetManager.GetCachedWidgetById(tokenProperties.WidgetId);
                if (widget != null)
                {
                    if (widget is AlertWidget)
                    {
                        widgetName = widget.Properties.Name;
                    }
                    if (widget is ManualWorkspaceWidget)
                    {
                        var workspaceManager = new WorkspaceManager(userControlData, renderWidgetDTO.language);
                        var workspaceWidget = widget as ManualWorkspaceWidget;
                        int tWorkspaceId;
                        if (workspaceWidget.Component != null &&
                            workspaceWidget.Component.WorkspacesCollection != null &&
                            workspaceWidget.Component.WorkspacesCollection.Count > 0 &&
                            Int32.TryParse(workspaceWidget.Component.WorkspacesCollection[0].ItemId, out tWorkspaceId))
                        {
                            var workspace =  workspaceManager.GetCachedWorkspaceById(tWorkspaceId);
                            if (workspace != null)
                            {
                                var manualWorkspace = (ManualWorkspace) workspace;
                                widgetName = manualWorkspace.Properties.Name;
                            }
                        }
                       
                    }
                    if (widget is AutomaticWorkspaceWidget)
                    {
                        var workspaceManager = new WorkspaceManager(userControlData, renderWidgetDTO.language);
                        var workspaceWidget = widget as AutomaticWorkspaceWidget;
                        int tWorkspaceId;
                        if (workspaceWidget.Component != null &&
                            workspaceWidget.Component.WorkspacesCollection != null &&
                            workspaceWidget.Component.WorkspacesCollection.Count > 0 &&
                            Int32.TryParse(workspaceWidget.Component.WorkspacesCollection[0].ItemId, out tWorkspaceId))
                        {
                             var workspace =  workspaceManager.GetCachedWorkspaceById(tWorkspaceId);
                             if (workspace != null)
                             {
                                 var automaticWorkspace = (AutomaticWorkspace)workspace;
                                 widgetName = automaticWorkspace.Properties.Name;
                             }
                        }
                    }
                }
            }
            catch (Exception) { }
            return escape ? escapeTitle(widgetName) : widgetName;
        }

        private static string escapeTitle(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str.Trim()))
                return string.Empty;
            return str.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// Processes the specified end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        private static void Process(IWidgetPortalEndPoint endPoint, HttpContext context, string fileName)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = endPoint.MimeType;
            context.Response.Write(endPoint.GetIntegrationCode());
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileName.Trim()))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + ".dwp\"");
            }
            context.Response.End();
        }

        /// <summary>
        /// Processes the specified end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <param name="context">The context.</param>
        private static void Process(IWidgetPortalEndPoint endPoint, HttpContext context)
        {
            Process(endPoint,context,null);
        }


        /// <summary>
        /// Gets a value indicating whether this instance is reusable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is reusable; otherwise, <c>false</c>.
        /// </value>
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the get basic XHTML page template.
        /// </summary>
        /// <value>The get basic XHTML page template.</value>
        protected static string GetBasicXHTMLPageTemplate
        {
            // 0 - page title
            // 1- body of page
            get
            {
                return @"
                    <!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                    <html xmlns=""http://www.w3.org/1999/xhtml"">
                        <head>
	                        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>
	                        <title>{0}</title>
                            <link rel=""shortcut icon"" href=""/favicon.ico"" />
                        </head>
                        <body>{1}</body>
                    </html>
                ".Trim();
            }
        }

        #endregion
    }
}