using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.controls.display
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateControl : AbstractWidgetDesignerControl
    {
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateControl"/> class.
        /// </summary>
        public UpdateControl() : base("div", "4")
        {
            ID = "wdgtUpdateCntrl";
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
            directionsControl.ID = "updateWidgetDirectionControl";
            directionsControl.InnerHtml = string.Format("{0}", ResourceText.GetInstance.GetString("updateWidgetDesignDirections"));
            directionsControl.Controls.Add(GetUpdateWidgetControl());
            return directionsControl;
        }

        private static Control GetUpdateWidgetControl()
        {
            // Right part of title
            HtmlGenericControl directionsControl = new HtmlGenericControl("div");
            directionsControl.Attributes.Add("class", "buttonCntr");
            directionsControl.ID = "updtWdgtButtonCntrl";
            directionsControl.InnerHtml = string.Format("<a id=\"showModalPopupClientButton\" class=\"button\" href=\"javascript:void(0)\" onclick=\"getUpdatePopup();return false;\"><span>{0}</span></a>", ResourceText.GetInstance.GetString("updateWidgetDesign"));
            //directionsControl.InnerHtml = string.Format("<span class=\"button\"><input id=\"showModalPopupClientButton\" type=\"button\" onclick=\"getUpdatePopup();return false;\" value=\"{0}\" /></span>", ResourceText.GetInstance.GetString("updateWidgetDesign"));
            return directionsControl;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            m_container.Controls.Add(GetTitleControl(ResourceText.GetInstance.GetString("updateWidgetDesign")));
            m_container.Controls.Add(GetDirectionsControl());
            base.OnPreRender(e);
        }
    }
}
