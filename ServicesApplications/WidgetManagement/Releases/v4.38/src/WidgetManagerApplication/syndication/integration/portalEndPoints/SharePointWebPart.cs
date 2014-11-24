using EMG.widgets.ui.syndication.integration.alertWidget;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;

namespace EMG.widgets.ui.syndication.integration.portalEndPoints
{
    internal class SharePointWebPart : AbstractWidgetPortalEndPoint
    {
        private const string TITLE = "addToSharePoint";
        private const string ICON_VIRTUAL_PATH = "~/img/syndication/icons/addto.gif";
        private const bool HAS_ICON_IMAGE = true;
        private const string BAR_IMAGE_VIRTUAL_PATH = "";
        private const bool HAS_BAR_IMAGE = false;
        private const string TEMPLATE_FILE_LOCATION = "~/syndication/integration/templates/sharepointwebpart.xml";
        private const MimeType MIME_TYPE = utility.MimeType.WEBPART;
        private RenderWidgetDTO _renderWidgetDTO;


         /// <summary>
        /// Gets or sets the render widget DTO.
        /// </summary>
        /// <value>The render widget DTO.</value>
        public RenderWidgetDTO RenderWidgetDTO
        {
            get { return _renderWidgetDTO; }
            set { _renderWidgetDTO = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointWebPart"/> class.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        public SharePointWebPart(RenderWidgetDTO renderWidgetDTO)
        {
            _renderWidgetDTO = renderWidgetDTO;
        }

        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        public override string MimeType
        {
            get { return utility.Utility.GetMimeType(MIME_TYPE); }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return ResourceText.GetInstance.GetString(TITLE); }
        }

        /// <summary>
        /// Gets the virtual image icon path.
        /// </summary>
        /// <value>The virtual image icon path.</value>
        public override string IconVirtualPath
        {
            get { return ICON_VIRTUAL_PATH; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has icon image.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has icon image; otherwise, <c>false</c>.
        /// </value>
        public override bool HasIconImage
        {
            get { return HAS_ICON_IMAGE; }
        }

        /// <summary>
        /// Gets the virtual bar image path.
        /// </summary>
        /// <value>The virtual bar path.</value>
        public override string BarImageVirtualPath
        {
            get { return BAR_IMAGE_VIRTUAL_PATH; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has bar image.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has bar image; otherwise, <c>false</c>.
        /// </value>
        public override bool HasBarImage
        {
            get { return HAS_BAR_IMAGE; }
        }

        /// <summary>
        /// Gets the integration code.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationCode()
        {
            // Get the widget information 
            return string.Format(GetTemplate,
                                 CodeSnippetManager.GetBaseSharePointJavaScriptWidgetCodeSnippet(_renderWidgetDTO.token, _renderWidgetDTO.type),
                                 ResourceText.GetInstance.GetString("sharepointDesc"),
                                 GetWidgetTitle(_renderWidgetDTO, true));
        }

        /// <summary>
        /// Gets the integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            return GetGenericIntegrationUrl(_renderWidgetDTO.token, _renderWidgetDTO.type, IntegrationTarget.SharePointWebPart);
        }

        public string GetWidgetTitle()
        {
            return GetWidgetTitle(_renderWidgetDTO, false);
        }

        /// <summary>
        /// Gets the gadget template.
        /// </summary>
        /// <returns></returns>
        private string GetTemplate
        {
            // 0 - title
            // 1 - integration javascript code snippet
            get { return TemplateManager.Instance.GetTemplate(GetType(), TEMPLATE_FILE_LOCATION); }
        }
    }
}
