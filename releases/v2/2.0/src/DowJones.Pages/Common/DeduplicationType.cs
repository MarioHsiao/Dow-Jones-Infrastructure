using System;
using System.Xml.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The de-duplication type.
    /// </summary>
    [Serializable]
    public enum DeduplicationType
    {
        /// <summary>
        /// The not applicable de-duplication type.
        /// </summary>
        [XmlEnum(Name = "NotApplicable")] NotApplicable, 

        /// <summary>
        /// The off de-duplication type.
        /// </summary>
        [XmlEnum(Name = "Off")] Off, 

        /// <summary>
        /// The virtually identical de-duplication type.
        /// </summary>
        [XmlEnum(Name = "VirtuallyIdentical")] VirtuallyIdentical, 

        /// <summary>
        /// The similar de-duplication type.
        /// </summary>
        [XmlEnum(Name = "Similar")] Similar, 
    }
}