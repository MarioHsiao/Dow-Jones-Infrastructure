using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.syndication.integration.alertWidget;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.syndication.integration
{
    /// <summary>
    /// Code Snippet Manager
    /// </summary>
    internal class CodeSnippetManager
    {
        private const string m_BaseIntegrationURl = "~/syndication/subscriber/InsertWidget.ashx";
        private const string m_BaseRenderUrl = "~/du/render.ashx";
        private const string m_LiveDotComTemplateFileLocation = "~/syndication/integration/templates/livedotcomIntegrationJs.txt";
        
        /// <summary>
        /// Gets the javascipt widget code snippet.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="showTitle">if set to <c>true</c> [show title].</param>
        /// <returns></returns>
        public static string GetBaseJavaScriptWidgetCodeSnippet(string token, WidgetType type, bool showTitle)
        {
            return GetBaseJavaScriptWidgetCodeSnippet(token, type, showTitle, IntegrationTarget.JavaScriptCode);
        }

        /// <summary>
        /// Gets the javascipt widget code snippet.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetBaseBloggerJavaScriptWidgetCodeSnippet(string token, WidgetType type)
        {
            return GetBaseJavaScriptWidgetCodeSnippet(token, type, true, IntegrationTarget.Blogger);
        }

        /// <summary>
        /// Gets the javascipt widget code snippet.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetBaseSharePointJavaScriptWidgetCodeSnippet(string token, WidgetType type)
        {
            return GetBaseJavaScriptWidgetCodeSnippet(token, type, false, IntegrationTarget.SharePointWebPart);
        }

        /// <summary>
        /// Gets the javascipt widget code snippet.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="showTitle">if set to <c>true</c> [show title].</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static string GetBaseJavaScriptWidgetCodeSnippet(string token, WidgetType type, bool showTitle, IntegrationTarget target)
        {
            return string.Format("<script src=\"{0}\" type=\"text/javascript\" charset=\"utf-8\"></script>",
                GetBaseWidgetIntegrationUrl(token, type, showTitle, target));
        }

        public static string GetBaseJavaScriptWidgetCodeSnippet(RenderWidgetDTO renderWidgetDTO)
        {
            return GetBaseJavaScriptWidgetCodeSnippet(renderWidgetDTO.token, renderWidgetDTO.type, renderWidgetDTO.showTitle);
        }

        public static string GetBaseWidgetIntegrationUrl(string token, WidgetType type, bool showTitle, IntegrationTarget target)
        {
            RenderWidgetDTO widgetDTO = new RenderWidgetDTO();
            widgetDTO.token = token;
            widgetDTO.showTitle = showTitle;
            widgetDTO.type = type;
            widgetDTO.integrationTarget = target;   
            return GetBaseWidgetIntegrationUrl(widgetDTO);
        }

        /// <summary>
        /// Gets the base widget integration URL.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetBaseWidgetIntegrationUrl(RenderWidgetDTO renderWidgetDTO)
        {
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = m_BaseIntegrationURl;
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "token"), renderWidgetDTO.token);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "type"), (int)renderWidgetDTO.type);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "showTitle"), renderWidgetDTO.showTitle);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "integrationTarget"), (int)renderWidgetDTO.integrationTarget);
            //urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "randomKey"), renderWidgetDTO.randomKey);
            //urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "language"), renderWidgetDTO.language);
            return urlBuilder.ToString(null);
        }

        /// <summary>
        /// Gets the base widget integration URL.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="responseFormat">The response format.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetSyndicatonUrl(string token, ResponseFormat responseFormat)
        {
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = m_BaseRenderUrl;
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "token"), token);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "type"), (int)  WidgetType.AlertHeadlineWidget);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "responseFormat"), (int)responseFormat);
            return urlBuilder.ToString();
        }


        /// <summary>
        /// Gets the live spaces widget integration URL.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <returns></returns>
        public static string GetLiveSpacesWidgetIntegrationUrl(RenderWidgetDTO renderWidgetDTO)
        {
            EncryptedRenderWidgetDTO encryptedRenderWidgetDTO = new EncryptedRenderWidgetDTO(renderWidgetDTO);
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = m_BaseIntegrationURl;
            urlBuilder.Append(encryptedRenderWidgetDTO.compositeToken);
            return urlBuilder.ToString();
        }



        /// <summary>
        /// Gets the generic live spaces javascript class template.
        /// </summary>
        /// <returns>
        /// A Template based <see cref="string"/> with the following fields
        /// {0} - randomKey
        /// {1} - Widget Management JS Library file
        /// {2} - Render Widget Manager
        /// {3} - JSON Script File
        /// </returns>
        public static string GetGenericLiveSpacesJavascriptClassTemplate()
        {
            return TemplateManager.Instance.GetTemplate(typeof(CodeSnippetManager), m_LiveDotComTemplateFileLocation);
        }

    }
}
