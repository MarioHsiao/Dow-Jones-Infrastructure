using System.Web;
using EMG.widgets.ui.dto.request;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.gallery
{
    /// <summary>
    /// 
    /// </summary>
    public class AbstractGalleryControl : BaseControl
    {
        public const string HTTP_CONTEXT_WIDGET_MANAGEMENT_DTO = "WidgetManagementDTO_Identifier";
        protected WidgetManagementDTO m_WidgetManagementDTO;

        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return m_WidgetManagementDTO; }
            set { m_WidgetManagementDTO = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[HTTP_CONTEXT_WIDGET_MANAGEMENT_DTO] != null)
                {
                    m_WidgetManagementDTO = (WidgetManagementDTO)HttpContext.Current.Items[HTTP_CONTEXT_WIDGET_MANAGEMENT_DTO];
                }
            }
            base.OnLoad(e);
        }

        
        
    }
}
