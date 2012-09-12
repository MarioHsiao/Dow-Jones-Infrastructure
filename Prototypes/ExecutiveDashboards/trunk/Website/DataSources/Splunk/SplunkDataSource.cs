using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;

namespace DowJones.Dash.Website.DataSources
{
    public abstract class SplunkDataSource : JsonWebDataSource
    {
        public SplunkDataSource(string savedSearch)
            : base(
                ConfigurationManager.AppSettings["Splunk.BasePath"] + "/search/jobs",
                new Dictionary<string, object>
                {
                    { "output_mode", "json" },
                    { "exec_mode", "oneshot" },
                    { "search", "savedsearch " + savedSearch }
                }
            )
        {
            Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["Splunk.Username"], 
                ConfigurationManager.AppSettings["Splunk.Password"]
            );
            Method = "POST";
            PollDelay = Convert.ToInt32(ConfigurationManager.AppSettings["Splunk.PollDelay"]);
        }
    }

    public class Browser : SplunkDataSource
    {
        public Browser() : base("RTBus_Browser")
        {
        }
    }

    public class Handheld : SplunkDataSource
    {
        public Handheld() : base("RTBus_Handheld")
        {
        }
    }

    public class HandheldVsPc : SplunkDataSource
    {
        public HandheldVsPc() : base("RTBus_HandheldvsPC")
        {
        }
    }
}