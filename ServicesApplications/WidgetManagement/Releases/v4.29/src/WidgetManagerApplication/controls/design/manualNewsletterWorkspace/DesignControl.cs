using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EMG.Toolkit.Web;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using factiva.nextgen.ui;
using EMG.widgets.ui.controls.basic;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.page;

namespace EMG.widgets.ui.controls.design.manualNewsletterWorkspace
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DesignControl : AbstractWidgetDesignerControl
    {
        private const string m_LabelFormat = "<label for=\"{0}\">{1}:</label>";
        private readonly HtmlGenericControl m_Container = new HtmlGenericControl("div");
        private WidgetManagementDTO m_WidgetManagementDTO; 

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignControl"/> class.
        /// </summary>
        public DesignControl() : base("div","1")
        {
            ID = "wdgtDsgnCntrl";
            m_Container.ID = string.Concat(ID, "Cnt");
            Controls.Add(m_Container);
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
            tbl.Rows.Add(GetWidgetNumberOfItemsPerSectionTableRow());
            tbl.Rows.Add(GetWidgetSelectTemplateTableRow());
            tbl.Rows.Add(GetWidgetSelectTemplateDescriptionTableRow());
            tbl.Rows.Add(GetWidgetFontFamilyTableRow());
            tbl.Rows.Add(GetWidgetFontSizeTableRow());
            tbl.Rows.Add(GetColorPickerEnabledTableRow("mainColor", ResourceText.GetInstance.GetString("mainColor"), string.Empty));
            tbl.Rows.Add(GetColorPickerEnabledTableRow("mainFontColor", ResourceText.GetInstance.GetString("mainFontColor"), string.Empty));
            tbl.Rows.Add(GetColorPickerEnabledTableRow("accentColor", ResourceText.GetInstance.GetString("accentColor"), string.Empty));
            tbl.Rows.Add(GetColorPickerEnabledTableRow("accentFontColor", ResourceText.GetInstance.GetString("accentFontColor"), string.Empty));
            //tbl.Rows.Add(GetGetWidgetDistributionTypeTableRow());

            return tbl;
        }

        /// <summary>
        /// Gets the font family drop down list.
        /// </summary>
        /// <returns></returns>
        private static DropDownList GetFontFamilyDropDownList()
        {
            return EMG.widgets.ui.utility.Utility.GetDropDownBaseOnEnum("fFmly", typeof(WidgetFontFamily), string.Empty, string.Empty);
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
        private static DropDownList GetNumberOfItemsPerSectionDropDownList()
        {
            int[] numOfHeadlines = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            return factiva.nextgen.ui.Utility.GetDropDownBaseOnIntArray("numHdlnes", numOfHeadlines, 0, string.Empty);
        }

        /// <summary>
        /// Gets the widget template drop down list.
        /// </summary>
        /// <returns></returns>
        private static DropDownList GetWidgetTemplateDropDownList()
        {
            return factiva.nextgen.ui.Utility.GetDropDownBaseOnEnum("selTemp", typeof(WidgetTemplate), string.Empty, string.Empty);
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
        /// Gets the color picker enabled table row.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="labelText">The label text.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        private static TableRow GetColorPickerEnabledTableRow(string id, string labelText, string color)
        {   
            if (string.IsNullOrEmpty(color))
            {
                color = "#FFFFFF";
            }
            
            TableRow tr = new TableRow();
            tr.ID = id + "Row";

            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, id + "_input", labelText)));

            HtmlGenericControl tContainer = new HtmlGenericControl("div");
            
            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            tContainer.Controls.Add(GetTextBox(id, color));

            ColorPickerExtender extender = new ColorPickerExtender();
            extender.ID = id + "_extender";
            extender.TargetControlID = id + "_input";
            extender.OnColorPickerUpdate = "onColorPickerUpdate";
            extender.AttachToBodyNode = true;
            extender.BehaviorID = id + "_input_behavior";
            tContainer.Controls.Add(extender);
            rightCell.Controls.Add(tContainer);
           
            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget number of headlines table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetNumberOfItemsPerSectionTableRow()
        {
            TableRow tr = new TableRow();
            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, "numHdlnes", ResourceText.GetInstance.GetString("numOfItemsPerSection"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetNumberOfItemsPerSectionDropDownList());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget select template table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetSelectTemplateTableRow()
        {
            TableRow tr = new TableRow();
            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, "selTemp", ResourceText.GetInstance.GetString("selectTemplate"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetWidgetTemplateDropDownList());

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
        }

        /// <summary>
        /// Gets the widget select template description table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetSelectTemplateDescriptionTableRow()
        {
            TableRow tr = new TableRow();
            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellDesc";
            rightCell.ColumnSpan = 2;
            rightCell.Text = string.Format("{0} \"{1}\".", ResourceText.GetInstance.GetString("selTempDesc"), ResourceText.GetInstance.GetString("editDesign"));

            tr.Controls.Add(rightCell);

            return tr;
        }

        /// <summary>
        /// Gets the widget font family table row.
        /// </summary>
        /// <returns></returns>
        private static TableRow GetWidgetFontFamilyTableRow()
        {
            TableRow tr = new TableRow();
            tr.ID = "fontFamilyRow";

            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, "fFmly",ResourceText.GetInstance.GetString("fontFamily"))));

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
            tr.ID = "fontSizeRow";

            TableCell leftCell = new TableCell();
            leftCell.CssClass = "designCellLbl";
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, "fSize", ResourceText.GetInstance.GetString("fontSize"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";
            rightCell.Controls.Add(GetFontSizeDropDownList());

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
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, "hdlnDspl_0", ResourceText.GetInstance.GetString("viewResults"))));

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
            leftCell.Controls.Add(new LiteralControl(string.Format(m_LabelFormat, "wName", ResourceText.GetInstance.GetString("widgetName"))));

            TableCell rightCell = new TableCell();
            rightCell.CssClass = "designCellCnts";

            TextBox wNameTextBox = new TextBox();
            wNameTextBox.ID = "wName";
            wNameTextBox.CssClass = "disabled";
            wNameTextBox.MaxLength = 50;
            rightCell.Controls.Add(wNameTextBox);

            tr.Cells.Add(leftCell);
            tr.Cells.Add(rightCell);
            return tr;
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
            m_Container.Controls.Add(GetTitleControl(ResourceText.GetInstance.GetString("design"), ResourceText.GetInstance.GetString("newsletterWidget")));
            m_Container.Controls.Add(GetDesignControlContainer());
            ((BasePage) Page).AjaxContainer.Controls.Add(GetColorPicker());
            // Add the AjaxToolkit controls here
            //Controls.Add(GetTextBoxWatermarkExtender());
            //Controls.Add(InitJavscriptHandlers());
            base.OnPreRender(e);
        }
    }
}