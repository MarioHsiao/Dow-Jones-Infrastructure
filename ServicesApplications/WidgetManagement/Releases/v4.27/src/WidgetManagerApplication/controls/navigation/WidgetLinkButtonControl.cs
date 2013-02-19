using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.navigation
{
    /// <summary>
    /// This control is used on the widget gallery page.
    /// </summary>
    public class WidgetLinkButtonControl : BaseControl
    {
        private string m_HRef;

        /// <summary>
        /// Gets or sets the H ref.
        /// </summary>
        /// <value>The H ref.</value>
        public string HRef
        {
            get { return m_HRef; }
            set { m_HRef = value; }
        }

        private string m_altTag;

        /// <summary>
        /// Gets or sets the alt tag.
        /// </summary>
        /// <value>The alt tag.</value>
        public string AltTag
        {
            get { return m_altTag; }
            set { m_altTag = value; }
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            HtmlAnchor linkButton = new HtmlAnchor();
            linkButton.InnerHtml = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("buildOneButton"));
            linkButton.Attributes.Add("class", "button");
            linkButton.HRef = HRef;
            linkButton.Title = AltTag;
            Controls.Add(linkButton);

            base.Render(writer);
        }

    }
}