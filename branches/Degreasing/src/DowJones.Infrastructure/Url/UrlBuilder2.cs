using System;
using System.Web;
using System.Web.UI;
using EMG.Utility.Url;

namespace EMG.Utility.Uri
{
    public class UrlBuilder : UriBuilder
    {
        #region Fields

        private readonly IQueryStringEncoder _encoder = new DefaultEncoder();
        private bool _basicUtf8UrlEncoding = true;
        private QueryStringDictionary _queryString = null;
        private UrlOutputType _urlOutputType = UrlOutputType.Absolute;

        #endregion

        #region Properties

        public enum UrlOutputType
        {
            Relative = 0,
            Absolute,
        }

        public QueryStringDictionary QueryString
        {
            get
            {
                if (_queryString == null)
                {
                    _queryString = new QueryStringDictionary();
                }

                return _queryString;
            }
        }

        public string BaseUrl
        {
            get { return GetBaseUrl(); }
            set
            {
                UriBuilder uriBuilder = new UriBuilder(MakeAbsoluteUrl(value));
                Fragment = uriBuilder.Fragment;
                Host = uriBuilder.Host;
                Password = uriBuilder.Password;
                Path = uriBuilder.Path;
                Port = uriBuilder.Port;
                Query = uriBuilder.Query;
                Scheme = uriBuilder.Scheme;
                Initialise();
            }
        }


        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        public string PageName
        {
            get
            {
                string path = Path;
                return path.Substring(path.LastIndexOf("/") + 1);
            }
            set
            {
                string path = Path;
                path = path.Substring(0, path.LastIndexOf("/"));
                Path = string.Concat(path, "/", value);
            }
        }

        /// <summary>
        /// Gets or sets the type of the output.
        /// </summary>
        /// <value>The type of the output.</value>
        public UrlOutputType OutputType
        {
            get { return _urlOutputType; }
            set { _urlOutputType = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [basic UTF8 URL encoding].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [basic UTF8 URL encoding]; otherwise, <c>false</c>.
        /// </value>
        public bool BasicUtf8UrlEncoding
        {
            get { return _basicUtf8UrlEncoding; }
            set { _basicUtf8UrlEncoding = value; }
        }

        #endregion

        #region Constructor overloads

        public UrlBuilder(Page page)
            : base(page.Request.Url)
        {
            Initialise();
        }

        public UrlBuilder(Page page, IQueryStringEncoder encoder)
            : base(page.Request.Url)
        {
            _encoder = encoder;
            Initialise();
        }

        public UrlBuilder(string uri) : base(MakeAbsoluteUrl(uri))
        {
            Initialise();
        }

        public UrlBuilder(string uri, IQueryStringEncoder encoder)
            : base(uri)
        {
            _encoder = encoder;
            Initialise();
        }

        public UrlBuilder(System.Uri uri)
            : base(uri)
        {
            Initialise();
        }

        public UrlBuilder(System.Uri uri, IQueryStringEncoder encoder)
            : base(uri)
        {
            _encoder = encoder;
            Initialise();
        }

        public UrlBuilder(string scheme, string host)
            : base(scheme, host)
        {
        }

        public UrlBuilder(string scheme, string host, int port)
            : base(scheme, host, port)
        {
        }

        public UrlBuilder(string scheme, string host, int port, string path)
            : base(scheme, host, port, path)
        {
        }

        public UrlBuilder(string scheme, string host, int port, string path, string extra)
            : base(scheme, host, port, path, extra)
        {
        }

        public UrlBuilder()
        {
        }

        // Exchange the above constructor for this one if you wish to use the current page Uri by default.
        // Effectively, this is the same as calling: new UrlBuilder(this);
        //public UrlBuilder() : base(((System.Web.UI.Page)HttpContext.Current.Handler).Request.Url) {
        //}

        #endregion

        #region Public methods

        public void Navigate()
        {
            _Navigate(true);
        }

        public void Navigate(bool endResponse)
        {
            _Navigate(endResponse);
        }

        private void _Navigate(bool endResponse)
        {
            string uri = ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(uri, endResponse);
            else
                throw new NullReferenceException("HttpContext.Current is not initialized");
        }

        /// <summary>
        /// Format options:
        ///	"e" is the default and returns the string encoded using the specified encoder, or the default encoder if none specified.
        ///	"p" always returns the string as plaintext.
        /// </summary>
        /// <returns>A string representation of the Uri</returns>
        new public string ToString()
        {
            return _ToString("e");
        }

        public string ToString(string format)
        {
            return _ToString(format);
        }

        private string _ToString(string format)
        {
            Query = _queryString.ToString(format, _basicUtf8UrlEncoding);
            string tUrl;
            switch (_urlOutputType)
            {
                case UrlOutputType.Absolute:
                    tUrl = Uri.AbsoluteUri;
                    break;
                default:
                    tUrl = GetApplicationUri().MakeRelativeUri(Uri).ToString();
                    break;
            }
            return tUrl;
        }

        private string GetBaseUrl()
        {
            if ((UserName.Length == 0) && (Password.Length > 0))
            {
                throw new UriFormatException("net_uri_BadUserPassword");
            }

            string str = (Scheme.Length != 0) ? (Scheme + System.Uri.SchemeDelimiter) : string.Empty;
            string[] strArray = new string[] {str, UserName, (Password.Length > 0) ? (":" + Password) : string.Empty, (UserName.Length > 0) ? "@" : string.Empty, Host, ((Port != -1) && (Host.Length > 0)) ? (":" + Port) : string.Empty, (((Host.Length > 0) && (Path.Length != 0)) && (Path[0] != '/')) ? "/" : string.Empty, Path};
            return string.Concat(strArray);
        }

        #endregion

        #region Private methods

        private void Initialise()
        {
            if (_queryString == null)
            {
                _queryString = new QueryStringDictionary(Query, _encoder);
            }
        }

        private static string MakeAbsoluteUrl(string url)
        {
            
            if (string.IsNullOrEmpty(url))
                return url;
            if (url[0] != '~')
                return url;
            string applicationPath = GetApplicationPath();
            if (url.Length == 1)
                return applicationPath;
            int indexOfUrl = 1;
            if (url[1] == '/' || url[1] == '\\')
                indexOfUrl = 2;
            return string.Concat(GetApplicationUri(), url.Substring(indexOfUrl));
        }

        private static string GetApplicationPath()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
            return "";
        }

        private static System.Uri GetApplicationUri()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                string applicationUri;
                applicationUri = string.Concat(context.Request.Url.Scheme, "://", context.Request.Url.Host);
                string applicationPath = context.Request.ApplicationPath;

                if (applicationPath.Length > 1)
                {
                    applicationUri = string.Concat(applicationUri, applicationPath);
                }

                return new System.Uri(applicationUri + "/"); 
            }
            return new System.Uri("http://localhost/");
        }


        public void Add(QueryStringDictionary dictionary)
        {
            QueryString.AddRange(dictionary);
        }

        #endregion

        #region Legacy Append Methods

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Append(string name)
        {
            _queryString.Append(name);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, string value)
        {
            _queryString.Append(name, value);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, int value)
        {
            _queryString.Append(name, value);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, double value)
        {
            _queryString.Append(name, value);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, float value)
        {
            _queryString.Append(name, value);
        }


        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void Append(string name, bool value)
        {
            _queryString.Append(name, value);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public void Append(string name, int[] values)
        {
            QueryStringDictionary.Append(name, values);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public void Append(string name, string[] values)
        {
            _queryString.Append(name,values);
        }

        #endregion
    }
}