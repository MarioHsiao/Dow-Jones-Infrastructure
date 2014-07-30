using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using AjaxControlToolkit;
using log4net;

namespace EMG.widgets.ui.core
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptCombiner
    {
        /// <summary>
        /// Request param name for the serialized combined scripts string
        /// </summary>
        private const string CombinedScriptsParamName = "_TSM_CombinedScripts_";

        /// <summary>
        /// Request param name for the hidden field name
        /// </summary>
        private const string HiddenFieldParamName = "_TSM_HiddenField_";

        private static readonly ILog Log = LogManager.GetLogger(typeof(ScriptCombiner));
        
        /// <summary>
        /// Regular expression for detecting WebResource/ScriptResource substitutions in script files
        /// </summary>
        protected static readonly Regex WebResourceRegex = new Regex("<%\\s*=\\s*(?<resourceType>WebResource|ScriptResource)\\(\"(?<resourceName>[^\"]*)\"\\)\\s*%>", RegexOptions.Singleline | RegexOptions.Multiline);

        /// <summary>
        /// Gets the assembly modified time.
        /// </summary>
        /// <returns><c ref="DateTime">DateTime object</c></returns>
        private static DateTime GetAssemblyModifiedTime()
        {
            var lastWriteTime = File.GetLastWriteTime(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath);
            return new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day, lastWriteTime.Hour, lastWriteTime.Minute, 0);
        }

        /// <summary>
        /// Outputs the combined script file requested by the HttpRequest to the HttpResponse
        /// </summary>
        /// <param name="context">HttpContext for the transaction</param>
        /// <returns>true if the script file was output</returns>
        public static bool OutputCombinedScriptFile(HttpContext context)
        {
            // See "if Modified since" was requested in the http headers, and check it with the assembly modified time
            var assemblyModifiedDate = GetAssemblyModifiedTime();

            var s = context.Request.Headers["If-Modified-Since"];
            DateTime tempDate;
            if (((s != null) && DateTime.TryParse(s, out tempDate)) && (tempDate >= assemblyModifiedDate))
            {
                context.Response.StatusCode = 0x130;
                return true;
            }

            // Initialize
            var request = context.Request;
            var hiddenFieldName = request.Params[HiddenFieldParamName];
            var combinedScripts = request.Params[CombinedScriptsParamName];

            if (string.IsNullOrEmpty(hiddenFieldName) || string.IsNullOrEmpty(combinedScripts))
            {
                return false;
            }

            // This is a request for a combined script file
            var response = context.Response;
            response.ContentType = "application/x-javascript";

            // Set the same (~forever) caching rules that ScriptResource.axd uses
            var cache = response.Cache;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByParams[HiddenFieldParamName] = true;
            cache.VaryByParams[CombinedScriptsParamName] = true;
            cache.VaryByContentEncodings["gzip"] = true;
            cache.VaryByContentEncodings["deflate"] = true;
            cache.SetOmitVaryStar(true);
            cache.SetExpires(DateTime.Now.AddDays(365));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(assemblyModifiedDate);

            // Get the stream to write the combined script to (using a compressed stream if requested)
            // Certain versions of IE6 have difficulty with compressed responses, so we
            // don't compress for those browsers (just like ASP.NET AJAX's ScriptResourceHandler)
            var outputStream = response.OutputStream;
            if (!request.Browser.IsBrowser("IE") || (6 < request.Browser.MajorVersion))
            {
                foreach (var acceptEncoding in (request.Headers["Accept-Encoding"] ?? "").ToUpperInvariant().Split(','))
                {
                    if ("GZIP" == acceptEncoding)
                    {
                        // Browser wants GZIP; wrap the output stream with a GZipStream
                        response.AddHeader("Content-encoding", "gzip");
                        outputStream = new GZipStream(outputStream, CompressionMode.Compress);
                        break;
                    }
                    if ("DEFLATE" != acceptEncoding)
                        continue;
                    // Browser wants Deflate; wrap the output stream with a DeflateStream
                    response.AddHeader("Content-encoding", "deflate");
                    outputStream = new DeflateStream(outputStream, CompressionMode.Compress);
                    break;
                }
            }

            // Output the combined script
            using (var outputWriter = new StreamWriter(outputStream))
            {
                // Get the list of scripts to combine
                var scriptEntries = DeserializeScriptEntries(HttpUtility.UrlDecode(combinedScripts), false);

                // Write the scripts
                WriteScripts(scriptEntries, outputWriter);

                // Write the ASP.NET AJAX script notification code
                outputWriter.WriteLine("if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();");

                // Write a handler to run on page load and update the hidden field tracking scripts loaded in the browser
                outputWriter.WriteLine(string.Format(CultureInfo.InvariantCulture,
                    "(function() {{" +
                    "var fn = function() {{" +
                    "$get('{0}').value += '{1}';" +
                    "Sys.Application.remove_load(fn);" +
                    "}};" +
                    "Sys.Application.add_load(fn);" +
                    "}})();", hiddenFieldName, SerializeScriptEntries(scriptEntries, true)));
            }

            return true;
        }

        /// <summary>
        /// Serialize a list of ScriptEntries
        /// </summary>
        /// <remarks>
        /// Serialized list looks like:
        /// ;Assembly1.dll Version=1:Culture:MVID1:ScriptName1Hash:ScriptName2Hash;Assembly2.dll Version=2:Culture:MVID1:ScriptName3Hash
        /// </remarks>
        /// <param name="scriptEntries">list of scripts to serialize</param>
        /// <param name="allScripts">true iff all scripts should be serialized; otherwise only not loaded ones</param>
        /// <returns>serialized list</returns>
        private static string SerializeScriptEntries(IEnumerable<ScriptEntry> scriptEntries, bool allScripts)
        {
            // Serialized string must never be null (';' is safe)
            var serializedScriptEntries = new StringBuilder(";");
            string currentAssembly = null;
            foreach (var scriptEntry in scriptEntries)
            {
                if (!allScripts && scriptEntry.Loaded)
                    continue;
                // Serializing this script name
                if (currentAssembly != scriptEntry.Assembly)
                {
                    // It's a different assembly, so serialize the assembly name and Culture.MVID value first
                    serializedScriptEntries.Append(";");
                    serializedScriptEntries.Append(scriptEntry.Assembly);
                    serializedScriptEntries.Append(":");
                    serializedScriptEntries.Append(CultureInfo.CurrentUICulture.IetfLanguageTag);
                    serializedScriptEntries.Append(":");
                    serializedScriptEntries.Append(scriptEntry.LoadAssembly().ManifestModule.ModuleVersionId);
                    currentAssembly = scriptEntry.Assembly;
                }
                // Serialize the script name hash
                serializedScriptEntries.Append(":");
                serializedScriptEntries.Append(scriptEntry.Name.GetHashCode().ToString("x", CultureInfo.InvariantCulture));
            }
            return serializedScriptEntries.ToString();
        }

        /// <summary>
        /// Checks if the specified ScriptEntry is combinable
        /// </summary>
        /// <param name="scriptEntry">ScriptEntry to check</param>
        /// <returns>true iff combinable</returns>
        private static bool IsScriptCombinable(ScriptEntry scriptEntry)
        {
            // Load the script's assembly and look for ScriptCombineAttribute
            var combinable = false;
            var assembly = scriptEntry.LoadAssembly();
            foreach (ScriptCombineAttribute scriptCombineAttribute in assembly.GetCustomAttributes(typeof (ScriptCombineAttribute), false))
            {
                if (string.IsNullOrEmpty(scriptCombineAttribute.IncludeScripts))
                {
                    // If the IncludeScripts property is empty, all scripts are combinable by default
                    combinable = true;
                }
                else
                {
                    // IncludeScripts specifies the combinable scripts
                    if (scriptCombineAttribute.IncludeScripts.Split(',')
                        .Any(includeScript => 0 == string.Compare(scriptEntry.Name, includeScript.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        combinable = true;
                    }
                }
                if (string.IsNullOrEmpty(scriptCombineAttribute.ExcludeScripts)) continue;
                // ExcludeScripts specifies the non-combinable scripts (and overrides IncludeScripts)
                if (scriptCombineAttribute.ExcludeScripts.Split(',')
                    .Any(excludeScript => 0 == string.Compare(scriptEntry.Name, excludeScript.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    combinable = false;
                }
            }
            
            if (!combinable)
            {
                return false;
            }

            // Make sure the script has an associated WebResourceAttribute (else ScriptManager wouldn't have served it)
            var correspondingWebResourceAttribute = assembly.GetCustomAttributes(typeof (WebResourceAttribute), false)
                                                    .Cast<WebResourceAttribute>()
                                                    .Any(webResourceAttribute => scriptEntry.Name == webResourceAttribute.WebResource);

            // Don't allow it to be combined if not
            combinable &= correspondingWebResourceAttribute;
            return combinable;
        }

        /// <summary>
        /// Deserialize a list of ScriptEntries
        /// </summary>
        /// <remarks>
        /// Serialized list looks like:
        /// ;Assembly1.dll Version=1:Culture:MVID1:ScriptName1Hash:ScriptName2Hash;Assembly2.dll Version=2:Culture:MVID1:ScriptName3Hash
        /// </remarks>
        /// <param name="serializedScriptEntries">serialized list</param>
        /// <param name="loaded">loaded state of the serialized scripts</param>
        /// <returns>list of scripts</returns>
        private static List<ScriptEntry> DeserializeScriptEntries(string serializedScriptEntries, bool loaded)
        {
            var scriptEntries = new List<ScriptEntry>();
            foreach (var assemblyScripts in serializedScriptEntries.Split(';'))
            {
                // Deserialize this assembly's scripts
                string assembly = null;
                string culture = null;
                string mvid = null;
                Dictionary<string, string> resourceNameHashToResourceName = null;
                foreach (var script in assemblyScripts.Split(':'))
                {
                    if (null == assembly)
                    {
                        // Haven't got the assembly name yet; this is it
                        assembly = script;
                    }
                    else if (null == culture)
                    {
                        // Haven't got the culture value yet; this is it
                        culture = script;
                    }
                    else if (null == mvid)
                    {
                        // Haven't got the MVID value yet; this is it
                        mvid = script;
                    }
                    else
                    {
                        if (null == resourceNameHashToResourceName)
                        {
                            // Populate the "resource name hash to resource name" dictionary for this assembly
                            resourceNameHashToResourceName = new Dictionary<string, string>();
                            foreach (string resourceName in (new ScriptEntry(assembly, null, null)).LoadAssembly().GetManifestResourceNames())
                            {
                                var hashCode = resourceName.GetHashCode().ToString("x", CultureInfo.InvariantCulture);
                                if (Log.IsInfoEnabled)
                                {
                                    Log.InfoFormat("hashCode {0} for resourceName:{1}", hashCode, resourceName);
                                }
                                if (resourceNameHashToResourceName.ContainsKey(hashCode))
                                {
                                    // Hash collisions are exceedingly rare, but possible
                                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Assembly \"{0}\" contains multiple scripts with hash code \"{1}\".", assembly, hashCode));
                                }
                                resourceNameHashToResourceName[hashCode] = resourceName;
                            }
                        }
                        // Map the script hash to a script name
                        string scriptName;
                        if (!resourceNameHashToResourceName.TryGetValue(script, out scriptName))
                        {
                            //throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Assembly \"{0}\" does not contain a script with hash code \"{1}\".", assembly, script));
                            if (Log.IsErrorEnabled)
                            { 
                                Log.ErrorFormat("Unable to find script:{0} in assembly:{1}", script, assembly); 
                            }
                        }

                        if (string.IsNullOrEmpty(scriptName))
                        {
                            continue;
                        }

                        // Create a ScriptEntry to represent the script
                        if (Log.IsErrorEnabled)
                        {
                            Log.DebugFormat("Found script hash:{0} name:{1} in assembly:{2}", script, scriptName, assembly);
                        }
                        var scriptEntry = new ScriptEntry(assembly, scriptName, culture) { Loaded = loaded };
                        scriptEntries.Add(scriptEntry);
                    }
                }
            }
            return scriptEntries;
        }

        /// <summary>
        /// Writes scripts (including localized script resources) to the specified stream
        /// </summary>
        /// <param name="scriptEntries">list of scripts to write</param>
        /// <param name="outputWriter">writer for output stream</param>
        private static void WriteScripts(IEnumerable<ScriptEntry> scriptEntries, TextWriter outputWriter)
        {
            foreach (var scriptEntry in scriptEntries)
            {
                if (!scriptEntry.Loaded)
                {
                    if (!IsScriptCombinable(scriptEntry))
                    {
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Combined script request includes un-combinable script \"{0}\".", scriptEntry.Name));
                    }

                    // This script hasn't been loaded by the browser, so add it to the combined script file
                    outputWriter.Write("//START ");
                    outputWriter.WriteLine(scriptEntry.Name);
                    var script = scriptEntry.GetScript();
                    if (WebResourceRegex.IsMatch(script))
                    {
                        // This script uses script substitution which isn't supported yet, so throw an exception since it's too late to fix
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "ToolkitScriptManager does not support <%= WebResource/ScriptResource(...) %> substitution as used by script file \"{0}\".", scriptEntry.Name));
                    }
                                       
                    outputWriter.WriteLine(script);

                    // Save current culture and set the specified culture
                    var currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                    try
                    {
                        try
                        {
                            Thread.CurrentThread.CurrentUICulture = new CultureInfo(scriptEntry.Culture);
                        }
                        catch (ArgumentException)
                        {
                            // Invalid culture; proceed with default culture (just as for unsupported cultures)
                        }

                        // Write out the associated script resources (if any) in the proper culture
                        var scriptAssembly = scriptEntry.LoadAssembly();
                        foreach (ScriptResourceAttribute scriptResourceAttribute in scriptAssembly.GetCustomAttributes(typeof (ScriptResourceAttribute), false))
                        {
                            if (scriptResourceAttribute.ScriptName != scriptEntry.Name)
                                continue;
                            // Found a matching script resource; write it out
                            outputWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={{", scriptResourceAttribute.TypeName));

                            // Get the script resource name (without the trailing ".resources")
                            var scriptResourceName = scriptResourceAttribute.ScriptResourceName;
                            if (scriptResourceName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                            {
                                scriptResourceName = scriptResourceName.Substring(0, scriptResourceName.Length - 10);
                            }

                            // Load a ResourceManager/ResourceSet and walk through the list to output them all
                            var resourceManager = new System.Resources.ResourceManager(scriptResourceName, scriptAssembly);
                            using (var resourceSet = resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true))
                            {
                                var first = true;
                                foreach (System.Collections.DictionaryEntry de in resourceSet)
                                {
                                    if (!first)
                                    {
                                        // Need a comma between all entries
                                        outputWriter.Write(",");
                                    }
                                    // Output the entry
                                    var name = (string) de.Key;
                                    var value = resourceManager.GetString(name);
                                    outputWriter.Write(string.Format(CultureInfo.InvariantCulture, "\"{0}\":\"{1}\"", QuoteString(name), QuoteString(value)));
                                    first = false;
                                }
                            }
                            outputWriter.WriteLine("};");
                        }
                    }
                    finally
                    {
                        // Restore culture
                        Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    }

                    // Done with this script
                    outputWriter.Write("//END ");
                    outputWriter.WriteLine(scriptEntry.Name);
                }

                // This script is now (or will be soon) loaded by the browser
                scriptEntry.Loaded = true;
            }
        }

        private class ScriptEntry
        {
            /// <summary>
            /// Containing Assembly
            /// </summary>
            public readonly string Assembly;

            /// <summary>
            /// Script name
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// Culture to render the script in
            /// </summary>
            public readonly string Culture;

            /// <summary>
            /// Loaded state of the script in the client browser
            /// </summary>
            public bool Loaded;

            /// <summary>
            /// Reference to the Assembly object (if loaded by LoadAssembly)
            /// </summary>
            private Assembly _loadedAssembly;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="assembly">containing assembly</param>
            /// <param name="name">script name</param>
            /// <param name="culture">culture for rendering the script</param>
            public ScriptEntry(string assembly, string name, string culture)
            {
                Assembly = assembly;
                Name = name;
                Culture = culture;
            }

            /// <summary>
            /// Gets the script corresponding to the object
            /// </summary>
            /// <returns>script text</returns>
            public string GetScript()
            {
                string script = null;
                using (var stream = LoadAssembly().GetManifestResourceStream(Name))
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            script = reader.ReadToEnd();
                        }
                }
                return script;
            }

            /// <summary>
            /// Loads the associated Assembly
            /// </summary>
            /// <returns>Assembly reference</returns>
            public Assembly LoadAssembly()
            {
                return _loadedAssembly ?? (_loadedAssembly = System.Reflection.Assembly.Load(Assembly));
            }

            /// <summary>
            /// Equals override to compare two ScriptEntry objects
            /// </summary>
            /// <param name="obj">comparison object</param>
            /// <returns>true iff both ScriptEntries represent the same script</returns>
            public override bool Equals(object obj)
            {
                var other = (ScriptEntry) obj;
                return (other.Assembly == Assembly) && (other.Name == Name);
            }

            /// <summary>
            /// GetHashCode override corresponding to the Equals override above
            /// </summary>
            /// <returns>hash code for the object</returns>
            public override int GetHashCode()
            {
                return Assembly.GetHashCode() ^ Name.GetHashCode();
            }
        }

        /// <summary>
        /// Callable implementation of System.Web.Script.Serialization.JavaScriptString.QuoteString
        /// </summary>
        /// <param name="value">value to quote</param>
        /// <returns>quoted string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Callable implementation of System.Web.Script.Serialization.JavaScriptString.QuoteString")]
        protected static string QuoteString(string value)
        {
            StringBuilder builder = null;
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var startIndex = 0;
            var count = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                if ((((c == '\r') || (c == '\t')) || ((c == '"') || (c == '\''))) || ((((c == '<') || (c == '>')) || ((c == '\\') || (c == '\n'))) || (((c == '\b') || (c == '\f')) || (c < ' '))))
                {
                    if (builder == null)
                    {
                        builder = new StringBuilder(value.Length + 5);
                    }
                    if (count > 0)
                    {
                        builder.Append(value, startIndex, count);
                    }
                    startIndex = i + 1;
                    count = 0;
                }
                if (builder != null)
                {
                    switch (c)
                    {
                        case '<':
                        case '>':
                        case '\'':
                            {
                                AppendCharAsUnicode(builder, c);
                                continue;
                            }
                        case '\\':
                            {
                                builder.Append(@"\\");
                                continue;
                            }
                        case '\b':
                            {
                                builder.Append(@"\b");
                                continue;
                            }
                        case '\t':
                            {
                                builder.Append(@"\t");
                                continue;
                            }
                        case '\n':
                            {
                                builder.Append(@"\n");
                                continue;
                            }
                        case '\f':
                            {
                                builder.Append(@"\f");
                                continue;
                            }
                        case '\r':
                            {
                                builder.Append(@"\r");
                                continue;
                            }
                        case '"':
                            {
                                builder.Append("\\\"");
                                continue;
                            }
                    }
                }
                if (c < ' ')
                {
                    AppendCharAsUnicode(builder, c);
                }
                else
                {
                    count++;
                }
            }
            if (builder == null)
            {
                return value;
            }
            if (count > 0)
            {
                builder.Append(value, startIndex, count);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Callable implementation of System.Web.Script.Serialization.JavaScriptString.AppendCharAsUnicode
        /// </summary>
        /// <param name="builder">string builder</param>
        /// <param name="c">character to append</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Callable implementation of System.Web.Script.Serialization.JavaScriptString.AppendCharAsUnicode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c", Justification = "Callable implementation of System.Web.Script.Serialization.JavaScriptString.AppendCharAsUnicode")]
        protected static void AppendCharAsUnicode(StringBuilder builder, char c)
        {
            builder.Append(@"\u");
            builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x4}", new object[] {(int) c});
        }
    }
}