// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasePage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Base Asp.net Page class used for site
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Factiva.BusinessLayerLogic.Exceptions;
using factiva.nextgen;
using factiva.nextgen.dto;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using factiva.nextgen.ui.page;
using log4net;
using BaseUrlBuilder = EMG.Utility.Uri.UrlBuilder;
using HeaderControl = EMG.widgets.ui.controls.basic.HeaderControl;

namespace EMG.widgets.ui.page
{
    /// <summary>
    /// Base Asp.net Page class used for site
    /// </summary>
    public class BasePage : AbstractBasePage
    {
        private const string BaseJavascriptVariables = "<script language='javascript' type='text/javascript'>var accessPointCode = \"{0}\";var debugLevel = {1};var interfaceLanguage = \"{2}\";var sessionStateParamName = \"{3}\"; var formStateParamName = \"{4}\";var sa_from=\"{5}\";</script>";
        private readonly string _designerCssFile = "~/du/r.ashx?d=css&n=designer&t=css&v=" + utility.Utility.GetVersion();
        //private readonly string FIREBUG_LITE_JS_FILE = "~/du/r.ashx?d=js%2Ffirebug&n=firebug&t=js&v=" + utility.Utility.GetVersion();
        private const string FirebugLiteJsFile = "~/js/firebug/firebug.js";
        private const string OmnitureLibraryFile = "~/js/omniture/library.js";
        private const string OmnitureHelperFile = "~/js/omniture/helper.js";
        private readonly string _globalCssFile = "~/du/r.ashx?d=css&n=globalv2&t=css&v=" + utility.Utility.GetVersion();
        private readonly string _mainJsaFile = "~/du/r.ashx?d=jsa&n=core&t=js&v=" + utility.Utility.GetVersion();
        private static readonly ILog Log = LogManager.GetLogger(typeof(BasePage));
        private readonly HtmlHead _mHeader;
        private readonly ColumnsContainerControl _columnsContainerControl;
        private ControlCollection _controls;

        /// <summary>
        /// _siteHeader Control of the HTML Page. Provides progromatic access.
        /// </summary>
        protected HeaderControl _siteHeader;

        /// <summary>
        /// </summary>
        public HtmlGenericControl AjaxContainer;

        /// <summary>
        /// </summary>
        public HtmlGenericControl AjaxToolkitHiddenControlsContainer;

        /// <summary>
        /// </summary>
        public HtmlGenericControl ContentBody;

        /// <summary>
        /// </summary>
        public HtmlGenericControl ContentBottom;

        /// <summary>
        /// </summary>
        public LeftColumnControl ContentLeft;

        /// <summary>
        /// </summary>
        public MiddleColumnControl ContentMain;

        /// <summary>
        /// </summary>
        public RightColumnControl ContentRight;

        /// <summary>
        /// </summary>
        public HtmlGenericControl ContentTop;

        /// <summary>
        /// </summary>
        public HtmlGenericControl ContentWrapper;

        /// <summary>
        /// </summary>
        public HtmlGenericControl DynamicContextWidget;

        /// <summary>
        /// _siteHeader Control of the HTML Page
        /// </summary>
        protected FooterControl _footer;

        /// <summary>
        /// Form object of the HTML page
        /// </summary>
        public new BaseForm Form;

        /// <summary>
        /// Gets the document header for the page if the head element is defined with a runat=server in the page declaration.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref="T:System.Web.UI.HtmlControls.HtmlHead"/> containing the page header.
        /// </returns>
        public new HtmlHead Header
        {
            get { return _mHeader; }
        }

        /// <summary>
        /// ASP.NET AJAX 1.0 SCRIPTMANAGER
        /// </summary>
        public ToolkitScriptManager ScriptManager;

        /// <summary>
        /// string Collection that contains references to files that are placed at head of HTML page. 
        /// Provides progromatic access.
        /// </summary>
        protected StringCollection _scriptTokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class.
        /// </summary>
        public BasePage()
        {
            Form = new BaseForm
                       {
                           ID = "PageBaseForm"
                       };
            _mHeader = new HtmlHead("head");
            _siteHeader = new HeaderControl();
            _footer = new FooterControl();
            AjaxToolkitHiddenControlsContainer = new HtmlGenericControl("div")
                                                     {
                                                         ID = "AjaxToolKitItemsHiddenItems"
                                                     };

            AjaxContainer = new HtmlGenericControl("div")
                                {
                                    ID = "tempId",
                                };
            _scriptTokens = new StringCollection();

            ScriptManager = new ToolkitScriptManager
                                {
                                    ID = "PageScriptManager", 
                                    EnablePartialRendering = false, 
                                    CombineScripts = true, 
                                    ScriptMode = ScriptMode.Release,
                                    LoadScriptsBeforeUI = false,
                                };

            var ub = new BaseUrlBuilder
                         {
                             BaseUrl = "~/CombineScriptsHandler.ashx",
                             OutputType = BaseUrlBuilder.UrlOutputType.Absolute
                         };

            ScriptManager.CombineScriptsHandlerUrl = new Uri(ub.ToString(), UriKind.Absolute);

            ContentLeft = new LeftColumnControl();
            ContentMain = new MiddleColumnControl();
            ContentRight = new RightColumnControl();
            ContentWrapper = new HtmlGenericControl("div");
            _columnsContainerControl = new ColumnsContainerControl();
            ContentTop = new HtmlGenericControl("div");
            ContentBody = new HtmlGenericControl("div");
            ContentBottom = new HtmlGenericControl("div");
            DynamicContextWidget = new HtmlGenericControl("div");
            
            Error += base.Page_Error;

            // Register Basic [Stylesheet|Script] files
            RegisterStyleSheet(_globalCssFile);
            RegisterStyleSheet(_designerCssFile);
            RegisterClientScriptFile(_mainJsaFile);
            RegisterClientScriptFile(FirebugLiteJsFile);
            RegisterClientScriptFile(OmnitureLibraryFile);
            RegisterClientScriptFile(OmnitureHelperFile);

            RegisterClientScriptTokens(typeof(GlobalTokens));

            var stylesheetAttributes = (StylesheetAttribute[]) Attribute.GetCustomAttributes(GetType(), typeof(StylesheetAttribute));
            if (stylesheetAttributes.Length > 0)
            {
                Array.Sort(stylesheetAttributes);
                foreach (var stylesheet in stylesheetAttributes)
                {
                    RegisterStyleSheet(stylesheet.Value);
                }
            }

            var clientScriptAttributes = (ClientScriptAttribute[]) Attribute.GetCustomAttributes(GetType(), typeof(ClientScriptAttribute));
            if (clientScriptAttributes.Length <= 0)
            {
                return;
            }

            Array.Sort(clientScriptAttributes);
            foreach (var clientScript in clientScriptAttributes)
            {
                RegisterClientScriptFile(clientScript.Value);
            }
        }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>The page title.</value>
        protected string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [process entry point metrics].
        /// </summary>
        /// <value>
        /// <c>true</c> if [process entry point metrics]; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool ProcessEntryPointMetrics
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the type of the feedback.
        /// </summary>
        /// <value>The type of the feedback.</value>
        protected virtual FeedbackType FeedbackType
        {
            get { return FeedbackType.None; }
        }

        /// <summary>
        /// Sets the page title.
        /// </summary>
        private void SetPageTitle()
        {
            // Page title should be set here
            if (SessionData == null)
            {
                return;
            }

            switch (SessionData.SiteUserType)
            {
                case SiteUserType.FactivaWidgetBuilderUser:
                    _pageTitle = "Factiva Widget Designer";
                    break;
                case SiteUserType.FactivaReader:
                    _pageTitle = "FactivaReader";
                    break;
                case SiteUserType.IWorksBasic:
                case SiteUserType.IWorksPlus:
                    _pageTitle = "iWorks";
                    break;
                default:
                    _pageTitle = "Factiva Search 2.0";
                    break;
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                if (SessionData != null)
                {
                    SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
                }

                base.OnInit(e);
            }
            catch (FactivaBusinessLogicException ex)
            {
                if (SessionData != null)
                {
                    SetPageTitle();
                    _siteHeader.SiteUserType = SessionData.SiteUserType;
                    _footer.SiteUserType = SessionData.SiteUserType;
                    if (SessionData.DebugLevel != 0)
                    {
                        _footer.HttpContextAppender = _httpContextAppender;
                    }
                }
                if (!headerHasBeenFlushed)
                {
                    FlushHeader(false);
                }

                //// sm- changed so that the redirection happens in case of a invalid session-5/24.
                //// HandleError(ex);
                HandleError(ex.ReturnCodeFromFactivaService.ToString(CultureInfo.InvariantCulture));
                Response.End();
                return;
            }

            if (ProcessEntryPointMetrics)
            {
                UpdateEntryPointMetrics();
            }

            // Pass the SiteUserType to template controls
            if (SessionData != null)
            {
                SetPageTitle();
                _siteHeader.SiteUserType = SessionData.SiteUserType;
                _footer.SiteUserType = SessionData.SiteUserType;
            }

            // Set up MemoryAppender --> used for debugging
            if (SessionData != null)
            {
                if (SessionData.DebugLevel != 0)
                {
                    _footer.HttpContextAppender = _httpContextAppender;
                }
            }

            if (Log.IsInfoEnabled)
            {
                Log.Info("Start OnInit ");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreInit"/> event at the beginning of page initialization.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            _controls = new ControlCollection(this);
            for (var i = 0; i < Controls.Count; i++)
            {
                _controls.Add(Controls[i]);
            }

            Controls.Clear();

            Controls.Add(Form);
            if (System.Web.UI.ScriptManager.GetCurrent(Page) == null)
            {
                var head = new HtmlHead("div")
                               {
                                   ID = "cssCntr"
                               };

                Form.Controls.AddAt(0, head);
                Form.Controls.AddAt(1, ScriptManager);
                Form.Controls.AddAt(2, AjaxToolkitHiddenControlsContainer);
            }

            base.OnPreInit(e);
        }

        /// <summary>
        /// Renders the method.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="container">The container.</param>
        public static void RenderMethod ( HtmlTextWriter output,Control container)
        {
            foreach (Control child in container.Controls)
            {
                child.RenderControl(output);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info("Start OnLoad ");
            }

            // flush header
            FlushHeader(true);

            ContentWrapper.ID = "content";
            Form.Controls.Add(ContentWrapper);

            ContentTop.ID = "contentTop";
            ContentWrapper.Controls.Add(ContentTop);

            ContentBody.ID = "contentBody";
            ContentWrapper.Controls.Add(ContentBody);

            ContentBottom.ID = "contentBottom";
            ContentWrapper.Controls.Add(ContentBottom);

            ContentBody.Controls.Add(_columnsContainerControl);

            ContentLeft.ID = "contentLeft";
            ContentMain.ID = "contentMain";
            ContentRight.ID = "contentRight";

            _columnsContainerControl.Controls.Add(ContentLeft);
            _columnsContainerControl.Controls.Add(ContentMain);
            _columnsContainerControl.Controls.Add(ContentRight);
            
            base.OnLoad(e);
            if (Log.IsInfoEnabled)
            {
                Log.Info("End OnLoad ");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // This was add to support help and fdk pages so that they will get consistent look and feel
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "tokens", GetClientScriptTokensFunction());
            if (_controls.Count > 0)
            {
                // add original controls to top of page
                // ControlCollection _tempControls = new ControlCollection(ContentMain);
                // ContentMain.Controls.Clear();
                for (int i = _controls.Count - 1; i >= 0; i--)
                {
                    var c = _controls[i];
                    ContentMain.Controls.AddAt(0, c);
                }
            }

            if (ContentMain.Visible == false || ContentMain.Controls.Count == 0)
            {
                _columnsContainerControl.Controls.Remove(ContentMain);
            }
            
            if (ContentLeft.Visible == false || ContentLeft.Controls.Count == 0)
            {
                _columnsContainerControl.Controls.Remove(ContentLeft);
            }
            
            if (ContentRight.Visible == false || ContentRight.Controls.Count == 0)
            {
                _columnsContainerControl.Controls.Remove(ContentRight);
            }
            
            if (_columnsContainerControl.Visible == false || _columnsContainerControl.Controls.Count == 0)
            {
                ContentBody.Controls.Remove(_columnsContainerControl);
            }

            // remove unused layout controls
            if (ContentTop.Visible == false || ContentTop.Controls.Count == 0)
            {
                ContentWrapper.Controls.Remove(ContentTop);
            }
            
            if (ContentBottom.Visible == false || ContentBottom.Controls.Count == 0)
            {
                ContentWrapper.Controls.Remove(ContentBottom);
            }
            
            if (ContentBody.Visible == false || ContentBody.Controls.Count == 0)
            {
                ContentWrapper.Controls.Remove(ContentBody);
            }

            // set content wrapper class attribute
            if (_columnsContainerControl.Controls.Contains(ContentLeft) &&
                _columnsContainerControl.Controls.Contains(ContentMain) &&
                _columnsContainerControl.Controls.Contains(ContentRight))
            {
                ContentWrapper.Attributes.Add("class", "threeColLayout");
            }
            else if (_columnsContainerControl.Controls.Contains(ContentLeft) &&
                     _columnsContainerControl.Controls.Contains(ContentMain))
            {
                ContentWrapper.Attributes.Add("class", "twoColLtLayout");
            }
            else if (_columnsContainerControl.Controls.Contains(ContentMain) &&
                     _columnsContainerControl.Controls.Contains(ContentRight))
            {
                ContentWrapper.Attributes.Add("class", "twoColRtLayout");
            }
            else if (_columnsContainerControl.Controls.Contains(ContentMain))
            {
                ContentWrapper.Attributes.Add("class", "baseLayout");
            }

            // register hidden fields
            ClientScript.RegisterHiddenField(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "accessPointCode"), SessionData.AccessPointCode);
            //// ClientScript.RegisterHiddenField(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "interfaceLanguage"), SessionData.InterfaceLanguage);

            base.OnPreRender(e);
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info("Start RenderChilderen ");
            }
            
            Form.Controls.Add(_footer);
            base.RenderChildren(writer);

            if (AjaxContainer.Controls.Count > 0)
            {
                AjaxContainer.ID = "tempId";
                AjaxContainer.RenderControl(writer);
            }

           
            //// _footer.RenderControl(writer);
            new LiteralControl("</body>\n</html>").RenderControl(writer);
        }

        #region << flush header >>

        /// <summary>
        /// Flushes the header.
        /// </summary>
        private void FlushHeader(bool flushAll)
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);
            writer.WriteLine(
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine("	<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"/>");
            writer.WriteLine("	<title>{0}</title>", PageTitle);
            if (Request.Browser.Browser.Equals("IE") && Request.Browser.MajorVersion >= 8)
            {
                writer.WriteLine("	<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\" />");
            }

            // Render Factiva Icon
            writer.WriteLine("  <link rel=\"shortcut icon\" href=\"{0}\" />", WebUtility.MakeAbsoluteUrl("~favicon.ico"));

            if (rssLinks != null && rssLinks.Length > 0)
            {
                foreach (RSSLink link in rssLinks)
                {
                    writer.WriteLine("	<link rel=\"alternate\" type=\"application/rss+xml\" title=\"{0}\" href=\"{1}\" />",
                                     (link.Title != null) ? link.Title.Replace("\"", "&quot;") : string.Empty,
                                     link.Href);
                }
            }


            // add css files additional files
            foreach (HtmlLink htmlLink in Header.Controls.OfType<HtmlLink>())
            {
                htmlLink.RenderControl(writer);
            }

            var senumr = _stylesheets.GetEnumerator();
            while (senumr.MoveNext())
            {
                writer.WriteLine("	<link rel=\"stylesheet\" type=\"text/css\" media=\"all\" href=\"{0}\"/>", WebUtility.MakeAbsoluteUrl(senumr.Current));
            }

            

            senumr = _scriptFiles.GetEnumerator();
            while (senumr.MoveNext())
            {
                writer.WriteLine("	<script src=\"{0}\" type=\"text/javascript\"></script>", WebUtility.MakeAbsoluteUrl(senumr.Current));
            }
            
            if (flushAll)
            {
                writer.WriteLine(BaseJavascriptVariables,
                                 (SessionData.Instance()!= null) ? SessionData.Instance().AccessPointCode : string.Empty,
                                 (SessionData.Instance()!= null) ? SessionData.Instance().DebugLevel : 0,
                                 (SessionData.Instance()!= null) ? SessionData.Instance().InterfaceLanguage: string.Empty,
                                 factiva.nextgen.ui.formstate.FormState.FORM_KEY_SESS_STATE,
                                 factiva.nextgen.ui.formstate.FormState.FORM_KEY_STATE,
                                 (SessionData.Instance()!= null) ? SessionData.Instance().ProductPrefix : string.Empty
                    );
            }

            writer.WriteLine("</head>");

            IDictionaryEnumerator denumr = PageBodyAttributes.GetEnumerator();
            while (denumr.MoveNext())
            {
                writer.AddAttribute(denumr.Key.ToString(), denumr.Value.ToString());
            }

            writer.RenderBeginTag("body");
            writer.Write("<div id=\"appendAJAX\" class=\"\">");
            writer.Write("</div>");

            // need error handling around call for ruleset made in this transaction
            _siteHeader.RenderControl(writer);

            Response.Write(sw.ToString());
            Response.Flush();
            headerHasBeenFlushed = true;
        }

        /// <summary>
        /// Handles the Error event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        new protected void Page_Error(object sender, EventArgs e)
        {
            var uiException = Server.GetLastError() as FactivaBusinessLogicException;
            if (uiException != null)
            {
                HandleError(uiException.ReturnCodeFromFactivaService.ToString(CultureInfo.InvariantCulture));
            }
            else if (Server.GetLastError().Message.IndexOf(ERR_INVALID_SESSION, StringComparison.Ordinal) > -1)
            {
                HandleError(ERR_INVALID_SESSION);
            }
            else if (!WebUtility.IsLocalUser())
            {
                HandleError("-1");
            }
        }

        /// <summary>
        /// Renders the error message.
        /// </summary>
        /// <param name="errorNumber">The error number.</param>
        public override void RenderErrorMessage(string errorNumber)
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);

            writer.WriteLine("<div style=\"margin:100px auto;width:250px;font-face:Verdana, Arial, Helvetica, sans-serif\">");
            writer.WriteLine("<div style=\"background-color:#54559B;color:#FFF;padding:5px;\"><b>{0}</b></div>",
                             ResourceText.GetInstance.GetString("error"));
            writer.WriteLine("<div style=\"background-color:#ccc;padding:5px;\">");
            writer.WriteLine("<p>{0}</p>", ResourceText.GetInstance.GetErrorMessage(errorNumber));
            writer.WriteLine("<p>{0}</p>", errorNumber);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");
            writer.WriteLine("</div></div></div>");
            writer.WriteLine("</div></div></div>");
            _footer.RenderControl(writer);

            writer.WriteLine("</div></div></div></body>");
            writer.WriteLine("</html>");

            Response.Write(sw.ToString());
            Response.End();
        }


        /// <summary>
        /// Renders the error message with OK button.
        /// </summary>
        /// <param name="errorNumber">The error number.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="showOkButton">if set to <c>true</c> [show ok button].</param>
        protected void RenderErrorMessageWithOkButton(string errorNumber, string redirectUrl, bool showOkButton)
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);

            writer.WriteLine("	<link rel=\"stylesheet\" type=\"text/css\" media=\"all\" href=\"{0}\"/>",
                             WebUtility.MakeAbsoluteUrl(_globalCssFile));
            writer.WriteLine(
                "<div style=\"margin:100px auto;width:250px;font-face:Verdana, Arial, Helvetica, sans-serif\">");
            writer.WriteLine("<div style=\"background-color:#54559B;color:#FFF;padding:5px;\"><b>{0}</b></div>",
                             ResourceText.GetInstance.GetString("error"));
            writer.WriteLine("<div style=\"background-color:#ccc;padding:5px;\">");
            writer.WriteLine("<p>{0}</p>", ResourceText.GetInstance.GetErrorMessage(errorNumber));
            writer.WriteLine("<p>{0}</p>", errorNumber);
            if (showOkButton && redirectUrl.Length > 0)
            {
                writer.WriteLine(
                    "<div style=\"background-color:#ccc;padding:5px;float:center;\"><a href=\"{0}\" class=\"majorButton\">{1}</a></div>",
                    WebUtility.MakeAbsoluteUrl(redirectUrl), ResourceText.GetInstance.GetString("ok"));
            }
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

            _footer.RenderControl(writer);

            writer.WriteLine("</div></div></div></body>");
            writer.WriteLine("</html>");

            Response.Write(sw.ToString());
            Response.End();
        }

        #endregion

        #region << private methods >>

        private string GetClientScriptTokensFunction()
        {
            var sb = new StringBuilder();
            var resourceText = ResourceText.GetInstance;

            sb.Append(@"<script type=""text/javascript"">");
            sb.Append("\n<!--\n");
            sb.Append("function _translate(token) {\n");
            sb.Append("var rsrcTxt={");

            var index = 0;
            if (_scriptTokens.Count > 0)
            {
                foreach (string token in _scriptTokens)
                {
                    sb.Append(index == 0 ? "\"" : ",\"");
                    sb.Append(token).Append("\":\"").Append(resourceText.GetString(token)).Append("\"");
                    index++;
                }
            }

            sb.Append("};\n");
            sb.Append("if ( rsrcTxt && token && rsrcTxt[token] ) return rsrcTxt[token];\n");
            sb.Append("else if ( token ) return '${' + token + '}'; \n");
            sb.Append("else return null;\n");

            sb.Append("}\n// -->\n</script>");
            return sb.ToString();
        }

        #endregion

        #region << public methods >>

        /// <summary>
        /// Registers the client script tokens.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        public void RegisterClientScriptTokens(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                return;
            }

            var tempStrings = Enum.GetNames(enumType);
            foreach (var tempString in tempStrings)
            {
                _scriptTokens.Add(tempString);
            }
        }

        /// <summary>
        /// Registers the client script tokens.
        /// </summary>
        /// <param name="pipeDelimitedTokens">The pipe delimited tokens.</param>
        public void RegisterClientScriptTokens(string pipeDelimitedTokens)
        {
            if (pipeDelimitedTokens == null || pipeDelimitedTokens.Trim().Length <= 0)
            {
                return;
            }

            var tmpArr = pipeDelimitedTokens.Split('|');
            foreach (var t in tmpArr)
            {
                _scriptTokens.Add(t);
            }
        }

        #endregion
    }
}
