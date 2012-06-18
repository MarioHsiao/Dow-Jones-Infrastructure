using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Properties;
using log4net;

namespace DowJones.Web
{
    public class ClientResourceHandler : HttpHandlerBase
    {
        public const string CachingTokenKey = "t";
        public const string ClientResourceIDKey = "id";
        public const string ClientTokenFilename = "~/ClientToken.txt";
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        public const string LanguageKey = "lang";
        public const string RequireModuleKey = "require";

        public static Func<ContentCacheItem, bool> IsCacheItemValid =
            cacheItem => cacheItem != null && cacheItem.IsValid;

        public static Func<HttpContextBase, bool> IsClientCachingEnabled =
            context => !context.DebugEnabled();

        public static Func<string> CacheTokenFactory = () => _cachingToken.Value;

        private static readonly Lazy<string> _cachingToken = new Lazy<string>(GetCachingToken);
        
        /// <summary>
        /// Function used to determine the LastModifiedTimestamp of a request
        /// </summary>
        public static Func<HttpContextBase, DateTime> LastModifiedCalculator
        {
            get
            {
                if (_lastModifiedCalculator == null)
                {
                    var timestamp = typeof (ClientResourceHandler).Assembly.GetAssemblyTimestamp();
                    return (context) => timestamp;
                }

                return _lastModifiedCalculator;
            }
            set { _lastModifiedCalculator = value; }
        }
        private volatile static Func<HttpContextBase, DateTime> _lastModifiedCalculator;

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected ILog Log { get; set; }

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected IContentCache ContentCache { get; set; }

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected IClientResourceManager ClientResourceManager { get; set; }

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected IEnumerable<IClientResourceProcessor> ClientResourceProcessors { get; set; }


        public static string GenerateUrl(string resourceId, InterfaceLanguage language, HttpContextBase context = null, bool? debug = null)
        {
            var culture = CultureManager.GetCultureInfoFromInterfaceLanguage(language);
            return GenerateUrl(resourceId, culture, context, debug);
        }

        // Target for Func<string, CultureInfo, string> delegate
        public static string GenerateUrl(string resourceId, CultureInfo culture)
        {
            return GenerateUrl(resourceId, culture, null, null);
        }

        public static string GenerateUrl(string resourceId, CultureInfo culture, HttpContextBase context, bool? debug)
        {
            Guard.IsNotNullOrEmpty(resourceId, "resourceId");

            if (!resourceId.Contains(";"))
            {
                if (resourceId.StartsWith("http", true, culture))
                    return resourceId;
                if (resourceId.StartsWith("~/"))
                    return VirtualPathUtility.ToAbsolute(resourceId);
            }

            var relativeUrl = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}",
                                    Settings.Default.ClientResourceHandlerPath,
                                    LanguageKey, culture.TwoLetterISOLanguageName,
                                    ClientResourceIDKey, HttpUtility.UrlEncode(resourceId).Replace("%3b", ";"),
                                    CachingTokenKey, CacheTokenFactory()
                                );

            if (debug == true || context.DebugEnabled())
                relativeUrl += "&debug=true";

            context = context ?? new HttpContextWrapper(HttpContext.Current);

            return context.GetExternalUrl(relativeUrl);
        }

        public static string GenerateRequireJsBaseUrl(CultureInfo culture = null, HttpContextBase context = null, bool? debug = null)
        {
            culture = culture ?? Thread.CurrentThread.CurrentCulture;
            context = context ?? new HttpContextWrapper(HttpContext.Current);

            var baseUrl = new StringBuilder();
            baseUrl.Append(context.GetExternalUrl(Settings.Default.ClientResourceHandlerPath));
            baseUrl.Append("?");
            baseUrl.AppendFormat("{0}={1}", LanguageKey, culture.TwoLetterISOLanguageName);
            baseUrl.AppendFormat("&{0}={1}", CachingTokenKey, CacheTokenFactory());

            if (debug == true || context.DebugEnabled())
                baseUrl.Append("&debug=true");

            baseUrl.AppendFormat("&{0}={1}", RequireModuleKey, string.Empty);

            return baseUrl.ToString();
        }


        protected override void OnProcessRequest(HttpContextBase context)
        {
            var culture = SetRequestLanguage(context.Request[LanguageKey]);

            var resourceId = context.Request[ClientResourceIDKey];

            if (string.IsNullOrWhiteSpace(resourceId))
            {
                // Check for a require.js request in the form of: /[ResourceId].js
                var requireResourceId = context.Request[RequireModuleKey];

                if (string.IsNullOrWhiteSpace(requireResourceId))
                {
                    throw new HttpException(400, "Invalid Client Resource");
                }

                // Chop off the beginning / and trailing .js
                resourceId = requireResourceId.Substring(1, requireResourceId.Length - 4);
            }

            if (ResourceHasNotBeenModified(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                return;
            }

            context.Response.Clear();
            context.Response.Buffer = true;

            RenderClientResources(context, resourceId, culture);
        }

        public void RenderClientResources(HttpContextBase context, string resourceId, CultureInfo culture = null)
        {
            var resourceNames = ClientResourceManager.ParseClientResourceNames(resourceId);
            RenderClientResources(context, resourceNames, culture);
        }

        public void RenderClientResources(HttpContextBase context, IEnumerable<string> resourceNames, CultureInfo culture = null)
        {
            var clientResources = GetClientResources(resourceNames, culture);

            if (!clientResources.Any())
                throw new HttpException(404, "Client Resource Not Found");

            // The resources aren't necessarily retrieved in the
            // correct order, so reorder them before they're rendered
            var orderedClientResources =
                from name in resourceNames
                from resource in clientResources
                where resource.Key.Id == name
                select resource;

            RenderClientResources(context, orderedClientResources, DefaultEncoding);
        }

        private void RenderClientResources(HttpContextBase context, IEnumerable<ContentCacheItem> cachedItems, Encoding contentEncoding)
        {
            Guard.IsNotNull(context, "context");
            Guard.IsNotNull(cachedItems, "cachedItems");

            var cachedItem = cachedItems.First();

            context.Response.ContentType = cachedItem.ContentType;
            context.Response.ContentEncoding = contentEncoding;

            if (IsClientCachingEnabled(context))
            {
                var expirationDate = DateTime.Now.AddYears(1);
                context.Response.Cache.SetExpires(expirationDate);
                context.Response.Cache.SetLastModified(LastModifiedCalculator(context));
                context.Response.Cache.SetMaxAge(new TimeSpan(expirationDate.ToFileTimeUtc()));
                context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate); // dacostad changed from public to not allow proxy servers to cache.
            }

            foreach (var cacheItem in cachedItems)
            {
                context.Response.WriteStream(cacheItem.GetContentStream(contentEncoding));
                context.Response.Write("\r\n");
            }
        }

        private bool ResourceHasNotBeenModified(HttpContextBase context)
        {
            // If client caching is disabled everything is always modified!
            if (!IsClientCachingEnabled(context))
                return false;

            var isModified = false;
            var ifModifiedSinceHeader = context.Request.Headers["If-Modified-Since"];
            DateTime ifModifiedSince;
            if (
                ifModifiedSinceHeader.HasValue() &&
                DateTime.TryParse(ifModifiedSinceHeader, out ifModifiedSince))
            {
                isModified = (ifModifiedSince.GetDay() <= LastModifiedCalculator(context));
            }
            return isModified;
        }

        private ProcessedClientResource ProcessClientResource(ClientResource resource)
        {
            var processedResource = new ProcessedClientResource(resource);

            var processors = ClientResourceProcessors
                .OrderBy(x => x.Order)
                .OrderBy(x => x.ProcessorKind);

            foreach (var processor in processors)
            {
                var processedDependentResources = new List<ProcessedClientResource>();
                foreach (var processedDependentResource in processedResource.ClientTemplates)
                {
                    processor.Process(processedDependentResource);
                    processedDependentResources.Add(processedDependentResource);
                }

                processedResource.ClientTemplates = processedDependentResources;

                processor.Process(processedResource);
            }
            return processedResource;
        }

        private IEnumerable<ContentCacheItem> GetCachedClientResources(IEnumerable<string> resourceNames, CultureInfo culture, HttpContextWrapper context = null)
        {
            context = context ?? new HttpContextWrapper(HttpContext.Current);

            // Don't cache anything in debug mode
            if (context.DebugEnabled())
            {
                Log.Info("Client resource caching disabled - skipping retrieval from cache.  (DowJones.Properties.Settings.BrowserCachingEnabled == false)");
                return Enumerable.Empty<ContentCacheItem>();
            }

            IEnumerable<ContentCacheItem> cachedItems =
                from id in resourceNames
                let cacheKey = new ContentCacheKey(id, culture)
                let cacheItem = ContentCache.Get(cacheKey)
                where IsCacheItemValid(cacheItem)
                select cacheItem;

            return cachedItems.ToArray();
        }

        private IEnumerable<ContentCacheItem> GetClientResources(IEnumerable<string> resourceNames, CultureInfo culture)
        {
            if (resourceNames == null || resourceNames.IsEmpty())
                return Enumerable.Empty<ContentCacheItem>();

            var cachedResources = GetCachedClientResources(resourceNames, culture).ToArray();

            var cachedResourceNames = cachedResources.Select(y => y.Key.Id).ToArray();

            var uncachedResourceNames = resourceNames.Except(cachedResourceNames).ToArray();

            if (Log.IsDebugEnabled)
            {
                Log.Debug("Client Resources requested but not in cache: " + string.Join(", ", uncachedResourceNames));
                Log.Debug("Client Resources retrieved from cache: " + string.Join(", ", cachedResourceNames));
            }

            var uncachedResources = LoadClientResources(uncachedResourceNames);

            var newlyCachedResources = uncachedResources.Select(resource => CacheClientResource(resource, culture));

            return cachedResources.Union(newlyCachedResources);
        }

        private IEnumerable<ProcessedClientResource> LoadClientResources(IEnumerable<string> resourceNames)
        {
            if (resourceNames == null || resourceNames.IsEmpty())
                return Enumerable.Empty<ProcessedClientResource>();

            var resources = ClientResourceManager.GetClientResources(resourceNames);

            var existingResourceNames = resources.Select(x => x.Name ?? x.Url);

            var resourcesRequestedButDidntExist = resourceNames.Except(existingResourceNames);
            if (resourcesRequestedButDidntExist.Any())
            {
                Log.Warn("Client Resources NOT retrieved: " +
                         string.Join(", ", resourcesRequestedButDidntExist));
            }

            var processedResources = resources.Select(ProcessClientResource).ToArray();

            return processedResources;
        }

        private ContentCacheItem CacheClientResource(ProcessedClientResource resource, CultureInfo culture)
        {
            var cacheKey = new ContentCacheKey(resource.Name, culture);
            return ContentCache.Add(cacheKey, resource.MimeType, resource.Content);
        }

        protected virtual CultureInfo SetRequestLanguage(string language)
        {
            var culture = CultureManager.GetCultureInfoFromInterfaceLanguage(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            return culture;
        }

        private static string GetCachingToken()
        {
            var cachingToken = Settings.Default.ClientResourceCachingToken;

            var context = new Lazy<HttpContextBase>(HttpContext.Current.ToHttpContextBase);

            // if no setting value was specified, try to grab it from a file
            if (string.IsNullOrWhiteSpace(cachingToken))
            {
                string tokenFile = context.Value.Server.MapPath(ClientTokenFilename);

                if (File.Exists(tokenFile))
                    cachingToken = File.ReadAllText(tokenFile).Trim();
            }

            if (string.IsNullOrWhiteSpace(cachingToken))
            {
                cachingToken = LastModifiedCalculator(context.Value).Ticks.ToString().Trim('0');
            }

            return cachingToken;
        }
    }

}
