using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using log4net;

namespace DowJones.Dash.DataSources
{
    public class SplunkDataSource : JsonWebDataSource
    {
        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(SplunkDataSource));

        public SplunkDataSource(string name, string dataName, string savedSearch)
            : base(
                name,
                dataName,
                ConfigurationManager.AppSettings["Splunk.BasePath"] + "/search/jobs",
                new Dictionary<string, object> {
                    { "output_mode", "json" },
                    { "exec_mode", "oneshot" },
                    { "search", "savedsearch " + savedSearch }
                },
                () => Convert.ToInt32(ConfigurationManager.AppSettings["Splunk.PollDelay"])
            )
        {
            Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["Splunk.Username"], 
                ConfigurationManager.AppSettings["Splunk.Password"]
            );
            Method = "POST";
        }
    }
}