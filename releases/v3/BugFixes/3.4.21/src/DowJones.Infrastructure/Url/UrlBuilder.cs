// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlBuilder.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Web;
using DowJones.Attributes;
using DowJones.Encoders;
using DowJones.Extensions;

namespace DowJones.Url
{
    /// <summary>
    /// The url builder.
    /// </summary>
    public class UrlBuilder : UriBuilder
    {
        #region Fields

        /// <summary>
        /// The _encoder.
        /// </summary>
        private readonly IEncoder _encoder = new DefaultEncoder();

        /// <summary>
        /// The _basic utf 8 url encoding.
        /// </summary>
        private bool _basicUtf8UrlEncoding = true;

        /// <summary>
        /// The _query string.
        /// </summary>
        private QueryStringDictionary _queryString;

        /// <summary>
        /// The _url output type.
        /// </summary>
        private UrlOutputType _urlOutputType = UrlOutputType.Absolute;

        #endregion

        #region Constructor overloads

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="uri">
        /// The URI (Uniform Resource Identifier).
        /// </param>
        public UrlBuilder(string uri) : base(MakeAbsoluteUrl(uri))
        {
            Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="uri">
        /// The URI (Uniform Resource Identifier).
        /// </param>
        /// <param name="encoder">
        /// The encoder.
        /// </param>
        public UrlBuilder(string uri, IEncoder encoder)
            : base(uri)
        {
            _encoder = encoder;
            Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="uri">
        /// The URI (Uniform Resource Identifier).
        /// </param>
        public UrlBuilder(System.Uri uri)
            : base(uri)
        {
            Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="uri">
        /// The URI (Uniform Resource Identifier).
        /// </param>
        /// <param name="encoder">
        /// The encoder.
        /// </param>
        public UrlBuilder(System.Uri uri, IEncoder encoder)
            : base(uri)
        {
            _encoder = encoder;
            Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="host">
        /// The url host.
        /// </param>
        public UrlBuilder(string scheme, string host)
            : base(scheme, host)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="host">
        /// The url host.
        /// </param>
        /// <param name="port">
        /// The url port.
        /// </param>
        public UrlBuilder(string scheme, string host, int port)
            : base(scheme, host, port)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="host">
        /// The url host.
        /// </param>
        /// <param name="port">
        /// The url port.
        /// </param>
        /// <param name="path">
        /// The url path.
        /// </param>
        public UrlBuilder(string scheme, string host, int port, string path)
            : base(scheme, host, port, path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="host">
        /// The url host.
        /// </param>
        /// <param name="port">
        /// The url port.
        /// </param>
        /// <param name="path">
        /// The url path.
        /// </param>
        /// <param name="extra">
        /// The url extra.
        /// </param>
        public UrlBuilder(string scheme, string host, int port, string path, string extra)
            : base(scheme, host, port, path, extra)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        public UrlBuilder()
        {
        }

        //// Exchange the above constructor for this one if you wish to use the current page Uri by default.
        //// Effectively, this is the same as calling: new UrlBuilder(this);
        //// public UrlBuilder() : base(((System.Web.UI.Page)HttpContext.Current.Handler).Request.Url) {
        //// }
        #endregion

        #region Enums

        /// <summary>
        /// Url Output Type
        /// </summary>
        public enum UrlOutputType
        {
            /// <summary>
            /// Output a Relative Url.
            /// </summary>
            Relative = 0, 

            /// <summary>
            /// Output an Absolute Url.
            /// </summary>
            Absolute, 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets QueryString.
        /// </summary>
        public QueryStringDictionary QueryString
        {
            get { return _queryString ?? (_queryString = new QueryStringDictionary()); }
            internal set { _queryString = value; }
        }

        /// <summary>
        /// Gets or sets BaseUrl.
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return GetBaseUrl();
            }

            set
            {
                var uriBuilder = new UriBuilder(MakeAbsoluteUrl(value));
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
                var path = Path;
                return path.Substring(path.LastIndexOf("/") + 1);
            }

            set
            {
                var path = Path;
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
        /// <c>true</c> if [basic UTF8 URL encoding]; otherwise, <c>false</c>.
        /// </value>
        public bool BasicUtf8UrlEncoding
        {
            get { return _basicUtf8UrlEncoding; }
            set { _basicUtf8UrlEncoding = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the application path.
        /// </summary>
        /// <returns> A <see cref="System.String"/> that represents the current Application Path.</returns>
        public static string GetApplicationPath()
        {
            return HttpContext.Current != null ? HttpContext.Current.Request.ApplicationPath : string.Empty;
        }

        /// <summary>
        /// Navigates this instance.
        /// </summary>
        public void Navigate()
        {
            Navigate(true);
        }

        /// <summary>
        /// Navigates the specified end response.
        /// </summary>
        /// <param name="endResponse">
        /// if set to <c>true</c> [end response].
        /// </param>
        public void Navigate(bool endResponse)
        {
            var uri = ToString();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Redirect(uri, endResponse);
            }
            else
            {
                throw new NullReferenceException("HttpContext.Current is not initialized");
            }
        }

        /// <summary>
        /// Format options:
        /// <para>
        /// "e" is the default and returns the string encoded using the specified encoder, or the default encoder if none specified.
        /// </para>
        /// <para>
        /// "p" always returns the string as plaintext.
        /// </para>
        /// </summary>
        /// <returns>A string representation of the Uri</returns>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        /// </PermissionSet>
        public new string ToString()
        {
            return BaseToString("e");
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return BaseToString(format);
        }
        
        #endregion
        
        #region Legacy Append Methods

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public void Append(string name)
        {
            _queryString.Append(name);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Append(string name, string value)
        {
            _queryString.Append(name, value);
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
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
            _queryString.Append(name, values);
        }

        /// <summary>
        /// Adds the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public void Add(QueryStringDictionary dictionary)
        {
            QueryString.AddRange(dictionary);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static string GetParameterName(Type type, string fieldName)
        {
            var fieldInfo = type.GetField(fieldName);
            var propInfo=type.GetProperty(fieldName);

            if (fieldInfo != null)
            {
                var parameterName = (ParameterName) Attribute.GetCustomAttribute(fieldInfo, typeof(ParameterName));
                if (parameterName != null)
                {
                    return parameterName.Value;
                }
            }
            else if (propInfo !=null)
            {
                var parameterName = (ParameterName)Attribute.GetCustomAttribute(propInfo, typeof(ParameterName));
                if (parameterName != null)
                {
                    return parameterName.Value;
                }
            }

            throw new ArgumentException(@"Does not reference a valid ParameterName Attribute", fieldName);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The make absolute url.
        /// </summary>
        /// <param name="url">The url (Uniform Resource Identifier).</param>
        /// <returns>The <see cref="System.string" />  of the url.</returns>
        private static string MakeAbsoluteUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            if (url[0] != '~')
            {
                return url;
            }

            var applicationPath = GetApplicationPath();
            if (url.Length == 1)
            {
                return applicationPath;
            }

            var indexOfUrl = 1;
            if (url[1] == '/' || url[1] == '\\')
            {
                indexOfUrl = 2;
            }

            return string.Concat(GetApplicationUri(), url.Substring(indexOfUrl));
        }

        /// <summary>
        /// The get application uri.
        /// </summary>
        /// <returns>The make absolute url.</returns>
        private static System.Uri GetApplicationUri()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var applicationUri = string.Concat(context.Request.Url.Scheme, "://", context.Request.Url.Host,
                    context.Request.Url.IsDefaultPort ? "" : ":{0}".FormatWith(context.Request.Url.Port));
                var applicationPath = context.Request.ApplicationPath;

                if (applicationPath.Length > 1)
                {
                    applicationUri = string.Concat(applicationUri, applicationPath);
                }

                return new System.Uri(applicationUri + "/");
            }

            return new System.Uri("http://localhost/");
        }

        /// <summary>
        /// Bases to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns> A <see cref="System.String"/> that represents the current Application Path.</returns>
        private string BaseToString(string format)
        {
            Query = _queryString.ToString(format, _basicUtf8UrlEncoding);
            string tempString;
            switch (_urlOutputType)
            {
                case UrlOutputType.Absolute:
                    tempString = Uri.AbsoluteUri;
                    break;
                default:
                    tempString = GetApplicationUri().MakeRelativeUri(Uri).ToString();
                    break;
            }

            return tempString;
        }

        /// <summary>
        /// The get base url.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> of the Base Url.
        /// </returns>
        /// <exception cref="UriFormatException">
        /// </exception>
        private string GetBaseUrl()
        {
            if ((UserName.Length == 0) && (Password.Length > 0))
            {
                throw new UriFormatException("net_uri_BadUserPassword");
            }

            var str = (Scheme.Length != 0) ? (Scheme + System.Uri.SchemeDelimiter) : string.Empty;
            var strArray = new[]
                               {
                                   str, 
                                   UserName, 
                                   (Password.Length > 0) ? (":" + Password) : string.Empty, (UserName.Length > 0) ? "@" : string.Empty, Host, ((Port != -1) && (Host.Length > 0)) ? (":" + Port) : string.Empty, (((Host.Length > 0) && (Path.Length != 0)) && (Path[0] != '/')) ? "/" : string.Empty, Path
                               };
            return string.Concat(strArray);
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        private void Initialise()
        {
            if (_queryString == null)
            {
                _queryString = new QueryStringDictionary(Query, _encoder);
            }
        }

       #endregion
    }
}
