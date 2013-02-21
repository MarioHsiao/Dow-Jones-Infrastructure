using EMG.widgets.ui.syndication.integration.alertWidget;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;

namespace EMG.widgets.ui.syndication.integration.portalEndPoints
{
    /// <summary>
    /// 
    /// </summary>
    internal class LiveDotComGadget : AbstractWidgetPortalEndPoint
    {
        private const string m_BarImageVirtualPath = "";
        private const bool m_HasBarImage = false;
        private const bool m_HasIconImage = true;
        private const string m_IconVirtualPath = "~/img/syndication/icons/LiveDotCom.gif";
        private const string m_BaseIntegrationUrl = "http://my.live.com/";
        private const string m_TemplateFileLocation = "~/syndication/integration/templates/livedotcomrss.xml";
        private const MimeType m_MimeType = utility.MimeType.XML;
        private const string m_Title = "addToLiveDotCom";
        private RenderWidgetDTO m_RenderWidgetDTO;


        /// <summary>
        /// Initializes a new instance of the <see cref="LiveDotComGadget"/> class.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public LiveDotComGadget(RenderWidgetDTO renderWidgetDTO)
        {
            m_RenderWidgetDTO = renderWidgetDTO;
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
            return string.Format(RssIntegrationTemplate,
                                 GetWidgetTitle(m_RenderWidgetDTO,true),
                                 m_RenderWidgetDTO.randomKey,
                                 CodeSnippetManager.GetLiveSpacesWidgetIntegrationUrl(m_RenderWidgetDTO)
                );
        }


        /// <summary>
        /// Generates the integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            string gagetRSSIntegrationUrl = GetGenericEncryptedIntegrationUrl(m_RenderWidgetDTO.token, m_RenderWidgetDTO.type, IntegrationTarget.LiveSpaces);
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = m_BaseIntegrationUrl;
            urlBuilder.Append("add", gagetRSSIntegrationUrl);
            return urlBuilder.ToString(null);
        }
        
        /// <summary>
        /// Gets the RSS integration template.
        /// </summary>
        /// <value>The RSS integration template.</value>
        public string RssIntegrationTemplate
        {
            // 0 - title
            // 1 - random key
            // 2 - script integration url

            get
            {
                return TemplateManager.Instance.GetTemplate(GetType(),m_TemplateFileLocation) ;
            }
        }
    }
}