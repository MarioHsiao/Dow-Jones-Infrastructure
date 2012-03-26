// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enum.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace DowJones.Pages
{
    public enum DataPointFrequency
    {
        /// <summary>
        /// Daily Frequency
        /// </summary>
        [XmlEnum(Name = "D")]
        Daily,

        /// <summary>
        /// Weekly Frequency
        /// </summary>
        [XmlEnum(Name = "W")]
        Weekly,

        /// <summary>
        /// Monthly Frequency
        /// </summary>
        [XmlEnum(Name = "M")]
        Monthly,

        /// <summary>
        /// Quarterly Frequency
        /// </summary>
        [XmlEnum(Name = "Q")]
        Quarterly,

        /// <summary>
        /// Yearly Frequency
        /// </summary>
        [XmlEnum(Name = "Y")]
        Yearly,
    }

}
