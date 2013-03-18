using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Managers.Search.CodedNewsQueries;

namespace DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList
{
    public class RealtimeHeadlineListDataResultRenderManager
    {
        private readonly HtmlTextWriter _tWriter;
        private readonly StringWriter _strWriter;
        private readonly StringBuilder _renderedOutput;
        private readonly HeadlineListDataResult _cResult;

        public TruncationType TruncationType { get; set; }
        public bool DisplayEntities { get; set; }
        public bool DisplayHeadlineTooltip { get; set; }
        public string DateTimeFormatingPreference { get; set; }
        public int ContainerWidth { get; set; }

        public RealtimeHeadlineListDataResultRenderManager(HeadlineListDataResult result)
        {
            _renderedOutput = new StringBuilder();
            _strWriter = new StringWriter(_renderedOutput);
            _tWriter = new HtmlTextWriter(_strWriter);
            _cResult = result;
        }

        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = string.IsNullOrWhiteSpace(value) ? "RealtimeHeadlineList" : value; }
        }

        public string AlertContext { get; set; }

        public int MaxHeadlinesToReturn { get; set; }

        public ClockType ClockType { get; set; }

        private RealtimeHeadlineListTokens _tokens = new RealtimeHeadlineListTokens();
        public RealtimeHeadlineListTokens Tokens
        {
            get { return _tokens; }
            set { _tokens = value; }
        }

        private void RenderHeadline(HeadlineInfo headline)
        {
            _tWriter.Write("<li class=\"{0} dj_emg_rtheadline_entry\" ref=\"{0}\">", headline.reference.guid);
            _tWriter.Write("<div class=\"headline-container\">");
            _tWriter.Write("<div class=\"headline-timestamp \">{0}</div>", headline.publicationDateTimeDescriptor);
            //Render Title
            RenderTitle(headline);
            _tWriter.Write("</div>");
            _tWriter.Write("</li>");
        }

        private void RenderTitle(HeadlineInfo headline)
        {
            var href = headline.reference.externalUri ?? "javascript:void(0)";

            _tWriter.Write("<div class=\"headline\"><a href=\"{0}\" class=\"dj_emg_rt_headlineTitle\">{1}</a></div>",
                href,
                GetTitle(headline)
                );
        }

        private void RenderHeaderControls()
        {
            _tWriter.Write("<div class=\"dg_emg_rt_headline_header\">");
            _tWriter.Write("<div class=\"dj_emg_rt_menucontrols\">");
            _tWriter.Write("<div class=\"header_menu_right\">");
            //_tWriter.Write(
            //    "<div class=\"viewAll\"><a class=\"dj_emg_rt_viewAll\" href=\"javascript:void(0)\">{0}</a></div>",
            //    Tokens.viewAllTkn);
            _tWriter.Write("<div class=\"play\"><a class=\"dj_emg_rt_play\" href=\"javascript:void(0)\"></a></div>");
            _tWriter.Write("<div class=\"pause_blue\"><a class=\"dj_emg_rt_pause\" href=\"javascript:void(0)\"></a></div>");
            _tWriter.Write("<div class=\"previous\"><a class=\"dj_emg_rt_previous\" href=\"javascript:void(0)\"></a></div>");
            _tWriter.Write("<div class=\"next\"><a class=\"dj_emg_rt_next\" href=\"javascript:void(0)\"></a></div>");
            _tWriter.Write("</div>");
            _tWriter.Write("<a class=\"dj_emg_rt_menu_header\">{0}</a>", Tokens.controlTitleTkn);
            _tWriter.Write("</div>");
            _tWriter.Write("</div>");
        }

        private void RenderFooterControls()
        {
            _tWriter.Write("<div class=\"dg_emg_rt_headline_footer\">");
            _tWriter.Write("<div class=\"dj_emg_rt_footercontrols\">");
            _tWriter.Write("<div class=\"footer_menu_right\">");
            _tWriter.Write("<div class=\"dj_emg_rt_queueCountText dj_emg_rt_text\">{0}:</div>", Tokens.queueTkn);
            _tWriter.Write("<div class=\"dj_emg_rt_queueCount dj_emg_rt_text\">0</div>");
            _tWriter.Write("<div class=\"refresh\"><a class=\"dj_emg_rt_refresh\" href=\"javascript:void(0)\"></a></div>");
            _tWriter.Write("</div>");
            _tWriter.Write("<div class=\"start-status\"></div>");
            _tWriter.Write("</div>");
            _tWriter.Write("</div>");
        }

        private void RenderHeadlineListControlHTML()
        {
            if (_cResult == null || _cResult.hitCount == null || _cResult.hitCount.Value <= 0)
            //if (_cResult == null || _cResult.hitCount == null)
                return;
            _tWriter.Write("<div id=\"dj_emg_rt_headlineNews\" style=\"width:" + Unit.Pixel(ContainerWidth) + " !important\">");
            //Render Menu Controls
            RenderHeaderControls();
            _tWriter.Write("<div id=\"nContainer\" class=\"dj_emg_rt_headlines\">");
            _tWriter.Write("<ol id=\"dj_emg_rt_headlineList\" class=\"listContainer\">");
            foreach (var info in _cResult.resultSet.headlines)
            {
                RenderHeadline(info);
            }
            _tWriter.Write("</ol>");
            _tWriter.Write("</div>");
            RenderFooterControls();
            _tWriter.Write("</div>");
        }

        private string GetTitle(HeadlineInfo headline)
        {
            var index = 0;
            var count = 0;
            var end = false;
            var sb = new StringBuilder();
            var prevtClassName = string.Empty;

            switch (TruncationType)
            {
                case TruncationType.XSmall:
                    if (headline.truncationRules.Extrasmall > 0)
                        index = headline.truncationRules.Extrasmall;
                    break;
                case TruncationType.Small:
                    if (headline.truncationRules.Small > 0)
                        index = headline.truncationRules.Small;
                    break;
                case TruncationType.Medium:
                    if (headline.truncationRules.Medium > 0)
                        index = headline.truncationRules.Medium;
                    break;
                case TruncationType.Large:
                    if (headline.truncationRules.Large > 0)
                        index = headline.truncationRules.Large;
                    break;
                default:
                    break;
            }
            foreach (var para in headline.title)
            {
                foreach (var item in para.items)
                {
                    var tClassName = "text";
                    if (DisplayEntities)
                    {
                        switch (item.type.ToLowerInvariant())
                        {
                            default:
                                tClassName = "dj_emg_text";
                                break;
                            case "highlight":
                                tClassName = "dj_emg_highlight";
                                break;
                            case "company":
                                tClassName = "dj_emg_company";
                                break;
                            case "person":
                                tClassName = "dj_emg_person";
                                break;
                        }
                    }
                    if (index > 0)
                    {
                        var temp = item.value.Split(' ');

                        for (var i = 0; i < temp.Length; i++)
                        {
                            if (count >= index && !end)
                            {
                                end = true;
                                sb.Append("</span><span class=\"dj_emg_space\">...</span>");
                                continue;
                            }
                            if (end)
                            {
                                continue;
                            }
                            count += temp[i].Length + 1;

                            // 2/8/10 added to limit number of elements rendered
                            if (i == 0)
                            {
                                sb.Append("<span class=\"" + tClassName + "\">" + temp[i]);
                            }
                            else if (prevtClassName != tClassName)
                            {
                                sb.Append("</span><span class=\"dj_emg_space\"> </span><span class=\"" + tClassName + "\">" + temp[i]);
                            }
                            else
                            {
                                sb.Append(" " + temp[i]);
                            }
                            prevtClassName = tClassName;
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<div class=\"{0}\"><span class=\"ellipsis_text\">{1}</span></div><span class=\"dj_emg_space\"> </span>", tClassName, item.value);
                    }
                }
            }
            return sb.ToString();
        }

        #region Implementation of IRenderManager

        public string GetHtml()
        {
            RenderHeadlineListControlHTML();
            _tWriter.Flush();
            _strWriter.Flush();
            return _renderedOutput.ToString();
        }

        #endregion
    }
}
