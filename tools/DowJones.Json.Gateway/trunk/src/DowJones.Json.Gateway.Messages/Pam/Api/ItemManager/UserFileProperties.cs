using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof(ImageProperties))]
    [DataContract(Name = "UserFileProperties")]
    public abstract class UserFileProperties : ItemProperties
    {
        [DataMember(Name = "filePath")]
        public string FilePath { get; set; }
    }
}