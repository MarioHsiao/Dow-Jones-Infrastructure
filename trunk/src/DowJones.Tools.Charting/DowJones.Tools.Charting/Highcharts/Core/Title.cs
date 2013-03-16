using System;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;
using Newtonsoft.Json;
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

        public Align? Align { get; set; }
        public int? Margin { get; set; }
        public int? Rotation { get; set; }
        public CSSObject Style { get; set; }
        public string Text { get; set; }

        internal void ApplyTheme(CSSObject themeConfiguration)
        {
            Style.CopyStyles(themeConfiguration);
        }

        public override string ToString()
        {
            string ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore, 
                                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });
            return string.Format("title: {0},", ignored);
        }
    }
}