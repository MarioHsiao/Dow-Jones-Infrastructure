using System;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class Labels
    { 
        private string _formatter;
        public Labels()
        {
            Enabled = true;
        }

        [JsonConverter(typeof (StringEnumConverter))]
        public Align? Align { get; set; }

        public bool? Enabled { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public int? BorderRadius { get; set; }
        public int? BorderWidth { get; set; }
        public int? Rotation { get; set; }
        public bool? Crop { get; set; }
        public bool? Shadow { get; set; }
        public bool? UseHTML { get; set; }
        public int? zIndex { get; set; }

        public CSSObject Style { get; set; }

        public string Formatter
        {
            get {
                return string.IsNullOrEmpty(_formatter) ? null : String.Format("function(event){{ var tmp = {0}; if(typeof(tmp) == 'function'){{return tmp(this);}}else{{ return tmp;}} }}", _formatter);
            }
            set { _formatter = value; }
        }
        
        public int? StaggerLines { get; set; }
        public int? Step { get; set; }

        public int? X { get; set; }
        public int? Y { get; set; }


        [JsonConverter(typeof (StringEnumConverter))]
        public VerticalAlign? VerticalAlign { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        DefaultValueHandling = DefaultValueHandling.Include,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });
            //return string.Format("legend: {0},", ignored);
            return ignored;
        }
    }
}