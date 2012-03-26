using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Corda;
using EMG.Utility.Exceptions;
using EMG.Utility.Loggers;
using log4net;

namespace EMG.Tools.Charting.Core
{
    /// <summary>
    /// Proxy class that abstracts Charting Server from user.
    /// </summary>
    internal class ChartingService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (ChartingService));
        private static readonly string CLUSTER_MONITOR_ADDRESS_CACHE_KEY = "emg.utility.charting.core.clusterAdress";

        /// <summary>
        ///  This string is used to put title tags on netscape and mozilla enabled browsers.
        /// </summary>
        protected static String AltReplacement = @"title=""${att}"" ${full}";

        /// <summary>
        /// Static Regular Expression
        /// </summary>
        protected static Regex AltAttributeRegex = new Regex(
            @"(?<full>alt=""(?<att>.*?)"")",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.ExplicitCapture
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );
        
        protected static Regex CommentRegex = new Regex(
            @"<!--*(?:[\S\s]*?)-->",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.Singleline
            | RegexOptions.ExplicitCapture
            | RegexOptions.Compiled
            );

        protected static Regex NoScriptRegex = new Regex(
            @"<noscript[^>]*(?<obj>\/>|>[\S\s]*?)<\/noscript>",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.Singleline
            | RegexOptions.ExplicitCapture
            | RegexOptions.Compiled
            );

        protected static Regex ScriptRegex = new Regex(
            @"<script[^>]*(?<obj>\/>|>[\S\s]*?)<\/script>",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.Singleline
            | RegexOptions.ExplicitCapture
            | RegexOptions.Compiled
            );

        /// <summary>
        /// 	<para>Procedure that is used to encode a utf-8 string to Corda Server input string for muitilingual character displays in chart.</para>
        /// </summary>
        /// <param name="input">Data <see cref="string"/> that is being inputed into chart.</param>
        /// <returns>A encoded string for use in PCScript</returns>
        public static string ConvertToAmpersandPoundHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                int d1 = input[i];
                string eachChar = input[i].ToString();
                if (d1 < 0)
                {
                    d1 = d1 + 65536;
                }

                if (d1 > 127)
                {
                    eachChar = "&#x" + d1.ToString("x4") + ";";
                }
                sb.Append(eachChar);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the image tag from HTML.
        /// </summary>
        /// <param name="embeddingHtml">The embedding HTML.</param>
        /// <returns></returns>
        public static string GetImageTagFromHTML(string embeddingHtml)
        {
            StringReader stringReader = new StringReader("<div>" + embeddingHtml + "</div>");
            XmlTextReader reader = new XmlTextReader(stringReader);
            bool bDone = false;
            string srcValue = string.Empty;
            while (!bDone && reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.LocalName.ToLower() == "img")
                        {
                            srcValue = reader.ReadOuterXml();
                            bDone = true;
                        }
                        break;
                }
            }
            return srcValue;
        }


        /// <summary>
        /// Gets the flash movie src.
        /// </summary>
        /// <param name="objectSourceString">The embedding HTML.</param>
        /// <returns></returns>
        public static string GetFlashMovieSrc(string objectSourceString)
        {
            StringReader stringReader = new StringReader("<div>" + objectSourceString + "</div>");
            XmlTextReader reader = new XmlTextReader(stringReader);
            bool bDone = false;
            string srcValue = string.Empty;
            while (!bDone && reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.LocalName.ToLower() == "embed")
                        {
                            if (reader.HasAttributes)
                            {
                                reader.MoveToAttribute("src");
                                srcValue = reader.Value;
                                bDone = true;
                            }
                        }
                        if (reader.LocalName.ToLower() == "object")
                        {
                            reader.Read();
                            while (!bDone && reader.Read())
                            {
                                if (reader.LocalName.ToLower() == "param")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        reader.MoveToAttribute("name");
                                        string temp = reader.Value;
                                        if (!string.IsNullOrEmpty(temp) && temp.ToUpper() == "MOVIE")
                                        {
                                            reader.MoveToAttribute("value");
                                            srcValue = reader.Value;
                                            bDone = true;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            return srcValue;
        }


        /// <summary>
        /// Converts to percent U.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected static string ConvertToPercentU(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                int d1 = input[i];
                string eachChar = input[i].ToString();
                if (d1 < 0)
                {
                    d1 = d1 + 65536;
                }

                if (d1 > 127)
                {
                    eachChar = "%u" + d1.ToString("x4");
                }
                sb.Append(eachChar);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Gets Internal Communication Port Address from config file that is married to ctClusterMonitor.dll.
        /// </summary>
        /// <param name="clusterMonitorAddress">Web address of isapi ctClusterMonitor.dll</param>
        /// <returns>
        /// string representation of ipaddress and port communication information
        /// </returns>
        public static string GetInternalCommPortAddress(string clusterMonitorAddress)
        {
            string serverLocation = null;
            if (HttpRuntime.Cache != null)
            {
                serverLocation = (string) HttpRuntime.Cache[CLUSTER_MONITOR_ADDRESS_CACHE_KEY];
            }

            if (string.IsNullOrEmpty(serverLocation))
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(clusterMonitorAddress);
                //sm 5/2/07.. issue when charts do not come up for the previewservice with dummy data in newsviews.
                //this fixes it.. apparently a known issue with MS>.. and a known workaround.
                request.KeepAlive = false;
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.ASCII);

                try
                {
                    serverLocation = stream.ReadToEnd();
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }

                finally
                {
                    stream.Close();
                }
                string url = string.Empty;
                if (!string.IsNullOrEmpty(serverLocation))
                {
                    String[] temp = serverLocation.Split(',');
                    url = "http://" + temp[1].Trim();
                    if (HttpRuntime.Cache != null)
                    {
                        HttpRuntime.Cache.Insert(CLUSTER_MONITOR_ADDRESS_CACHE_KEY, url, null, DateTime.Now.AddSeconds(10), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }
                }
                return url;
            }
            else
            {
                return serverLocation;
            }
        }


        private static string GetDataInsideANoScript(string input)
        {
            input = CommentRegex.Replace(input, string.Empty).Trim();
            Match temp = NoScriptRegex.Match(input);
            if (temp.Success && temp.Groups.Count > 0)
            {
                return temp.Groups["obj"].Value.Trim();
            }
            return input;
        }

        private static string StripJavscript(string input)
        {
            string original = new string(input.ToCharArray());
            input = ScriptRegex.Replace(input, string.Empty);
            input = CommentRegex.Replace(input, string.Empty);
            input = NoScriptRegex.Replace(input, string.Empty);


            StringBuilder sb = new StringBuilder(input);
            Match temp = NoScriptRegex.Match(original);
            if (temp.Success && temp.Groups.Count > 0)
            {
                sb.Append(temp.Groups["obj"].Value.Trim());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get Chart(s) from Corda charting server.
        /// </summary>
        /// <param name="itxml">The itxml.</param>
        /// <param name="template">A <see cref="ChartTemplate"/> that has been defined in configuration file.</param>
        /// <returns>A string of html representing the chart.</returns>
        public static string GetChartEmbededHtmlByITXML(string itxml, ChartTemplate template)
        {
            TransactionLogger logger = new TransactionLogger(_log);
            try
            {
                Corda7Embedder _image = new Corda7Embedder();
                string _itxml = ConvertToAmpersandPoundHex(itxml);
                _image.externalServerAddress = "http://nevada.dev.us.factiva.com/pcredirector/v7/ctredirector.dll";
                _image.internalCommPortAddress = GetInternalCommPortAddress("http://nevada.dev.us.factiva.com/pcredirector/v7/ctClusterMonitor.dll");
                _image.imageTemplate = template.appearanceFile;

                if (HttpContext.Current != null)
                {
                    _image.userAgent = HttpContext.Current.Request.UserAgent;
                }

                _image.addITXML(_itxml);
                _image.width = template.width;
                _image.height = template.height;
                _image.returnDescriptiveLink = false;
                _image.outputType = template.chartType.ToString();

                string _data;
                switch (template.chartType)
                {
                    case ChartType.PNG:
                    case ChartType.GIF:
                    case ChartType.JPEG:
                        _data = AltAttributeRegex.Replace(_image.getEmbeddingHTML(), AltReplacement);
                        break;
                    case ChartType.GIF_WITH_JAVASCRIPT_INTERACTIVITY:
                    case ChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY:
                    case ChartType.JPEG_WITH_JAVASCRIPT_INTERACTIVITY:
                        _data = StripJavscript(AltAttributeRegex.Replace(_image.getEmbeddingHTML(), AltReplacement));
                        break;
                    case ChartType.FLASH_WITH_ACTIVEX_FIX:
                        _data = _image.getEmbeddingHTML();
                        break;
                    case ChartType.FLASH:
                        // Parse out the noScript Tag and get the object
                        _data = GetDataInsideANoScript(_image.getEmbeddingHTML());
                        break;
                    default:
                        _data = _image.getEmbeddingHTML();
                        break;
                }
                if (_log.IsDebugEnabled)
                {
                    _log.Debug("ExternalServerAddress: " + _image.externalServerAddress);
                    _log.Debug("InternalServerAddress: " + _image.internalCommPortAddress);
                    _log.Debug("ApperanceFile: " + _image.imageTemplate);
                    _log.Debug("OutputType: " + _image.outputType);
                    _log.Debug("ITXML: " + _itxml);
                    _log.Debug("Resulting Html: " + _data);
                }
                logger.LogTimeSinceInvocation(MethodInfo.GetCurrentMethod().Name);
                return _data;
            }
            catch (Exception ex)
            {
                throw new EMGUtilitiesException(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}