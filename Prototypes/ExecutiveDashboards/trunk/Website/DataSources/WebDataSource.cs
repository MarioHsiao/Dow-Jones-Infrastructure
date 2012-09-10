using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Dash.Website.DataSources
{
    public abstract class WebDataSource : DataSource
    {
        public int ErrorDelay
        {
            get { return PollDelay * 2; }
        }

        public IDictionary<string, object> Parameters { get; private set; }

        public string Path { get; set; }

        public int PollDelay { get; set; }

        public string QueryString
        {
            get
            {
                var parameters = Parameters.Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode((string) x.Value.ToString())));
                return string.Join("&", parameters);
            }
        }

        public string Url
        {
            get { return string.Format("{0}?{1}", Path, QueryString);  }
        }

        public WebDataSource(string path, IDictionary<string, object> parameters = null, int pollDelay = 3)
        {
            Guard.IsNotNullOrEmpty(path, "path");

            Path = path;
            Parameters = parameters ?? new Dictionary<string, object>();
            PollDelay = pollDelay;

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
                using (var stream = response.GetResponseStream())
                {
                    var data = ParseResponse(stream);
                    OnDataReceived(data);
                }

                Thread.Sleep(PollDelay * 1000);
                Poll();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected internal abstract dynamic ParseResponse(Stream stream);
    }

    public abstract class JsonWebDataSource : WebDataSource
    {
        public JsonWebDataSource(string path, IDictionary<string, object> parameters = null, int pollDelay = 3) 
            : base(path, parameters, pollDelay)
        {
        }

        protected internal override dynamic ParseResponse(Stream stream)
        {
            var json = new StreamReader(stream).ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            return data;
        }
    }

    public abstract class XmlWebDataSource : WebDataSource
    {
        public XmlWebDataSource(string path, IDictionary<string, object> parameters = null, int pollDelay = 3) 
            : base(path, parameters, pollDelay)
        {
        }

        protected internal override dynamic ParseResponse(Stream stream)
        {
            XNode node = XNode.ReadFrom(new XmlTextReader(new StreamReader(stream)));
            var json = JsonConvert.SerializeXNode(node, Newtonsoft.Json.Formatting.None, true);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            return data;
        }
    }
}