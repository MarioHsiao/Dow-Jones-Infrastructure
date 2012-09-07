using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Dash.Website.DataSources
{
    public abstract class ChartBeatDataSource : DataSource
    {
        public string ApiKey
        {
            get { return ConfigurationManager.AppSettings["ChartBeat.ApiKey"]; }
        }

        public string BasePath
        {
            get { return ConfigurationManager.AppSettings["ChartBeat.BasePath"]; }
        }

        public int ErrorDelay
        {
            get { return PollDelay * 2; }
        }

        public IDictionary<string, object> Parameters { get; private set; }

        public string Path { get; set; }

        public int PollDelay
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["ChartBeat.PollDelay"]); }
        }

        public string Url
        {
            get
            {
                var parameters = Parameters.Select(x => string.Format("{0}={1}", x.Key, x.Value));
                return string.Format("{0}{1}?{2}", BasePath, Path, string.Join("&", parameters));
            }
        }

        public ChartBeatDataSource(string path, string host = "online.wsj.com")
        {
            Guard.IsNotNullOrEmpty(path, "path");

            Parameters = new Dictionary<string, object>
                {
                    {"apikey", ApiKey},
                    {"host", host},
                };

            Path = path;

            Task.Factory.StartNew(Poll);
        }

        protected internal void Poll()
        {
            try
            {
                WebRequest
                    .Create(Url)
                    .GetResponseAsync()
                    .ContinueWith(task => OnResponse(task.Result));
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected virtual void OnError(Exception ex = null)
        {
            Trace.TraceError("{0} request failed: {1}", Path, ex);
            Thread.Sleep(ErrorDelay * 1000);
            Poll();
        }

        protected void OnResponse(WebResponse response)
        {
            try
            {
                using(var stream = response.GetResponseStream())
                {
                    var json = new StreamReader(stream).ReadToEnd();
                    var data = JsonConvert.DeserializeObject<dynamic>(json);
                    var mapped = Map(data);
                    OnDataReceived(mapped);
                }

                Thread.Sleep(PollDelay * 1000);
                Poll();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected virtual dynamic Map(dynamic response)
        {
            return response;
        }
    }
}