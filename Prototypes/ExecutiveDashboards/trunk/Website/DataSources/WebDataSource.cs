using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using DowJones.Infrastructure;
using Newtonsoft.Json;
using ICredentials=System.Net.ICredentials;

namespace DowJones.Dash.Website.DataSources
{
    public abstract class WebDataSource : DataSource
    {
        public ICredentials Credentials { get; set; }

        public int ErrorDelay
        {
            get { return PollDelay * 2; }
        }

        public HttpMethod Method { get; set; }

        public IDictionary<string, object> Parameters { get; private set; }

        public string Path { get; private set; }

        public int PollDelay { get; set; }

        public string Url
        {
            get
            {
                if(Method == HttpMethod.Get)
                    return string.Format("{0}?{1}", Path, SerializeParameters());

                return Path;
            }
        }

        public WebDataSource(string path, IDictionary<string, object> parameters = null, int pollDelay = 3)
        {
            Guard.IsNotNullOrEmpty(path, "path");

            Method = HttpMethod.Get;
            Path = path;
            Parameters = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>());
            PollDelay = pollDelay;
        }

        public override void Start()
        {
            ServicePointManager.ServerCertificateValidationCallback = (x, y, z, a) => true;

            Task.Factory.StartNew(Poll);
        }

        protected internal void Poll()
        {
            try
            {
                var request = WebRequest.Create(Url);

                request.Method = Method.ToString().ToUpper();

                if (Credentials != null)
                {
                    request.Credentials = Credentials.GetCredential(request.RequestUri, "Basic");
                    request.PreAuthenticate = true;
                }

                if (Method == HttpMethod.Post)
                {
                    var parameters = SerializeParameters();
                    var postData = Encoding.UTF8.GetBytes(parameters);
                    var contentLength = postData.Length;
                    request.ContentLength = contentLength;
                    request.GetRequestStream().Write(postData, 0, contentLength);
                }

                request.GetResponseAsync()
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

        private string SerializeParameters()
        {
            var parameters = Parameters.Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode(x.Value.ToString())));
            return string.Join("&", parameters);
        }

        protected internal abstract dynamic ParseResponse(Stream stream);
    }

    public abstract class JsonWebDataSource : WebDataSource
    {
        public JsonWebDataSource(string path, IDictionary<string, object> parameters = null)
            : base(path, parameters)
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
        public XmlWebDataSource(string path, IDictionary<string, object> parameters = null)
            : base(path, parameters)
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