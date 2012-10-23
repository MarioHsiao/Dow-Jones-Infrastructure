using System;
using System.Collections.Generic;
using System.Configuration;
using log4net;

namespace DowJones.Dash.Common.DataSources
{
    public class ChartBeatApiDataSource : JsonWebDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChartBeatApiDataSource));


        public ChartBeatApiDataSource(string name, string dataName, string relativePath, string host, IDictionary<string, object> parameters = null, int? pollDelay = null)
            : base(
                name,
                dataName,
                ConfigurationManager.AppSettings["ChartBeatApi.BasePath"] + relativePath,
                new Dictionary<string, object>(parameters ?? new Dictionary<string, object>())
                    {
                        {"apikey", ConfigurationManager.AppSettings["ChartBeatApi.ApiKey"]},
                        {"host", host},
                    },
                () => pollDelay ?? Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"])
            )
        {
        }
    }

    public class ChartBeatSiteDataSource : JsonWebDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChartBeatApiDataSource));


        public ChartBeatSiteDataSource(string name, string dataName, string relativePath, string host, IDictionary<string, object> parameters = null, int? pollDelay = null)
            : base(
                name,
                dataName,
                ConfigurationManager.AppSettings["ChartBeatSite.BasePath"] + relativePath,
                new Dictionary<string, object>(parameters ?? new Dictionary<string, object>())
                    {
                        {"sessionid", ConfigurationManager.AppSettings["ChartBeatSite.ApiKey"]},
                        {"host", host},
                    },
                () => pollDelay ?? Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"])
            )
        {
        }
    }
}