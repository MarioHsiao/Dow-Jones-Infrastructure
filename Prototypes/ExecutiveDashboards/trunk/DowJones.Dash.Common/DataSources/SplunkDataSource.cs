using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace DowJones.Dash.DataSources
{
    public class SplunkDataSource : JsonWebDataSource
    {
        public SplunkDataSource(string name, string savedSearch)
            : base(
                name,
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