// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using EMG.widgets.ui.Properties;
using EMG.widgets.ui.ajax.controls.WidgetDesigner;
using EMG.widgets.ui.ajax.controls.WidgetPreview;
using EMG.widgets.ui.attributes;
using EMG.widgets.ui.controls.gallery;
using EMG.widgets.ui.controls.management;
using EMG.widgets.ui.controls.navigation;
using EMG.widgets.ui.controls.proxy;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.page;
using EMG.widgets.ui.utility;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using factiva.nextgen;
using factiva.nextgen.ui;
using ClientScript = factiva.nextgen.ui.page.ClientScriptAttribute;
using UrlBuilder = EMG.Utility.Uri.UrlBuilder;
using WidgetType = EMG.widgets.ui.dto.WidgetType;

namespace EMG.widgets.ui.widgetManagement
{
#if (DEBUG)
    [EnhancedClientScript("~/du/r.ashx?d=js&n=renderManager&t=js", 0)]
    [EnhancedClientScript("~/du/r.ashx?d=js&n=designer%2EalertHeadlineWidget&t=js", 1)]
    [EnhancedClientScript("~/du/r.ashx?d=js&n=designer%2EautomaticWorkspaceWidget&t=js", 2)]
    [EnhancedClientScript("~/du/r.ashx?d=js&n=designer%2EmanualNewsletterWorkspaceWidget&t=js", 3)]
    [EnhancedClientScript("~/du/r.ashx?d=js&n=designer&t=js", 4)]
#else

    /// <summary>
    /// The default.
    /// </summary>
    [EnhancedClientScript("~/du/r.ashx?d=jsa&n=designer&t=js", 0)]
#endif
    
    public class Default : BasePage
    {
        /// <summary>
        /// The m_ widget management dto.
        /// </summary>
        private WidgetManagementDTO widgetManagementDTO;

        /// <summary>
        /// The _gallery settings.
        /// </summary>
        private GallerySettings gallerySettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Default"/> class.
        /// </summary>
        public Default()
        {
            OverrideDefaultPrefix = true;

            // Do not validate let the next page do the work.
            base.ValidateSessionId = true;
        }

        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return widgetManagementDTO; }
            set { widgetManagementDTO = value; }
        }

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Page_Init(object sender, EventArgs e)
        {
            // Add the base script file
            // ScriptManager.Services.Add(new ServiceReference("~/services/ajax/WidgetDesignerManager.asmx"));
            ScriptManager.Services.Add(new ServiceReference("~/EMG.widgets.services.WidgetDesignerManager.asmx"));

            widgetManagementDTO = (WidgetManagementDTO) FormState.Accept(typeof (WidgetManagementDTO), true);
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;

            _siteHeader.DoneUrl = widgetManagementDTO.doneUrl;

            if (!widgetManagementDTO.IsValid())
            {
                throw new EmgWidgetsUIException(EmgWidgetsUIException.RETURN_URL_IS_NOT_SPECIFIED);
            }

            AddWidgetManagementDtoToContext();
            Form.Action = QuerystringManager.GetLocalUrl("~/widgetmanagement/default.aspx");
        }

        /// <summary>
        /// Gets the widget gallery videos control.
        /// </summary>
        /// <returns>
        /// </returns>
        private WidgetGalleryVideosControl GetWidgetGalleryVideosControl()
        {
            var control = new WidgetGalleryVideosControl
                              {
                                  WidgetManagementDTO = widgetManagementDTO
                              };
            return control;
        }

        /// <summary>
        /// Gets the search widget list control.
        /// </summary>
        /// <returns>
        /// </returns>
        private SearchWidgetListControl GetSearchWidgetListControl()
        {
            var control = new SearchWidgetListControl
                              {
                                  WidgetManagementDTO = widgetManagementDTO
                              };
            control.Attributes.Add("class", "searchWidgetListCtrl");
            return control;
        }

        /// <summary>
        /// Gets the learn about factiva widgets control.
        /// </summary>
        /// <returns>
        /// </returns>
        private LearnAboutFactivaWidgetsControl GetLearnAboutFactivaWidgetsControl()
        {
            var control = new LearnAboutFactivaWidgetsControl
                              {
                                  WidgetManagementDTO = widgetManagementDTO
                              };
            return control;
        }

        /// <summary>
        /// Gets the default discovery tabs.
        /// </summary>
        /// <returns>
        /// The get default discovery tabs.
        /// </returns>
        private static string GetDefaultDiscoveryTabs()
        {
            var tabs = new WidgetDiscoveryTabCollection();

            var headlines = new WidgetDiscoveryTab
                                {
                                    Active = true, 
                                    DisplayCheckbox = false, 
                                    Id = "headlines", 
                                    Text = ResourceText.GetInstance.GetString("headlines")
                                };

            var companies = new WidgetDiscoveryTab();
            companies.Active = true;
            companies.DisplayCheckbox = true;
            companies.Id = "companies";
            companies.Text = ResourceText.GetInstance.GetString("companies");

            var executives = new WidgetDiscoveryTab();
            executives.Active = true;
            executives.DisplayCheckbox = true;
            executives.Id = "executives";
            executives.Text = ResourceText.GetInstance.GetString("executives");

            var industries = new WidgetDiscoveryTab();
            industries.Active = true;
            industries.DisplayCheckbox = true;
            industries.Id = "industries";
            industries.Text = ResourceText.GetInstance.GetString("industries");

            var regions = new WidgetDiscoveryTab();
            regions.Active = true;
            regions.DisplayCheckbox = true;
            regions.Id = "regions";
            regions.Text = ResourceText.GetInstance.GetString("regions");

            var subjects = new WidgetDiscoveryTab();
            subjects.Active = true;
            subjects.DisplayCheckbox = true;
            subjects.Id = "subjects";
            subjects.Text = ResourceText.GetInstance.GetString("newsSubjects");

            tabs.Add(headlines);
            tabs.Add(companies);
            tabs.Add(executives);
            tabs.Add(industries);
            tabs.Add(regions);
            tabs.Add(subjects);

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(tabs);
        }

        /// <summary>
        /// Adds the gallery controls.  
        /// </summary>
        /// <param name="modules">
        /// The modules.
        /// </param>
        private void AddGalleryControls(ArrayList modules)
        {
            // Add javascript to tell client script what page this is 
            ClientScript.RegisterClientScriptBlock(GetType(), "identifier", GetClientScriptIdentifier("gallery"));

            // Add sub navigation control
            ContentTop.Controls.Add(AddTabs());
            ContentTop.Controls.Add(GetWidgetGalleryVideosControl());

            // Add controls to the body
            foreach (GallerySettings.ModuleSettings moduleSettings in modules)
            {
                LoadGalleryModules(moduleSettings);
            }

            // Add the Hidden Inputs Control
            var navHiddenInputsContainerControl = new HiddenInputsContainerControl();
            navHiddenInputsContainerControl.SiteUserType = SessionData.SiteUserType;
            navHiddenInputsContainerControl.WidgetManagementDTO = widgetManagementDTO;
            ContentBottom.Controls.Add(navHiddenInputsContainerControl);
        }

        /// <summary>
        /// Loads the gallery modules.
        /// </summary>
        /// <param name="moduleSettings">
        /// The module settings.
        /// </param>
        private void LoadGalleryModules(GallerySettings.ModuleSettings moduleSettings)
        {
            var tempControl = FindControl(moduleSettings.targetControl);
            if (tempControl != null)
            {
                AddGalleryControl(moduleSettings.moduleDetail.moduleType, tempControl);
            }
        }

        /// <summary>
        /// Adds the gallery control.
        /// </summary>
        /// <param name="moduleType">
        /// Type of the module.
        /// </param>
        /// <param name="targetControl">
        /// The target control.
        /// </param>
        private void AddGalleryControl(string moduleType, Control targetControl)
        {
            moduleType = moduleType.Trim();
            if (ValidateString(moduleType))
            {
                object serverControl;
                if (Cache.Get(moduleType) == null)
                {
                    var typeInfo = moduleType;
                    var index = typeInfo.IndexOf(",");
                    if (index > 0)
                    {
                        typeInfo = typeInfo.Substring(0, index);
                    }

                    try
                    {
                        var type = Assembly.GetCallingAssembly().GetType(typeInfo, true, true);
                        var ci = type.GetConstructor(new Type[0]);
                        serverControl = ci.Invoke(null);
                        Cache.Insert(moduleType, ci);
                    }
                    catch (Exception)
                    {
                        var assemblyInfo = moduleType;
                        assemblyInfo = assemblyInfo.Substring(index + 1);
                        var cAssembly = LoadAssembly(assemblyInfo);

                        try
                        {
                            var type = cAssembly.GetType(typeInfo, true, true);
                            var ci = type.GetConstructor(new Type[0]);
                            serverControl = ci.Invoke(null);
                            Cache.Insert(moduleType, ci);
                        }
                        catch (Exception nex)
                        {
                            throw new FactivaUIException(nex, -1);
                        }
                    }
                }
                else
                {
                    serverControl = ((ConstructorInfo) Cache[moduleType]).Invoke(null);
                }

                if (serverControl != null)
                {
                    if (serverControl is BasePage)
                    {
                        ((BasePage) serverControl).Page = this;
                    }

                    if (serverControl.GetType() == typeof (AlertWidgetGalleryControl) &&
                        SessionData.Instance().FactivaAccessObject.IsTrackCoreServiceOn &&
                        widgetManagementDTO.SA_FROM == "GL" &&
                        !SessionData.Instance().FactivaAccessObject.IsDotComTrackDisplayServiceOn)
                    {
                        AddNonBreakingSpace(targetControl);
                    }
                    else if (serverControl.GetType() == typeof (AlertWidgetGalleryControl) &&
                             !SessionData.Instance().FactivaAccessObject.IsSharingOn)
                    {
                        AddNonBreakingSpace(targetControl);
                    }
                    else if (serverControl.GetType() == typeof (AlertWidgetGalleryControl)
                             && !SessionData.Instance().FactivaAccessObject.IsTrackCoreServiceOn)
                    {
                        AddNonBreakingSpace(targetControl);
                    }
                    else if (serverControl.GetType() == typeof (NewsletterWidgetGalleryControl) && !SessionData.Instance().FactivaAccessObject.IsDotComNewsletterDisplayServiceOn)
                    {
                        AddNonBreakingSpace(targetControl);
                    }
                    else if (serverControl.GetType() == typeof (WorkspaceWidgetGalleryControl) && !SessionData.Instance().FactivaAccessObject.IsDotComWorkspaceDisplayServiceOn)
                    {
                        AddNonBreakingSpace(targetControl);
                    }
                    else
                    {
                        targetControl.Controls.Add((Control) serverControl);
                    }
                }
            }
        }

        /// <summary>
        /// The add non breaking space.
        /// </summary>
        /// <param name="targetControl">
        /// The target control.
        /// </param>
        private static void AddNonBreakingSpace(Control targetControl)
        {
            if (targetControl.Controls.Count == 0) targetControl.Controls.Add(new LiteralControl("&nbsp;"));
        }

        /// <summary>
        /// Adds the controls.
        /// </summary>
        private void AddCreateSlashUpdateWidgetControls()
        {
            // Add javascript to tell client script what page this is 
            ClientScript.RegisterClientScriptBlock(GetType(), "identifier", GetClientScriptIdentifier("designer", widgetManagementDTO.widgetType, widgetManagementDTO.secondaryAction));

            // Add sub navigation control
            ContentTop.Controls.Add(AddTabs());

            // Add the Hidden Inputs Control
            var navHiddenInputsContainerControl = new HiddenInputsContainerControl();
            navHiddenInputsContainerControl.SiteUserType = SessionData.SiteUserType;
            navHiddenInputsContainerControl.WidgetManagementDTO = widgetManagementDTO;
            ContentBottom.Controls.Add(navHiddenInputsContainerControl);

            var proxyControl = new DesignerProxyControl();
            proxyControl.SiteUserType = SessionData.SiteUserType;
            proxyControl.WidgetManagementDTO = widgetManagementDTO;
            ContentTop.Controls.Add(proxyControl);

            if (widgetManagementDTO.widgetType != WidgetType.AlertHeadlineWidget)
                return;
            var wDesignerExtender = new WidgetDesignerExtender();
            wDesignerExtender.ID = "wDesignerBehavior";
            wDesignerExtender.AppendInsideControlID = "wSortContainer";
            wDesignerExtender.DiscoveryTabs = GetDefaultDiscoveryTabs();
            wDesignerExtender.Containment = "window";
            wDesignerExtender.OnWidgetDesignerUpdate = "updateAlertHeadlineWidgetPreview";
            wDesignerExtender.Revert = false;
            wDesignerExtender.TargetControlID = "tabStates";
            ContentBottom.Controls.Add(wDesignerExtender);
        }

        /// <summary>
        /// Adds the list widgets controls.
        /// </summary>
        private void AddListWidgetsControls()
        {
            // Add javascript to tell client script what page this is 
            ClientScript.RegisterClientScriptBlock(GetType(), "identifier", GetClientScriptIdentifier("management"));

            // Add sub navigation control
            ContentMain.Controls.Add(AddTabs());
            ContentMain.Controls.Add(GetLearnAboutFactivaWidgetsControl());
            ContentMain.Controls.Add(GetSearchWidgetListControl());

            // Add the Hidden Inputs Control
            var navHiddenInputsContainerControl = new HiddenInputsContainerControl();
            navHiddenInputsContainerControl.SiteUserType = SessionData.SiteUserType;
            navHiddenInputsContainerControl.WidgetManagementDTO = widgetManagementDTO;
            ContentBottom.Controls.Add(navHiddenInputsContainerControl);

            var listContainer = new HtmlGenericControl("div");
            listContainer.ID = "listCntr";
            ContentMain.Controls.Add(listContainer);

            // Add the Preview Control
            var previewControl = new HtmlGenericControl("div");
            previewControl.ID = "wPreviewContainer";

// previewControl.InnerText = ResourceText.GetInstance.GetString("loading");
            listContainer.Controls.Add(previewControl);


            // Add the additional navigation controls
            var previewControlExtender = new WidgetPreviewExtender();

// previewControlExtender.ContentCssClass = "rock";
            // Events
            previewControlExtender.ID = "wPreviewExtender";
            previewControlExtender.TargetControlID = "wPreviewContainer";
            previewControlExtender.OnWidgetDelete = "onWidgetDelete";
            previewControlExtender.OnWidgetEdit = "onWidgetEdit";
            previewControlExtender.OnWidgetPreview = "onWidgetPreview";
            previewControlExtender.OnWidgetPublish = "onWidgetPublish";
            previewControlExtender.OnPreviewBack = "onPreviewBack";

// Tokens
            previewControlExtender.EditToken = ResourceText.GetInstance.GetString("editDesign");
            previewControlExtender.PreviewToken = ResourceText.GetInstance.GetString("preview");
            previewControlExtender.DeleteToken = ResourceText.GetInstance.GetString("delete");
            previewControlExtender.DateToken = ResourceText.GetInstance.GetString("date");
            previewControlExtender.LoadingToken = ResourceText.GetInstance.GetString("loading");
            previewControlExtender.BackToken = ResourceText.GetInstance.GetString("back");
            previewControlExtender.NoWidgetsToken = ResourceText.GetInstance.GetString("noWidgets");
            previewControlExtender.PublishToken = ResourceText.GetInstance.GetString("publish");
            previewControlExtender.NameToken = ResourceText.GetInstance.GetString("name");
            previewControlExtender.TypeToken = ResourceText.GetInstance.GetString("typeLabel");
            previewControlExtender.AlertToken = ResourceText.GetInstance.GetString("alert");
            previewControlExtender.NewsletterToken = ResourceText.GetInstance.GetString("newsletter");
            previewControlExtender.WorkspaceToken = ResourceText.GetInstance.GetString("workspace");

            // Add resources 
            listContainer.Controls.Add(previewControlExtender);

            var previewContainer = new HtmlGenericControl("div");
            previewContainer.ID = "previewCntr";
            var previewArea = new HtmlGenericControl("div");
            previewArea.ID = "previewArea";
            previewContainer.Controls.Add(previewArea);
        }

        /// <summary>
        /// The add tabs.
        /// </summary>
        /// <returns>
        /// </returns>
        private TabContainer AddTabs()
        {
            var tabContainer = new TabContainer();
            tabContainer.ID = "wTabContainer";
            tabContainer.OnClientActiveTabChanged = "ActiveTabChanged";
            tabContainer.CssClass = "ajax__tab_emg";


            if (widgetManagementDTO.action == WidgetManagementAction.Create ||
                (!string.IsNullOrEmpty(widgetManagementDTO.widgetId) &&
                 !string.IsNullOrEmpty(widgetManagementDTO.widgetId.Trim())))
            {
                // Set the correct index
                switch (widgetManagementDTO.action)
                {
                    case WidgetManagementAction.List:
                        tabContainer.ActiveTabIndex = 1;
                        break;
                    case WidgetManagementAction.Gallery:
                        tabContainer.ActiveTabIndex = 2;
                        break;
                    default:
                        break;
                }

                var widgetsDesignerPanel = new TabPanel();
                widgetsDesignerPanel.HeaderText = ResourceText.GetInstance.GetString("widgetDesigner");
                widgetsDesignerPanel.OnClientClick = "fireMyWidgetDesigner";
                tabContainer.Tabs.Add(widgetsDesignerPanel);
            }
            else
            {
                // Set the correct index
                switch (widgetManagementDTO.action)
                {
                    case WidgetManagementAction.Gallery:
                        tabContainer.ActiveTabIndex = 1;
                        break;
                    default:
                        break;
                }
            }

            var myWidgetsPanel = new TabPanel();
            myWidgetsPanel.HeaderText = ResourceText.GetInstance.GetString("widgetManagement");
            myWidgetsPanel.OnClientClick = "fireMyWidgets";
            tabContainer.Tabs.Add(myWidgetsPanel);

            var widgetGallery = new TabPanel();
            widgetGallery.HeaderText = ResourceText.GetInstance.GetString("widgetGallery");
            widgetGallery.OnClientClick = "fireWidgetGallery";
            tabContainer.Tabs.Add(widgetGallery);


            return tabContainer;
        }

        /// <summary>
        /// The redirect to return url.
        /// </summary>
        /// <param name="widgetManagementDTO">
        /// The widget management dto.
        /// </param>
        private void RedirectToReturnUrl(WidgetManagementDTO widgetManagementDTO)
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);

            if (!headerHasBeenFlushed)
                writer.WriteLine("<html><body>");
            writer.Write("<form id=\"login\" name=\"login\" action=\"{0}\" method=\"post\">", widgetManagementDTO.doneUrl);
            writer.Write("<script type=\"text/javascript\">document.forms['login'].submit();</script>");
            writer.Write("</form><body></html>");

            Response.Write(sw.ToString());
            Response.End();
        }

        /// <summary>
        /// Redirects to login server.
        /// </summary>
        public override void HandleInvalidSessionError()
        {
            if (widgetManagementDTO == null)
                widgetManagementDTO = (WidgetManagementDTO) FormState.Accept(typeof (WidgetManagementDTO), true);

            if (widgetManagementDTO != null && widgetManagementDTO.IsValid())
            {
                RedirectToReturnUrl(widgetManagementDTO);
            }

            base.HandleInvalidSessionError();
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        public void Page_Load(object sender, EventArgs e)
        {
            var pageName = "";
            if (!widgetManagementDTO.IsValid()) return;
            switch (widgetManagementDTO.action)
            {
                case WidgetManagementAction.Create:
                    pageName = "DJ_FF_WidgetDesigner";
                    AddCreateSlashUpdateWidgetControls();
                    break;
                case WidgetManagementAction.Update:
                    pageName = "DJ_FF_WidgetDesigner";
// Get Widget and update the dto with right widgetType
                    var widgetManager = new WidgetManager(SessionData.SessionBasedControlDataEx, SessionData.InterfaceLanguage);
                    var widget = widgetManager.GetCachedWidgetById(widgetManagementDTO.widgetId);

                    // update the type
                    if (widget is AlertWidget)
                    {
                        widgetManagementDTO.widgetType = WidgetType.AlertHeadlineWidget;
                    }
                    else if (widget is AutomaticWorkspaceWidget)
                    {
                        widgetManagementDTO.widgetType = WidgetType.AutomaticWorkspaceWidget;
                    }
                    else if (widget is ManualWorkspaceWidget)
                    {
                        widgetManagementDTO.widgetType = WidgetType.ManualNewsletterWorkspaceWidget;
                    }

                    FormState.Remove(widgetManagementDTO);
                    FormState.Add(widgetManagementDTO);
                    AddWidgetManagementDtoToContext();
                    AddCreateSlashUpdateWidgetControls();
                    break;
                case WidgetManagementAction.Gallery:
                    pageName = "DJ_FF_Widgetgallery";
// Add Galery Controls
                    gallerySettings = new GallerySettings(widgetManagementDTO);

                    AddGalleryControls(gallerySettings.portalSettings.modules);
                    break;
                default:
                    pageName = "DJ_FF_MyWidgets";
                    AddListWidgetsControls();
                    break;
            }
            ClientScript.RegisterClientScriptBlock(GetType(), "omniture", GetOmnitureClientScript(pageName));
            Controls.Add(new LiteralControl("<noscript><img src=\"http://om.dowjoneson.com/b/ss/djfactivatesting/1/H.22.1--NS/0\" height=\"1\" width=\"1\" border=\"0\" alt=\"\" /></noscript>"));
        }

        private string GetOmnitureClientScript(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                return "";

            var accountsToSkip = Settings.Default.SkipOmnitureAccounts.Trim().Split(',');

            foreach(var account in accountsToSkip)
            {
                if (!string.IsNullOrEmpty(account) && SessionData.AccountId.StartsWith(account))
                    return "";
            }

            var sb = new StringBuilder();
            sb.Append(@"<script type=""text/javascript"">");
            sb.Append("\n<!--\n");
            sb.Append("\ntry {");
            sb.AppendFormat("logOmniture = {0};", Settings.Default.EnableOmnitureLogging.ToString().ToLower());
            sb.AppendFormat("InitializeOmniture('{0}');", Settings.Default.OmnitureAccount);
            sb.AppendFormat("DJOmniture.Property.{0} = \"{1}\";", "PageName", pageName);
            sb.AppendFormat("DJOmniture.Property.{0} = \"{1}\";", "FullURL", Request.Url);
            sb.AppendFormat("DJOmniture.Property.{0} = \"{1}_{2}\";", "UserId_Ns", SessionData.UserId, SessionData.ProductId);
            sb.AppendFormat("DJOmniture.Property.{0} = \"{1}\";", "AccountId", SessionData.AccountId);
            sb.AppendFormat("DJOmniture.Property.{0} = \"{1}\";", "AccessCode", SessionData.AccessPointCode);
            sb.AppendFormat("DJOmniture.Property.{0} = \"{1}\";", "sessionId", SessionData.SessionId);
            sb.Append("DJOmniture.log();");
            sb.Append("}catch (ex) {}");
            sb.Append("\n// -->\n</script>");
            return sb.ToString();
        }

        /// <summary>
        /// The add widget management dto to context.
        /// </summary>
        private void AddWidgetManagementDtoToContext()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[AbstractGalleryControl.HTTP_CONTEXT_WIDGET_MANAGEMENT_DTO] = widgetManagementDTO;
            }
        }

        /// <summary>
        /// The get client script identifier.
        /// </summary>
        /// <param name="identifier">
        /// The identifier.
        /// </param>
        /// <returns>
        /// The get client script identifier.
        /// </returns>
        private static string GetClientScriptIdentifier(string identifier)
        {
            var ub = new UrlBuilder("~/services/chart.ashx");
            var sb = new StringBuilder();

            sb.Append(@"<script type=""text/javascript"">");
            sb.Append("\n<!--\n");
            sb.AppendFormat("var emg_baseScreen = \"{0}\";", identifier);
            sb.AppendFormat("var emg_selectedTab = \"{0}\";", 1);
            sb.AppendFormat("var emg_baseChartingUrl = \"{0}\";", ub.ToString());
            sb.Append("\n// -->\n</script>");
            return sb.ToString();
        }

        /// <summary>
        /// The get client script identifier.
        /// </summary>
        /// <param name="identifier">
        /// The identifier.
        /// </param>
        /// <param name="widgetType">
        /// The widget type.
        /// </param>
        /// <param name="secondaryAction">
        /// The secondary action.
        /// </param>
        /// <returns>
        /// The get client script identifier.
        /// </returns>
        private static string GetClientScriptIdentifier(string identifier, WidgetType widgetType, WidgetManagementSecondaryAction secondaryAction)
        {
            var ub = new UrlBuilder("~/services/chart.ashx");
            var sb = new StringBuilder();

            sb.Append(@"<script type=""text/javascript"">");
            sb.Append("\n<!--\n");
            sb.AppendFormat("var emg_baseScreen = \"{0}\";", identifier);
            sb.AppendFormat("var emg_widgetType = \"{0}\";", widgetType);
            sb.AppendFormat("var emg_screenAction = \"{0}\";", secondaryAction);
            sb.AppendFormat("var emg_baseChartingUrl = \"{0}\";", ub.ToString());
            sb.AppendFormat("var emg_selectedTab = \"{0}\";", 1);
            sb.Append("\n// -->\n</script>");
            return sb.ToString();
        }
    }
}