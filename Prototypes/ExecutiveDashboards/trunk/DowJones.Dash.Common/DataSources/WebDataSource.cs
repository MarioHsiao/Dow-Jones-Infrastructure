using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using Newtonsoft.Json;
using ICredentials = System.Net.ICredentials;

namespace DowJones.Dash.DataSources
{
    public abstract class WebDataSource : PollingDataSource, IInitializable
    {
        public ICredentials Credentials { get; set; }

        public string Method { get; set; }

        public IDictionary<string, object> Parameters { get; private set; }

        public string Path { get; private set; }


        public string Url
        {
            get
            {
                if("GET".Equals(Method, StringComparison.OrdinalIgnoreCase))
                    return string.Format("{0}?{1}", Path, SerializeParameters());

                return Path;
            }
        }

        public WebDataSource(string path, IDictionary<string, object> parameters = null, int pollDelay = 3)
        {
            Guard.IsNotNullOrEmpty(path, "path");

            Method = "GET";
            Path = path;
            Parameters = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>());
            PollDelay = pollDelay;
        }

        public void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback = (x, y, z, a) => true;
        }

        protected override void Poll()
        {
            try
            {
                Log("Polling {0}...", Url);

                var request = WebRequest.Create(Url);

                request.Method = Method.ToUpper();

                if (Credentials != null)
                {
                    request.Credentials = Credentials.GetCredential(request.RequestUri, "Basic");
                    request.PreAuthenticate = true;
                }

                if (request.Method == "POST")
                {
                    var parameters = SerializeParameters();
                    var postData = Encoding.UTF8.GetBytes(parameters);
                    var contentLength = postData.Length;
                    request.ContentLength = contentLength;
                    request.GetRequestStream().Write(postData, 0, contentLength);
                }

                request.GetResponseAsync()
                       .ContinueWith(task => {
                               try
                               {
                                   OnResponse(task.Result);
                               }
                               catch (Exception ex)
                               {
                                   OnError(ex);
                               }
                           });
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
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
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected override void OnError(Exception ex = null)
        {
            if(ex != null && ex is HttpException)
            {
                var message = string.Format("HTTP error: {0}", ((HttpException) ex).ErrorCode);
                ex = new ApplicationException(message, ex);
            }

            base.OnError(ex);
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