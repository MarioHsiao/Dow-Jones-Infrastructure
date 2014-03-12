using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "CreateItem")]
    public class CreateItem
    {
        public Item Item;
    }
}