using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "syndicationItemsPackage", Namespace = "")]
    public class SyndicationItemsPackage
    {
        [DataMember(Name = "syndicationItems")]
        public List<SyndicationItem> SyndicationItems { get; set; }
    }

    [DataContract(Name = "syndicationItem", Namespace = "")]
    public class SyndicationItem
    {
        [DataMember(Name = "id")]
        public long Id;

        [DataMember(Name = "url")]
        public string Url;
    }
}
