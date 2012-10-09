using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
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
        public const string ClientResourceIDKey = "id";
        public const string LanguageKey = "lang";
        public const string CachingTokenKey = "t";
        public const string ClientTokenFilename = "~/ClientToken.txt";
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        internal static readonly Func<ContentCacheItem, bool> IsCacheItemValid =
            (cacheItem) => cacheItem != null && cacheItem.IsValid;

        protected DateTime AssemblyTimestamp
        {
            get
            {
                if (_assemblyTimestampTicks == null)
                {
                    var writeTime = File.GetLastWriteTime(Assembly.GetCallingAssembly().Location);
                    _assemblyTimestampTicks = writeTime.GetDay().Ticks;
                }

                return new DateTime(_assemblyTimestampTicks.Value);
            }
        }
        private static long? _assemblyTimestampTicks;

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected ILog Log { get; set; }

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected IContentCache ContentCache { get; set; }

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected IClientResourceManager ClientResourceManager { get; set; }

        [Inject("Cannot use constructor injection on HttpHandlers")]
        protected IEnumerable<IClientResourceProcessor> ClientResourceProcessors { get; set; }

        protected static string CachingToken
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cachingToken))
                    _cachingToken = Settings.Default.ClientResourceCachingToken;
                
                // if no setting value was specified, try to grab it from a file
                if (string.IsNullOrWhiteSpace(_cachingToken))
                {
                    string tokenFile = HttpContext.Current.Server.MapPath(ClientTokenFilename);
                    if (File.Exists(tokenFile))
                        _cachingToken = File.ReadAllText(tokenFile).Trim();
                }

                // If neither the settings value was set nor a file existed, throw an error
                if (string.IsNullOrWhiteSpace(_cachingToken))
                    throw new DowJonesUtilitiesException("Please specify a value for the ClientResourceCachingToken setting (most likely in the DowJones.Properties settings section)");

                return _cachingToken;
            }
        }
        private static string _cachingToken;


        public static string GenerateUrl(string resourceId, InterfaceLanguage language, HttpRequestBase request = null)
        {
            var culture = CultureManager.GetCultureInfoFromInterfaceLanguage(language);
            return GenerateUrl(resourceId, culture, request);
        }

        public static string GenerateUrl(string resourceId, CultureInfo culture)
        {
            return GenerateUrl(resourceId, culture, null);
        }

        public static string GenerateUrl(string resourceId, CultureInfo culture, HttpRequestBase request)
        {
            Guard.IsNotNullOrEmpty(resourceId, "resourceId");

            if(!resourceId.Contains(";"))
            {
                if (resourceId.StartsWith("http", true, culture))
                    return resourceId;
                else if (resourceId.StartsWith("~/"))
                    return VirtualPathUtility.ToAbsolute(resourceId);
            }

            var relativeUrl = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}",
                                    Settings.Default.ClientResourceHandlerPath,
                                    LanguageKey, MapLanguageKey(culture.TwoLetterISOLanguageName),
                                    ClientResourceIDKey, HttpUtility.UrlEncode(resourceId).Replace("%3b", ";"),
                                    CachingTokenKey, CachingToken
                                );

            request = request ?? new HttpContextWrapper(HttpContext.Current).Request;

            string rootUrl = 
                string.Format(@"{0}://{1}",
                              request.Url.Scheme,
                              request.Url.Authority
                             );

            return rootUrl + VirtualPathUtility.ToAbsolute(relativeUrl);
        }

        public static string MapLanguageKey(CultureInfo culture)
        {
            switch (culture.ThreeLetterWindowsLanguageName)
            {
                case "CHT":
                    return "zhtw";
                case "CHS":
                    return "zhcn";
                default:
                    return culture.TwoLetterISOLanguageName;
            }
        }

        protected override void OnProcessRequest(HttpContextBase context)
        {
            string language = context.Request[LanguageKey];
            string resourceId = context.Request[ClientResourceIDKey];

            if(context.Request[CachingTokenKey] == "clear")
            {
                foreach (DictionaryEntry entry in context.Cache)
                    context.Cache.Remove((string)entry.Key);
            }

            if (string.IsNullOrWhiteSpace(resourceId))
                throw new HttpException(400, "Invalid Client Resource");

            if (ResourceHasNotBeenModified(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                return;
            }


            CultureInfo culture = CultureManager.GetCultureInfoFromInterfaceLanguage(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            IEnumerable<ContentCacheItem> clientResources = 
                GetClientResources(resourceId, culture);

            if (!clientResources.Any())
                throw new HttpException(404, "Client Resource Not Found");

            RenderClientResources(context, clientResources, DefaultEncoding);
        }

        private bool ResourceHasNotBeenModified(HttpContextBase context)
        {
            bool isModified = false;
            var ifModifiedSinceHeader = context.Request.Headers["If-Modified-Since"];
            DateTime ifModifiedSince;
            if (
                ifModifiedSinceHeader.HasValue() &&
                DateTime.TryParse(ifModifiedSinceHeader, out ifModifiedSince))
            {
                isModified = (ifModifiedSince.GetDay() <= AssemblyTimestamp);
            }
            return isModified;
        }

        private IEnumerable<ContentCacheItem> GetClientResources(string resourceId, CultureInfo culture)
        {
            IEnumerable<string> resourceNames = ClientResourceManager.ParseClientResourceNames(resourceId);

            IEnumerable<string> cachedResourceNames = 
                GetCachedItems(resourceNames, culture)
                    .Select(y => y.Key.Id)
                    .ToArray();

            var uncachedResourceNames = resourceNames.Except(cachedResourceNames);

            if (uncachedResourceNames.Any())
                CacheClientResources(uncachedResourceNames, culture);

            return GetCachedItems(resourceNames, culture);
        }

        private void CacheClientResources(IEnumerable<string> resourceNames, CultureInfo culture)
        {
            var resources = ClientResourceManager.GetClientResources(resourceNames);
            
            var existingResourceNames = resources.Select(x => x.Name ?? x.Url);
            
            if(Log.IsDebugEnabled)
            {
                Log.Debug("Client Resources requested but not in cache: " + string.Join(", ", resourceNames));
                Log.Debug("Client Resources retrieved: " + string.Join(", ", existingResourceNames));
            }

            var resourcesRequestedButDidntExist = resourceNames.Except(existingResourceNames);
            if (resourcesRequestedButDidntExist.Any())
            {
                Log.Warn("Client Resources NOT retrieved: " +
                         string.Join(", ", resourcesRequestedButDidntExist));
            }

            foreach(var resource in resources)
            {
                var processedResource = new ProcessedClientResource(resource);

                var processors = ClientResourceProcessors
                                    .OrderBy(x => x.Order)
                                    .OrderBy(x => x.ProcessorKind);

                foreach (var processor in processors)
                    processor.Process(processedResource);

                var cacheKey = new ContentCacheKey(resource.Name, culture);
                ContentCache.Add(cacheKey, processedResource.MimeType, processedResource.Content);
            }
        }

        private IEnumerable<ContentCacheItem> GetCachedItems(IEnumerable<string> resourceNames, CultureInfo culture)
        {
            IEnumerable<ContentCacheItem> cachedItems =
                from id in resourceNames
                let cacheKey = new ContentCacheKey(id, culture)
                let cacheItem = ContentCache.Get(cacheKey)
                where IsCacheItemValid(cacheItem)
                select cacheItem;
            
            return cachedItems.ToArray();
        }

        private void RenderClientResources(HttpContextBase context, IEnumerable<ContentCacheItem> cachedItems, Encoding contentEncoding)
        {
            Guard.IsNotNull(context, "context");
            Guard.IsNotNull(cachedItems, "cachedItems");

            ContentCacheItem cachedItem = cachedItems.First();

            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ContentType = cachedItem.ContentType;
            context.Response.ContentEncoding = contentEncoding;

            DateTime expirationDate = DateTime.Now.AddYears(1);
            context.Response.Cache.SetExpires(expirationDate);
            context.Response.Cache.SetLastModified(AssemblyTimestamp);
            context.Response.Cache.SetMaxAge(new TimeSpan(expirationDate.ToFileTimeUtc()));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            foreach (var cacheItem in cachedItems)
                context.Response.WriteStream(cacheItem.GetContentStream(contentEncoding));
        }
    }

}
