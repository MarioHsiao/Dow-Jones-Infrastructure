using Factiva.BusinessLayerLogic.Utility;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;

namespace EMG.widgets.ui.syndication.integration.alertWidget
{
    /// <summary>
    /// 
    /// </summary>
    internal class LiveSpacesGadget : LiveDotComGadget
    {
        private const string m_BarImageVirtualPath = "";
        private const bool m_HasBarImage = false;
        private const bool m_HasIconImage = true;
        private const string m_IconVirtualPath = "~/img/syndication/icons/LiveSpaces.gif";
        private const MimeType m_MimeType = utility.MimeType.XML;
        private const string m_BaseIntegrationUrl = "http://spaces.live.com/spacesapi.aspx";
        private const string m_Title = "addToLiveSpaces";
        private readonly RenderWidgetDTO m_RenderWidgetDTO;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggerWidget"/> class.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public LiveSpacesGadget(RenderWidgetDTO renderWidgetDTO) : base(renderWidgetDTO)
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
            m_RenderWidgetDTO.showTitle = false;
            m_RenderWidgetDTO.integrationTarget = IntegrationTarget.LiveSpaces;
            m_RenderWidgetDTO.randomKey = RandomKeyGenerator.GetRandomKey(10, RandomKeyGenerator.CharacterSet.Alpha);
           
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
            urlBuilder.Append("wx_action", "create");
            urlBuilder.Append("wx_url", gagetRSSIntegrationUrl);
            return urlBuilder.ToString(null);
        }
    }
}