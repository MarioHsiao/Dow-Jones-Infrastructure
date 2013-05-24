using System;
using System.Xml.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The search mode.
    /// </summary>
    [Serializable]
    public enum SearchMode
    {
        /// <summary>
        /// The simple.
        /// </summary>
        [XmlEnum(Name = "Simple")] Simple, 

        /// <summary>
        /// The traditional.
        /// </summary>
        [XmlEnum(Name = "Traditional")] Traditional, 
    }
}