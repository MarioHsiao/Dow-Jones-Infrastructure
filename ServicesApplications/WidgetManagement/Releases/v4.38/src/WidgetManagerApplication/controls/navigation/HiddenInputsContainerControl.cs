using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.page;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class HiddenInputsContainerControl : BaseControl
    {
        private const string m_Label = "<label for=\"{0}\"> {1}: </label>";
        private const string m_InitialTabState =
            "[{\"Active\":true,\"DisplayCheckbox\":false,\"Id\":\"headlines\",\"Text\":\"Headlines\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"companies\",\"Text\":\"Companies\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"industries\",\"Text\":\"Industries\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"subjects\",\"Text\":\"Subjects\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"executives\",\"Text\":\"Executives\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"regions\",\"Text\":\"Regions\"}]";
        private WidgetManagementDTO m_WidgetManagementDTO;

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
        /// Gets the modal popup control.
        /// </summary>
        /// <returns></returns>
        private static Control GetCodeModalPopupControl()
        {
            Panel modalControl = new Panel();
            modalControl.ID = "programmaticPopup";
            modalControl.CssClass = "confirm-dialog";
            modalControl.Attributes.Add("style", "display:none");
            modalControl.Controls.Add(GetContentForCodeModalPopup());
            return modalControl;
        }

        /// <summary>
        /// Gets the confirm update modal popup control.
        /// </summary>
        /// <returns></returns>
        private static Control GetConfirmUpdateModalPopupControl()
        {
            Panel modalControl = new Panel();
            modalControl.ID = "proxyConfirmPopup";
            modalControl.CssClass = "confirm-dialog";
            modalControl.Attributes.Add("style", "display:none");
            modalControl.Controls.Add(GetContentForUpdateConfirmModalPopup());
            return modalControl;
        }

        /// <summary>
        /// Gets the ajax modal extender control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAjaxConfirmUpdateModalExtenderControl()
        {
            ModalPopupExtender modalPopupExtender = new ModalPopupExtender();
            modalPopupExtender.ID = "proxyConfirmModalPopup";
            modalPopupExtender.BehaviorID = "proxyConfirmModalPopupBehavior";
            modalPopupExtender.PopupControlID = "proxyConfirmPopup";
            modalPopupExtender.TargetControlID = "hiddenTargetControlForConfirmModalPopup";
            modalPopupExtender.BackgroundCssClass = "modalBackground";
            //modalPopupExtender.OkControlID = "confirmBtnOK";
            //modalPopupExtender.CancelControlID = "confirmBtnClose";
            return modalPopupExtender;
        }

        /// <summary>
        /// Gets the hidden target control form modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetHiddenTargetControlForModalPopup()
        {
            LinkButton linkButton = new LinkButton();
            linkButton.ID = "hiddenTargetControlForModalPopup";
            linkButton.Attributes.Add("style", "display:none");
            return linkButton;
        }

        /// <summary>
        /// Gets the content for update confirm modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetContentForUpdateConfirmModalPopup()
        {
            // main container 
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "mUpdateTitleBar";
            content.Attributes.Add("class", "inner");

            // header
            HtmlGenericControl header = new HtmlGenericControl("h2");
            header.ID = "mUpdatePopHeader";
            header.InnerText = ResourceText.GetInstance.GetString("updateWidgetDesign");
            content.Controls.Add(header);

            // Code container div
            HtmlGenericControl codeDiv = new HtmlGenericControl("div");
            codeDiv.ID = "updateCntr";
            codeDiv.Controls.Add(new LiteralControl(string.Format("<div class=\"instructions\">{0}</div>", ResourceText.GetInstance.GetString("widgetUpdateInstructions"))));
            
            content.Controls.Add(codeDiv);
            content.Controls.Add(GetButtonControlsForConfirmModalPopup());
            return content;

        }


        /// <summary>
        /// Gets the content for modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetContentForCodeModalPopup()
        {
            // main container 
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "mTitleBar";
            content.Attributes.Add("class", "inner");

            // header
            HtmlGenericControl header = new HtmlGenericControl("h2");
            header.ID = "mPopHeader";
            header.InnerText = ResourceText.GetInstance.GetString("displayCode");
            content.Controls.Add(header);

            // loading container div
            HtmlGenericControl loadingDiv = new HtmlGenericControl("div");
            loadingDiv.ID = "codeLoadingCntr";
            loadingDiv.InnerText = string.Concat(ResourceText.GetInstance.GetString("loading"), "...");
            content.Controls.Add(loadingDiv);

            // updating container div
            HtmlGenericControl updatingDiv = new HtmlGenericControl("div");
            updatingDiv.ID = "codeUpdatingCntr";
            updatingDiv.InnerText = string.Concat(ResourceText.GetInstance.GetString("updating"), "...");
            content.Controls.Add(updatingDiv);

            // Code container div
            HtmlGenericControl codeDiv = new HtmlGenericControl("div");
            codeDiv.ID = "codeCntr";
            codeDiv.Controls.Add(new LiteralControl(string.Format("<div class=\"instructions\">{0}</div>", ResourceText.GetInstance.GetString("widgetCodeInstructions"))));
            codeDiv.Controls.Add(new LiteralControl("<div id=\"scriptTextContainer\"><textarea id=\"scriptCode\" height=\"200px\"></textarea></div>"));
            codeDiv.Controls.Add(GetHttpRadioOptionsForModalPopup());

            HtmlGenericControl pubContainer = new HtmlGenericControl("div");
            pubContainer.ID = "publishingContainer";
            codeDiv.Controls.Add(pubContainer);

            content.Controls.Add(codeDiv);
            content.Controls.Add(GetButtonControlsForModalPopup());
            return content;
        }

        /// <summary>
        /// Gets the HTTP radio options for modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetHttpRadioOptionsForModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "divHttp";

            HtmlInputRadioButton radio1 = new HtmlInputRadioButton();
            radio1.Checked = true;
            radio1.ID = "http";
            radio1.Name = "ssl";
            radio1.Value = "http";

            HtmlGenericControl span1 = new HtmlGenericControl("span");
            span1.InnerText = "HTTP";
            
            HtmlInputRadioButton radio2 = new HtmlInputRadioButton();
            radio2.ID = "https";
            radio2.Name = "ssl";
            radio2.Value = "https";

            HtmlGenericControl span2 = new HtmlGenericControl("span");
            span2.InnerText = "HTTPS";

            content.Controls.Add(radio1);
            content.Controls.Add(span1);
            content.Controls.Add(radio2);
            content.Controls.Add(span2);

            return content;
        }

        /// <summary>
        /// Gets the button controls for modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetButtonControlsForModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "divBase";
            content.Attributes.Add("class", "base");

            LinkButton btnOK = new LinkButton();
            btnOK.CssClass = "button";
            btnOK.ID = "btnOK";
            btnOK.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("ok"));
            content.Controls.Add(btnOK);

            LinkButton lnkClose = new LinkButton();
            lnkClose.CssClass = "close";
            lnkClose.ID = "lnkClose";
            lnkClose.OnClientClick = "closeModal();this.blur();return false;";
            content.Controls.Add(lnkClose);

            return content;
        }

        /// <summary>
        /// Gets the button controls for confirm modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetButtonControlsForConfirmModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "confirmDivBase";
            content.Attributes.Add("class", "base");

            HtmlGenericControl publishControls = new HtmlGenericControl("span");
            publishControls.ID = "publishConfirmControl";

            LinkButton btnOK = new LinkButton();
            btnOK.ID = "confirmBtnOK";
            btnOK.CssClass = "button";
            btnOK.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("yes"));
            btnOK.OnClientClick = "okUpdate();this.blur();return false;";
            publishControls.Controls.Add(btnOK);

            LinkButton lnkClose = new LinkButton();
            lnkClose.ID = "confirmBtnClose";
            lnkClose.CssClass = "button";
            lnkClose.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("no"));
            lnkClose.OnClientClick = "closeUpdate();this.blur();return false;";
            publishControls.Controls.Add(lnkClose);

            HtmlGenericControl proxyControls = new HtmlGenericControl("span");
            proxyControls.ID = "proxyConfirmControl";

            btnOK = new LinkButton();
            btnOK.ID = "confirmProxyBtnOK";
            btnOK.CssClass = "button";
            btnOK.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("yes"));
            btnOK.OnClientClick = "okProxyUpdate();this.blur();return false;";
            proxyControls.Controls.Add(btnOK);

            lnkClose = new LinkButton();
            lnkClose.ID = "confirmProxyBtnClose";
            lnkClose.CssClass = "button";
            lnkClose.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("no"));
            lnkClose.OnClientClick = "closeProxyUpdate();this.blur();return false;";
            proxyControls.Controls.Add(lnkClose);

            content.Controls.Add(publishControls);
            content.Controls.Add(proxyControls);

            return content;
        }

        /// <summary>
        /// Gets the ajax modal extender control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAjaxProxyCredExtenderControl()
        {
            ModalPopupExtender modalPopupExtender = new ModalPopupExtender();
            modalPopupExtender.ID = "proxyCredModalPopup";
            modalPopupExtender.BehaviorID = "proxyCredModalPopupBehavior";
            modalPopupExtender.PopupControlID = "proxyCredPopup";
            modalPopupExtender.TargetControlID = "hiddenTargetControlForProxyCredModalPopup";
            modalPopupExtender.BackgroundCssClass = "modalBackground";
            //modalPopupExtender.OkControlID = "proxyCredBtnOK";
            return modalPopupExtender;
        }

        /// <summary>
        /// Gets the proxy cred modal popup control.
        /// </summary>
        /// <returns></returns>
        private static Control GetProxyCredModalPopupControl()
        {
            Panel modalControl = new Panel();
            modalControl.ID = "proxyCredPopup";
            modalControl.CssClass = "confirm-dialog";
            modalControl.Attributes.Add("style", "display:none");
            modalControl.Controls.Add(GetContentForProxyCredModalPopup());
            return modalControl;
        }


        /// <summary>
        /// Gets the content for proxy cred modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetContentForProxyCredModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "creditleBar";
            content.Attributes.Add("class", "inner");
            content.Controls.Add(new LiteralControl(string.Format("<h2 id=\"proxyCredLabel\">{0}</h2>", ResourceText.GetInstance.GetString("setProxyCredentials"))));

            HtmlGenericControl authContainer = new HtmlGenericControl("div");
            authContainer.ID = "proxyCredAuthContainer";
            authContainer.Controls.Add(factiva.nextgen.ui.Utility.GetRadioButtonListBasedOnEnum("authScheme", typeof(WidgetAuthenticationScheme), WidgetAuthenticationScheme.UserId.ToString()));

            HtmlGenericControl fieldsContainer = new HtmlGenericControl("div");
            fieldsContainer.Controls.Add(GetGenericFieldConainer("proxyCredUserId", "enterUserId"));
            fieldsContainer.Controls.Add(GetGenericFieldConainer("proxyCredEmailAddress", "enterEmailAddress"));
            fieldsContainer.Controls.Add(GetGenericFieldConainer("proxyCredPassword", "enterpasswd", true));
            fieldsContainer.Controls.Add(GetGenericFieldConainer("proxyCredNamespace", "enternamespace"));
            
            // Add area to place error messages
            content.Controls.Add(new LiteralControl("<div id=\"proxyCredErrorMsg\"></div>"));
            // Add auth table
            content.Controls.Add(authContainer);
            // Add fields
            content.Controls.Add(fieldsContainer);
            //Add button controls
            content.Controls.Add(GetButtonControlsForProxyCredentialsModalPopup());
            return content;
        }


        /// <summary>
        /// Gets the generic field conainer.
        /// </summary>
        /// <param name="inputTextId">The input text id.</param>
        /// <param name="resourceToken">The resource token.</param>
        /// <returns></returns>
        private static Control GetGenericFieldConainer(string inputTextId, string resourceToken)
        {
            return GetGenericFieldConainer(inputTextId, resourceToken, false);
        }

        /// <summary>
        /// Gets the generic field conainer.
        /// </summary>
        /// <param name="inputTextId">The input text id.</param>
        /// <param name="resourceToken">The resource token.</param>
        /// <param name="isPassword">if set to <c>true</c> [is password].</param>
        /// <returns></returns>
        private static Control GetGenericFieldConainer(string inputTextId, string resourceToken, bool isPassword)
        {
            HtmlGenericControl genericControl = new HtmlGenericControl("div");
            genericControl.ID = inputTextId + "Row";
            genericControl.Attributes.Add("class","proxyCred");

            HtmlInputControl inputText = new HtmlInputText();

            if (isPassword)
            {
                inputText = new HtmlInputPassword();    
            } 
            inputText.ID = inputTextId; 

            HtmlGenericControl labelCntr = new HtmlGenericControl("div");
            labelCntr.Controls.Add(new LiteralControl(string.Format(m_Label, inputText.ID, ResourceText.GetInstance.GetString(resourceToken).Replace(":",""))));

            HtmlGenericControl inputCntr = new HtmlGenericControl("div");
            inputCntr.Controls.Add(inputText);

            genericControl.Controls.Add(labelCntr);
            genericControl.Controls.Add(inputCntr);
            return genericControl;
        }

        /// <summary>
        /// Gets the button controls for proxy credentials modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetButtonControlsForProxyCredentialsModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "proxyCredBase";
            content.Attributes.Add("class", "base");

            HtmlGenericControl container = new HtmlGenericControl("div");
            container.ID = "proxyCredButtonContainer";
            container.Attributes.Add("class", "cntr");
            content.Controls.Add(container);

            LinkButton btnOK = new LinkButton();
            btnOK.CssClass = "button";
            btnOK.ID = "proxyCredBtnOK";
            btnOK.Text = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("ok"));
            btnOK.OnClientClick = "openUpdateWidgetModal();this.blur();return false;";
            container.Controls.Add(btnOK);

            LinkButton lnkClose = new LinkButton();
            lnkClose.CssClass = "button";
            lnkClose.ID = "proxyCredLnkClose";
            lnkClose.Text = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("close"));
            lnkClose.OnClientClick = "closeProxyCredModal();this.blur();return false;";
            container.Controls.Add(lnkClose);

            return content;
        }

        /// <summary>
        /// Gets the hidden target control form modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetHiddenTargetControlForProxyCredModalPopup()
        {
            LinkButton linkButton = new LinkButton();
            linkButton.ID = "hiddenTargetControlForProxyCredModalPopup";
            linkButton.Attributes.Add("style", "display:none");
            return linkButton;
        }

        /// <summary>
        /// Gets the hidden target control form modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetHiddenTargetControlForConfirmModalPopup()
        {
            LinkButton linkButton = new LinkButton();
            linkButton.ID = "hiddenTargetControlForConfirmModalPopup";
            linkButton.Attributes.Add("style", "display:none");
            return linkButton;
        }

        /// <summary>
        /// Gets the ajax code modal extender control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAjaxCodeModalExtenderControl()
        {
            ModalPopupExtender modalPopupExtender = new ModalPopupExtender();
            modalPopupExtender.ID = "programmaticModalPopup";
            modalPopupExtender.BehaviorID = "programmaticModalPopupBehavior";
            modalPopupExtender.PopupControlID = "programmaticPopup";
            modalPopupExtender.TargetControlID = "hiddenTargetControlForModalPopup";
            modalPopupExtender.BackgroundCssClass = "modalBackground";
            modalPopupExtender.OkControlID = "btnOK";
            return modalPopupExtender;
        }

        /// <summary>
        /// Gets the modal popup control.
        /// </summary>
        /// <returns></returns>
        private static Control GetConfirmDeleteModalPopupControl()
        {
            Panel modalControl = new Panel();
            modalControl.ID = "confirmDeletePopup";
            modalControl.CssClass = "confirm-dialog";
            modalControl.Attributes.Add("style", "display:none");
            modalControl.Controls.Add(GetContentForConfirmDeleteModalPopup());
            return modalControl;
        }

        /// <summary>
        /// Gets the content for confirm delete modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetContentForConfirmDeleteModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "confDelBase";
            content.Attributes.Add("class", "inner");

            // header
            HtmlGenericControl header = new HtmlGenericControl("h2");
            header.ID = "confDelHeader";
            header.InnerText = string.Concat(ResourceText.GetInstance.GetString("delete"), " Widget");
            content.Controls.Add(header);

            HtmlGenericControl textContainer = new HtmlGenericControl("div");
            textContainer.ID = "confDelButtonTextContainer";
            
            HtmlGenericControl text = new HtmlGenericControl("div");
            text.ID = "confDelButtonText";
            text.Attributes.Add("class", "instructions");
            text.InnerText = ResourceText.GetInstance.GetString("deleteWidget");

            textContainer.Controls.Add(text);
            content.Controls.Add(textContainer);

            HtmlGenericControl container = new HtmlGenericControl("div");
            container.ID = "confDelButtonContainer";
            container.Attributes.Add("class", "base");
            content.Controls.Add(container);

            LinkButton btnOK = new LinkButton();
            btnOK.CssClass = "button";
            btnOK.ID = "confDelBtnOK";
            btnOK.Text = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("yes"));
            btnOK.OnClientClick = "confirmDeletePopupOK();this.blur();return false;";
            container.Controls.Add(btnOK);

            LinkButton lnkClose = new LinkButton();
            lnkClose.CssClass = "button";
            lnkClose.ID = "confDelBtnClose";
            lnkClose.Text = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("no"));
            lnkClose.OnClientClick = "this.blur();return false;";
            container.Controls.Add(lnkClose);

            return content;
        }

        /// <summary>
        /// Gets the modal confirm delete extender control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAjaxModalConfirmDeleteExtenderControl()
        {
            ModalPopupExtender modalPopupExtender = new ModalPopupExtender();
            modalPopupExtender.ID = "modalConfirmDeleteButton";
            modalPopupExtender.BehaviorID = "modalConfirmDeleteButtonBehavior";
            modalPopupExtender.PopupControlID = "confirmDeletePopup";
            modalPopupExtender.TargetControlID = "hiddenTargetControlForModalPopup";
            modalPopupExtender.BackgroundCssClass = "modalBackground";
            modalPopupExtender.OkControlID = "confDelBtnOK";
            modalPopupExtender.CancelControlID = "confDelBtnClose";
            return modalPopupExtender;
        }

        /// <summary>
        /// Gets the widget management page hidden input control.
        /// </summary>
        /// <returns></returns>
        private Control GetWidgetManagementPageHiddenInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof(WidgetManagementDTO), "page");
            if (WidgetManagementDTO != null)
            {
                inputControl.Value = WidgetManagementDTO.page.ToString();
            }

            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the widget management page size hidden input control.
        /// </summary>
        /// <returns></returns>
        private Control GetWidgetManagementPageSizeHiddenInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof(WidgetManagementDTO), "pageSize");
            if (WidgetManagementDTO != null)
            {
                inputControl.Value = WidgetManagementDTO.pageSize.ToString();
            }
            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the widget management page size hidden input control.
        /// </summary>
        /// <returns></returns>
        private Control GetWidgetManagement_SA_FROM_HiddenInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof(WidgetManagementDTO), "SA_FROM");
            if (WidgetManagementDTO != null)
            {
                inputControl.Value = WidgetManagementDTO.SA_FROM;
            }
            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the widget management sort order hidden input control.
        /// </summary>
        /// <returns></returns>
        private Control GetWidgetManagementSortOrderHiddenInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "sortOrder");
            if (WidgetManagementDTO != null)
            {
                inputControl.Value = WidgetManagementDTO.sortOrder;
            }
            if (SessionData.Instance().DebugLevel ==3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the widget management action input control.
        /// </summary>
        /// <returns></returns>
        private Control GetWidgetManagementActionInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "action");
            if (WidgetManagementDTO != null)
            {
                inputControl.Value = ((int)WidgetManagementDTO.action).ToString();
            }
            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the widget id hiddent input control.
        /// </summary>
        /// <returns></returns>
        private Control GetWidgetIdHiddentInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "widgetId");

            if (WidgetManagementDTO != null && WidgetManagementDTO.IsValid() &&
                WidgetManagementDTO.IsValid(WidgetManagementDTO.widgetId))
            {
                inputControl.Value = WidgetManagementDTO.widgetId;
            }
            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the folder I ds hidden input control.
        /// </summary>
        /// <returns></returns>
        private Control GetFolderIdsHiddenInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control 
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof(WidgetManagementDTO), "assetIds");
            if (WidgetManagementDTO != null && WidgetManagementDTO.IsValid() &&
                WidgetManagementDTO.IsValid(WidgetManagementDTO.assetIds))
            {
                inputControl.Value = string.Join(",", WidgetManagementDTO.assetIds);
            }
            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the sortable tabs hidden input control. Used as a target for the extender control.
        /// </summary>
        /// <returns></returns>
        private Control GetSortableTabsHiddenInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialzie the control            
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = "tabStates";
            inputControl.Value = m_InitialTabState;

            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }

        /// <summary>
        /// Gets the folder I ds hidden input control.
        /// </summary>
        /// <returns></returns>
        private Control GetReturnUrlInputControl()
        {
            HtmlGenericControl debugDiv = new HtmlGenericControl("div");
            // Initialize the control 
            HtmlInputControl inputControl = new HtmlInputHidden();
            if (SessionData.Instance().DebugLevel == 3)
                inputControl = new HtmlInputText();

            inputControl.ID = UrlBuilder.GetParameterName(typeof (WidgetManagementDTO), "doneUrl");
            if (WidgetManagementDTO != null && WidgetManagementDTO.IsValid() &&
                WidgetManagementDTO.IsValid(WidgetManagementDTO.doneUrl))
            {
                inputControl.Value = WidgetManagementDTO.doneUrl;
            }
            if (SessionData.Instance().DebugLevel == 3)
            {
                debugDiv.Controls.Add(inputControl);
                return debugDiv;
            }
            return inputControl;
        }


        /// <summary>
        /// Gets the ajax modal extender control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAjaxErrorModalExtenderControl()
        {
            ModalPopupExtender modalPopupExtender = new ModalPopupExtender();
            modalPopupExtender.ID = "programmaticErrorModalPopup";
            modalPopupExtender.BehaviorID = "programmaticErrorModalPopupBehavior";
            modalPopupExtender.PopupControlID = "programmaticErrorPopup";
            modalPopupExtender.TargetControlID = "hiddenTargetControlForErrorModalPopup";
            modalPopupExtender.BackgroundCssClass = "modalBackground";
            modalPopupExtender.OkControlID = "errBtnOK";
            return modalPopupExtender;
        }

        /// <summary>
        /// Gets the ajax modal extender control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAjaxMovieModalExtenderControl()
        {
            ModalPopupExtender modalPopupExtender = new ModalPopupExtender();
            modalPopupExtender.ID = "programmaticMovieModalPopup";
            modalPopupExtender.BehaviorID = "programmaticMovieModalPopupBehavior";
            modalPopupExtender.PopupControlID = "programmaticMoviePopup";
            modalPopupExtender.TargetControlID = "hiddenTargetControlForMovieModalPopup";
            modalPopupExtender.BackgroundCssClass = "modalBackground";
            modalPopupExtender.OkControlID = "movieBtnOK";
            modalPopupExtender.RepositionMode = ModalPopupRepositionMode.None;
            return modalPopupExtender;
        }

        /// <summary>
        /// Gets the content for modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetContentForErrorModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "titleBar";
            content.Attributes.Add("class", "inner");
            content.Controls.Add(new LiteralControl(string.Format("<h2 id=\"errorLbl\">{0}</h2>", ResourceText.GetInstance.GetString("Error"))));
            content.Controls.Add(new LiteralControl(string.Format("<div id=\"errorMsg\" class=\"instructions\">{0}</div>", ResourceText.GetInstance.GetString("widgetCodeInstructions"))));
            content.Controls.Add(GetButtonControlsForErrorModalPopup());
            return content;
        }

        /// <summary>
        /// Gets the content for modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetContentForMovieModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "movieTitleBar";
            content.Attributes.Add("class", "inner");
            content.Controls.Add(new LiteralControl(string.Format("<h2 id=\"movieLabel\">{0}</h2>", ResourceText.GetInstance.GetString("learnMoreAboutFactivaWidgets"))));
            content.Controls.Add(new LiteralControl(string.Format("<div id=\"movieContainer\" class=\"mContainer\">{0}</div>", string.Empty)));
            content.Controls.Add(GetButtonControlsForMovieModalPopup());
            return content;
        }

        /// <summary>
        /// Gets the button controls for error modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetButtonControlsForErrorModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "errBaseDiv";
            content.Attributes.Add("class", "base");

            LinkButton btnOK = new LinkButton();
            btnOK.ID = "errBtnOK";
            btnOK.CssClass = "button";
            btnOK.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("ok"));
            content.Controls.Add(btnOK);

            LinkButton lnkClose = new LinkButton();
            lnkClose.CssClass = "close";
            lnkClose.ID = "errLnkClose";
            lnkClose.OnClientClick = "closeErrorModal();return false;";
            content.Controls.Add(lnkClose);

            return content;
        }

        /// <summary>
        /// Gets the button controls for movie modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetButtonControlsForMovieModalPopup()
        {
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.ID = "movieBaseDiv";
            content.Attributes.Add("class", "base");

            LinkButton btnOK = new LinkButton();
            btnOK.ID = "movieBtnOK";
            btnOK.CssClass = "button";
            btnOK.Text = string.Format("<span>{0}</span>",ResourceText.GetInstance.GetString("ok"));
            content.Controls.Add(btnOK);

            LinkButton lnkClose = new LinkButton();
            lnkClose.CssClass = "close";
            lnkClose.ID = "movieLnkClose";
            lnkClose.OnClientClick = "closeMovieModal();return false;";
            lnkClose.Text = "X&nbsp;" + ResourceText.GetInstance.GetString("close");
            content.Controls.Add(lnkClose);

            return content;
        }

        /// <summary>
        /// Gets the modal popup control.
        /// </summary>
        /// <returns></returns>
        private static Control GetErrorModalPopupControl()
        {
            Panel modalControl = new Panel();
            modalControl.ID = "programmaticErrorPopup";
            modalControl.CssClass = "confirm-dialog";
            modalControl.Attributes.Add("style", "display:none");
            modalControl.Controls.Add(GetContentForErrorModalPopup());
            return modalControl;
        }

        /// <summary>
        /// Gets the movie modal popup control.
        /// </summary>
        /// <returns></returns>
        private static Control GetMovieModalPopupControl()
        {
            Panel modalControl = new Panel();
            modalControl.ID = "programmaticMoviePopup";
            modalControl.CssClass = "movie-dialog";
            modalControl.Attributes.Add("style", "display:none");
            modalControl.Controls.Add(GetContentForMovieModalPopup());
            return modalControl;
        }

        /// <summary>
        /// Gets the hidden target control form modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetHiddenTargetControlForErrorModalPopup()
        {
            LinkButton linkButton = new LinkButton();
            linkButton.ID = "hiddenTargetControlForErrorModalPopup";
            linkButton.Attributes.Add("style", "display:none");
            return linkButton;
        }

        /// <summary>
        /// Gets the hidden target control form modal popup.
        /// </summary>
        /// <returns></returns>
        private static Control GetHiddenTargetControlForModalMoviePopup()
        {
            LinkButton linkButton = new LinkButton();
            linkButton.ID = "hiddenTargetControlForMovieModalPopup";
            linkButton.Attributes.Add("style", "display:none");
            return linkButton;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Added Extenders
            Controls.Add(GetAjaxErrorModalExtenderControl());
            Controls.Add(GetAjaxMovieModalExtenderControl());
            Controls.Add(GetAjaxProxyCredExtenderControl());
            Controls.Add(GetAjaxConfirmUpdateModalExtenderControl());
            Controls.Add(GetAjaxCodeModalExtenderControl());
            Controls.Add(GetAjaxModalConfirmDeleteExtenderControl());

            // Hidden Controls
            Controls.Add(GetWidgetIdHiddentInputControl());
            Controls.Add(GetFolderIdsHiddenInputControl());
            Controls.Add(GetWidgetManagementPageSizeHiddenInputControl());
            Controls.Add(GetWidgetManagementPageHiddenInputControl());
            Controls.Add(GetWidgetManagementSortOrderHiddenInputControl());
            Controls.Add(GetWidgetManagement_SA_FROM_HiddenInputControl());
            Controls.Add(GetSortableTabsHiddenInputControl());

            //Controls.Add(GetSAFromHiddentInputControl());
            Controls.Add(GetReturnUrlInputControl());
            Controls.Add(GetWidgetManagementActionInputControl());
            Controls.Add(GetHiddenTargetControlForErrorModalPopup());
            Controls.Add(GetHiddenTargetControlForModalMoviePopup());
            Controls.Add(GetHiddenTargetControlForModalPopup());
            Controls.Add(GetHiddenTargetControlForProxyCredModalPopup());
            Controls.Add(GetHiddenTargetControlForConfirmModalPopup());

            ((BasePage) Page).AjaxToolkitHiddenControlsContainer.Controls.Add(GetCodeModalPopupControl());
            ((BasePage) Page).AjaxToolkitHiddenControlsContainer.Controls.Add(GetErrorModalPopupControl());
            ((BasePage) Page).AjaxToolkitHiddenControlsContainer.Controls.Add(GetMovieModalPopupControl());
            ((BasePage) Page).AjaxToolkitHiddenControlsContainer.Controls.Add(GetProxyCredModalPopupControl());
            ((BasePage) Page).AjaxToolkitHiddenControlsContainer.Controls.Add(GetConfirmUpdateModalPopupControl());
            ((BasePage) Page).AjaxToolkitHiddenControlsContainer.Controls.Add(GetConfirmDeleteModalPopupControl());

            base.OnPreRender(e);
        }
    }
}