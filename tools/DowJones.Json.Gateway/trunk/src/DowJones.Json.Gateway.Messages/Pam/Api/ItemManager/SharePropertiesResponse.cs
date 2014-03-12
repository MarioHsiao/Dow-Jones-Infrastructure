using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "SharePropertiesResponse")]
    public class SharePropertiesResponse : ShareProperties
    {
        [DataMember(Name = "shareStatus")]
        public ShareStatus ShareStatus { get; set; }

        [DataMember(Name = "isOwner")]
        public bool IsOwner { get; set; }

        [DataMember(Name = "shareType")]
        public ShareType ShareType { get; set; }

        [DataMember(Name = "rootID")]
        public long RootId { get; set; }

        [DataMember(Name = "rootAccessControlScope")]
        public ShareScope RootAccessControlScope { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public string LastModifiedDate { get; set; }

        [DataMember(Name = "previousACScope")]
        public ShareScope PreviousAcScope { get; set; }

        [DataMember(Name = "internalAccess")]
        public bool InternalAccess { get; set; }

        [DataMember(Name = "allowCopy")]
        public bool AllowCopy { get; set; }
    }
}