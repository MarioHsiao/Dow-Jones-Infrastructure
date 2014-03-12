using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "Image")]
    public class Image : UserFile
    {
        public Image()
        {
            Properties = new ImageProperties();
        }

        [DataMember(Name = "properties")]
        public ImageProperties Properties { get; set; }
    }
}