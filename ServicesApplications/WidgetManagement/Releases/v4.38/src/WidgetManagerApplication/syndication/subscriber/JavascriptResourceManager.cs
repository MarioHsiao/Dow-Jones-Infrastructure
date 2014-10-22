// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JavascriptResourceManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.syndication.integration;
using EMG.widgets.ui.syndication.integration.alertWidget;
using Factiva.BusinessLayerLogic.Utility;
using factiva.nextgen.ui;

#if (DEBUG)

[assembly: WebResource("EMG.widgets.ui.syndication.subscriber.widgetManager_orig.js", "text/javascript")]
#else 
    [assembly: WebResource("EMG.widgets.ui.syndication.subscriber.widgetManager.js", "text/javascript")]
#endif

namespace EMG.widgets.ui.syndication.subscriber
{
    /// <summary>
    /// The javascript resource manager.
    /// </summary>
    internal sealed class JavascriptResourceManager
    {
        /// <summary>
        /// The m_ base java script integration template.
        /// </summary>
        private const string m_BaseJavaScriptIntegrationTemplate = "~/syndication/integration/templates/BaseJavaScript.txt";

        /// <summary>
        /// The instance.
        /// </summary>
        private static readonly JavascriptResourceManager instance = new JavascriptResourceManager();

        /// <summary>
        /// The m_ resource_ render widget manager.
        /// </summary>
        private readonly string m_Resource_RenderWidgetManager = string.Empty;

        /// <summary>
        /// The m_ resource_ widget manager.
        /// </summary>
        private readonly string m_Resource_WidgetManager = string.Empty;

        /// <summary>
        /// The m_ embedded resource script source.
        /// </summary>
        private string m_EmbeddedResourceScriptSource = string.Empty;

        /// <summary>
        /// The m_ embedded resource url.
        /// </summary>
        private string m_EmbeddedResourceUrl = string.Empty;

        /// <summary>
        /// Prevents a default instance of the <see cref="JavascriptResourceManager"/> class from being created.
        /// </summary>
        private JavascriptResourceManager()
        {
            if (!string.IsNullOrEmpty(m_Resource_WidgetManager) || !string.IsNullOrEmpty(m_Resource_RenderWidgetManager))
            {
                return;
            }

#if (DEBUG)
            m_Resource_WidgetManager = "widgetManager_orig.js";

////m_Resource_RenderWidgetManager = "~/syndication/subscriber/factivaWidgetRenderManager_orig.js";
            m_Resource_RenderWidgetManager = "~/du/r.ashx?d=syndication%2Fsubscriber&n=factivaWidgetRenderManager_orig&t=js&v=" + utility.Utility.GetVersion();
#else
            m_Resource_WidgetManager = "widgetManager.js";
            m_Resource_RenderWidgetManager = "~/du/r.ashx?d=syndication%2Fsubscriber&n=factivaWidgetRenderManager&t=js&v=" + utility.Utility.GetVersion();
#endif
        }

        /// <summary>
        /// Gets Resource_RenderWidgetManager.
        /// </summary>
        public string Resource_RenderWidgetManager
        {
            get { return m_Resource_RenderWidgetManager; }
        }

        /// <summary>
        /// Gets Resource_WidgetManager.
        /// </summary>
        public string Resource_WidgetManager
        {
            get { return m_Resource_WidgetManager; }
        }

        /// <summary>
        /// Gets ScriptSource.
        /// </summary>
        public string ScriptSource
        {
            get { return m_EmbeddedResourceScriptSource; }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static JavascriptResourceManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the embedded widget manager support script.
        /// </summary>
        /// <returns>
        /// The get embedded widget manager support script.
        /// </returns>
        public string GetEmbeddedWidgetManagerSupportScript()
        {
            return GetEmbeddedScript(Resource_WidgetManager);
        }

        /// <summary>
        /// Gets the embedded widget manager support script.
        /// </summary>
        /// <returns>
        /// The get embedded widget manager script url.
        /// </returns>
        public string GetEmbeddedWidgetManagerScriptUrl()
        {
            return GetEmbeddedScriptUrl(Resource_WidgetManager);
        }

        /// <summary>
        /// Gets the intergartion script.
        /// </summary>
        /// <param name="renderWidgetDTO">
        /// The render widget DTO.
        /// </param>
        /// <returns>
        /// The get intergartion script.
        /// </returns>
        public string GetIntergartionScript(RenderWidgetDTO renderWidgetDTO)
        {
            return GetBaseScriptFunction(renderWidgetDTO);
        }


        /// <summary>
        /// Gets the base render widget library.
        /// </summary>
        /// <returns>
        /// The get render widget manager script url.
        /// </returns>
        public string GetRenderWidgetManagerScriptUrl()
        {
            return GetBaseScriptUrl(Resource_RenderWidgetManager);
        }

        /// <summary>
        /// Gets the base render widget URL.
        /// </summary>
        /// <param name="renderWidgetDTO">
        /// The render widget DTO.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The get jsonp data url.
        /// </returns>
        public static string GetJsonpDataUrl(RenderWidgetDTO renderWidgetDTO, IntegrationTarget target)
        {
            return GetWidgetDataJsonpUrl(renderWidgetDTO, target);
        }

        /// <summary>
        /// Gets the embedded support script.
        /// </summary>
        /// <param name="resourceName">
        /// Name of the resource.
        /// </param>
        /// <returns>
        /// The get embedded script.
        /// </returns>
        private string GetEmbeddedScript(string resourceName)
        {
            if (string.IsNullOrEmpty(m_EmbeddedResourceScriptSource))
            {
                lock (m_EmbeddedResourceScriptSource)
                {
                    var type = typeof (InsertWidget);
                    using (var stream = type.Assembly.GetManifestResourceStream(type, resourceName))
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                m_EmbeddedResourceScriptSource = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return m_EmbeddedResourceScriptSource;
        }

        /// <summary>
        /// Gets the embedded support script.
        /// </summary>
        /// <param name="resourceName">
        /// Name of the resource.
        /// </param>
        /// <returns>
        /// The get embedded script url.
        /// </returns>
        private string GetEmbeddedScriptUrl(string resourceName)
        {
            if (string.IsNullOrEmpty(m_EmbeddedResourceUrl))
            {
                lock (m_EmbeddedResourceUrl)
                {
                    var type = typeof (InsertWidget);
                    var pageHandler = new Page();
                    m_EmbeddedResourceUrl = pageHandler.ClientScript.GetWebResourceUrl(type, string.Concat("EMG.widgets.ui.syndication.subscriber.", resourceName));
                }
            }

            return string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host, m_EmbeddedResourceUrl);
        }

        /// <summary>
        /// Gets the base script function.
        /// </summary>
        /// <param name="renderWidgetDTO">
        /// The render widget DTO.
        /// </param>
        /// <returns>
        /// The get base script function.
        /// </returns>
        private string GetBaseScriptFunction(RenderWidgetDTO renderWidgetDTO)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var key = RandomKeyGenerator.GetRandomKey(16, RandomKeyGenerator.CharacterSet.Alpha);
            return string.Format(
                                 GetBaseJavascriptClassTemplate(), 
                                 key, 
                                 GetBaseScriptUrl(Resource_RenderWidgetManager).Replace("\"", "&quot;"), 
                                 GetWidgetDataJsonpUrl(renderWidgetDTO, renderWidgetDTO.integrationTarget).Replace("\"", "&quot;"), 
                                 version);
        }

        /// <summary>
        /// Gets the render URL.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <param name="target">The target.</param>
        /// <returns>A url with to the Widget Data</returns>
        private static string GetWidgetDataJsonpUrl(RenderWidgetDTO renderWidgetDTO, IntegrationTarget target)
        {
            if (target == IntegrationTarget.UnSpecified)
            {
                target = renderWidgetDTO.integrationTarget;
            }

            var callBackFunction = "FACTIVA_WIDGET_MGR.processAlertWidget";
            switch (target)
            {
                case IntegrationTarget.IGoogle:
                case IntegrationTarget.Netvibes:
                case IntegrationTarget.PageFlakes:
                case IntegrationTarget.LiveDotCom:
                case IntegrationTarget.LiveSpaces:
                case IntegrationTarget.SharePointWebPart:
                    switch (renderWidgetDTO.type)
                    {
                        case WidgetType.AlertHeadlineWidget:
                            callBackFunction = "FACTIVA_WIDGET_MGR.processAlertHeadlineWidgetNoTitle";
                            break;
                        case WidgetType.AutomaticWorkspaceWidget:
                            callBackFunction = "FACTIVA_WIDGET_MGR.processAutomaticWorkspaceWidgetNoTitle";
                            break;
                        case WidgetType.ManualNewsletterWorkspaceWidget:
                            callBackFunction = "FACTIVA_WIDGET_MGR.processManualNewsletterWorkspaceWidgetNoTitle";
                            break;
                    }

                    break;
                default:
                    switch (renderWidgetDTO.type)
                    {
                        case WidgetType.AlertHeadlineWidget:
                            callBackFunction = "FACTIVA_WIDGET_MGR.processAlertHeadlineWidget";
                            break;
                        case WidgetType.AutomaticWorkspaceWidget:
                            callBackFunction = "FACTIVA_WIDGET_MGR.processAutomaticWorkspaceWidget";
                            break;
                        case WidgetType.ManualNewsletterWorkspaceWidget:
                            callBackFunction = "FACTIVA_WIDGET_MGR.processManualNewsletterWorkspaceWidget";
                            break;
                    }

                    break;
            }

            var urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = "~/du/render.aspx";
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "token"), renderWidgetDTO.token);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "type"), (int) renderWidgetDTO.type);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "language"), renderWidgetDTO.language);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "callBackFunction"), callBackFunction);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "callBackParam"), string.Empty);
            return urlBuilder.ToString(null);
        }

        /// <summary>
        /// Gets the render URL.
        /// </summary>
        /// <param name="renderWidgetDTO">
        /// The render widget DTO.
        /// </param>
        /// <returns>
        /// The get widget data json url.
        /// </returns>
        public static string GetWidgetDataJsonUrl(RenderWidgetDTO renderWidgetDTO)
        {
            var urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = "~/du/render.aspx";
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "token"), renderWidgetDTO.token);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "type"), (int) renderWidgetDTO.type);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "language"), renderWidgetDTO.language);
            return urlBuilder.ToString(null);
        }

        /// <summary>
        /// The get base script url.
        /// </summary>
        /// <param name="resourcePath">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The get base script url.
        /// </returns>
        private static string GetBaseScriptUrl(string resourcePath)
        {
            var urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = resourcePath;
            var t = urlBuilder.ToString(null);
            return t.Replace("??", "?");
        }

        /// <summary>
        /// The get live dot com integration script.
        /// </summary>
        /// <param name="renderWidgetDTO">
        /// The render widget dto.
        /// </param>
        /// <returns>
        /// The get live dot com integration script.
        /// </returns>
        public static string GetLiveDotComIntegrationScript(RenderWidgetDTO renderWidgetDTO)
        {
            return string.Format(
                                 CodeSnippetManager.GetGenericLiveSpacesJavascriptClassTemplate(), 
                                 renderWidgetDTO.randomKey, 
                                 Instance.GetEmbeddedWidgetManagerScriptUrl(), 
                                 Instance.GetRenderWidgetManagerScriptUrl(), 
                                 GetJsonpDataUrl(renderWidgetDTO, IntegrationTarget.LiveSpaces));
        }

        /// <summary>
        /// The get base javascript class template.
        /// </summary>
        /// <returns>
        /// A string with the base javascript class template.
        /// </returns>
        private string GetBaseJavascriptClassTemplate()
        {
            return TemplateManager.Instance.GetTemplate(GetType(), m_BaseJavaScriptIntegrationTemplate);
        }
    }
}