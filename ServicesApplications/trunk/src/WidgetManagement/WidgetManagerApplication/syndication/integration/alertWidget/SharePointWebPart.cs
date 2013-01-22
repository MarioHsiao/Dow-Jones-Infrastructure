using Factiva.BusinessLayerLogic.Utility;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;

namespace EMG.widgets.ui.syndication.integration.alertWidget
{
    internal class SharePointWebPart : AbstractWidgetPortalEndPoint
    {
        private const string m_Title = "addToSharePoint";
        private const string m_IconVirtualPath = "~/img/syndication/icons/addto.gif";
        private const bool m_HasIconImage = true;
        private const string m_BarImageVirtualPath = "";
        private const bool m_HasBarImage = false;
        private const string m_TemplateFileLocation = "~/syndication/integration/alertwidget/templates/sharepointwebpart.xml";
        private const MimeType m_MimeType = utility.MimeType.WEBPART;
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
        public SharePointWebPart(RenderWidgetDTO renderWidgetDTO)
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
            // Get the widget information 
            return string.Format(GetTemplate,
                                 CodeSnippetManager.GetBaseSharePointJavaScriptWidgetCodeSnippet(m_RenderWidgetDTO.token, WidgetType.AlertHeadlineWidget),
                                 GetWidgetTitle(m_RenderWidgetDTO, true));
        }

        /// <summary>
        /// Gets the integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            return GetGenericIntegrationUrl(m_RenderWidgetDTO.token, m_RenderWidgetDTO.type, IntegrationTarget.SharePointWebPart);
        }

        public string GetWidgetTitle()
        {
            return GetWidgetTitle(m_RenderWidgetDTO, false);
        }

        /// <summary>
        /// Gets the gadget template.
        /// </summary>
        /// <returns></returns>
        private string GetTemplate
        {
            // 0 - title
            // 1 - integration javascript code snippet
            get { return TemplateManager.Instance.GetTemplate(GetType(), m_TemplateFileLocation); }
        }
    }
}
