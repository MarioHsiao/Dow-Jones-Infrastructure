using System.Runtime.Serialization;

namespace DowJones.Models.Company
{
    [DataContract(Name = "exchange", Namespace = "")]
    public class Exchange
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }

        [DataMember(Name = "timeZoneDescriptor")]
        public string TimeZoneDescriptor { get; set; }

        [DataMember(Name = "timeZoneAbbreviation")]
        public string TimeZoneAbbreviation{ get; set; }

        [DataMember(Name = "isPrimary")]
        public bool IsPrimary { get; set; }
    }
}