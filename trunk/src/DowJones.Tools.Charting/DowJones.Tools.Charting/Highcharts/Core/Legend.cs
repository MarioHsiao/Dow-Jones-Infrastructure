using System;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class Legend
    {
        [JsonIgnore] 
        private string _formatter;

        public Legend()
        {
            Enabled = true;
        }

        [JsonConverter(typeof (StringEnumConverter))]
        public Align? Align { get; set; }

        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public int? BorderRadius { get; set; }
        public int? BorderWidth { get; set; }
        public bool? Enabled { get; set; }
        public bool? Floating { get; set; }
        public ItemStyle ItemHiddenStyle { get; set; }
        public ItemStyle ItemHoverStyle { get; set; }
        public ItemStyle ItemStyle { get; set; }
        public int? ItemWidth { get; set; }
        public int? LineHeight { get; set; }
        public int? Margin { get; set; }
        public bool? Reversed { get; set; }
        public bool? Shadow { get; set; }
        public CSSObject Style { get; set; }
        public int? SymbolPadding { get; set; }
        public int? SymbolWidth { get; set; }
        public int? Width { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VerticalAlign? VerticalAlign { get; set; }


        [JsonConverter(typeof (StringEnumConverter))]
        public Layout? Layout { get; set; }

        public string LabelFormatter
        {
            get {
                return string.IsNullOrEmpty(_formatter) ? null : String.Format("function(event){{ var tmp = {0}; if(typeof(tmp) == 'function'){{return tmp(this);}}else{{ return tmp;}} }}", _formatter);
            }
            set { _formatter = value; }
        }
        
        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });
            return ignored == "{\"enabled\":true}" ? string.Empty : string.Format("legend: {0},", ignored);
        }

        internal void ApplyTheme(LegendStyle themeConfiguration)
        {
            ItemHiddenStyle.CopyStyles(themeConfiguration.HiddenStyle);
            ItemHoverStyle.CopyStyles(themeConfiguration.HoverStyle);
            ItemStyle.CopyStyles(themeConfiguration.Style);
        }
    }
}