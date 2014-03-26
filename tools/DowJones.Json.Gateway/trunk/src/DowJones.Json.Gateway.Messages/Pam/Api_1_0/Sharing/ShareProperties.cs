using System.Runtime.Serialization;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareProperties", Namespace = "")]
    [KnownType(typeof (SharePropertiesResponse))]
    public class ShareProperties
    {
        [DataMember(Name = "accessPermission", IsRequired = true, EmitDefaultValue = false)]
        public Permission AccessPermission { get; set; }

        [DataMember(Name = "assignPermission", EmitDefaultValue = false)]
        public Permission AssignPermission { get; set; }

        [DataMember(Name = "updatePermission", EmitDefaultValue = false)]
        public Permission UpdatePermission { get; set; }

        [DataMember(Name = "deletePermission", EmitDefaultValue = false)]
        public Permission DeletePermission { get; set; }

        [DataMember(Name = "listingScope")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShareScope ListingScope { get; set; }

        [DataMember(Name = "sharePromotion")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShareScope SharePromotion { get; set; }

        [DataMember(Name = "qualifier")]
        public AccessQualifier Qualifier { get; set; }
    }
}