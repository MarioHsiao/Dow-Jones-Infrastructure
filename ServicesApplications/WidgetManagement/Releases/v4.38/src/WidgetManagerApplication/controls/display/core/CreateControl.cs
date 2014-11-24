using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.controls.display
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateControl : AbstractWidgetDesignerControl
    {
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateControl"/> class.
        /// </summary>
        public CreateControl() : base("div", "4")
        {
            ID = "wdgtCreateCntrl";
            m_container.ID = string.Concat(ID, "Cnt");
            Attributes.CssStyle.Add("display", "none");
            Controls.Add(m_container);
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
            directionsControl.InnerHtml = string.Format("<a id=\"showModalPopupClientButton\" class=\"button\" href=\"javascript:void(0)\" onclick=\"getCreatePopup();return false;\"><span>{0}</span></a>", ResourceText.GetInstance.GetString("saveWidgetDesignNow"));
            return directionsControl;
        }


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            m_container.Controls.Add(GetTitleControl(ResourceText.GetInstance.GetString("CreateWidget")));
            m_container.Controls.Add(GetDirectionsControl());
            base.OnPreRender(e);
        }
    }
}
