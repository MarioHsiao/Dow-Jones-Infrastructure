using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core.Appearance
{
    [Serializable]
    public class ColorSet
    {
        /// <summary>
        ///     When all colors are used, new colors are pulled from the start again. Defaults to:
        ///     colors: ['#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92']
        /// </summary>
        public string[] Colors { get; set; }

        internal void ApplyTheme(ColorSet themeConfiguration)
        {
            if (Colors != null) return;
            if (themeConfiguration.Colors != null && themeConfiguration.Colors.Any())
            {
                Colors = themeConfiguration.Colors;
            }
        }

        public override string ToString()
        {
            if (Colors == null || !Colors.Any())
            {
                return string.Empty;
            }
            
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                    });

            return ignored.Replace("{", string.Empty).Replace("}", string.Empty) + ",";
        }
    }
}