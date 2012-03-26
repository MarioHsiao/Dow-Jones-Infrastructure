
using System;
using System.Xml.Serialization;

namespace EMG.Tools.Ajax.Suggest
{
    [Serializable]
    public enum Filter
    {
        [XmlEnum(Name = "None")]
        None,
        [XmlEnum(Name = "True")]
        True,
        [XmlEnum(Name = "False")]
        False
    }
    public class SuggestCompanyRequest
    {
        public string package;
        
        //public Filter 

    }
}
