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
            Method = HttpMethod.Post;
            PollDelay = Convert.ToInt32(ConfigurationManager.AppSettings["Splunk.PollDelay"]);
        }
    }

    public class BrowserDataSource : SplunkDataSource
    {
        public BrowserDataSource() : base("RTBus_Browser")
        {
        }
    }

    public class HandheldDataSource : SplunkDataSource
    {
        public HandheldDataSource() : base("RTBus_Handheld")
        {
        }
    }

    public class ReferrerDataSource : SplunkDataSource
    {
        public ReferrerDataSource() : base("RTBus_ReferrerBreakdown")
        {
        }
    }

    public class HandheldVsPcDataSource : SplunkDataSource
    {
        public HandheldVsPcDataSource() : base("RTBus_HandheldvsPC")
        {
        }
    }
}