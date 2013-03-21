using System;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class Title
    {
        public Title(string titleText)
        {
            Text = titleText;
        }

        public Title()
        {
            Text = string.Empty;
        }

        public Align? Align { get; set; }

        public int? Floating { get; set; }
        
        public int? Margin { get; set; }
        
        public CSSObject Style { get; set; }
        
        public string Text { get; set; }
        
        [JsonProperty("useHTML")]
        public bool? UseHtml { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VerticalAlign? VerticalAlign { get; set; }
        
        public int? X { get; set; }
        
        public int? Y { get; set; }

        internal void ApplyTheme(CSSObject themeConfiguration)
        {
            Style.CopyStyles(themeConfiguration);
        }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore, 
                                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });
            return string.Format("title: {0},", ignored);
        }
    }
}