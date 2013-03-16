using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public abstract class GenericChart
    {
        private AJAXSource _ajaxDataSource;
        private Appearance _appearance;
        private ColorSet _colors;
        private Exporting _exporting;
        private Localization _lang;
        private Legend _legend;
        private SerieCollection _series;
        private SubTitle _subTitle;
        private Title _title;
        private ToolTip _toolTip;
        private XAxis _xAxis;
        private YAxis _yAxis;

        /// <summary>
        ///     An array containing the default colors for the chart's series.
        /// </summary>
        public virtual ColorSet Colors
        {
            get { return _colors ?? (_colors = new ColorSet()); }
            set { _colors = value; }
        }

        public virtual Localization Lang
        {
            get { return _lang ?? (_lang = new Localization()); }
            set { _lang = value; }
        }

        /// <summary>
        ///     By default, you can set: "skies", "grid", "gray", "dark-blue", "pink-floral" or "dark-green";
        /// </summary>
        public virtual string Theme { get; set; }

        public Appearance Appearance
        {
            get { return _appearance ?? (_appearance = new Appearance()); }
            set { _appearance = value; }
        }

        public virtual Legend Legend
        {
            get { return _legend ?? (_legend = new Legend()); }
            set { _legend = value; }
        }

        public virtual Exporting Exporting
        {
            get { return _exporting ?? (_exporting = new Exporting()); }
            set { _exporting = value; }
        }

        public virtual ToolTip Tooltip
        {
            get { return _toolTip ?? (_toolTip = new ToolTip("'<b>'+ this.series.name + ((typeof(this.point.name) != 'undefined') ? '->'  +this.point.name : '' )+ ' </b><br/>'+ this.x +': '+ this.y")); }
            set { _toolTip = value; }
        }

        public virtual YAxis YAxis
        {
            get { return _yAxis ?? (_yAxis = new YAxis()); }
            set { _yAxis = value; }
        }

        public virtual XAxis XAxis
        {
            get { return _xAxis ?? (_xAxis = new XAxis()); }
            set { _xAxis = value; }
        }

        public virtual AJAXSource AjaxDataSource
        {
            get { return _ajaxDataSource ?? (_ajaxDataSource = new AJAXSource()); }
            set { _ajaxDataSource = value; }
        }

        /// <summary>
        ///     Whether to show the credits text. Defaults to false.
        /// </summary>
        public virtual bool ShowCredits { get; set; }

        public virtual Title Title
        {
            get { return _title ?? (_title = null); }
            set { _title = value; }
        }

        public virtual SubTitle SubTitle
        {
            get { return _subTitle ?? (_subTitle = null); }
            set { _subTitle = value; }
        }

        public RenderType RenderType { get; set; }


        public virtual SerieCollection Series
        {
            get { return _series ?? (_series = new SerieCollection()); }
            set { _series = value; }
        }
    }
}