using Factiva.BusinessLayerLogic.Utility;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.syndication.subscriber;
using EMG.widgets.ui.utility;
using EMGUtility = EMG.widgets.ui.utility.Utility;
using BaseUrlBuilder = EMG.Utility.Uri.UrlBuilder;

namespace EMG.widgets.ui.syndication.integration.alertWidget
{
    /// <summary>
    /// 
    /// </summary>
    internal class NetvibesModule : AbstractWidgetPortalEndPoint
    {

        private const string m_Title = "addToNetvibes";
        private const string m_IconVirtualPath = "~/img/syndication/icons/netvibes.png";
        private const bool m_HasIconImage = true;
        private const string m_BarImageVirtualPath = "";
        private const bool m_HasBarImage = false;
        private const string m_BaseIntegrationUrl = "http://www.netvibes.com/subscribe.php";
        private const string m_BrandingIcon = "~/img/branding/djicon.png";
        private const string m_TemplateFileLocation = "~/syndication/integration/alertwidget/templates/netvibesmodule.html";
        private const MimeType m_MimeType = utility.MimeType.HTML;
        private RenderWidgetDTO m_RenderWidgetDTO;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetvibesModule"/> class.
        /// </summary>
        /// <param name="dto">The dto.</param>
        public NetvibesModule(RenderWidgetDTO dto)
        {
            m_RenderWidgetDTO = dto;
        }

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
        /// Gets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        public override string MimeType
        {
            get { return EMGUtility.GetMimeType(m_MimeType); }
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
                                 JavascriptResourceManager.GetJsonpDataUrl(m_RenderWidgetDTO,IntegrationTarget.Netvibes),
                                 GetWidgetTitle(m_RenderWidgetDTO,true),
                                 ub);
        }

        /// <summary>
        /// Generates the integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            string gagetUrl = GetGenericIntegrationUrl(m_RenderWidgetDTO.token, m_RenderWidgetDTO.type, IntegrationTarget.Netvibes);
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = m_BaseIntegrationUrl;
            urlBuilder.Append("module", "api");
            urlBuilder.Append("moduleUrl", gagetUrl);
            return urlBuilder.ToString(null);
        }

        private string GetTemplate
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
