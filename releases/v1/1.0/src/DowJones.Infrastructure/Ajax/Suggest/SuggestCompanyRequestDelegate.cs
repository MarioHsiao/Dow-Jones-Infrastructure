
using System;
using System.Xml.Serialization;
using DowJones.Tools.WebServices.Suggest;
using DowJones.Tools.ServiceLayer.WebServices;

namespace DowJones.Tools.Ajax.Suggest
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

    public class SuggestCompanyRequestDelegate : IAjaxRequestDelegate
    {
        public string Package;

        public int MaxResults;

        public filter FilterNewsCoded;

        public filter FilterFCE;

        public filter FilterCQS;

        public string SearchText;

    }
}
