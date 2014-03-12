using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "BookmarkItem")]
    public class BookmarkItem : Item
    {

        [DataMember(Name = "properties")]
        public BookmarkItemProperties Properties { get; set; }

        public BookmarkItem()
        {
            Properties = new BookmarkItemProperties();
        }
    }
}