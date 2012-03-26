using System.Xml.Serialization;

namespace EMG.Tools.Charting.DataModels
{
    public enum LineStyle
    {
        [XmlEnum("plain")]
        Solid,
        [XmlEnum("dashed")]
        Dashed,
        [XmlEnum("dotted")]
        Dotted,
        [XmlEnum("dash-dot")]
        DashDot,
    }


}
