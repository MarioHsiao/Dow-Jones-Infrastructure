using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class SubTitle : Title
    {
        public SubTitle(string subTitleText) : base(subTitleText)
        {
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return string.Empty;
            }
            string ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore, 
                                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });
            return string.Format("subtitle: {0},", ignored);
        }
    }
}