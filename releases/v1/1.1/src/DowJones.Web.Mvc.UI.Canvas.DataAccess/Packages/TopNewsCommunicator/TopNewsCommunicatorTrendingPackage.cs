using System.Runtime.Serialization;
using DowJones.Web.Mvc.Models.News;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.TopNewsCommunicator
{
    [DataContract(Name = "topNewsCommunicatorTrendingPackage", Namespace = "")]
    public class TopNewsCommunicatorTrendingPackage : AbstractTopNewsCommunicatorPackage
    {
        [DataMember(Name = "result")]
        public TagCollection Result { get; protected internal set; }
    }
}
