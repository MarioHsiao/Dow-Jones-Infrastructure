/*using System;
using System.Text;
using System.Web;
using EMG.Utility.Managers.Core;

namespace EMG.Utility.Uri
{
        /// <summary>
        /// Summary description for UrlBuilder.
        /// </summary>
        public class Old_UrlBuilder
        {
            public enum UrlOutputType
            {
                Relative = 0,
                Absolute,
            }

            public enum UrlScheme
            {
                UnSpecified,
                /// <summary>
                /// HTTP: Hyper Text Transfer Protocol
                /// </summary>
                http,
                https,
                ftp,
                iptc,
                feed,
                pcast,
            }

            private string m_BaseUrl;
            private bool m_BasicUtf8UrlEncoding = true;
            private readonly StringBuilder m_StringBuilder = new StringBuilder();
            private UrlOutputType m_UrlOutputType = UrlOutputType.Relative;
            private UrlScheme m_UrlScheme = UrlScheme.UnSpecified;

            public string BaseUrl
            {
                get { return m_BaseUrl; }
                set
                {
                    m_BaseUrl = value;
                }
            }

            /// <summary>
            /// Gets or sets the m_ URL scheme.
            /// </summary>
            /// <value>The m_ URL scheme.</value>
            public UrlScheme Scheme
            {
                get { return m_UrlScheme; }
                set { m_UrlScheme = value; }
            }

            /// <summary>
            /// Gets or sets the type of the output.
            /// </summary>
            /// <value>The type of the output.</value>
            public UrlOutputType OutputType
            {
                get { return m_UrlOutputType; }
                set { m_UrlOutputType = value; }
            }

            
            /// <summary>
            /// Gets or sets a value indicating whether [basic UTF8 URL encoding].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [basic UTF8 URL encoding]; otherwise, <c>false</c>.
            /// </value>
            public bool BasicUtf8UrlEncoding
            {
                get { return m_BasicUtf8UrlEncoding; }
                set { m_BasicUtf8UrlEncoding = value; }
            }

            /// <summary>
            /// Appends the specified value.
            /// </summary>
            /// <param name="value">The value.</param>
            public void Append(string value)
            {
                if (string.IsNullOrEmpty(value)) return;
                m_StringBuilder.Append(HttpUtility.UrlEncode(value));
            }

            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public void Append(string name, string value)
            {
                if (string.IsNullOrEmpty(name)) return;
                if (value != null)
                {
                    if (m_StringBuilder.Length > 0)
                        m_StringBuilder.Append("&");
                    m_StringBuilder.Append(name).Append("=");
                    if (m_BasicUtf8UrlEncoding)
                        m_StringBuilder.Append(HttpUtility.UrlEncode(value));
                    else
                        m_StringBuilder.Append(HttpUtility.UrlEncode(value).Replace("+", "%20"));
                }
            }

            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public void Append(string name, int value)
            {
                Append(name, value.ToString());
            }

            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public void Append(string name, double value)
            {
                Append(name, value.ToString());
            }

            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public void Append(string name, float value)
            {
                Append(name, value.ToString());
            }


            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">if set to <c>true</c> [value].</param>
            public void Append(string name, bool value)
            {
                if (string.IsNullOrEmpty(name)) return;
                if (m_StringBuilder.Length > 0)
                    m_StringBuilder.Append("&");
                m_StringBuilder.Append(name).Append("=");
                m_StringBuilder.Append((value) ? 1.ToString() : 0.ToString()); // 0 represents false;
            }

            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="intValues">The values.</param>
            public void Append(string name, int[] intValues)
            {
                if (intValues != null && intValues.Length > 0)
                {
                    string[] values = Array.ConvertAll<int, string>(intValues, delegate(int i) { return i.ToString(); });
                    if (values.Length > 0)
                    {
                        Append(name, values);
                    }
                }
            }

            /// <summary>
            /// Appends the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="values">The values.</param>
            public void Append(string name, string[] values)
            {
                if (string.IsNullOrEmpty(name)) return;
                if (BasicUtilitiesManager.IsValid(values))
                {
                    if (m_StringBuilder.Length > 0)
                        m_StringBuilder.Append("&");
                    m_StringBuilder.Append(name).Append("=");
                    int i = 0;
                    foreach (string value in values)
                    {
                        if (string.IsNullOrEmpty(value)) continue;
                        if (i != 0)
                            m_StringBuilder.Append(",");
                        if (m_BasicUtf8UrlEncoding)
                            m_StringBuilder.Append(HttpUtility.UrlEncode(value));
                        else
                            m_StringBuilder.Append(HttpUtility.UrlEncode(value).Replace("+", "%20"));
                        i++;
                    }
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                // insert url
                if (m_StringBuilder.Length > 0)
                {
                    m_StringBuilder.Insert(0, "?");
                }
                switch (m_UrlOutputType)
                {
                    case UrlOutputType.Absolute:
                        m_StringBuilder.Insert(0, MakeAbsoluteUrl(m_BaseUrl));
                        break;
                    default:
                        m_StringBuilder.Insert(0, MakeRelativeUrl(m_BaseUrl));
                        break;

                }
                return m_StringBuilder.ToString();
            }

            private static string MakeRelativeUrl(string url)
            {
                return WebUtilitiesManager.MakeRelativeUrl(url);
            }

            private string MakeAbsoluteUrl(string url)
            {
                if (string.IsNullOrEmpty(url))
                    return url;
                if (url[0] != '~')
                    return url;
                string applicationPath = HttpContext.Current.Request.ApplicationPath;
                if (url.Length == 1)
                    return applicationPath;
                int indexOfUrl = 1;
                if (url[1] == '/' || url[1] == '\\')
                    indexOfUrl = 2;
                return string.Concat(GetApplicationUrl(), "/", url.Substring(indexOfUrl));
            }

            private string GetApplicationUrl()
            {
                HttpContext context = HttpContext.Current;
                string applicationUrl;

                switch (m_UrlScheme)
                {
                    case UrlScheme.UnSpecified:
                        applicationUrl = string.Concat(context.Request.Url.Scheme, "://", context.Request.Url.Host);
                        break;
                    default:
                        applicationUrl = string.Concat(m_UrlScheme.ToString(), "://", context.Request.Url.Host);
                        break;
                }
                string applicationPath = context.Request.ApplicationPath;

                if (applicationPath.Length > 1)
                {
                    applicationUrl = string.Concat(applicationUrl, applicationPath);
                }

                return applicationUrl;
            }
        }
    }
*/