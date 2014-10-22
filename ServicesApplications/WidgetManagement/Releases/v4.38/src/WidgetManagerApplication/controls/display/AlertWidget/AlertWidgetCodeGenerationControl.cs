using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.display
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AlertWidgetCodeGenerationControl : BaseControl
    {
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertWidgetCodeGenerationControl"/> class.
        /// </summary>
        public AlertWidgetCodeGenerationControl() : base("div")
        {
            ID = "alrtWdgtCodeGenCntrl";
            m_container.ID = string.Concat(ID, "Cnt");
            Attributes.CssStyle.Add("display","none");
            Controls.Add(m_container);
        }

        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        private static Control GetTitleControl()
        {
            HtmlGenericControl displayTitle = new HtmlGenericControl("div");
            displayTitle.Attributes.Add("class", "sectionTitle");
            displayTitle.ID = "codeGenTitle";
            displayTitle.InnerText = string.Format("4. {0}", ResourceText.GetInstance.GetString("codeGeneration"));
            return displayTitle;
        }

        /// <summary>
        /// Gets the directions control.
        /// </summary>
        /// <returns></returns>
        private static Control GetDirectionsControl()
        {
            // Right part of title
            HtmlGenericControl directionsControl = new HtmlGenericControl("div");
            directionsControl.Attributes.Add("class", "directionContainer");
            directionsControl.ID = "codeGenerationDirectionsControl";
            directionsControl.InnerHtml = string.Format("{0}", ResourceText.GetInstance.GetString("displayCodeDirections"));
            directionsControl.Controls.Add(GetCodeGenerationControl());
            return directionsControl;
        }

        private static Control GetCodeGenerationControl()
        {
            // Right part of title
            HtmlGenericControl directionsControl = new HtmlGenericControl("div");
            directionsControl.Attributes.Add("class", "codeGeneContainer");
            directionsControl.ID = "codeGenControl";
            directionsControl.InnerHtml = string.Format("<a id=\"showModalPopupClientButton\" href=\"javascript:void(0)\" onclick=\"getCodeGenerationPopup();return false;\">{0}</a>", ResourceText.GetInstance.GetString("displayCode"));
            return directionsControl;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            // Add the modal Page to the top of the page
            //((BasePage) Page).AjaxContainer.Controls.Add(GetCodeModalPopupControl());
            //((BasePage) Page).AjaxContainer.Controls.Add(GetErrorModalPopupControl());
            base.OnLoad(e);
        }


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            m_container.Controls.Add(GetTitleControl());
            m_container.Controls.Add(GetDirectionsControl());
            
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