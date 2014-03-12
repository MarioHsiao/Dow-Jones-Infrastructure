using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ShareProperties", Namespace = "")]
    [KnownType(typeof (SharePropertiesResponse))]
    public class ShareProperties
    {
        [DataMember(Name = "accessPermission", IsRequired = true, EmitDefaultValue = false)]
        public Permission AccessPermission { get; set; }

        [DataMember(Name = "assignPermission", IsRequired = true, EmitDefaultValue = false)]
        public Permission AssignPermission { get; set; }

        [DataMember(Name = "updatePermission", IsRequired = true, EmitDefaultValue = false)]
        public Permission UpdatePermission { get; set; }

        [DataMember(Name = "deletePermission", IsRequired = true, EmitDefaultValue = false, Order = 3)]
        public Permission DeletePermission { get; set; }

        [DataMember(Name = "listingScope", IsRequired = true, Order = 4)]
        public ShareScope ListingScope { get; set; }

        [DataMember(Name = "sharePromotion", IsRequired = true, Order = 5)]
        public ShareScope SharePromotion { get; set; }

        [DataMember(Name = "qualifier", IsRequired = true, Order = 6)]
        public AccessQualifier Qualifier { get; set; }
    }
}