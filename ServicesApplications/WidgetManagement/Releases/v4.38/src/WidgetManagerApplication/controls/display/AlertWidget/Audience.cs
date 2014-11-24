using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.display.AlertWidget
{
    /// <summary>
    /// 
    /// </summary>
    public class Audience : BaseControl
    {
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");

        /// <summary>
        /// Initializes a new instance of the <see cref="Audience"/> class.
        /// </summary>
        public Audience() : base("div")
        {
            ID = "alrtWdgtAudienceCntrl";
            m_container.ID = string.Concat(ID, "Cnt");
            Attributes.CssStyle.Add("display", "none");
            Controls.Add(m_container);
        }

        /// <summary>
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        private static Control GetTitleControl()
        {
            HtmlGenericControl displayTitle = new HtmlGenericControl("div");
            displayTitle.Attributes.Add("class", "sectionTitle");
            displayTitle.ID = "codeGenTitle";
            displayTitle.InnerText = string.Format("4. {0}", ResourceText.GetInstance.GetString("codeGeneration"));
            return displayTitle;
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
            audienceControl.InnerHtml = string.Format("{0}", ResourceText.GetInstance.GetString("createWidgetDirections"));
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
            table.Rows.Add(GetProxyCredentialsAudienceRow());
            table.Rows.Add(GetExternalReaderAudienceRow());

            return table;
        }


        private static HtmlTableRow GetInternalAccountAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(GetRadioCell(string.Empty,"internalAudience","audience","Internal"));
            return row;
        }

        private static HtmlTableRow GetExternalAccountAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(GetRadioCell(string.Empty, "internalAudience", "audience", "External"));
            return row;
        }

        private static HtmlTableRow GetProxyCredentialsAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(GetRadioCell(string.Empty, "internalAudience", "audience", "Proxy"));
            return row;
        }

        private static HtmlTableRow GetExternalReaderAudienceRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            row.Cells.Add(GetRadioCell(string.Empty, "internalAudience", "audience", "Reader"));
            return row;
        }

        private static HtmlTableCell GetRadioCell(string className, string id, string name, string value)
        {
            HtmlTableCell cell = new HtmlTableCell("td");
            cell.Attributes.Add("class", className);
            HtmlInputRadioButton button = new HtmlInputRadioButton();
            button.ID = id;
            button.Name = name;
            button.Value = value;
            cell.Controls.Add(button);
            return cell;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            m_container.Controls.Add(GetTitleControl());
            m_container.Controls.Add(GetAudienceControl());
        }
    }
}
