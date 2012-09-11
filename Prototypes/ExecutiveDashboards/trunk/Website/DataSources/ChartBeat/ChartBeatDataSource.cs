using System;
using System.Collections.Generic;
using System.Configuration;

namespace DowJones.Dash.Website.DataSources
{
    public abstract class ChartBeatDataSource : JsonWebDataSource
    {
        protected ChartBeatDataSource(string relativePath, string host = "online.wsj.com")
            : base(
                ConfigurationManager.AppSettings["ChartBeat.BasePath"] + relativePath,
                new Dictionary<string, object>
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