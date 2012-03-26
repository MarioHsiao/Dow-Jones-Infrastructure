using System;
using System.IO;

namespace DowJones.Infrastructure
{
    public interface IContentCache
    {
        void Add(ContentCacheKey key, string contentType, string content);
        void Add(ContentCacheKey key, string contentType, Stream contentStream);

        ContentCacheItem Get(ContentCacheKey key);
    }


    public static class IContentCacheExtensions
    {

        public static void Add(this IContentCache cache, ContentCacheKey key, string contentType, string content)
        {
            cache.Add(key, contentType, content);
        }

        public static void Add(this IContentCache cache, ContentCacheKey key, string contentType, Stream contentStream)
        {
            cache.Add(key, contentType, contentStream);
        }

        public static bool TryGet(this IContentCache cache, ContentCacheKey key, out ContentCacheItem contentCacheItem)
        {
            try
            {
                contentCacheItem = cache.Get(key);
                return contentCacheItem != null;
            }
            catch(Exception ex) 
            {
                // TODO:  Better exception handling (i.e. log exception)
                Console.Error.WriteLine("Error getting item '{0}' from cache: {1}", key, ex);

                contentCacheItem = null;
                return false;
            }
        }

    }
}
