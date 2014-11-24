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
    public class AlertWidgetCreateControl : BaseControl
    {
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertWidgetCreateControl"/> class.
        /// </summary>
        public AlertWidgetCreateControl()
            : base("div")
        {
            ID = "alrtWdgtCreateCntrl";
            m_container.ID = string.Concat(ID, "Cnt");
            Attributes.CssStyle.Add("display", "none");
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
            displayTitle.ID = "createTitle";
            displayTitle.InnerText = string.Format("3. {0}", ResourceText.GetInstance.GetString("CreateWidget"));
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
            directionsControl.ID = "createWidgetDirectionControl";
            directionsControl.InnerHtml = string.Format("{0}", ResourceText.GetInstance.GetString("createWidgetDirections"));
            directionsControl.Controls.Add(GetCreateWidgetControl());
            return directionsControl;
        }

        private static Control GetCreateWidgetControl()
        {
            // Right part of title
            HtmlGenericControl directionsControl = new HtmlGenericControl("div");
            directionsControl.Attributes.Add("class", "buttonCntr");
            directionsControl.ID = "crtWdgtButtonCntrl";
            directionsControl.InnerHtml = string.Format("<a id=\"showModalPopupClientButton\" href=\"javascript:void(0)\" onclick=\"getCreatePopup();return false;\">{0}</a>", ResourceText.GetInstance.GetString("saveWidgetDesignNow"));
            return directionsControl;
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
