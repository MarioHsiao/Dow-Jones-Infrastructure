using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.dto.request;

namespace EMG.widgets.ui.controls.preview
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PreviewControl : AbstractWidgetDesignerControl
    {
        private readonly HtmlGenericControl m_Container = new HtmlGenericControl("div");
        private WidgetManagementDTO m_WidgetManagementDTO; 

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewControl"/> class.
        /// </summary>
        public PreviewControl() : base("div")
        {
            ID = "wdgtPrvwCntrl";
            m_Container.ID = string.Concat(ID, "Cnt");
            Controls.Add(m_Container);
        }

        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return m_WidgetManagementDTO; }
            set { m_WidgetManagementDTO = value; }
        }

        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        private static Control GetTitleControl()
        {
            HtmlGenericControl previewTitle = new HtmlGenericControl("div");
            previewTitle.Attributes.Add("class", "sectionTitle");
            previewTitle.ID = "previewTitle";

            previewTitle.InnerText = string.Format("2. {0}", ResourceText.GetInstance.GetString("preview"));
            return previewTitle;
        }

        /// <summary>
        /// Gets the preview container control.
        /// </summary>
        /// <returns></returns>
        private static Control GetPreviewContainerControl()
        {
            HtmlGenericControl previewContainer = new HtmlGenericControl("div");
            previewContainer.Attributes.Add("class", "previewContainer");
            previewContainer.ID = "previewContainer";
            //previewContainer.InnerHtml = string.Format("<div id=\"previewContents\"><div class=\"loadingDiv\">{0}...</div></div>", ResourceText.GetInstance.GetString("loading"));
            previewContainer.InnerHtml = "<div id=\"previewContents\"></div>";
            return previewContainer;
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
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            m_Container.Controls.Add(GetTitleControl());
            m_Container.Controls.Add(GetPreviewContainerControl());
            base.OnPreRender(e);
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            base.RenderChildren(writer);
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}