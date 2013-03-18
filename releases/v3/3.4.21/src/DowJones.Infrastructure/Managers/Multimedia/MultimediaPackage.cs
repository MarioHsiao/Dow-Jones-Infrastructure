using System;
using System.Xml.Serialization;

namespace DowJones.Managers.Multimedia
{
    [Serializable]
    [XmlType(TypeName = "multimediaPackage", Namespace = "")]
    public class MultimediaPackage 
    {
        [XmlElement(ElementName = "guid")]
        public string Guid;

        [XmlElement(ElementName = "mustPlayFromSource")]
        public MustPlayFromSource MustPlayFromSource { get; set; }

        [XmlElement(ElementName = "mediaContents")]
        public MediaContents MediaContents { get; set; }
        
    }
}
