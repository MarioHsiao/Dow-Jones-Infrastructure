using System;
using System.IO;
using System.Text;
using DowJones.Web;

namespace DowJones.Infrastructure
{
    public abstract class ContentCacheItem : IEquatable<ContentCacheItem>
    {
        private readonly ContentCacheKey _key;

        public string ContentType { get; protected internal set; }

        public virtual bool IsValid
        {
            get
            {
                var stream = GetContentStream(Encoding.Default);

                return stream != null
                       && stream.CanRead
                       && stream.Length > 0;
            }
        }

        public ContentCacheKey Key
        {
            get { return _key; }
        }


        protected ContentCacheItem(ContentCacheKey key, string contentType = null)
        {
            Guard.IsNotNull(key, "key");

            _key = key;
            ContentType = contentType ?? KnownMimeTypes.Content;
        }


        public abstract Stream GetContentStream(Encoding encoding);


        #region Equality Operators

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ContentCacheItem)) return false;
            return Equals((ContentCacheItem) obj);
        }

        public bool Equals(ContentCacheItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Key, Key);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public static bool operator ==(ContentCacheItem left, ContentCacheItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ContentCacheItem left, ContentCacheItem right)
        {
            return !Equals(left, right);
        }

        #endregion

    }
}