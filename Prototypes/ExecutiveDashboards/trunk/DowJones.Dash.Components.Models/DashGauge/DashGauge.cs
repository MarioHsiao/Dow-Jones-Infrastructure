using System.Collections.Generic;
using DowJones.Web.Mvc.UI;
using Newtonsoft.Json;

namespace DowJones.Dash.Components.Models.DashGauge
{
    public class Band
    {
        [JsonProperty("to")]
        public double To { get; set; }

        [JsonProperty("from")]
        public double From { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("innerRadius")]
        public string InnerRadius { get; set; }

        [JsonProperty("outterRadius")]
        public string OutterRadius { get; set; }
    }

    public class Dial
    {
        public Dial()
        {
            BackgroundColor = "#000";
            BaseLength = "70%";
            BaseWidth = 3;
            BorderColor = "silver";
            BorderWidth = 0;
            Radius = "95%";
            RearLength = "10%";
            TopWidth = 1;
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius or length of the dial, in percentages relative to the radius of the gauge itself. Defaults to 80%.
        /// </value>
        [JsonProperty("radius")]
        public string Radius { get; set; }

        /// <summary>
        /// The background or fill color of the gauge's dial. Defaults to black.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>
        /// The border color or stroke of the gauge's dial. By default, the borderWidth is 0, so this must be set in addition to a custom border color. Defaults to silver.
        /// </value>
        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the length of the rear.
        /// </summary>
        /// <value>
        /// The length of the dial's rear end, the part that extends out on the other side of the pivot. Relative to the dial's length. Defaults to 10%.
        /// </value>
        [JsonProperty("rearLength")]
        public string RearLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the base.
        /// </summary>
        /// <value>
        /// The pixel width of the base of the gauge dial. The base is the part closest to the pivot, defined by baseLength. Defaults to 3.
        /// </value>
        [JsonProperty("baseWidth")]
        public int BaseWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        /// <value>
        /// The width of the gauge dial border in pixels. Defaults to 0.
        /// </value>
        [JsonProperty("borderWidth")]
        public int BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the top.
        /// </summary>
        /// <value>
        /// The width of the top of the dial, closest to the perimeter. The pivot narrows in from the base to the top. Defaults to 1.
        /// </value>
        [JsonProperty("topWidth")]
        public int TopWidth { get; set; }

        /// <summary>
        /// Gets or sets the length of the base.
        /// </summary>
        /// <value>
        /// The length of the dial's base part, relative to the total radius or length of the dial. Defaults to 70%.
        /// </value>
        [JsonProperty("baseLength")]
        public string BaseLength { get; set; }
    }

    public enum  GaugeType
    {
        Speedometer,
        Meter,
    }

    public class DashGaugeModel : ViewComponentModel
    {
        public DashGaugeModel()
        {
            Max = 100;
            Min = 0;
            Data = -20;
            Angle = 65;
            Title = string.Empty;
            Footer = string.Empty;
            GaugeType = GaugeType.Speedometer;
        }

        [ClientProperty("max")]
        public double Max { get; set; }

        [ClientProperty("min")]
        public double Min { get; set; }

        [ClientData("data")]
        public double Data { get; set; }

        [ClientProperty("bands")]
        public List<Band> Bands { get; set; }

        [ClientProperty("dial")]
        public Dial Dial { get; set; }

        [ClientProperty("angle")]
        public uint Angle { get; set; }

        [ClientProperty("title")]
        public string Title { get; set; }

        [ClientProperty("footer")]
        public string Footer { get; set; }

        [ClientProperty("gaugeType")]
        public GaugeType GaugeType { get; set; }

        [ClientProperty("colors")]
        public List<string> Colors { get; set; }
    }
}