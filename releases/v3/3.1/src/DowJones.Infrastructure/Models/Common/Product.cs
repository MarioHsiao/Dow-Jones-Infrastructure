using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Infrastructure.Common
{
    public interface IProduct
    {
        /// <summary>
        /// Source configuration id is used to get source grouping list asset from PAM.
        /// </summary>
        string SourceGroupConfigurationId { get; }

        /// <summary>
        /// Used for generating cache key for page asset
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Used for identifying application in session cache API
        /// </summary>
        string CacheApplication { get; }
    }

    public class Product : IProduct, IEquatable<Product>, IComparable<Product>
    {
        public Product(string productId, string cacheApplication) : this(productId, cacheApplication, null)
        {
        }

        public Product(string productId, string cacheApplication, string sourceGroupConfigurationId)
        {
            CacheApplication = cacheApplication;
            Id = productId;
            SourceGroupConfigurationId = sourceGroupConfigurationId;
        }

        /// <summary>
        /// Source configuration id is used to get source grouping list asset from PAM.
        /// </summary>
        public string SourceGroupConfigurationId { get; private set; }

        /// <summary>
        /// Used for generating cache key for page asset
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Used for identifying application in session cache API
        /// </summary>
        public string CacheApplication { get; private set; }


        public int CompareTo(Product other)
        {
            if (ReferenceEquals(other, null)) 
                return default(int);

            return StringComparer.OrdinalIgnoreCase.Compare(Id, other.Id);
        }

        public bool Equals(Product other)
        {
            return other != null 
                && String.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class GlobalProduct : Product
    {
        public GlobalProduct()
            : base("GL", "Global")
        {
        }
    }

    public class ProductCollection : List<Product>
    {
        public ProductCollection()
        {
        }

        public ProductCollection(IEnumerable<Product> products)
            : base(products)
        {
        }

        public Product FindByProductId(string productId)
        {
            return this.FirstOrDefault(x =>
                                       string.Equals(productId, x.Id, StringComparison.OrdinalIgnoreCase));
        }
    }
}