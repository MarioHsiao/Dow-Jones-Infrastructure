using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.basic
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderControl : BaseControl
    {
        private bool m_ShowToolbar = true;
        private readonly ToolbarControl m_Toolbar;
        private string m_DoneUrl;
        private bool m_Degrade;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderControl"/> class.
        /// </summary>
        public HeaderControl()
            : base("div", true)
        {
            m_Toolbar = new ToolbarControl();
        }


        /// <summary>
        /// Gets or sets the Done URL.
        /// </summary>
        /// <value>URL</value>
        public string DoneUrl
        {
            get { return m_DoneUrl; }
            set { m_DoneUrl = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show toolbar].
        /// </summary>
        /// <value><c>true</c> if [show toolbar]; otherwise, <c>false</c>.</value>
        public bool ShowToolbar
        {
            get { return m_ShowToolbar; }
            set { m_ShowToolbar = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HeaderControl"/> is degrade.
        /// </summary>
        /// <value><c>true</c> if degrade; otherwise, <c>false</c>.</value>
        public bool Degrade
        {
            get { return m_Degrade; }
            set { m_Degrade = value; }
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (m_Degrade)
            {
                writer.Write("<div class=\"DegradedHeader\">");
                writer.Write("<div id=\"factivaLogo\"><img src=\"../img/factivaw.gif\"/></div>");
                writer.Write("<div id=\"djnrLogo\"><img src=\"../img/djnrw.gif\"/></div>");
                writer.Write("</div>");
                return;
            }

            base.ID = "header";
            base.Render(writer);
        }

        private static HtmlAnchor GetHelpLink()
        {
            HtmlAnchor anchor = new HtmlAnchor();
            anchor.ID = "helpLnk";
            anchor.InnerText = ResourceText.GetInstance.GetString("supportTxt");
            anchor.HRef = QuerystringManager.GetSupportUrl(SessionData.Instance().InterfaceLanguage);
            anchor.Attributes.Add("onclick", "if (typeof(xWinOpen) != 'undefined' ) xWinOpen(this.href);return false;");
            anchor.Attributes.Add("target", "support");
            anchor.Attributes.Add("class", "support");
            return anchor;
        }


        private HtmlAnchor GetReturnLink()
        {
            HtmlAnchor anchor = new HtmlAnchor();
            anchor.ID = "returnLnk";
            anchor.InnerText = ResourceText.GetInstance.GetString("exitWidgetBuilder");
            anchor.HRef = DoneUrl;
            anchor.Attributes.Add("class", "toolbarItem");
            return anchor;
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            switch (_siteUserType)
            {
                case SiteUserType.FactivaWidgetBuilderUser:
                    writer.Write("<div class=\"headerFactivaWidgetBuilderUser\">&nbsp;</div>");
                    break;
                case SiteUserType.FactivaSubscriber:
                    writer.Write("<div class=\"headerFactivaSubscriber\">&nbsp;</div>");
                    break;

                case SiteUserType.FactivaReader:
                    writer.Write("<div class=\"headerFactivaReader\">&nbsp;</div>");
                    break;
                case SiteUserType.Unknown:
                case SiteUserType.UnregisterdUser:
                case SiteUserType.IWorksBasic:
                case SiteUserType.IWorksPlus:
                    writer.Write("<div class=\"headerIWorksBasic\">&nbsp;</div>");
                    break;
                case SiteUserType.IWorksPremium:
                default:
                    writer.Write("<div class=\"headerIWorksPrem\">&nbsp;</div>");
                    break;
            }
            writer.Write("<div id=\"toolbar\">");

            if (!string.IsNullOrEmpty(DoneUrl) && !string.IsNullOrEmpty(DoneUrl.Trim()))
            {
                GetReturnLink().RenderControl(writer);
            }

            if (SessionData.Instance() != null)
            {
                if (_siteUserType >= SiteUserType.FactivaSubscriber)
                {
                    //show the support for the factivasubscriber too...
                    GetHelpLink().RenderControl(writer);

                }
            }
            writer.Write("</div>");
        }
    }
}