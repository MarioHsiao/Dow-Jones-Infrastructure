using System;
using System.Web;
using DowJones.Factiva.Currents.Website.Contracts;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class HttpContextCacheProvider<T> : ICacheProvider<T>
	{
		#region ICacheProvider Members

		public T GetItemFromCache(string key,
		                          int minutes,
		                          Func<T> builder)
		{

			// build the cache key.
			var cacheKey = string.Format(string.Concat(typeof(T), "{0}"), key);

			// get current cache.
			var context = HttpContext.Current;

			// build cache entry.
			if (context.Cache[cacheKey] == null)
				context.Cache.Insert(cacheKey,
				                     builder.Invoke(),
				                     null,
				                     DateTime.Now.AddMinutes(minutes),
				                     TimeSpan.Zero);

			// return value.
			return (T)context.Cache[cacheKey];
		}

		#endregion
	}
}