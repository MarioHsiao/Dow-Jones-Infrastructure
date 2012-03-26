using System.Xml.Serialization;

namespace DowJones.Utilities.Security
{
    /// <summary>
    /// Summary description for AC6.
    /// </summary>
    [XmlRoot("AC6")]
    public class AC6
    {
        [XmlElement]
        public AC6ITEM[] ITEM;
    }

    public class AC6ITEM
    {
        [XmlAttribute("class")]
        public int @class;

        [XmlAttribute]
        public int count;
    }

}