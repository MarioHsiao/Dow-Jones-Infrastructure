using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.com.FactivaCharting;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.page;
using BasePage=EMG.widgets.ui.page.BasePage;

namespace EMG.widgets.ui.test
{
    [ClientScript("~/js/flashobject.js", 0)]
    public partial class Corda :  BasePage
    {
        private readonly string baseData = "<GraphData Name=\"graph\" Method=\"Replace\"><Series name=\"temp\"> <Data Value=\"1463\" Popup=\"Motor Vehicles\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai351&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"1453\" Popup=\"Passenger Cars\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai35101&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"1061\" Popup=\"Broadcasting\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai97411&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"854\" Popup=\"Banking/Credit\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3aibnk&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"712\" Popup=\"Real Estate Transactions\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai85&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"542\" Popup=\"Energy\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai1&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"521\" Popup=\"Health Care\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai951&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"512\" Popup=\"Crude Oil/Natural Gas\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai13&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"503\" Popup=\"Insurance\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai82&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /><Data Value=\"501\" Popup=\"Automobiles\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3aiaut&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" color=\"\" /></Series><Categories> <Category Name=\"Motor Vehicles\" Popup=\"Motor Vehicles\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai351&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Passenger Cars\" Popup=\"Passenger Cars\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai35101&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Broadcasting\" Popup=\"Broadcasting\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai97411&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Banking/Credit\" Popup=\"Banking/Credit\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3aibnk&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Real Estate Transactions\" Popup=\"Real Estate Transactions\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai85&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Energy\" Popup=\"Energy\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai1&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Health Care\" Popup=\"Health Care\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai951&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Crude Oil/Natural Gas\" Popup=\"Crude Oil/Natural Gas\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai13&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Insurance\" Popup=\"Insurance\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3ai82&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /><Category Name=\"Automobiles\" Popup=\"Automobiles\" DrilldownURL=\"/results/default.aspx?ss=bush&csa=15364&fid=0&fr=0&thm=-1&md=5&hso=0&sc=0&sq=0&dq=0&ct=1&dp=0&rrst=0&fs=in%3aiaut&slst=&ckey=OVK9XF2D4DT8HNST&uxfr=0&ddlvl=0&fhrcoll=&dbg=3\" /></Categories></GraphData>";

        /// <summary>
        /// 
        /// </summary>
        protected static Regex NoScriptRegex = new Regex(
           @"<noscript[^>]*(?<obj>\/>|>[\S\s]*?)<\/noscript>",
           RegexOptions.IgnoreCase
           | RegexOptions.Multiline
           | RegexOptions.Singleline
           | RegexOptions.ExplicitCapture
           | RegexOptions.Compiled
           );

        /// <summary>
        /// 
        /// </summary>
        protected static Regex CommentRegex = new Regex(
            @"<!--*(?:[\S\s]*?)-->",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.Singleline
            | RegexOptions.ExplicitCapture
            | RegexOptions.Compiled
            );

        /// <summary>
        /// 
        /// </summary>
        protected static Regex ScriptRegex = new Regex(
            @"<script[^>]*(?<obj>\/>|>[\S\s]*?)<\/script>",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.Singleline
            | RegexOptions.ExplicitCapture
            | RegexOptions.Compiled
            );
        /// <summary>
        /// Initializes a new instance of the <see cref="Corda"/> class.
        /// </summary>
        public Corda()
        {
            OverrideDefaultPrefix = true;
            // Do not validate let the next page do the work.
            base.ValidateSessionId = true;
        }
        /// <summary>
        /// Handles the PreInit event of the Pate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Pate_PreInit(object sender, EventArgs e)
        {
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = QuerystringManager.GetLocalUrl("~/test/corda.aspx");


            ContentLeft.Controls.Add(GetFlashCordaImage());
            ContentLeft.Controls.Add(GetPNGScriptInteractiveCordaImage());
            ContentLeft.Controls.Add(GetPNGCordaImage());

            //ContentRight.Controls.Add(new LiteralControl("&nbsp;"));
        }


        private Control GetFlashCordaImage()
        {
            HtmlGenericControl _baseControl = new HtmlGenericControl("div");
            
            HtmlGenericControl _titleControl = new HtmlGenericControl("div");
            _titleControl.InnerText = "Corda Flash";

            HtmlGenericControl _dataContainer = new HtmlGenericControl("div");
            ChartTemplate chartTemplate = Config.GetChartTemplate("HorizontalBar_10");

            string output = ChartingService.GetChartByPCXML(baseData, chartTemplate, true);
            _dataContainer.Controls.Add(new LiteralControl(output));

            HtmlGenericControl _data = new HtmlGenericControl("div");
            _data.InnerText = output;
            _baseControl.Controls.Add(_titleControl);
            _baseControl.Controls.Add(_dataContainer);
            _baseControl.Controls.Add(_data);
            _baseControl.Controls.Add(new LiteralControl("<hr/><br/>"));

            return _baseControl;
        }

        private Control GetPNGCordaImage()
        {
            HtmlGenericControl _baseControl = new HtmlGenericControl("div");

            HtmlGenericControl _titleControl = new HtmlGenericControl("div");
            _titleControl.InnerText = "Corda PNG : Map/Img";

            HtmlGenericControl _dataContainer = new HtmlGenericControl("div");
            ChartTemplate chartTemplate = Config.GetChartTemplate("HorizontalBar_9");
            chartTemplate.chartType = ChartType.JPEG;
            
            string output = StripJavscript(ChartingService.GetChartByPCXML(baseData, chartTemplate, true));
            _dataContainer.Controls.Add(new LiteralControl(output));

            HtmlGenericControl _data = new HtmlGenericControl("div");
            _data.InnerText = output;
            _baseControl.Controls.Add(_titleControl);
            _baseControl.Controls.Add(_dataContainer);
            _baseControl.Controls.Add(_data);
            _baseControl.Controls.Add(new LiteralControl("<hr/><br/>"));

            return _baseControl;
        }

        private Control GetPNGScriptInteractiveCordaImage()
        {
            HtmlGenericControl _baseControl = new HtmlGenericControl("div");
            
            HtmlGenericControl _titleControl = new HtmlGenericControl("div");
            _titleControl.InnerText = "Corda PNG : Interactive/Map/Img";

            HtmlGenericControl _dataContainer = new HtmlGenericControl("div");
            ChartTemplate chartTemplate = Config.GetChartTemplate("HorizontalBar_10");
            chartTemplate.chartType = ChartType.PNG;
            
            string output = ChartingService.GetChartByPCXML(baseData, chartTemplate, true);
            _dataContainer.Controls.Add(new LiteralControl( output ));
            
             HtmlGenericControl _data = new HtmlGenericControl("div");
            _data.InnerText = output;
            _baseControl.Controls.Add(_titleControl);
            _baseControl.Controls.Add(_dataContainer);
            _baseControl.Controls.Add(_data);
            _baseControl.Controls.Add(new LiteralControl("<hr/><br/>"));

            return _baseControl;
        }

        private static string StripJavscript(string input)
        {
            string original = new string(input.ToCharArray());
            input = ScriptRegex.Replace(input, string.Empty);
            input = CommentRegex.Replace(input, string.Empty);
            input = NoScriptRegex.Replace(input, string.Empty);


            StringBuilder sb = new StringBuilder(input);
            Match temp = NoScriptRegex.Match(original);
            if (temp.Success && temp.Groups.Count > 0)
            {
                sb.Append(temp.Groups["obj"].Value.Trim());
            }
            return sb.ToString();
        }
    }
}
