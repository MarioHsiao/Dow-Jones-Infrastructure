using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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

        public WebDataSource(string name, string path, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, pollDelayFactory, errorDelayFactory)
        {
            Guard.IsNotNullOrEmpty(path, "path");

            Method = "GET";
            Path = path;
            Parameters = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>());
        }

        public virtual void Initialize()
        {
            // Accept insecure certificates -- we're only hitting known services
            ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = (x, y, z, a) => true;
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // Always accept
                return true;
            }
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

    public class JsonWebDataSource : WebDataSource
    {
        public JsonWebDataSource(string name, string path, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, path, parameters, pollDelayFactory, errorDelayFactory)
        {
        }

        protected internal override dynamic ParseResponse(Stream stream)
        {
            var json = new StreamReader(stream).ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            return data;
        }
    }

    public class XmlWebDataSource : WebDataSource
    {
        public XmlWebDataSource(string name, string path, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, path, parameters, pollDelayFactory, errorDelayFactory)
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