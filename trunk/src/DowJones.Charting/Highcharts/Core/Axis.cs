using System;
using DowJones.Charting.Highcharts.Core.Appearance;
using DowJones.Charting.Highcharts.Core.PlotOptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class Axis
    {
        public int? TickWidth;

        protected Axis()
        {
            Title = new Title(string.Empty);
        }

        public bool? AllowDecimals { get; set; }
        public string AlternateGridColor { get; set; }
        public DateTimeLabelFormats DateTimeLabelFormats { get; set; }
        public bool? EndOnTick { get; set; }
        public string GridLineColor { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public DashStyle? GridLineDashStyle { get; set; }

        public int? GridLineWidth { get; set; }
        public string Id { get; set; }
        public Labels Labels { get; set; }
        public string LineColor { get; set; }
        public int? LineWidth { get; set; }
        public int? LinkedTo { get; set; }
        public int? Max { get; set; }
        public double? MaxPadding { get; set; }
        public int? MaxZoom { get; set; }
        public int? Min { get; set; }
        public string MinorGridLineColor { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public DashStyle? MinorGridLineDashStyle { get; set; }

        public int? MinorGridLineWidth { get; set; }
        public string MinorTickColor { get; set; }
        public string MinorTickInterval { get; set; }
        public int? MinorTickLength { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public TickPosition? MinorTickPosition { get; set; }

        public int? MinorTickWidth { get; set; }
        public double? MinPadding { get; set; }
        public int? Offset { get; set; }
        public bool? Opposite { get; set; }
        public PlotBands PlotBands { get; set; }
        public PlotLines PlotLines { get; set; }
        public bool? Reversed { get; set; }
        public bool? ShowFirstLabel { get; set; }
        public bool? ShowLastLabel { get; set; }
        public DataLabels StackLabels { get; set; }
        public int? StartOfWeek { get; set; }
        public bool? StartOnTick { get; set; }
        public string TickColor { get; set; }
        public long? TickInterval { get; set; }
        public int? TickLength { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public TickPlacement? TickmarkPlacement { get; set; }

        public int? TickPixelInterval { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public TickPosition? TickPosition { get; set; }

        public Title Title { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public AxisDataType? Type { get; set; }

        internal void ApplyTheme(CSSObject themeConfiguration)
        {
            Title.ApplyTheme(themeConfiguration);
        }
    }
}