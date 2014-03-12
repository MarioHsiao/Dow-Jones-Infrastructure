using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof (SharePropertiesResponse))]
    [DataContract(Name = "ShareProperties")]
    public class ShareProperties
    {
        public ShareProperties()
        {
            DELETEPermission = new Permission();
            UpdatePermission = new Permission();
            AssignPermission = new Permission();
            AccessPermission = new Permission();
        }

        [DataMember(Name = "accessPermission")]
        public Permission AccessPermission { get; set; }

        [DataMember(Name = "assignPermission")]
        public Permission AssignPermission { get; set; }

        [DataMember(Name = "updatePermission")]
        public Permission UpdatePermission { get; set; }

        [DataMember(Name = "deletePermission")]
        public Permission DELETEPermission { get; set; }

        [DataMember(Name = "listingScope")]
        public ShareScope ListingScope { get; set; }

        [DataMember(Name = "sharePromotion")]
        public ShareScope SharePromotion { get; set; }

        [DataMember(Name = "qualifier")]
        public AccessQualifier Qualifier { get; set; }
    }
}