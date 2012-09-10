using System;
using System.Collections.Generic;
using System.Configuration;

namespace DowJones.Dash.Website.DataSources
{
    public abstract class ChartBeatDataSource : JsonWebDataSource
    {
        public ChartBeatDataSource(string relativePath, string host = "online.wsj.com", int? pollDelay = null)
            : base(ResolveChartBeatPath(relativePath), GetParameters(host), GetPollDelay(pollDelay))
        {
            Path = string.Format("{0}{1}", ConfigurationManager.AppSettings["ChartBeat.BasePath"], relativePath);
            PollDelay = Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"]);
        }

        private static string ResolveChartBeatPath(string relativePath)
        {
            return string.Format("{0}{1}", ConfigurationManager.AppSettings["ChartBeat.BasePath"], relativePath);
        }

        private static IDictionary<string, object> GetParameters(string host)
        {
            return new Dictionary<string, object>
                {
                    {"apikey", ConfigurationManager.AppSettings["ChartBeat.ApiKey"]},
                    {"host", host},
                };
        }

        private static int GetPollDelay(int? pollDelay)
        {
            return pollDelay.GetValueOrDefault(Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"]));
        }
    }
}