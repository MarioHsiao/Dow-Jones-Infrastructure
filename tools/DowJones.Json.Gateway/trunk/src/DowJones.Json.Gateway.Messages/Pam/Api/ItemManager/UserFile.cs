using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof(Image))]
    [DataContract(Name = "UserFile")]
    public abstract class UserFile : Item
    {
    }
}