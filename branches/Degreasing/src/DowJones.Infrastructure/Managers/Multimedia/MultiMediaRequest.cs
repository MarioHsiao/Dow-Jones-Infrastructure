using System;
using System.Xml.Serialization;

namespace DowJones.Managers.Multimedia
{
    [Serializable]
    [XmlType(TypeName = "multimediaRequest", Namespace = "")]
    public class MultimediaRequest
    {
        [XmlElement(ElementName = "accessionNumber")]
        public string AccessionNumber { get; set; }
    }
}
