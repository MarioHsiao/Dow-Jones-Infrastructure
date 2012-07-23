using System.Web;
using DowJones.Extensions;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Session
{
    public class ReferringProductFactory : Factory<ReferringProduct>
    {
        public const string AccessPointCodeKey = "napc";
        public const string ProductPrefixKey = "SA_FROM";

        private readonly HttpRequestBase _request;

        public ReferringProductFactory(HttpRequestBase request)
        {
            _request = request;
        }

        public override ReferringProduct Create()
        {
            var accessPointCode = _request[AccessPointCodeKey].GetValueOrDefault();
            var productPrefix = _request[ProductPrefixKey].GetValueOrDefault();


            if (_request.Headers["credentials"].HasValue())
            {
                var credentials = JsonConvert.DeserializeObject<ReferringProductCredentials>(_request.Headers["credentials"]); 

                if (accessPointCode.IsNullOrEmpty() && credentials.AccessPointCode.HasValue())
                    accessPointCode = credentials.AccessPointCode;

                if (productPrefix.IsNullOrEmpty() && credentials.SA_FROM.HasValue())
                    productPrefix = credentials.SA_FROM;
            }

            return new ReferringProduct {
                AccessPointCode = accessPointCode,
                ProductPrefix = productPrefix,
            };
        }

        class ReferringProductCredentials
        {
            public string AccessPointCode { get; set; }
            public string SA_FROM { get; set; }
        }
    }
}