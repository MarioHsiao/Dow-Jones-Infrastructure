using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Company
{
    [DataContract(Namespace = "")]
    public abstract class AbstractBaseCompany
    {
        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "fcode")]
        public string FCode { get; set; }

        [DataMember(Name = "ticker")]
        public string DjTicker { get; set; }

        [DataMember(Name = "ric")]
        public string Ric { get; set; }

        [DataMember(Name = "listedExchanges")]
        public List<Exchange> ListedExchanges { get; set; }

        [DataMember(Name = "marketIndices")]
        public List<MarketIndex> MarketIndicies { get; set; }
    }
}