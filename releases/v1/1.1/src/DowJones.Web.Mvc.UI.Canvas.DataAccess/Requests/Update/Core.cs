using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update
{
    [CollectionDataContract(Name = "syndicationIds", ItemName = "syndicationId", Namespace = "")]
    public class SyndicationIdCollection : List<string>
    {
    }

    [CollectionDataContract(Name = "alerts", ItemName = "alert", Namespace = "")]
    public class AlertCollection : List<AlertID>
    {
    }

    [CollectionDataContract(Name = "fcodes", ItemName = "fcode", Namespace = "")]
    public class FCodeCollection : List<string>
    {
    }
}
