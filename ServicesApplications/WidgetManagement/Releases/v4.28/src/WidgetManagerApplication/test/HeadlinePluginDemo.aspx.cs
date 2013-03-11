using System;
using System.IO;
using System.Web.UI;
using EMG.widgets.ui.controls.navigation;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.exception;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.page;
using System.Web.UI.HtmlControls;
using BasePage=EMG.widgets.ui.page.BasePage;

namespace emg.widgets.ui.demo
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [ClientScript("~/test/lib/jquery/jquery-1.3.2.js",1)]
    [ClientScript("~/test/lib/jquery/jquery.extend.js", 2)]
    [ClientScript("~/test/lib/jquery/jquery-ui-1.7.1.custom.min.js", 3)]
    [ClientScript("~/test/lib/jquery/jquery.dj_emg_headlineList.js", 4)]
    [ClientScript("~/test/lib/jquery/bindings.js", 5)]
    [Stylesheet("~/test/css/base.css")]
    public class HeadlinePluginDemo : BasePage
    {
        private HeadlinePluginWidgetRequestDTO _mHeadlinePluginWidgetRequestDTO;
        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public HeadlinePluginWidgetRequestDTO HeadlinePluginWidgetRequestDTO
        {
            get { return _mHeadlinePluginWidgetRequestDTO; }
            set { _mHeadlinePluginWidgetRequestDTO = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinePluginDemo"/> class.
        /// </summary>
        public HeadlinePluginDemo()
        {
            OverrideDefaultPrefix = true;
            // Do not validate let the next page do the work.
            base.ValidateSessionId = true;
        }

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            // Add the base script file
            ScriptManager.Services.Add(new ServiceReference("~/test/services/headlinedelivery.asmx"));

            _mHeadlinePluginWidgetRequestDTO = (HeadlinePluginWidgetRequestDTO)FormState.Accept(typeof(HeadlinePluginWidgetRequestDTO), true);
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;

            _siteHeader.DoneUrl = _mHeadlinePluginWidgetRequestDTO.doneUrl;

            if (!_mHeadlinePluginWidgetRequestDTO.IsValid())
            {
                throw new EmgWidgetsUIException(EmgWidgetsUIException.RETURN_URL_IS_NOT_SPECIFIED);
            }
            Form.Action = QuerystringManager.GetLocalUrl("~/test/headlineplugindemo/default.aspx");

        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_mHeadlinePluginWidgetRequestDTO.IsValid()) return;
            ContentTop.Controls.Add(GetPageTitle());
            ContentLeft.Controls.Add(AddLoadArea("Factiva: SEC","leftTarget"));
            ContentMain.Controls.Add(AddLoadArea("Summary","mainTopTarget"));
            ContentMain.Controls.Add(AddLoadArea("NYT RSS Feed","mainBottomTarget"));

            ContentRight.Controls.Add(AddDropArea("dropTarget"));
            ContentRight.Controls.Add(AddLoadArea("Twitter ATOM Feed","rightTarget"));
            

            // Add the Hidden Inputs Control
            HiddenInputsContainerControl navHiddenInputsContainerControl = new HiddenInputsContainerControl();
            navHiddenInputsContainerControl.SiteUserType = SessionData.SiteUserType;
            navHiddenInputsContainerControl.WidgetManagementDTO = null;
            ContentBottom.Controls.Add(navHiddenInputsContainerControl);
        }

        protected static Control AddLoadArea(string name,string target)
        {
            LiteralControl control = new LiteralControl();
            control.Text = string.Format("<div class=\"box\"><div class=\"boxTop\">" + name + "</div><div class=\"boxBody\"><div id=\"{0}\" class=\"headlineContainer\"></div></div><div class=\"boxBottom\"></div></div>", target, QuerystringManager.GetLocalUrl("~/test/img/loading/25-1.gif"));
            return control;
        }

        protected static Control AddDropArea(string target)
        {
            LiteralControl control = new LiteralControl();
            control.Text = string.Format("<div class=\"box\"><div class=\"boxTop\"></div><div class=\"boxBody\"><div id=\"{0}\" class=\"headlineContainer\"><ul id=\"{0}UL\" class=\"DropContainer\"></ul></div></div><div class=\"boxBottom\"></div></div>", target, QuerystringManager.GetLocalUrl("~/test/img/loading/25-1.gif"));
            return control;
        }

        protected static Control GetPageTitle()
        {
            HtmlGenericControl control = new HtmlGenericControl("h1");
            control.Attributes.Add("class", "pageTitle");
            control.InnerText = ResourceText.GetInstance.GetString("headlineWidgetControl");
            return control;
        }

        protected void RedirectToReturnUrl(WidgetManagementDTO widgetManagementDTO)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);

            if (!headerHasBeenFlushed)
                writer.WriteLine("<html><body>");
            writer.Write("<form id=\"login\" name=\"login\" action=\"{0}\" method=\"post\">", widgetManagementDTO.doneUrl);
            writer.Write("<script type=\"text/javascript\">document.forms['login'].submit();</script>");
            writer.Write("</form><body></html>");

            Response.Write(sw.ToString());
            Response.End();
        }
    }
}
