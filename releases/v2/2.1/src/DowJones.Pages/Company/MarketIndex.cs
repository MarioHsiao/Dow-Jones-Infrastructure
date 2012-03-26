using System.Runtime.Serialization;

namespace DowJones.Pages.Company
{
    [DataContract(Name = "marketIndex", Namespace = "")]
    public class MarketIndex
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }
    }
}