using System.Runtime.Serialization;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListPropertiesItem", Namespace = "")]
    public class ListPropertiesItem
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "properties", IsRequired = true, EmitDefaultValue = false)]
        public ListProperties Properties { get; set; }

        [DataMember(Name = "numberOfItems", IsRequired = true)]
        public int NumberOfItems { get; set; }

        [DataMember(Name = "shareProperties", IsRequired = true, EmitDefaultValue = false)]
        public ShareProperties ShareProperties { get; set; }
    }
}