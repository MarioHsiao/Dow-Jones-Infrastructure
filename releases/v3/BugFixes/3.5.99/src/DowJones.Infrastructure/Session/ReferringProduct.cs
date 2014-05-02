using DowJones.Infrastructure.Common;

namespace DowJones.Session
{
    public class ReferringProduct
    {
        public virtual string AccessPointCode { get; set; }
        public virtual string ProductPrefix { get; set; }
        public virtual string ClientTypeCode { get; set; }

        public static implicit operator Product(ReferringProduct product)
        {
            return product == null ? null : new Product(product.ProductPrefix, null);
        }
    }
}