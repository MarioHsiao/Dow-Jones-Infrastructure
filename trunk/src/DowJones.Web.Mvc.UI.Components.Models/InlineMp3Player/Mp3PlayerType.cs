using System.Xml.Serialization;
using DowJones.Web.Mvc.UI.Components.Attributes;

namespace DowJones.Web.Mvc.UI.Components.InlineMp3Player
{
    public enum MP3PlayerType
    {
        [XmlEnum(Name = "Mini")]
        [FlashPlayerMetaData(160, 20, "1.9", "7")]
        Mini = 0,
        
        [XmlEnum(Name = "Normal")]
        [FlashPlayerMetaData(200, 20, "1.9", "7")]
        Normal = 1,
        
        [XmlEnum(Name = "Volume")]
        [FlashPlayerMetaData(240, 20, "1.5", "7")]
        Volume = 2,

        [XmlEnum(Name = "Multiple")]
        [FlashPlayerMetaData(240, 20, "1.9", "7")]
        Multiple = 3,

        [XmlEnum(Name = "ReadSpeaker")]
        [FlashPlayerMetaData(250, 20, "1.9", "7")]
        ReadSpeaker = 4,

        [XmlEnum(Name = "DowJones")]
        [FlashPlayerMetaData(240, 20, "1.5", "7")]
        DowJones = 5,
    }
}
