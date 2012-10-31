using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using DowJones.Dash.Extentions;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using ICredentials = System.Net.ICredentials;

namespace DowJones.Dash.Common.DataSources
{
    
    public abstract class WebDataSource : PollingDataSource, IInitializable
    {
        private string _method;
        public ICredentials Credentials { get; set; }

        public string Method
        {
            get { return _method; }
            set { _method = (value ?? string.Empty).ToUpper(); }
        }

        public IDictionary<string, object> Parameters { get; private set; }

        public string Path { get; private set; }

        public string Url
        {
            get
            {
                return "GET".Equals(Method, StringComparison.OrdinalIgnoreCase) ? string.Format("{0}?{1}", Path, SerializeParameters()) : Path;
            }
        }


        protected WebDataSource(string name, string dataName, string path, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, dataName, pollDelayFactory, errorDelayFactory)
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
                Log.DebugFormat("{0} {1}", Method, Url);

                var request = WebRequest.Create(Url);
                request.Method = Method;

                if (Credentials != null)
                {
                    request.Credentials = Credentials.GetCredential(request.RequestUri, "Basic");
                    request.PreAuthenticate = true;
                }

                if (Method == "POST")
                {
                    var parameters = SerializeParameters();

                    Log.DebugFormat("Parameters: {0}", parameters);

                    var postData = Encoding.UTF8.GetBytes(parameters);
                    var contentLength = postData.Length;
                    request.ContentLength = contentLength;
                    request.GetRequestStream().Write(postData, 0, contentLength);
                }

                request.GetResponseAsync()
                       .ContinueWith(task => {
                               try
                               {
                                   Log.DebugFormat("Processing response from {0}", Url);
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

        protected virtual void OnResponse(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                var data = ParseResponse(stream);
                OnDataReceived(data);
            }
        }

        protected override void OnError(Exception ex = null, string name = null)
        {
            var httpException = ex as HttpException;
            if (httpException != null)
            {
                var message = string.Format("HTTP error: {0}", httpException.ErrorCode);
                ex = new ApplicationException(message, ex);
            }

            base.OnError(ex, name);
        }

        private string SerializeParameters()
        {
            //var parameters = Parameters.Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode(x.Value.ToString())));

            var parameters = (from parameter in Parameters let key = parameter.Key let val = parameter.Value.ToString() 
                        where !string.IsNullOrEmpty(val) 
                        select "{0}={1}".FormatWith(key, HttpUtility.UrlEncode(val))).ToList();
            return string.Join("&", parameters);
        }

        protected internal abstract dynamic ParseResponse(Stream stream);
    }
}