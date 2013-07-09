using System.Web;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Prod.X.Common;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Common
{
    public class ReferringProductFactory : Factory<ReferringProduct>
    {
        private readonly HttpRequestBase _request;
        private const string AccessPointCode = "2";

        public ReferringProductFactory(HttpRequestBase request)
        {
            _request = request;
        }

        public override ReferringProduct Create()
        {

            var productPrefix = _request[CommonRequestParameterNames.ProductPrefix].GetValueOrDefault("FP");

            var tReferringProduct = new ReferringProduct
            {
                AccessPointCode = AccessPointCode,
                ProductPrefix = productPrefix,
                ClientTypeCode = "D"
            };

            return tReferringProduct;
        }
    }
}