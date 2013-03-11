using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Handlers;
using System.Web.UI;
using EMG.widgets.ui.Modules.Compression;
using log4net;

namespace EMG.widgets.ui.Modules
{
    internal class WebResourceRegex : Regex
    {
        // Methods
        public WebResourceRegex()
        {
            pattern = "<%\\s*=\\s*WebResource\\(\"(?<resourceName>[^\"]*)\"\\)\\s*%>";
            roptions = RegexOptions.Singleline | RegexOptions.Multiline;
            capnames = new Hashtable
                           {
                               { "resourceName", 1 }, 
                               { "0", 0 }
                           };
            capslist = new[] { "0", "resourceName" };
            capsize = 2;
            InitializeReferences();
        }
    }

    namespace Compression
    {
        /// <summary>
        /// The web resource compression module.
        /// </summary>
        public sealed class WebResourceCompressionModule : IHttpModule
        {
            /// <summary>
            /// The _web resource cache.
            /// </summary>
            private static readonly IDictionary _webResourceCache = Hashtable.Synchronized(new Hashtable());
            private static bool _handlerExistenceChecked;

            /// <summary>
            /// The current log.
            /// </summary>
            private readonly ILog _log = LogManager.GetLogger(typeof(WebResourceCompressionModule));


            #region IHttpModule Members

            /// <summary>
            /// Release resources used by the module (Nothing realy in our case)
            /// </summary>
            void IHttpModule.Dispose()
            {
                // Nothing to dispose in our case;
            }

            /// <summary>
            /// Initializes a module and prepares it to handle requests.
            /// </summary>
            /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
            void IHttpModule.Init(HttpApplication context)
            {
                context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
            }

            #endregion

            #region // The PreRequestHandlerExecute event
          
            /// <summary>
            /// The on pre request handler execute.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The e.</param>
            private void OnPreRequestHandlerExecute(object sender, EventArgs e)
            {
                HttpResponse response;
                HttpRequest request;

                var app = sender as HttpApplication;
                if (app != null)
                {
                    response = app.Context.Response;
                    request = app.Context.Request;

                    _log.Debug(request.Url.AbsoluteUri);

                    // Check content type [Short Circuit]
                    var str = request.ContentType.ToLowerInvariant();
                    _log.Debug(response.ContentType.ToLowerInvariant());
                }

                // Check if the current request is Webresource.axd
                if (app != null)
                {
                    if (app.Context.CurrentHandler is AssemblyResourceLoader)
                    {
                        response = app.Context.Response;

                        var queryHash = app.Context.Request.QueryString.ToString().GetHashCode();
                        _log.DebugFormat("queryHash: {0}", queryHash);

                        // Check if the ETag is match. If so, we don't send nothing to the client, and stop here.
                        CheckETag(app, queryHash);

                        try
                        {
                            // Parse the QueryString into parts
                            var urlInfo = GetDataFromQuery(app.Context.Request.QueryString);

                            // Load the assembly
                            var assembly = GetAssembly(urlInfo.First, urlInfo.Second);

                            if (assembly == null)
                            {
                                ThrowHttpException(404, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_ASSEMBLY_NOT_FOUND, urlInfo.Fourth);
                            }

                            // Get the resource info from assembly.
                            var resourceInfo = GetResourceInfo(assembly, urlInfo.Third);

                            if (!resourceInfo.First)
                            {
                                ThrowHttpException(404, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_ASSEMBLY_NOT_FOUND, urlInfo.Fourth);
                            }

                            // If the WebResource needs to perform substitution (WebResource inside WebResource), we leave it to the original AssemblyResourceLoader handler ;-)
                            if (resourceInfo.Second)
                            {
                               
                            }

                            response.Clear();

                            // Set the response cache headers
                            SetCachingHeadersForWebResource(response.Cache, queryHash);

                            // Set the response content type
                            response.ContentType = resourceInfo.Third;

                            var encodingMgr = new EncodingManager(app.Context);

                            // Write content with compression
                            if (resourceInfo.Fourth)
                            {
                                // && (Settings.Instance.CompressWebResource || Settings.Instance.MinifyContent))
                                using (var resourceStream = new StreamReader(assembly.GetManifestResourceStream(urlInfo.Third), true))
                                {
                                    ProcessAndWriteToStream(resourceStream, app.Context, encodingMgr);
                                }
                            }

                                // Write content without compression or minify
                            else
                            {
                                using (var resourceStream = assembly.GetManifestResourceStream(urlInfo.Third))
                                {
                                    WriteToStream(resourceStream, response.OutputStream);
                                }
                            }

                            response.OutputStream.Flush();
                            app.CompleteRequest();
                        }
                        catch (ArgumentNullException)
                        {
                            return;
                        }
                        catch (TargetInvocationException)
                        {
                            return;
                        }
                        catch (CryptographicException)
                        {
                            return;
                        }
                    }}
            }

            #endregion

            #region // Private methods

            /// <summary>
            /// The process and write to stream.
            /// </summary>
            /// <param name="reader">
            /// The reader.
            /// </param>
            /// <param name="context">
            /// The context.
            /// </param>
            /// <param name="encodingMgr">
            /// The encoding mgr.
            /// </param>
            private static void ProcessAndWriteToStream(StreamReader reader, HttpContext context, EncodingManager encodingMgr)
            {
                var content = ServiceResources.GetString(ServiceResources.CREDIT_STRING);
                content += reader.ReadToEnd();

                if (encodingMgr.IsEncodingEnabled)
                {
                    encodingMgr.SetResponseEncodingType();
                    var compressed = encodingMgr.CompressString(content);
                    context.Response.OutputStream.Write(compressed, 0, compressed.Length);
                }
                else
                {
                    context.Response.Write(content);
                }
            }


            /// <summary>
            /// Write a StreamReader to an output stream
            /// </summary>
            /// <param name="stream">
            /// The stream.
            /// </param>
            /// <param name="outputStream">
            /// </param>
            private static void WriteToStream(Stream stream, Stream outputStream)
            {
                Util.StreamCopy(stream, outputStream);
            }


            /// <summary>
            /// Get the info about the resource that in the assembly
            /// </summary>
            /// <param name="assembly">
            /// The assembly.
            /// </param>
            /// <param name="resourceName">
            /// </param>
            /// <returns>
            /// </returns>
            private static Quadruplet<bool, bool, string, bool> GetResourceInfo(Assembly assembly, string resourceName)
            {
                // Create a unique cache key
                var cacheKey = Util.CombineHashCodes(assembly.GetHashCode(), resourceName.GetHashCode());

                var resourceInfo = _webResourceCache[cacheKey] as Quadruplet<bool, bool, string, bool>;

                // Assembly info was not in the cache
                if (resourceInfo == null)
                {
                    var first = false;
                    var second = false;
                    var third = string.Empty;
                    var forth = false;

                    var customAttributes = assembly.GetCustomAttributes(false);
                    for (var j = 0; j < customAttributes.Length; j++)
                    {
                        var attribute = customAttributes[j] as WebResourceAttribute;
                        if ((attribute != null) && string.Equals(attribute.WebResource, resourceName, StringComparison.Ordinal))
                        {
                            first = true;
                            second = attribute.PerformSubstitution;
                            third = attribute.ContentType;
                            forth = Util.IsContentTypeCompressible(attribute.ContentType);
                            break;
                        }
                    }

                    resourceInfo = new Quadruplet<bool, bool, string, bool>(first, second, third, forth);
                    _webResourceCache[cacheKey] = resourceInfo;
                }

                return resourceInfo;
            }


            /// <summary>
            /// Load the specifid assembly curresponding to the given signal.
            /// </summary>
            /// <param name="signal">
            /// </param>
            /// <param name="assemblyName">
            /// </param>
            /// <returns>
            /// </returns>
            private static Assembly GetAssembly(char signal, string assemblyName)
            {
                Assembly assembly = null;
                switch (signal)
                {
                    case 's':
                        assembly = typeof(AssemblyResourceLoader).Assembly;
                        break;
                    case 'p':
                        assembly = Assembly.Load(assemblyName);
                        break;
                    case 'f':
                        {
                            var strArray = assemblyName.Split(new[] { ',' });
                            if (strArray.Length != 4)
                            {
                                ThrowHttpException(400, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_INVALID_REQUEST);
                            }

                            var assemblyRef = new AssemblyName();
                            assemblyRef.Name = strArray[0];
                            assemblyRef.Version = new Version(strArray[1]);
                            var name = strArray[2];
                            if (name.Length > 0)
                            {
                                assemblyRef.CultureInfo = new CultureInfo(name);
                            }
                            else
                            {
                                assemblyRef.CultureInfo = CultureInfo.InvariantCulture;
                            }

                            var tokens = strArray[3];
                            var publicKeyToken = new byte[tokens.Length / 2];
                            for (var i = 0; i < publicKeyToken.Length; i++)
                            {
                                publicKeyToken[i] = byte.Parse(tokens.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            }

                            assemblyRef.SetPublicKeyToken(publicKeyToken);
                            assembly = Assembly.Load(assemblyRef);
                            break;
                        }

                    default:
                        ThrowHttpException(400, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_INVALID_REQUEST);
                        break;
                }

                return assembly;
            }


            /// <summary>
            /// Collect the necessary data from the query string
            /// </summary>
            /// <param name="queryString">
            /// </param>
            /// <returns>
            /// </returns>
            private static Quadruplet<char, string, string, string> GetDataFromQuery(NameValueCollection queryString)
            {
                var queryParam = queryString["d"];
                if (string.IsNullOrEmpty(queryParam))
                {
                    ThrowHttpException(400, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_INVALID_REQUEST);
                }

                var decryptedParam = string.Empty;
                try
                {
                    decryptedParam = Util.DecryptString(queryParam);
                }
                catch (ProviderException)
                {
                    ThrowHttpException(403, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_MACHINE_KEY_MISSING);
                }
                catch (Exception ex)
                {
                    ThrowHttpException(400, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_INVALID_REQUEST, ex);
                }

                var pipeIndex = decryptedParam.IndexOf('|');

                if (pipeIndex < 1 || pipeIndex > (decryptedParam.Length - 2))
                {
                    ThrowHttpException(404, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_ASSEMBLY_NOT_FOUND, decryptedParam);
                }

                if (pipeIndex > (decryptedParam.Length - 2))
                {
                    ThrowHttpException(404, ServiceResources.WEB_RESOURCE_COMPRESSION_MODULE_RESOURCE_NOT_FOUND, decryptedParam);
                }

                var assemblyName = decryptedParam.Substring(1, pipeIndex - 1);
                var resourceName = decryptedParam.Substring(pipeIndex + 1);
                return new Quadruplet<char, string, string, string>(decryptedParam[0], assemblyName, resourceName, decryptedParam);
            }


            /// <summary>
            /// Check if the ETag that sent from the client is match to the current ETag.
            /// If so, set the status code to 'Not Modified' and stop the response.
            /// </summary>
            /// <param name="app">
            /// The app.
            /// </param>
            /// <param name="etagCode">
            /// The etag Code.
            /// </param>
            private static void CheckETag(HttpApplication app, int etagCode)
            {
                var etag = "\"" + etagCode + "\"";

                if (etag.Equals(app.Request.Headers["If-None-Match"], StringComparison.Ordinal))
                {
                    app.Response.Cache.SetETag(etag);
                    app.Response.AppendHeader("Content-Length", "0");
                    app.Response.StatusCode = (int)HttpStatusCode.NotModified;
                    app.Response.SuppressContent = true;
                    app.Response.End();
                }
            }

            /// <summary>
            /// Set the response cache headers for WebResource
            /// </summary>
            /// <param name="cache">
            /// </param>
            /// <param name="etag">
            /// </param>
            private static void SetCachingHeadersForWebResource(HttpCachePolicy cache, int etag)
            {
                cache.VaryByParams["d"] = true;
                cache.VaryByHeaders["Accept-Encoding"] = true;
                cache.SetOmitVaryStar(true);

                // Keep in the client cache for the time configured in the Web.Config
                cache.SetExpires(DateTime.Now.AddDays(365));
                cache.SetMaxAge(TimeSpan.FromDays(365));
                cache.SetValidUntilExpires(true);
                cache.SetLastModified(DateTime.Now);

                cache.SetCacheability(HttpCacheability.Public);

                cache.SetETag(string.Concat("\"", etag, "\""));
            }

            #endregion

            #region // Throw HttpException

            /// <summary>
            /// The throw http exception.
            /// </summary>
            /// <param name="num">
            /// The num.
            /// </param>
            /// <param name="resourceName">
            /// The sr name.
            /// </param>
            /// <exception cref="HttpException">
            /// </exception>
            private static void ThrowHttpException(int num, string resourceName)
            {
                throw new HttpException(num, ServiceResources.GetString(resourceName));
            }

            /// <summary>
            /// The throw http exception.
            /// </summary>
            /// <param name="num">The num.</param>
            /// <param name="resourceName">The sr name.</param>
            /// <param name="param1">The param 1.</param>
            /// <exception cref="HttpException">
            /// </exception>
            private static void ThrowHttpException(int num, string resourceName, string param1)
            {
                throw new HttpException(num, ServiceResources.GetString(resourceName, param1));
            }

            /// <summary>
            /// The throw http exception.
            /// </summary>
            /// <param name="num">The num.</param>
            /// <param name="resourceName">The sr name.</param>
            /// <param name="innerException">The inner exception.</param>
            /// <exception cref="HttpException">
            /// </exception>
            private static void ThrowHttpException(int num, string resourceName, Exception innerException)
            {
                throw new HttpException(num, ServiceResources.GetString(resourceName), innerException);
            }

            #endregion
        }
    }
}
