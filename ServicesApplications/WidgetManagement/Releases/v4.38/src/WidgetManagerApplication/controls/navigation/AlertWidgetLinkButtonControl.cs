using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.navigation
{
    /// <summary>
    /// This control is used on the widget gallery page to link to the create alert widget page.
    /// </summary>
    public class AlertWidgetLinkButtonControl : BaseControl
    {
        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            HtmlInputButton linkButton = new HtmlInputButton();
            linkButton.Value = string.Format("{0}", ResourceText.GetInstance.GetString("buildOneButton"));
            linkButton.Attributes.Add("class", "majorButton");
            //TODO: chance link reference later
            linkButton.Attributes.Add("onclick", "location.href='default.aspx'");
            Controls.Add(linkButton);

            base.Render(writer);
        }

    }
}