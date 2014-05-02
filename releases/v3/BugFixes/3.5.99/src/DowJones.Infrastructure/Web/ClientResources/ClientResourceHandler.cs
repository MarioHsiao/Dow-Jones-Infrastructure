using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Properties;
using DowJones.Utilities;
using log4net;

namespace DowJones.Web
{
	public class ClientResourceHandler : HttpHandlerBase
	{
		public const string CachingTokenKey = "t";
		public const string ClientResourceIdKey = "id";
		public const string ClientTokenFilename = "~/ClientToken.txt";
		public static readonly Encoding DefaultEncoding = Encoding.UTF8;
		public const string LanguageKey = "lang";
		public const string RequireModuleKey = "require";

		public static Func<ContentCacheItem, bool> IsCacheItemValid =
			cacheItem => cacheItem != null && cacheItem.IsValid;

		public static Func<HttpContextBase, bool> IsClientCachingEnabled =
			context => !context.DebugEnabled();

		public static Func<string> CacheTokenFactory = () => CachingToken.Value;

		private static readonly Lazy<string> CachingToken = new Lazy<string>(GetCachingToken);

		/// <summary>
		/// Function used to determine the LastModifiedTimestamp of a request
		/// </summary>
		public static Func<HttpContextBase, DateTime> LastModifiedCalculator
		{
			get
			{
				if (lastModifiedCalculator == null)
				{
					var timestamp = typeof(ClientResourceHandler).Assembly.GetAssemblyTimestamp();
					return context => timestamp;
				}

				return lastModifiedCalculator;
			}
			set { lastModifiedCalculator = value; }
		}
		private volatile static Func<HttpContextBase, DateTime> lastModifiedCalculator;

		[Inject("Cannot use constructor injection on HttpHandlers")]
		protected internal ILog Log { get; set; }

		[Inject("Cannot use constructor injection on HttpHandlers")]
		protected internal IContentCache ContentCache { get; set; }

		[Inject("Cannot use constructor injection on HttpHandlers")]
		protected internal IClientResourceManager ClientResourceManager { get; set; }

		[Inject("Cannot use constructor injection on HttpHandlers")]
		protected internal IEnumerable<IClientResourceProcessor> ClientResourceProcessors { get; set; }


		public static string GenerateUrl(string resourceId, InterfaceLanguage language, HttpContextBase context = null, bool? debug = null)
		{
			var culture = CultureManager.GetCultureInfoFromInterfaceLanguage(language);
			return GenerateUrl(resourceId, culture, context, debug);
		}

		// Target for Func<string, string> delegate
		public static string GenerateUrl(string resourceId)
		{
			return GenerateUrl(resourceId, null, null, null);
		}

		// Target for Func<string, CultureInfo, string> delegate
		public static string GenerateUrl(string resourceId, CultureInfo culture)
		{
			return GenerateUrl(resourceId, culture, null, null);
		}

		public static string GenerateUrl(string resourceId, CultureInfo culture, HttpContextBase context, bool? debug)
		{
			Guard.IsNotNullOrEmpty(resourceId, "resourceId");

			culture = culture ?? Thread.CurrentThread.CurrentCulture;

			if (!resourceId.Contains(";"))
			{
				if (resourceId.StartsWith("http", true, culture))
				{
				    return resourceId;
				}

				if (resourceId.StartsWith("~/"))
				{
				    return string.Format("{0}?{1}={2}",
				                         VirtualPathUtility.ToAbsolute(resourceId),
				                         CachingTokenKey,
				                         CacheTokenFactory()
				        );
				}
			}

		    var tResourceId = HttpUtility.UrlEncode(resourceId);
			var relativeUrl = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}",
			                                Settings.Default.ClientResourceHandlerPath,
			                                LanguageKey, MapLanguageKey(culture),
			                                ClientResourceIdKey, tResourceId != null ? tResourceId.Replace("%3b", ";") : string.Empty,
			                                CachingTokenKey, CacheTokenFactory()
			    );

			if (debug == true || context.DebugEnabled())
				relativeUrl += "&debug=true";

			context = context ?? new HttpContextWrapper(HttpContext.Current);

			return context.GetExternalUrl(relativeUrl);
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

		public static string GenerateRequireJsBaseUrl(CultureInfo culture = null, HttpContextBase context = null, bool? debug = null)
		{
			culture = culture ?? Thread.CurrentThread.CurrentCulture;
			context = context ?? new HttpContextWrapper(HttpContext.Current);

			var baseUrl = new StringBuilder();
			baseUrl.Append(context.GetExternalUrl(Settings.Default.ClientResourceHandlerPath));
			baseUrl.Append("?");
			baseUrl.AppendFormat("{0}={1}", LanguageKey, MapLanguageKey(culture));
			baseUrl.AppendFormat("&{0}={1}", CachingTokenKey, CacheTokenFactory());

			if (debug == true || context.DebugEnabled())
				baseUrl.Append("&debug=true");

			baseUrl.AppendFormat("&{0}={1}", RequireModuleKey, string.Empty);

			return baseUrl.ToString();
		}


		public override void OnProcessRequest(HttpContextBase context)
		{
			var culture = SetRequestLanguage(context.Request[LanguageKey]);

			var resourceId = context.Request[ClientResourceIdKey];

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
			resourceNames = resourceNames as List<string> ?? resourceNames.ToList();
			var clientResources = GetClientResources(context, resourceNames, culture);

			var contentResourcesCacheItems = clientResources as List<ContentCacheItem> ?? clientResources.ToList();
			if (!contentResourcesCacheItems.Any())
				throw new HttpException(404, "Client Resource Not Found");

			// The resources aren't necessarily retrieved in the
			// correct order, so reorder them before they're rendered
			var orderedClientResources =
				from name in resourceNames
				from resource in contentResourcesCacheItems
				where resource.Key.Id == name
				select resource;

			RenderClientResources(context, orderedClientResources, DefaultEncoding);
		}

		private static void RenderClientResources(HttpContextBase context, IEnumerable<ContentCacheItem> cachedItems, Encoding contentEncoding)
		{
			Guard.IsNotNull(context, "context");
			Guard.IsNotNull(cachedItems, "cachedItems");

			cachedItems = cachedItems as List<ContentCacheItem> ?? cachedItems.ToList();
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

		private static bool ResourceHasNotBeenModified(HttpContextBase context)
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

		private static ProcessedClientResource ProcessClientResource(IEnumerable<IClientResourceProcessor> clientResourceProcessors,
			HttpContextBase context, ClientResource resource)
		{
			var processedResource = new ProcessedClientResource(resource);

			var processors = clientResourceProcessors
				.OrderBy(x => x.Order)
				.OrderBy(x => x.ProcessorKind).ToArray();

			foreach (var processor in processors)
			{
				processor.HttpContext = context;
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

		private IEnumerable<ContentCacheItem> GetCachedClientResources(HttpContextBase context, IEnumerable<string> resourceNames, CultureInfo culture)
		{
			context = context ?? new HttpContextWrapper(HttpContext.Current);

			// Don't cache anything in debug mode
			if (context.DebugEnabled())
			{
				Log.Info("Client resource caching disabled - skipping retrieval from cache.  (DowJones.Properties.Settings.BrowserCachingEnabled == false)");
				return Enumerable.Empty<ContentCacheItem>();
			}

			var cachedItems =
				from id in resourceNames
				let cacheKey = new ContentCacheKey(id, culture)
				let cacheItem = ContentCache.Get(cacheKey)
				where IsCacheItemValid(cacheItem)
				select cacheItem;

			return cachedItems.ToArray();
		}

		private IEnumerable<ContentCacheItem> GetClientResources(HttpContextBase context, IEnumerable<string> resourceNames, CultureInfo culture)
		{
			resourceNames = resourceNames as List<string> ?? resourceNames.ToList();
			if (resourceNames.IsEmpty())
				return Enumerable.Empty<ContentCacheItem>();

			var cachedResources = GetCachedClientResources(context, resourceNames, culture).ToArray();

			var cachedResourceNames = cachedResources.Select(y => y.Key.Id).ToArray();

			var uncachedResourceNames = resourceNames.Except(cachedResourceNames).ToArray();

			if (Log.IsDebugEnabled)
			{
				Log.Debug("Client Resources requested but not in cache: " + string.Join(", ", uncachedResourceNames));
				Log.Debug("Client Resources retrieved from cache: " + string.Join(", ", cachedResourceNames));
			}

			var uncachedResources = LoadClientResources(context, uncachedResourceNames);

			var newlyCachedResources = uncachedResources.Select(resource => CacheClientResource(resource, culture));

			return cachedResources.Union(newlyCachedResources);
		}

		private IEnumerable<ProcessedClientResource> LoadClientResources(HttpContextBase context, IEnumerable<string> resourceNames)
		{
			resourceNames = resourceNames as List<string> ?? resourceNames.ToList();
			if (resourceNames.IsEmpty())
				return Enumerable.Empty<ProcessedClientResource>();

			var resources = ClientResourceManager.GetClientResources(resourceNames);

			var clientResources = resources as List<ClientResource> ?? resources.ToList();
			var existingResourceNames = clientResources.Select(x => x.Name ?? x.Url);

			var queue = new ConcurrentQueue<ProcessedClientResource>();

			var resourcesRequestedButDidntExist = resourceNames.Except(existingResourceNames);
			var requestedButDidntExist = resourcesRequestedButDidntExist as List<string> ?? resourcesRequestedButDidntExist.ToList();
			var factory = TaskFactoryManager.Instance.GetLimitedConcurrencyLevelTaskFactory();
			if (requestedButDidntExist.Any())
			{
				Log.Warn("Client Resources NOT retrieved: " +
						 string.Join(", ", requestedButDidntExist));
			}

			var processors = ClientResourceProcessors.ToArray();
			var lang = context.Request[LanguageKey];

			try
			{
				Task.WaitAll(
					clientResources.Select(
					clientResource => factory.StartNew(
						() =>
						{
							SetRequestLanguage(lang);
							queue.Enqueue(ProcessClientResource(processors, context, clientResource));
						})).ToArray());

				return queue.ToList();
			}
			catch (AggregateException aggregateException)
			{
				Log.Debug(aggregateException.Message);
				//throw new HttpException(500, "AggregateException");
				throw;
			}
			catch (Exception ex)
			{
				Log.Debug(ex.Message);
				throw new HttpException(500, "General Error");
			}
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
				cachingToken = LastModifiedCalculator(context.Value).Ticks.ToString(CultureInfo.InvariantCulture).Trim('0');
			}

			return cachingToken;
		}
	}

}
