// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartingManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Proxy class that abstracts Charting Server from user.
// </summary>
// <author>David Da Costa</author>
// <lastModified>
//  <entry type="update"><date>10/02/20010</date><author>David Da Costa</author></entry>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------
using System;
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
using DowJones.Exceptions;
using DowJones.Formatters.Algorithms.Text;
using DowJones.Loggers;
using DowJones.Managers.Core;
using DowJones.Properties;
using log4net;

namespace DowJones.Charting.Manager
{
    /// <summary>
    /// Proxy class that abstracts Charting Server from user.
    /// </summary>
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class ChartingManager
    {
        /// <summary>
        /// The cache time in seconds.
        /// </summary>
        private const int CacheTimeInSeconds = 2000;

        /// <summary>
        /// The cluster monitor address cache key.
        /// </summary>
        private const string ClusterMonitorAddressCacheKey = "emg.utility.charting.core.clusterAdress";

        /// <summary>
        ///  This string is used to put title tags on netscape and mozilla enabled browsers.
        /// </summary>
        private const string AltReplacement = @"title=""${att}"" ${full}";

        /// <summary>
        /// The reference to the Log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChartingManager));

        /// <summary>
        /// The reg options.
        /// </summary>
        private const RegexOptions RegOptions = RegexOptions.IgnoreCase
                                                          | RegexOptions.Multiline
                                                          | RegexOptions.ExplicitCapture
                                                          | RegexOptions.IgnorePatternWhitespace
                                                          | RegexOptions.Compiled;

        /// <summary>
        /// Static Regular Expression
        /// </summary>
        private static readonly Regex AltAttributeRegex = new Regex(
            @"(?<full>alt=""(?<att>.*?)"")", 
            RegOptions);

        /// <summary>
        /// The comment regex.
        /// </summary>
        private static readonly Regex CommentRegex = new Regex(
            @"<!--*(?:[\S\s]*?)-->", 
            RegOptions);

        /// <summary>
        /// The no script regex.
        /// </summary>
        private static readonly Regex NoScriptRegex = new Regex(
            @"<noscript[^>]*(?<obj>\/>|>[\S\s]*?)<\/noscript>", 
            RegOptions);

        /// <summary>
        /// The script regex.
        /// </summary>
        private static readonly Regex ScriptRegex = new Regex(
            @"<script[^>]*(?<obj>\/>|>[\S\s]*?)<\/script>", 
            RegOptions);

        /// <summary>
        /// Get Chart(s) from Corda charting server.
        /// </summary>
        /// <param name="itxml">The itxml.</param>
        /// <param name="pcScript">The pc script.</param>
        /// <param name="template">A <see cref="ChartTemplate"/> that has been defined in configuration file.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns>A string of html representing the chart.</returns>
        public static string GetChartEmbededHtmlByITXML(string itxml, string pcScript, ChartTemplate template, bool useCache)
        {
            return GetChartEmbededHtmlByITXML(itxml, pcScript, template, useCache, false);
        }

        /// <summary>
        /// Prepares the string.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns>
        /// A correctly prepared string for both LTR and RTL languages
        /// </returns>
        internal static string PrepareString(string original)
        {
            return PrepareString(original, false, 0);
        }

        /// <summary>
        /// Prepares the string.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="showEllipsis">if set to <c>true</c> [show ellipsis].</param>
        /// <returns>
        /// A correctly prepared string for both LTR and RTL languages
        /// </returns>
        internal static string PrepareString(string original, bool showEllipsis)
        {
            return PrepareString(original, showEllipsis, 0);
        }

        /// <summary>
        /// Prepares the string.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="showEllipsis">if set to <c>true</c> [show ellipsis].</param>
        /// <param name="index">The target index.</param>
        /// <returns>
        /// A correctly prepared string for both LTR and RTL languages
        /// </returns>
        internal static string PrepareString(string original, bool showEllipsis, int index)
        {
            if (index <= 3)
            {
                index = 14;    
            }

            if (StringUtilitiesManager.HasArabicSlashHebreCharacters(original))
            {
                var res = Bidi.LogicalToVisual(original);
                if (showEllipsis && index > 0)
                {
                    return string.Concat("...", res.Substring(res.Length - index + 3));
                }

                return res;
            }

            return original;
        }

        /// <summary>
        /// Get Chart(s) from Corda charting server.
        /// </summary>
        /// <param name="itxml">The itxml.</param>
        /// <param name="pcScript">The pc script.</param>
        /// <param name="template">A <see cref="ChartTemplate"/> that has been defined in configuration file.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="isSecure">if set to <c>true</c> [use https].</param>
        /// <returns>A string of html representing the chart.</returns>
        public static string GetChartEmbededHtmlByITXML(string itxml, string pcScript, ChartTemplate template, bool useCache, bool isSecure)
        {
            var logger = new TransactionLogger(_log);
            try
            {
                var image = GetCordaEmbedder(itxml, pcScript, template, useCache, isSecure);

                string data;
                switch (template.OutputChartType)
                {
                    case OutputChartType.PNG:
                    case OutputChartType.GIF:
                    case OutputChartType.JPEG:
                        data = StripJavscript(AltAttributeRegex.Replace(image.getEmbeddingHTML(), AltReplacement));
                        break;
                    case OutputChartType.GIF_WITH_JAVASCRIPT_INTERACTIVITY:
                    case OutputChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY:
                    case OutputChartType.JPEG_WITH_JAVASCRIPT_INTERACTIVITY:
                        data = AltAttributeRegex.Replace(image.getEmbeddingHTML(), AltReplacement);
                        break;
                    case OutputChartType.FLASH_WITH_ACTIVEX_FIX:
                        image.addObjectParamTag("wmode", "transparent");
                        data = image.getEmbeddingHTML();
                        break;
                    case OutputChartType.FLASH:

                        // Parse out the noScript Tag and get the object
                        image.addObjectParamTag("wmode", "transparent");
                        data = GetDataInsideANoScriptTag(image.getEmbeddingHTML());
                        if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim()))
                        { 
                            return image.getEmbeddingHTML();
                        }

                        break;
                    default:
                        data = image.getEmbeddingHTML();
                        break;
                }

                if (_log.IsDebugEnabled)
                {
                    _log.Debug("Resulting Html: " + data);
                    _log.Debug(image.getOutputGenerationError());
                }

                logger.LogTimeSinceInvocation(MethodBase.GetCurrentMethod().Name);
                return data;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Procedure that is used to encode a utf-8 string to Corda Server input string for muitilingual character displays in chart.
        /// </summary>
        /// <param name="input">Data <see cref="string"/> that is being inputed into chart.</param>
        /// <returns>A encoded string for use in PCScript</returns>
        public static string ConvertToAmpersandPoundHex(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(input.Trim()))
            {
                return null;
            }

            var sb = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                int d1 = input[i];
                var eachChar = input[i].ToString();
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
        /// Encodes for special characters for PcScript.
        /// </summary>
        /// <param name="obj">The base obj to encode for.</param>
        /// <returns>A string with properly encoded characters</returns>
        public static string EncodeSpecialCharsForPcScript(object obj)
        {
            if (obj is string)
            {
                var input = (string)obj;
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(input.Trim()))
                {
                    return null;
                }

                var sb = new StringBuilder(input);
                sb.Replace(",", "\\|,");
                sb.Replace(";", "\\|;");
                return sb.ToString();
            }

            return EncodeSpecialCharsForPcScript(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", obj));
        }

        /// <summary>
        /// Gets the image tag from HTML.
        /// </summary>
        /// <param name="embeddingHtml">
        /// The embedding HTML.
        /// </param>
        /// <returns>
        /// The extract image tag from html.
        /// </returns>
        public static string ExtractImageTagFromHTML(string embeddingHtml)
        {
            var stringReader = new StringReader("<div>" + embeddingHtml + "</div>");
            var reader = new XmlTextReader(stringReader);
            var bDone = false;
            var srcValue = string.Empty;
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
        /// <param name="embeddingHtml">The embedding HTML.</param>
        /// <returns>
        /// The extract source attribute from embeded html.
        /// </returns>
        public static string ExtractSourceAttributeFromEmbededHTML(string embeddingHtml)
        {
            var stringReader = new StringReader("<div>" + embeddingHtml + "</div>");
            var reader = new XmlTextReader(stringReader);
            var bDone = false;
            var srcValue = string.Empty;
            while (!bDone && reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.LocalName.ToLower() == "img")
                        {
                            if (reader.HasAttributes)
                            {
                                reader.MoveToAttribute("src");
                                srcValue = reader.Value;
                                bDone = true;
                            }
                        }

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
                            while (!bDone && reader.Read())
                            {
                                if (reader.LocalName.ToLower() != "param" || !reader.HasAttributes)
                                {
                                    continue;
                                }

                                reader.MoveToAttribute("name");
                                var temp = reader.Value;
                                if (string.IsNullOrEmpty(temp) || temp.ToUpper() != "MOVIE")
                                {
                                    continue;
                                }

                                reader.MoveToAttribute("value");
                                srcValue = reader.Value;
                                bDone = true;
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
                serverLocation = (string) HttpRuntime.Cache[ClusterMonitorAddressCacheKey];
            }

            if (string.IsNullOrEmpty(serverLocation))
            {
                var request = (HttpWebRequest) WebRequest.Create(clusterMonitorAddress);

                // sm 5/2/07.. issue when charts do not come up for the previewservice with dummy data in newsviews.
                // this fixes it.. apparently a known issue with MS>.. and a known workaround.
                request.KeepAlive = false;
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                if (!String.IsNullOrEmpty(Settings.Default.WebResourcesProxy))
                {
                    request.Proxy = new WebProxy(Settings.Default.WebResourcesProxy);
                }

                var response = (HttpWebResponse) request.GetResponse();
                var stream = new StreamReader(response.GetResponseStream(), Encoding.ASCII);

                try
                {
                    serverLocation = stream.ReadToEnd();
                }
                catch (Exception e)
                {
                    _log.Debug(e.Message);
                }
                finally
                {
                    stream.Close();
                }

                var url = string.Empty;
                if (!string.IsNullOrEmpty(serverLocation))
                {
                    var temp = serverLocation.Split(',');
                    url = "http://" + temp[1].Trim();
                    if (HttpRuntime.Cache != null)
                    {
                        HttpRuntime.Cache.Insert(ClusterMonitorAddressCacheKey, url, null, DateTime.Now.AddMilliseconds(Settings.Default.Corda_InternalServerAddress_Cache_MS), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }
                }

                return url;
            }

            return serverLocation;
        }

        /// <summary>
        /// Gets the chart bytes.
        /// </summary>
        /// <param name="itxml">The itxml.</param>
        /// <param name="pcScript">The pc script.</param>
        /// <param name="template">The template.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns>The chart byte array.</returns>
        public static byte[] GetChartBytes(string itxml, string pcScript, ChartTemplate template, bool useCache)
        {
            try
            {
                var image = GetCordaEmbedder(itxml, pcScript, template, useCache, false);

                byte[] data;
                switch (template.OutputChartType)
                {
                    default:
                        data = image.getBytes();
                        break;
                }

                if (_log.IsDebugEnabled)
                {
                    _log.Debug(image.getOutputGenerationError());
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Converts to percent U.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The convert to percent u.</returns>
        public static string ConvertToPercentU(string input)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                int d1 = input[i];
                var eachChar = input[i].ToString();
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
        /// The get data inside a no script tag.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A string value of the data inside the NOSCRIPT html tag.</returns>
        private static string GetDataInsideANoScriptTag(string input)
        {
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("Input: {0}", input);
            }

            var sb = new StringBuilder(ScriptRegex.Replace(input, string.Empty));
            sb = new StringBuilder(CommentRegex.Replace(sb.ToString(), string.Empty));

            var temp = NoScriptRegex.Match(sb.ToString());
            if (temp.Success && temp.Captures.Count > 0)
            {
                var replacer = new StringBuilder(temp.Captures[0].Value.Trim());
                replacer = replacer.Replace("<noscript>", string.Empty);
                return replacer.Replace("</noscript>", string.Empty).ToString();
            }

            return sb.ToString();
        }

        /// <summary>
        /// The strip javscript.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The string containint the striped javscript.</returns>
        private static string StripJavscript(string input)
        {
            var original = new string(input.ToCharArray());

            var sb = new StringBuilder(ScriptRegex.Replace(input, string.Empty));
            sb = new StringBuilder(CommentRegex.Replace(sb.ToString(), string.Empty));
            sb = new StringBuilder(NoScriptRegex.Replace(sb.ToString(), string.Empty));
            var temp = NoScriptRegex.Match(original);
            if (temp.Success && temp.Groups.Count > 0)
            {
                sb.Append(temp.Groups["obj"].Value.Trim().TrimStart('>'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// The normalize enumeration.
        /// </summary>
        /// <param name="outputChartType">The output chart type.</param>
        /// <returns>A normalize enumeration.</returns>
        private static string NormalizeEnumeration(OutputChartType outputChartType)
        {
            return outputChartType.ToString()
                .Replace("_WITH_JAVASCRIPT_INTERACTIVITY", string.Empty)
                .Replace("_WITH_ACTIVEX_FIX", string.Empty);
        }

        /// <summary>
        /// The get secure external server address.
        /// </summary>
        /// <param name="isSecure">The is secure.</param>
        /// <returns>The string of the secure external server address.</returns>
        private static string GetSecureExternalServerAddress(bool isSecure)
        {
            if (isSecure && Settings.Default.Corda_ExternalServerAddress.ToLowerInvariant().StartsWith("http://"))
            {
                return string.Concat("https", Settings.Default.Corda_ExternalServerAddress.Substring(4));
            }

            return Settings.Default.Corda_ExternalServerAddress;
        }

        /// <summary>
        /// Gets the corda embedder.
        /// </summary>
        /// <param name="itxml">The itxml.</param>
        /// <param name="pcscript">The pc script.</param>
        /// <param name="template">The template.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="isSecure">if set to <c>true</c> [is secure].</param>
        /// <returns>The Corda 7 version of the Corda Embedder</returns>
        private static Corda7Embedder GetCordaEmbedder(string itxml, string pcscript, ChartTemplate template, bool useCache, bool isSecure)
        {
            var image = new CordaEmbedder();
            itxml = ConvertToAmpersandPoundHex(itxml);
            pcscript = ConvertToPercentU(pcscript);
            image.externalServerAddress = GetSecureExternalServerAddress(isSecure);
            image.internalCommPortAddress = GetInternalCommPortAddress(Settings.Default.Corda_InternalServerAddress);
            image.imageTemplate = template.AppearanceFile;
            image.width = template.Width;
            image.height = template.Height;
            image.returnDescriptiveLink = false;
            image.outputType = NormalizeEnumeration(template.OutputChartType);
            image.useCache = useCache;

            // Get the UserAgent if applicable
            if (HttpContext.Current != null)
            {
                image.userAgent = HttpContext.Current.Request.UserAgent;
            }

            // Add PCScript
            if (!string.IsNullOrEmpty(pcscript) && !string.IsNullOrEmpty(pcscript.Trim()))
            {
                image.pcScript = pcscript;
            }

            // Add ITXML
            if (!string.IsNullOrEmpty(itxml) && !string.IsNullOrEmpty(itxml.Trim()))
            {
                image.addITXML(itxml);
            }

            if (_log.IsDebugEnabled)
            {
                _log.Debug("ExternalServerAddress: " + image.externalServerAddress);
                _log.Debug("InternalServerAddress: " + image.internalCommPortAddress);
                _log.Debug("ApperanceFile: " + image.imageTemplate);
                _log.Debug("OutputType: " + image.outputType);
                _log.Debug("ITXML: " + itxml);
                _log.Debug("pcscript: " + pcscript);
            }

            return image;
        }
    }
}