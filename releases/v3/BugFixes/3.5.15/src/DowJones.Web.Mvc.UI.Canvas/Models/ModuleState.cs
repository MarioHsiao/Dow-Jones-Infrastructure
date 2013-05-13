using System;
using System.Xml.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas
{
    [Serializable]
    public enum ModuleState
    {
        [XmlEnum(Name = "Minimized")]
        Minimized,
        [XmlEnum(Name = "Maximized")]
        Maximized,
        [XmlEnum(Name = "Hidden")]
        Hidden,
    }
}