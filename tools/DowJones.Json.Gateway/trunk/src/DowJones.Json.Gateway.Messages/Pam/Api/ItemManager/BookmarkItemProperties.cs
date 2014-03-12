using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "BookmarkItemProperties")]
    public class BookmarkItemProperties : ItemProperties
    {
        public BookmarkItemProperties()
        {
            Category = new List<string>();
        }

        [DataMember(Name = "type")]
        public BookmarkValueType Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "category")]
        public List<string> Category { get; set; }
    }
}