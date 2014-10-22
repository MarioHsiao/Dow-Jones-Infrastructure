// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertWidget.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the InsertWidget type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using EMG.Utility.Handlers;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;
using factiva.nextgen.ui.formstate;

namespace EMG.widgets.ui.syndication.subscriber
{
    /// <summary>
    /// </summary>
    public class InsertWidget : Page, IHttpAsyncHandler
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

            var handlerType = Basic_HttpHandlerFactory.GetServiceType("EMG.widgets.ui.syndication.subscriber.BaseInsertWidget");

            // if we did not find any send it on to the original ajax script service handler.
            if (handlerType == null)
            {
                return;
            }

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
    public class BaseInsertWidget : IHttpHandler
    {
       #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the<see cref="T:System.Web.IHttpHandler"></see>interface.
        /// </summary>
        /// <param name="context">An<see cref="T:System.Web.HttpContext"></see>object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var formState = new FormState(string.Empty);
            var renderWidgetDTO = (RenderWidgetDTO)formState.Accept(typeof(RenderWidgetDTO), false);
            var sb = new StringBuilder();

            if (renderWidgetDTO != null && !renderWidgetDTO.IsValid())
            {
                var encryptedRenderWidgetRequestDTO = new EncryptedRenderWidgetDTO
                                                          {
                                                              compositeToken = context.Request.QueryString.ToString()
                                                          };
                renderWidgetDTO = encryptedRenderWidgetRequestDTO.Deserialize();
                
                // Deserialization failed, try again with urldecode string
                if (renderWidgetDTO == null)
                {
                    encryptedRenderWidgetRequestDTO.compositeToken = context.Request.QueryString.ToString();
                    renderWidgetDTO = encryptedRenderWidgetRequestDTO.Deserialize();
                } 
            }

            if (renderWidgetDTO != null && renderWidgetDTO.IsValid())
            {
                // Attach the token 
                switch (renderWidgetDTO.integrationTarget)
                {
                    case IntegrationTarget.LiveDotCom:
                        // Access static function.
                        sb.Append(JavascriptResourceManager.GetLiveDotComIntegrationScript(renderWidgetDTO));
                        break;
                    case IntegrationTarget.LiveSpaces:
                        // Access static function.
                        sb.Append(JavascriptResourceManager.GetLiveDotComIntegrationScript(renderWidgetDTO));
                        break;
                    default:
                        // Make sure this does not get cached.
                        sb.Append(JavascriptResourceManager.Instance.GetEmbeddedWidgetManagerSupportScript());
                        sb.Append(JavascriptResourceManager.Instance.GetIntergartionScript(renderWidgetDTO));
                        break;
                }

                context.Response.AppendHeader("Pragma", "no-cache");
                context.Response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, private");
                context.Response.AppendHeader("Expires", "-1");
                context.Response.ContentEncoding = Encoding.ASCII;
                context.Response.ContentType = utility.Utility.GetMimeType(MimeType.JS);
                context.Response.Write(sb.ToString());
                context.Response.End();
            }    
            
            return;
        }
        #endregion
    }
}