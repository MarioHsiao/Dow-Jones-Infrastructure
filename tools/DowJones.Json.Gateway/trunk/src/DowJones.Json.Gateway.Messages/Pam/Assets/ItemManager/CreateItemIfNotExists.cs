using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "CreateItemIfNotExists")]
    public class CreateItemIfNotExists
    {
        public CreateItemIfNotExists()
        {
            Item = new List<Item>();
        }

        [DataMember(Name = "item")]
        public List<Item> Item { get; set; }
    }
}