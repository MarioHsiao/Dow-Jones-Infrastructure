using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class WebContentCache : IContentCache
    {
        private readonly System.Web.Caching.Cache _cache;

        public WebContentCache(System.Web.Caching.Cache cache)
        {
            _cache = cache;
        }

        public ContentCacheItem Add(ContentCacheKey key, string contentType, string content)
        {
            return Add(new WebContentCacheItem(key, contentType, content));
        }

        public ContentCacheItem Add(ContentCacheKey key, string contentType, Stream stream)
        {
            // Read the full contents of the stream into the cache
            using (var reader = new StreamReader(stream))
            {
                return Add(new WebContentCacheItem(key, contentType, reader.ReadToEnd()));
            }
        }

        public void Clear()
        {
            var enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _cache.Remove((string)enumerator.Key);
            }
        }

        private ContentCacheItem Add(WebContentCacheItem cacheItem)
        {
            Guard.IsNotNull(cacheItem, "cacheItem");
            Guard.IsNotNull(cacheItem.Key, "cacheItem.Key");

            var cacheKey = new WebCacheKey(cacheItem.Key);
            _cache.Insert(cacheKey.Value, cacheItem);
            return cacheItem;
        }

        public ContentCacheItem Get(ContentCacheKey key)
        {
            Guard.IsNotNull(key, "key");

            var cacheKey = new WebCacheKey(key);
            object cachedItem = _cache.Get(cacheKey.Value);
            return cachedItem as WebContentCacheItem;
        }

        public void Remove(ContentCacheKey key)
        {
            if (key == null)
                return;

            var cacheKey = new WebCacheKey(key);

            try
            {
                _cache.Remove(cacheKey.Value);
            }
            catch (Exception ex)
            {
                // Swallow
                Trace.WriteLine(string.Format(
                        "Could not remove cached item '{0}': {1}",
                        cacheKey.Value,
                        ex
                    ));
            }
        }


        private class WebCacheKey
        {
            private readonly ContentCacheKey key;

            public WebCacheKey(ContentCacheKey key)
            {
                Guard.IsNotNull(key, "ContentCacheKey");
                Guard.IsNotNull(key.Id, "ContentCacheKey.Key");

                this.key = key;
            }

            public string Value
            {
                get { return string.Format("{0}_{1}", key.Id, key.Culture); }
            }
        }

        private class WebContentCacheItem : ContentCacheItem
        {
            private readonly string _content;

            public WebContentCacheItem(ContentCacheKey key, string contentType, string content)
                : base(key, contentType)
            {
                Guard.IsNotNull(key, "key");
                Guard.IsNotNull(contentType, "contentType");
                Guard.IsNotNull(content, "content");

                _content = content;
            }

            public override Stream GetContentStream(Encoding encoding)
            {
                return new MemoryStream(encoding.GetBytes(_content));
            }
        }
    }
}