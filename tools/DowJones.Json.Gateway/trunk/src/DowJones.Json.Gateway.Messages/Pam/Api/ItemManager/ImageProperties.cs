using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ImageProperties")]
    public class ImageProperties : UserFileProperties
    {
        [DataMember(Name = "imageMimeType")]
        public ImageMimeType ImageMimeType { get; set; }
    }
}