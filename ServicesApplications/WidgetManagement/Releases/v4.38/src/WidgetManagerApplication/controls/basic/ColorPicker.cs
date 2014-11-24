using System.Web.UI;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.basic
{
    /// <summary>
    /// UI Controls that outputs the basic html needed to render the colorpicker.
    /// </summary>
    public class ColorPicker : BaseControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker() : base("div")
        {
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div id=\"plugin\" onmousedown=\"HSVslide('drag','plugin',event)\" style=\"TOP: 37px; LEFT: 25px; Z-INDEX: 20;\">");
            writer.Write("<div id=\"plugCUR\"></div><div id=\"plugHEX\" onmousedown=\"stop=0; setTimeout('stop=1',100);\">333333</div><div id=\"plugCLOSE\" title=\"{0}\" onmousedown=\"toggle('plugin')\">X</div><br />",ResourceText.GetInstance.GetString("close"));
            writer.Write("<div id=\"SV\" onmousedown=\"HSVslide('SVslide','plugin',event)\" title=\"Saturation + Value\">");
            writer.Write("<div id=\"SVslide\" style=\"TOP: -4px; LEFT: -4px;\"><br /></div>");
            writer.Write("</div>");
            writer.Write("<form id=\"H\" onmousedown=\"HSVslide('Hslide','plugin',event)\" title=\"Hue\">");
            writer.Write("<div id=\"Hslide\" style=\"TOP: -7px; LEFT: -8px;\"><br /></div>");
            writer.Write("<div id=\"Hmodel\"></div>");
            writer.Write("</form>");
            writer.Write("</div>");
        }
    }
}