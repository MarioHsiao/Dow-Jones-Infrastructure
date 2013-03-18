using System;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class ToolTip
    {
        private string _formatter;

        public ToolTip()
        {
        }

        public ToolTip(string format)
        {
            Formatter = format;
        }

        public string BackgroundColor { get; set; }

        public string BorderColor { get; set; }

        public int? BorderRadius { get; set; }

        public int? BorderWidth { get; set; }

        public bool? Crosshairs { get; set; }

        public bool? Enabled { get; set; }

        public string Formatter
        {
            get {
                return string.IsNullOrEmpty(_formatter) ? null : String.Format("function(event){{ var tmp = {0}; if(typeof(tmp) == 'function'){{return tmp(this);}}else{{ return tmp;}} }}", _formatter);
            }
            set { _formatter = value; }
        }


        public bool? Shadow { get; set; }

        public bool? Shared { get; set; }

        public int? Snap { get; set; }

        public CSSObject Style { get; set; }
        
        public override string ToString()
        {
            var tmp = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                             {
                                                                                 NullValueHandling = NullValueHandling.Ignore,
                                                                                 ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                             });
            return string.Format("tooltip: {0} ,", tmp);
        }
    }
}