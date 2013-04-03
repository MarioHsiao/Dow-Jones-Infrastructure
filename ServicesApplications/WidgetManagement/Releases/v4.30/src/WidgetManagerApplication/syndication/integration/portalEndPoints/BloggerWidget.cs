using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;
using EMGUtility=EMG.widgets.ui.utility.Utility;

namespace EMG.widgets.ui.syndication.integration.portalEndPoints
{
    /// <summary>
    /// 
    /// </summary>
    internal class BloggerWidget : AbstractWidgetPortalEndPoint
    {
        private const MimeType m_MimeType = utility.MimeType.HTML;
        private const string m_Title = "addToBlogger";
        private const string m_IconVirtualPath = "~/img/syndication/icons/blogger.png";
        private const bool m_HasIconImage = true;
        private const string m_BarImageVirtualPath = "";
        private const bool m_HasBarImage = false;
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
            m_RenderWidgetDTO.showTitle = true;
            return CodeSnippetManager.GetBaseBloggerJavaScriptWidgetCodeSnippet(m_RenderWidgetDTO.token, m_RenderWidgetDTO.type);
        }

        /// <summary>
        /// Generates the integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
           // Note: This integration url points to a local handler that does a http post op to blogger
           // Make sure to render the title
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = "~/syndication/blogger.aspx";
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "token"), m_RenderWidgetDTO.token);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "type"), (int)m_RenderWidgetDTO.type);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "integrationTarget"), (int) IntegrationTarget.Blogger);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof(RenderWidgetDTO), "showTitle"), true);
            return urlBuilder.ToString(null);
          
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggerWidget"/> class.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public BloggerWidget(RenderWidgetDTO renderWidgetDTO)
        {
            m_RenderWidgetDTO = renderWidgetDTO;
        }
    }
}
