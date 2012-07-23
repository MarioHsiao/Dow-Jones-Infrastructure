using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace DowJones.Infrastructure
{
    [DataContract(Name = "cacheKey", Namespace = "")]
    public class ContentCacheKey : IEquatable<ContentCacheKey>
    {
        [DataMember(Name = "key")]
        public string Id { get; protected internal set; }

        [DataMember(Name = "culture")]
        public string Culture { get; protected internal set; }


        public ContentCacheKey(string key, CultureInfo culture)
            : this(key, culture.ThreeLetterISOLanguageName)
        {
        }

        public ContentCacheKey(string key, string culture)
        {
            Guard.IsNotNull(key, "key");

            Id = key;
            Culture = culture;
        }


        public bool Equals(ContentCacheKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id 
                && Culture == other.Culture;
        }
    }
}
