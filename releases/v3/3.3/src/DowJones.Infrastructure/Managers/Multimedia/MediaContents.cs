using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DowJones.Managers.Multimedia
{
    [Serializable]
    [XmlType(TypeName = "mediaContent", Namespace = "")]
    public class MediaContent
    {
        [XmlElement(ElementName = "type")]
        public string Type;

        [XmlElement(ElementName = "medium")]
        public string Medium;

        [XmlElement(ElementName = "bitRate")]
        public string BitRate;

        [XmlElement(ElementName = "frameRate")]
        public string FrameRate;

        [XmlElement(ElementName = "duration")]
        public string Duration;

        [XmlElement(ElementName = "width")]
        public string Width;

        [XmlElement(ElementName = "height")]
        public string Height;

        [XmlElement(ElementName = "streamer")]
        public string Streamer;

        [XmlElement(ElementName = "file")]
        public string File;

        [XmlElement(ElementName = "language")]
        public string Language;

        [XmlElement(ElementName = "url")]
        public string Url;
    }

    [Serializable]
    [XmlType(TypeName = "mustPlayFromSource", Namespace = "")]
    public class MustPlayFromSource
    {

        [XmlElement(ElementName = "status")]
        public bool Status;

        [XmlElement(ElementName = "url")]
        public string Url;
    }

    [Serializable]
    [XmlType(TypeName = "mediaContents", Namespace = "")]
    public class MediaContents : List<MediaContent> { }
}
