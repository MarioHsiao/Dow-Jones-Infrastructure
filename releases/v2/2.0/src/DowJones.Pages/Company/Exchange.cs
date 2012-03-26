using System.Runtime.Serialization;

namespace DowJones.Pages.Company
{
    [DataContract(Name = "exchange", Namespace = "")]
    public class Exchange
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }

        [DataMember(Name = "isPrimary")]
        public bool IsPrimary { get; set; }
    }
}