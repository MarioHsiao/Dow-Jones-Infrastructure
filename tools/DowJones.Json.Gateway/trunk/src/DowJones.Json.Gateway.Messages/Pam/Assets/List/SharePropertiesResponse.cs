using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "SharePropertiesResponse", Namespace = "")]
    public class SharePropertiesResponse : ShareProperties
    {
        [DataMember(Name = "shareStatus", IsRequired = true)]
        public ShareStatus ShareStatus { get; set; }

        [DataMember(Name = "isOwner", IsRequired = true)]
        public bool IsOwner { get; set; }

        [DataMember(Name = "shareType", IsRequired = true)]
        public ShareType ShareType { get; set; }

        [DataMember(Name = "rootID", IsRequired = true)]
        public long RootId { get; set; }

        [DataMember(Name = "rootAccessControlScope", IsRequired = true)]
        public ShareScope RootAccessControlScope { get; set; }

        [DataMember(Name = "lastModifiedDate", IsRequired = true, EmitDefaultValue = false)]
        public string LastModifiedDate { get; set; }

        [DataMember(Name = "previousACScope", IsRequired = true)]
        public ShareScope PreviousAcScope { get; set; }

        [DataMember(Name = "internalAccess", IsRequired = true)]
        public bool InternalAccess { get; set; }

        [DataMember(Name = "allowCopy", IsRequired = true)]
        public bool AllowCopy { get; set; }
    }
}