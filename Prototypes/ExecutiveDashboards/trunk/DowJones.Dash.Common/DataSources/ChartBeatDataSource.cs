using System;
using System.Collections.Generic;
using System.Configuration;

namespace DowJones.Dash.DataSources
{
    public class ChartBeatDataSource : JsonWebDataSource
    {
        public ChartBeatDataSource(string name, string relativePath, string host = "online.wsj.com", IDictionary<string,object> parameters = null)
            : base(
                name,
                ConfigurationManager.AppSettings["ChartBeat.BasePath"] + relativePath,
                new Dictionary<string, object>(parameters ?? new Dictionary<string, object>())
                    {
                        {"apikey", ConfigurationManager.AppSettings["ChartBeat.ApiKey"]},
                        {"host", host},
                    }
            )
        {
            PollDelay = Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"]);
        }
    }
}