using System;
using DowJones.Charting.Highcharts.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Charting.Highcharts.Core.PlotOptions
{
    [Serializable]
    public abstract class PlotOptionsSeries
    {
        public bool? AllowPointSelect { get; set; }
        public bool? Animation { get; set; }
        public string Color { get; set; }
        public bool? ConnectNulls { get; set; }
        public string Cursor { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public DashStyle? DashStyle { get; set; }

        public DataLabels DataLabels { get; set; }
        public bool? EnableMouseTracking { get; set; }
        public PlotOptionEvents Events { get; set; }
        public string Id { get; set; }
        public int? LineWidth { get; set; }
        public Marker Marker { get; set; }
        public PlotPointEvents Point { get; set; }
        public int? PointStart { get; set; }
        public long? PointInterval { get; set; }
        public bool? Shadow { get; set; }
        public bool? ShowInLegend { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public Stacking? Stacking { get; set; }

        public SerieStates States { get; set; }
        public bool? StickyTracking { get; set; }
        public bool? Visible { get; set; }
        public int? zIndex { get; set; }
    }
}