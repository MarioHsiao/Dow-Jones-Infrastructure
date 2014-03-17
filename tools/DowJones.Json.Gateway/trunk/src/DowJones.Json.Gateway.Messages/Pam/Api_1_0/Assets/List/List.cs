using System.Runtime.Serialization;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "List", Namespace = "")]
    [KnownType(typeof (AuthorList))]
    [KnownType(typeof (IndustryList))]
    [KnownType(typeof (RegionList))]
    [KnownType(typeof (SubjectList))]
    [KnownType(typeof (ExecutiveList))]
    public class List
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "customCode")]
        public string CustomCode { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "itemGroupCollection")]
        public ItemGroupCollection ItemGroupCollection { get; set; }

        [DataMember(Name = "shareProperties")]
        public ShareProperties ShareProperties { get; set; }
    }
}