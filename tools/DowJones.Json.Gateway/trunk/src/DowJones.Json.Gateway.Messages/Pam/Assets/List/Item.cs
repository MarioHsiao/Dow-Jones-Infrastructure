using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "Item", Namespace = "")]
    [KnownType(typeof(AuthorItem))]
    [KnownType(typeof(IndustryItem))]
    [KnownType(typeof(RegionItem))]
    [KnownType(typeof(SubjectItem))]
    [KnownType(typeof(ExecutiveItem))]
    public class Item
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long id { get; set; }
    }
}