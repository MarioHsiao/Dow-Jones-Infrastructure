using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Factiva.BusinessLayerLogic.DataTransferObject.Widget;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.controls.basic;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.page;

namespace EMG.widgets.ui.controls.design
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AlertWidgetDesignControl : BaseControl
    {
        private const string m_LabelFormat = "{0}:";
        private readonly HtmlGenericControl m_container = new HtmlGenericControl("div");
        private WidgetManagementDTO m_WidgetManagementDTO = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertWidgetDesignControl"/> class.
        /// </summary>
        public AlertWidgetDesignControl() : base("div")
        {
            ID = "alrtWdgtDsgnCntrl";
            m_container.ID = string.Concat(ID, "Cnt");
            Controls.Add(m_container);
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
        /// Gets the title control.
        /// </summary>
        /// <returns></returns>
        private static Control GetTitleControl()
        {
            HtmlGenericControl designTitle = new HtmlGenericControl("div");
            designTitle.Attributes.Add("class", "sectionTitle");
            designTitle.ID = "designTitle";
            designTitle.InnerText = string.Format("1. {0}", ResourceText.GetInstance.GetString("design"));
            return designTitle;
        }

        

        /// <summary>
        /// Gets the design control container.
        /// </summary>
        /// <returns></returns>
        private static Control GetDesignControlContainer()
        {
            HtmlGenericControl designControlContainer = new HtmlGenericControl("div");
            designControlContainer.Attributes.Add("class", "designCntrlsCntr");
            designControlContainer.ID = "designControlsContainer";
            designControlContainer.Controls.Add(GetDesignerTable());
            return designControlContainer;
        }

        /// <summary>
        /// Gets the designer table.
        /// </summary>
        /// <returns></returns>
        private static Table GetDesignerTable()
        {
            Table tbl = new Table();
            tbl.BorderStyle = BorderStyle.None;
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;
            tbl.CssClass = "designerControlsTbl";
            tbl.Rows.Add(GetWidgetNameTableRow());
            tbl.Rows.Add(GetWidgetHeadlinesDisplayTypeTableRow());
            tbl.Rows.Add(GetWidgetNumberOfHeadlinesTableRow());
            tbl.Rows.Add(GetWidgetFontFamilyTableRow());
            tbl.Rows.Add(GetWidgetFontSizeTableRow());
            tbl.Rows.Add(GetColorPickerEnabledTableRow("mainColor", ResourceText.GetInstance.GetString("mainColor"), string.Empty));
            tbl.Rows.Add(GetColorPickerEnabledTableRow("mainFontColor", ResourceText.GetInstance.GetString("mainFontColor"), string.Empty));
            tbl.Rows.Add(GetColorPickerEnabledTableRow("accentColor", ResourceText.GetInstance.GetString("accentColor"), string.Empty));
            tbl.Rows.Add(GetColorPickerEnabledTableRow("accentFontColor", ResourceText.GetInstance.GetString("accentFontColor"), string.Empty));
            tbl.Rows.Add(GetGetWidgetDistributionTypeTableRow());

            return tbl;
        }

        /// <summary>
        /// Gets the font family drop down list.
        /// </summary>
        /// <returns></returns>
        private static DropDownList GetFontFamilyDropDownList()
        {
            return factiva.nextgen.ui.Utility.GetDropDownBaseOnEnumAndAssigendString("fFmly", typeof(WidgetFontFamily), string.Empty, string.Empty);
        }


        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static Control GetTextBox(string id, string value)
        {
            TextBox textBox = new TextBox();
            textBox.ID = id + "_input";
            textBox.CssClass = "colorPickerTextBox";
            textBox.Text = !string.IsNullOrEmpty(value) ? value : string.Empty;
            return textBox;
        }

        /// <summary>
        /// Gets the color picker link.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <returns></returns>
        private static Control GetColorPickerLink(string id, string backgroundColor)
        {
            HtmlGenericControl colorPicker = new HtmlGenericControl("div");
            colorPicker.Attributes.Add("class", "colorPicker");
            HtmlAnchor anchor = new HtmlAnchor();
            anchor.ID = id;
            anchor.HRef = "javascript:void(0)";
            anchor.Attributes.Add("onclick", "showColorPicker(this);return false;");
            anchor.Attributes.Add("class", "colorIcon");
            anchor.Attributes.Add("style", string.Format("background-color:{0};color:{0}", backgroundColor));
            anchor.Title = ResourceText.GetInstance.GetString("getColorPickerTool");
            anchor.InnerHtml = string.Format("<span class=\"chip\">{0}</span>", backgroundColor.Replace("#", string.Empty));
            colorPicker.Controls.Add(anchor);
            return colorPicker;
        }

        /// <summary>
        /// Gets the font size drop down list.
        /// </summary>
        /// <returns></returns>
        private static DropDownList GetFontSizeDropDownList()
        {
            return factiva.nextgen.ui.Utility.GetDropDownBaseOnEnum("fSize", typeof(WidgetFontSize), string.Empty, string.Empty);
        }

        /// <summary>
        /// Gets the number of headlines drop down list.
        /// </summary>
        /// <returns></returns>
        private static DropDownList GetNumberOfHeadlinesDropDownList()
        {
            int[] numOfHeadlines = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            return factiva.nextgen.ui.Utility.GetDropDownBaseOnIntArray("numHdlnes", numOfHeadlines, 0, string.Empty);
        }

        /// <summary>
        /// Gets the widget headline display type radio button list.
        /// </summary>
        /// <returns></returns>
        private static RadioButtonList GetWidgetHeadlineDisplayTypeRadioButtonList()
        {
            return factiva.nextgen.ui.Utility.GetRadioButtonListBasedOnEnum("hdlnDspl", typeof(WidgetHeadlineDisplayType), WidgetHeadlineDisplayType.HeadlinesWithSnippets.ToString());
        }

        /// <summary>
        /// Gets the type of the widget distribution.
        /// </summary>
        /// <returns></returns>
        private static RadioButtonList GetWidgetDistributionType()
        {
            return factiva.nextgen.ui.Utility.GetRadioButtonListBasedOnEnum("dstrbtnTyp", typeof (WidgetDistributionType), WidgetDistributionType.OnlyUsersInMyAccount.ToString());
        }


        /// <summary>
        /// Gets the color picker enabled table row.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="labelText">The label text.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        private static TableRow GetColorPickerEnabledTableRow(string id, string labelText, string color)
        {
            TableRow tr = new TableRow();
            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, labelText)));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetTextBox(id, color));
            if (string.IsNullOrEmpty(color))
            {
                color = "#FFFFFF";
            }
            rightCell.Controls.Add(GetColorPickerLink(id, color));
            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget number of headlines table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetNumberOfHeadlinesTableRow()
        {
            TableRow tr = new TableRow();
            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, ResourceText.GetInstance.GetString("numOfHeadlines"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetNumberOfHeadlinesDropDownList());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget font family table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetFontFamilyTableRow()
        {
            TableRow tr = new TableRow();
            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, ResourceText.GetInstance.GetString("fontFamily"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetFontFamilyDropDownList());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget font size table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetFontSizeTableRow()
        {
            TableRow tr = new TableRow();
            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, ResourceText.GetInstance.GetString("fontSize"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetFontSizeDropDownList());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the get widget distribution type table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetGetWidgetDistributionTypeTableRow()
        {
            TableRow tr = new TableRow();

            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, ResourceText.GetInstance.GetString("audience"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetWidgetDistributionType());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget headlines display type table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetHeadlinesDisplayTypeTableRow()
        {
            TableRow tr = new TableRow();

            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, ResourceText.GetInstance.GetString("viewResults"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetWidgetHeadlineDisplayTypeRadioButtonList());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget name table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetNameTableRow()
        {
            TableRow tr = new TableRow();

            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, ResourceText.GetInstance.GetString("widgetName"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";

            TextBox wNameTextBox = new TextBox();
            wNameTextBox.ID = "wName";
            wNameTextBox.CssClass = "unwatermarked";
            wNameTextBox.MaxLength = 50;

            rightCell.Controls.Add(wNameTextBox);

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private static Control GetColorPicker()
        {
            ColorPicker colorpicker = new ColorPicker();
            return colorpicker;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            m_container.Controls.Add(GetTitleControl());
            m_container.Controls.Add(GetDesignControlContainer());
            ((BasePage) Page).AjaxContainer.Controls.Add(GetColorPicker());
            // Add the AjaxToolkit controls here
            //Controls.Add(GetTextBoxWatermarkExtender());
            //Controls.Add(InitJavscriptHandlers());
            base.OnPreRender(e);
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            base.RenderChildren(writer);
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}