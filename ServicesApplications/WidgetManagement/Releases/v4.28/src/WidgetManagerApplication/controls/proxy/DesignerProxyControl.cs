using System;
using System.Web.UI;
using EMG.widgets.ui.controls.display;
using EMG.widgets.ui.controls.preview;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.page;
using Factiva.BusinessLayerLogic.Exceptions;
using factiva.nextgen;
using factiva.nextgen.ui.controls;
using AlertHeadlineWidgetDesignControl = EMG.widgets.ui.controls.design.alertWidget.DesignControl;
using AutomaticWorkspaceWidgetDesignControl = EMG.widgets.ui.controls.design.automaticWorkspace.DesignControl;
using ManualNewsletterWorkspaceWidgetDesignControl = EMG.widgets.ui.controls.design.manualNewsletterWorkspace.DesignControl;
using WidgetType=EMG.widgets.ui.dto.WidgetType;

namespace EMG.widgets.ui.controls.proxy
{
    /// <summary>
    /// 
    /// </summary>
    public class DesignerProxyControl : ProxyControl
    {
        private readonly FactivaBusinessLogicException m_FactivaBusinessLogicException = null;
        private WidgetManagementDTO m_WidgetManagementDTO;


        /// <summary>
        /// Gets the session data.
        /// </summary>
        /// <value>The session data.</value>
        protected static SessionData SessionData
        {
            get { return SessionData.Instance(); }
        }

        /// <summary>
        /// Gets the base page.
        /// </summary>
        /// <value>The base page.</value>
        protected BasePage BasePage
        {
            get { return (BasePage) Page; }
        }

        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return m_WidgetManagementDTO; }
            set { m_WidgetManagementDTO = value; }
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (m_FactivaBusinessLogicException != null)
            {
                BasePage basePage = (BasePage) Page;
                writer.Write(basePage.GetBasicErrorHtmlText(m_FactivaBusinessLogicException.ReturnCodeFromFactivaService.ToString()));
            }
            else
            {
                base.Render(writer);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (SessionData != null)
            {
                switch (m_WidgetManagementDTO.widgetType)
                {
                    case WidgetType.AlertHeadlineWidget:
                        LoadAlertHeadlineWidgetDesigner();
                        break;
                    case WidgetType.AutomaticWorkspaceWidget:
                        LoadAutomaticWorkspaceWidgetDesigner();
                        break;
                    case WidgetType.ManualNewsletterWorkspaceWidget:
                        LoadManaualNewsletterWorkspaceWidgetDesigner();
                        break;
                }
            }
        }

        /// <summary>
        /// Loads the alert headline widget designer.
        /// </summary>
        private void LoadAlertHeadlineWidgetDesigner()
        {
            // Add the Design Control
            AlertHeadlineWidgetDesignControl widgetDesignControl = new AlertHeadlineWidgetDesignControl();
            widgetDesignControl.SiteUserType = SessionData.SiteUserType;
            widgetDesignControl.WidgetManagementDTO = m_WidgetManagementDTO;
            BasePage.ContentLeft.Controls.Add(widgetDesignControl);

            // Add the preview Control
            PreviewControl alertWidgetPreviewControl = new PreviewControl();
            alertWidgetPreviewControl.SiteUserType = SessionData.SiteUserType;
            alertWidgetPreviewControl.WidgetManagementDTO = m_WidgetManagementDTO;
            BasePage.ContentMain.Controls.Add(alertWidgetPreviewControl);

            // Add Audience Control
            AudienceControl audienceControl = new AudienceControl();
            audienceControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(audienceControl);

            // Add create Control
            CreateControl alertWidgetCreateControl = new CreateControl();
            alertWidgetCreateControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(alertWidgetCreateControl);

            // Add update Control
            UpdateControl widgetUpdateControl = new UpdateControl();
            widgetUpdateControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(widgetUpdateControl);

            // Add Control Generation Control
            CodeGenerationControl widgetCodeGenerationControl = new CodeGenerationControl();
            widgetCodeGenerationControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(widgetCodeGenerationControl);
        }

        /// <summary>
        /// Loads the automatic workspaces designer.
        /// </summary>
        private void LoadAutomaticWorkspaceWidgetDesigner()
        {
            // Add the Design Control
            AutomaticWorkspaceWidgetDesignControl widgetDesignControl = new AutomaticWorkspaceWidgetDesignControl();
            widgetDesignControl.SiteUserType = SessionData.SiteUserType;
            widgetDesignControl.WidgetManagementDTO = m_WidgetManagementDTO;
            widgetDesignControl.SectionIdentifier = "1";

            LoadWorkspace(widgetDesignControl);
        }

        private void LoadManaualNewsletterWorkspaceWidgetDesigner()
        {
            // Add the Design Control
            ManualNewsletterWorkspaceWidgetDesignControl widgetDesignControl = new ManualNewsletterWorkspaceWidgetDesignControl();
            widgetDesignControl.SiteUserType = SessionData.SiteUserType;
            widgetDesignControl.WidgetManagementDTO = m_WidgetManagementDTO;
            widgetDesignControl.SectionIdentifier = "1";

            LoadWorkspace(widgetDesignControl);
        }

        private void LoadWorkspace(Control widgetDesignControl)
        {
            BasePage.ContentLeft.Controls.Add(widgetDesignControl);
            // Add the preview Control
            PreviewControl alertWidgetPreviewControl = new PreviewControl();
            alertWidgetPreviewControl.SiteUserType = SessionData.SiteUserType;
            alertWidgetPreviewControl.WidgetManagementDTO = m_WidgetManagementDTO;
            alertWidgetPreviewControl.SectionIdentifier = "2";
            BasePage.ContentMain.Controls.Add(alertWidgetPreviewControl);

            // Add create Control
            CreateControl widgetCreateControl = new CreateControl();
            widgetCreateControl.SectionIdentifier = "3";
            widgetCreateControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(widgetCreateControl);

            // Add update Control
            UpdateControl widgetUpdateControl = new UpdateControl();
            widgetUpdateControl.SectionIdentifier = "3";
            widgetUpdateControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(widgetUpdateControl);

            // Add Control Generation Control
            CodeGenerationControl widgetCodeGenerationControl = new CodeGenerationControl();
            widgetCodeGenerationControl.SectionIdentifier = "4";
            widgetCodeGenerationControl.SiteUserType = SessionData.SiteUserType;
            BasePage.ContentRight.Controls.Add(widgetCodeGenerationControl);
        }
    }
}