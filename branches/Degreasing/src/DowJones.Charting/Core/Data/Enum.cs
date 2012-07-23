// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enum.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the LineStyle type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace DowJones.Charting.Core.Data
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

    public enum LegendPosition
    {
        [XmlEnum("bottom")]
        Bottom,
        [XmlEnum("right")]
        Right,
        [XmlEnum("top")]
        Top,
    }

    public enum PieLegendPosition
    {
        [XmlEnum("bottom")]
        Bottom,
        [XmlEnum("top")]
        Top,
    }
}
