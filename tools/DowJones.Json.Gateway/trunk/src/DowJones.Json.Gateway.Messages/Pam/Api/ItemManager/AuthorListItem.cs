using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "AuthorListItem")]
    public class AuthorListItem : Item
    {
        [DataMember(Name = "properties")]
        public AuthorListItemProperties Properties { get; set; }

        [DataMember(Name = "status")]
        private ItemStatus Status { get; set; }

        public AuthorListItem()
        {
            Properties = new AuthorListItemProperties();
        }
    }
}