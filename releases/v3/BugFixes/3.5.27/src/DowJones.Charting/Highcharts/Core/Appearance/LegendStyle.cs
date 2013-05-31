using System;

namespace DowJones.Charting.Highcharts.Core.Appearance
{
    [Serializable]
    public class LegendStyle
    {
        public ItemStyle Style { get; set; }
        public ItemStyle HoverStyle { get; set; }
        public ItemStyle HiddenStyle { get; set; }
    }
}