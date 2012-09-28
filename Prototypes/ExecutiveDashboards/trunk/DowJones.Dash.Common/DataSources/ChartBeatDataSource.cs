using System;
using System.Collections.Generic;
using System.Configuration;
using log4net;

namespace DowJones.Dash.DataSources
{
    public class ChartBeatDataSource : JsonWebDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChartBeatDataSource));


        public ChartBeatDataSource(string name, string dataName, string relativePath, string host, IDictionary<string, object> parameters = null)
            : base(
                name,
                dataName,
                ConfigurationManager.AppSettings["ChartBeat.BasePath"] + relativePath,
                new Dictionary<string, object>(parameters ?? new Dictionary<string, object>())
                    {
                        {"apikey", ConfigurationManager.AppSettings["ChartBeat.ApiKey"]},
                        {"host", host},
                    },
                () => Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"])
            )
        {
        }
    }
}