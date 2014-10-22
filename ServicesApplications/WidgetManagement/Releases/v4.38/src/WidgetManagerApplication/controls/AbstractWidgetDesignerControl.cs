using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls 
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractWidgetDesignerControl : BaseControl
    {
        private string m_SectionIdentifier = "5";

        protected AbstractWidgetDesignerControl(string tagName, bool flush, string m_SectionIdentifier) : base(tagName, flush)
        {
            this.m_SectionIdentifier = m_SectionIdentifier;
        }

        protected AbstractWidgetDesignerControl(string tagName, string m_SectionIdentifier) : base(tagName)
        {
            this.m_SectionIdentifier = m_SectionIdentifier;
        }

        protected AbstractWidgetDesignerControl(string m_SectionIdentifier)
        {
            this.m_SectionIdentifier = m_SectionIdentifier;
        }

        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>The section number.</value>
        public string SectionIdentifier
        {
            get { return m_SectionIdentifier; }
            set { m_SectionIdentifier = value; }
        }


        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        protected Control GetTitleControl(string title)
        {
            HtmlGenericControl displayTitle = new HtmlGenericControl("div");
            displayTitle.Attributes.Add("class", "sectionTitle");
            displayTitle.InnerText = string.Format("{0}. {1}", m_SectionIdentifier, title);
            return displayTitle;
        }

        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="widgetType">Type of the widget.</param>
        /// <returns></returns>
        protected Control GetTitleControl(string title, string widgetType)
        {
            HtmlGenericControl displayTitle = new HtmlGenericControl("div");
            displayTitle.Attributes.Add("class", "sectionTitle");
            displayTitle.InnerText = string.Format("{0}. {1} - {2}", m_SectionIdentifier, title, widgetType);
            return displayTitle;
        }
    }
}
