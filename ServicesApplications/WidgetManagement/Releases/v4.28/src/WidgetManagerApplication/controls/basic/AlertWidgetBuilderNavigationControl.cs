using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.dto.request;
using WidgetType=EMG.widgets.ui.dto.WidgetType;

namespace EMG.widgets.ui.controls.basic
{
    /// <summary>
    /// 
    /// </summary>
    public class AlertWidgetBuilderNavigationControl : BaseControl
    {
        /// <summary>
        /// 
        /// </summary>
        private WidgetManagementDTO widgetManagementDTO = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertWidgetBuilderNavigationControl"/> class.
        /// </summary>
        public AlertWidgetBuilderNavigationControl() : base("div")
        {
            ID = "alertWidgetBuilderNav";
        }

        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return widgetManagementDTO; }
            set { widgetManagementDTO = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            // Add the return url to navigation panel

            HtmlAnchor returnUrlAnchor = new HtmlAnchor();
            returnUrlAnchor.ID = "returnUrlAnchor";
            returnUrlAnchor.HRef = widgetManagementDTO.doneUrl;
            returnUrlAnchor.InnerText = ResourceText.GetInstance.GetString("returnUrlAnchorText");
            //returnUrlAnchor.Attributes.Add("onclick", "xPost(this.href);return false;");
            Controls.Add(returnUrlAnchor);
            base.RenderChildren(writer);
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (widgetManagementDTO != null &&
                widgetManagementDTO.widgetType == WidgetType.AlertHeadlineWidget &&
                widgetManagementDTO.IsValid())
            {
                base.Render(writer);
            }
        }
    }
}