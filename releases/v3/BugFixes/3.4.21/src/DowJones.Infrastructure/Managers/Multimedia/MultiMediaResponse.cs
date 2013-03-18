
using System;
using System.Xml.Serialization;

namespace DowJones.Managers.Multimedia
{
    [Serializable]
    [XmlType(TypeName = "multimediaResponse", Namespace = "")]
    public class MultimediaResponse
    {
        [XmlElement(ElementName = "status")]
        public long Status { get; set; }

        [XmlElement(ElementName = "multimediaResult")]
        public MultimediaPackage MultimediaResult { get; set; }
    }
}
