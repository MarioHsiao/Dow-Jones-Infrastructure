using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EMG.Utility.Attributes;
using EMG.widgets.ui.exception;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Membership.ExternalReader.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Services.V1_0;
using factiva.nextgen;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.controls.display
{
    /// <summary>
    /// 
    /// </summary>
    public class AudienceControl : AbstractWidgetDesignerControl
    {
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");
        private const string OptionFormat = "{0}: {1} | {2} | {3}";
        private const string LegendFormat = "<div class=\"audienceLegend\">{0}: {1}: {2} | {3} | {4}</div>";


        /// <summary>
        /// Initializes a new instance of the <see cref="AudienceControl"/> class.
        /// </summary>
        public AudienceControl()
            : base("div", "3")
        {
            base.ID = "wdgtAudienceCntrl";
            m_container.ID = string.Concat(base.ID, "Cnt");
            Attributes.CssStyle.Add("display", "none");
            base.Controls.Add(m_container);
        }

        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        private static Control GetAudienceControl()
        {
            HtmlGenericControl audienceControl = new HtmlGenericControl("div");
            audienceControl.Attributes.Add("class", "directionContainer");
            audienceControl.ID = "createWidgetAudienceControl";
            audienceControl.Controls.Add(GetBaselineAudienceTable());
            return audienceControl;
        }

        private static Control GetBaselineAudienceTable()
        {
            // Initialize a basic control
            HtmlTable table = new HtmlTable();
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Border = 0;
            table.Rows.Add(GetInternalAccountAudienceRow());
            table.Rows.Add(GetExternalAccountAudienceRow());
            if (SessionData.Instance().FactivaAccessObject.IsAllowedToSetTTLProxyCredentials)
            {
                table.Rows.Add(GetProxyCredentialsAudienceRow());
            }
            if (SessionData.Instance().FactivaAccessObject.IsAllowedToSetExternalReaderProfiles)
            {
                if (CheckExternalReaderPreference() || SessionData.Instance().FactivaAccessObject.IsAccountAdministrator){
                    DropDownList list = GetClientProfiles();
                    if (list.Items.Count > 0)
                    {
                        table.Rows.Add(GetExternalReaderAudienceRow(list));
                        table.Rows.Add(GetExternalReaderLegendRow());
                    }
                }
            }
            return table;
        }

        private static bool CheckExternalReaderPreference()
        {
            try
            {
                GetItemsByClassIDRequest request = new GetItemsByClassIDRequest();
                request.ClassID = new PreferenceClassID[] { PreferenceClassID.AdminExternalReader };

                PreferenceResponse preferenceResponse = PreferenceService.GetItemsByClassID(ControlDataManager.Clone(SessionData.Instance().SessionBasedControlDataEx), request);
                return preferenceResponse.AdminExternalReader.CanRedistributeResults;
            }
            catch (Exception ex)
            {
                new EmgWidgetsUIException(ex, -1);
            }
            return false;
        }

        private static HtmlTableRow GetExternalReaderLegendRow()
        {
            HtmlTableRow row = new HtmlTableRow();

            HtmlTableCell emptyCell = new HtmlTableCell();

            HtmlTableCell directionsCell = new HtmlTableCell();
            directionsCell.Controls.Add(
                    new LiteralControl(
                        string.Format(LegendFormat,
                            ResourceText.GetInstance.GetString("legend"),
                            ResourceText.GetInstance.GetString("profile"),
                            ResourceText.GetInstance.GetString("seatsAllocated"),
                            ResourceText.GetInstance.GetString("seatsRegistered"),
                            ResourceText.GetInstance.GetString("available"))));
            row.Cells.Add(emptyCell);
            row.Cells.Add(directionsCell);
            return row;
        }

        private static HtmlTableRow GetInternalAccountAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            string id = "audience_" + ((int)WidgetDistributionType.OnlyUsersInMyAccount);
            row.Cells.Add(GetRadioCell(string.Empty, id, "audience", WidgetDistributionType.OnlyUsersInMyAccount, true));
            row.Cells.Add(GetAssignedTokenCell(id, string.Empty, WidgetDistributionType.OnlyUsersInMyAccount));
            return row;
        }

        private static HtmlTableRow GetExternalAccountAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            string id = "audience_" + ((int)WidgetDistributionType.UsersOutsideMyAccount);
            row.Cells.Add(GetRadioCell(string.Empty, id, "audience", WidgetDistributionType.UsersOutsideMyAccount, false));
            row.Cells.Add(GetAssignedTokenCell(id, string.Empty, WidgetDistributionType.UsersOutsideMyAccount));
            return row;
        }

        private static HtmlTableRow GetProxyCredentialsAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            string id = "audience_" + ((int)WidgetDistributionType.TTLProxyAccount);
            row.Cells.Add(GetRadioCell(string.Empty, id, "audience", WidgetDistributionType.TTLProxyAccount, false));
            row.Cells.Add(GetTTLProxyAccountCell(id, string.Empty));
            return row;
        }

        private static HtmlTableRow GetExternalReaderAudienceRow(DropDownList list)
        {
            HtmlTableRow row = new HtmlTableRow();
            string id = "audience_" + ((int)WidgetDistributionType.ExternalReader);
            row.Cells.Add(GetRadioCell(string.Empty, id, "audience", WidgetDistributionType.ExternalReader, false));
            row.Cells.Add(GetExternalReaderCell(id, string.Empty, list));
            return row;
        }

        private static HtmlTableCell GetRadioCell(string className, string id, string name, WidgetDistributionType value, bool isChecked)
        {
            HtmlTableCell cell = new HtmlTableCell("td");
            cell.Attributes.Add("class", className);
            HtmlInputRadioButton radioButton = new HtmlInputRadioButton();
            radioButton.ID = id;
            radioButton.Name = name;
            radioButton.Value = ((int)value).ToString();
            radioButton.Checked = isChecked;
            cell.Controls.Add(radioButton);
            return cell;
        }

        private static HtmlTableCell GetAssignedTokenCell(string labelId, string className, WidgetDistributionType value)
        {
            HtmlTableCell cell = new HtmlTableCell("td");
            cell.Width = "100%";
            cell.Attributes.Add("class", className);
            cell.InnerHtml = string.Format("<label for=\"{0}\">{1}</label>", labelId, GetAssignedToken(value));
            return cell;
        }

        private static HtmlTableCell GetExternalReaderCell(string labelId, string className, Control list)
        {
            HtmlTableCell cell = new HtmlTableCell("td");
            cell.Attributes.Add("class", className);

            HtmlGenericControl label = new HtmlGenericControl("label");
            label.Attributes.Add("for", labelId);

            label.Controls.Add(new LiteralControl(GetAssignedToken(WidgetDistributionType.ExternalReader) + " "));
            label.Controls.Add(list);
            cell.Controls.Add(label);
            return cell;
        }

        private static HtmlTableCell GetTTLProxyAccountCell(string labelId, string className)
        {
            HtmlTableCell cell = new HtmlTableCell("td");
            cell.Attributes.Add("class", className);

            HtmlGenericControl label = new HtmlGenericControl("label");
            label.Attributes.Add("for", labelId);

            label.Controls.Add(new LiteralControl(GetAssignedToken(WidgetDistributionType.TTLProxyAccount) + " "));
            label.Controls.Add(GetSetProxyCredentialsLink());
            cell.Controls.Add(label);
            return cell;
        }

        private static string GetAssignedToken(WidgetDistributionType value)
        {
            Type enumType = typeof(WidgetDistributionType);
            string s = value.ToString();
            AssignedToken assignedToken = (AssignedToken)Attribute.GetCustomAttribute(enumType.GetField(s), typeof(AssignedToken));
            return assignedToken != null ? ResourceText.GetInstance.GetString(assignedToken.Token) : string.Empty;
        }

        private static Control GetSetProxyCredentialsLink()
        {
            // Right part of title
            HtmlGenericControl directionsControl = new HtmlGenericControl("span");
            directionsControl.Attributes.Add("class", "proxyCredentialsContainer");
            directionsControl.ID = "proxyCredentialsControl";
            directionsControl.InnerHtml = string.Format("<a id=\"showModalPopupProxyCredButton\" href=\"javascript:void(0)\" onclick=\"getProxyCredentialsPopup();return false;\">{0}</a>", ResourceText.GetInstance.GetString("setProxyCredentials"));
            return directionsControl;
        }

        private static DropDownList GetClientProfiles()
        {
            ListItemCollection lc = new ListItemCollection();

            WidgetManager widgetManager = new WidgetManager(SessionData.Instance().SessionBasedControlDataEx, SessionData.Instance().InterfaceLanguage);
            ProfileCollection profiles = widgetManager.GetExternalReaderProfiles();

            if (profiles != null)
            {
                foreach (Profile profile in profiles)
                {
                    lc.Add(
                        new ListItem(string.Format(OptionFormat, profile.Properties.Name,
                           profile.Properties.MaxNumberOfSeats,
                           profile.Properties.RegisteredNumberOfSeats,
                           (profile.Properties.MaxNumberOfSeats -
                            profile.Properties.RegisteredNumberOfSeats)), profile.Id)
                        );
                }
            }

            DropDownList ddList = new DropDownList();
            ddList.Attributes.Add("onclick", "setExternalReaderAudience();return false;");

            ddList.ID = "profileId";
            ddList.DataSource = lc;
            ddList.DataTextField = "Text";
            ddList.DataValueField = "Value";
            ddList.DataBind();
            return ddList;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            m_container.Controls.Add(GetTitleControl(ResourceText.GetInstance.GetString("audience")));
            m_container.Controls.Add(GetAudienceControl());
        }
    }
}
