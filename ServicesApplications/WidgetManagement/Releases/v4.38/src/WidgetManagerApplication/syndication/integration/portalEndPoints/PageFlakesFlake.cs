using EMG.widgets.ui.syndication.integration.alertWidget;
using Factiva.BusinessLayerLogic.Utility;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.syndication.subscriber;
using EMG.widgets.ui.utility;
using BaseUrlBuilder = EMG.Utility.Uri.UrlBuilder;

namespace EMG.widgets.ui.syndication.integration.portalEndPoints
{
    /// <summary>
    /// 
    /// </summary>
    internal class PageFlakesFlake : AbstractWidgetPortalEndPoint
    {
        private const string m_Title = "addToPageFlakes";
        private const string m_IconVirtualPath = "~/img/syndication/icons/pageflakes.png";
        private const bool m_HasIconImage = true;
        private const string m_BarImageVirtualPath = "~/img/syndication/bar/plus_pageflakes.gif";
        private const bool m_HasBarImage = true;
        private const string m_BaseIntegrationUrl = "http://www.pageflakes.com/addFlake.aspx";
        private const string m_BrandingIcon = "~/img/branding/djicon.gif";
        private const string m_TemplateFileLocation = "~/syndication/integration/templates/pageflakesflake.html";
        private const MimeType m_MimeType = utility.MimeType.HTML;
        private RenderWidgetDTO m_RenderWidgetDTO;

        /// <summary>
        /// Gets or sets the render widget DTO.
        /// </summary>
        /// <value>The render widget DTO.</value>
        public RenderWidgetDTO RenderWidgetDTO
        {
            get { return m_RenderWidgetDTO; }
            set { m_RenderWidgetDTO = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageFlakesFlake"/> class.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public PageFlakesFlake(RenderWidgetDTO renderWidgetDTO)
        {
            m_RenderWidgetDTO = renderWidgetDTO;
        }

        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        public override string MimeType
        {
            get { return utility.Utility.GetMimeType(m_MimeType); }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return ResourceText.GetInstance.GetString(m_Title); }
        }

        /// <summary>
        /// Gets the virtual image icon path.
        /// </summary>
        /// <value>The virtual image icon path.</value>
        public override string IconVirtualPath
        {
            get { return m_IconVirtualPath; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has icon image.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has icon image; otherwise, <c>false</c>.
        /// </value>
        public override bool HasIconImage
        {
            get { return m_HasIconImage; }
        }

        /// <summary>
        /// Gets the virtual bar image path.
        /// </summary>
        /// <value>The virtual bar path.</value>
        public override string BarImageVirtualPath
        {
            get { return m_BarImageVirtualPath; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has bar image.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has bar image; otherwise, <c>false</c>.
        /// </value>
        public override bool HasBarImage
        {
            get { return m_HasBarImage; }
        }


        /// <summary>
        /// Gets the integration code.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationCode()
        {
            BaseUrlBuilder ub = new BaseUrlBuilder();
            ub.BaseUrl = m_BrandingIcon;
            ub.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            m_RenderWidgetDTO.showTitle = false;
            string key = RandomKeyGenerator.GetRandomKey(16, RandomKeyGenerator.CharacterSet.Alpha);
            return string.Format(GetTemplate,
                                 key,
                                 JavascriptResourceManager.Instance.GetEmbeddedWidgetManagerScriptUrl(),
                                 JavascriptResourceManager.Instance.GetRenderWidgetManagerScriptUrl(),
                                 JavascriptResourceManager.GetJsonpDataUrl(m_RenderWidgetDTO,IntegrationTarget.PageFlakes),
                                 GetWidgetTitle(m_RenderWidgetDTO,true),
                                 ub);
        }

        
        /// <summary>
        /// Generates the PageFlakes.com add flake URL.
        /// </summary>tempUrl
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            string gagetUrl = GetGenericIntegrationUrl(m_RenderWidgetDTO.token, m_RenderWidgetDTO.type, IntegrationTarget.PageFlakes);
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = m_BaseIntegrationUrl;
            urlBuilder.Append("title", GetWidgetTitle(m_RenderWidgetDTO,false));
            urlBuilder.Append("url", gagetUrl.Replace("http://",""));
            return urlBuilder.ToString(null);
        }

        /// <summary>
        /// Gets the flake template.
        /// </summary>
        /// <returns></returns>
        protected string GetTemplate
        {
            /*
             * {0} - RandomKey
             * {1} - Widget Management JS Library file
             * {2} - Render Widget Manager
             * {3} - JSON Script File
             * {4} - WidgetTitle
             */

            get { return TemplateManager.Instance.GetTemplate(GetType(), m_TemplateFileLocation); }
        }
    }
}
