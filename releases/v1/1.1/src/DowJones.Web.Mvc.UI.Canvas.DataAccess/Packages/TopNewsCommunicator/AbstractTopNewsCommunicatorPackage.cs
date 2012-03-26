using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.TopNewsCommunicator
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(TopNewsCommunicatorBreakingNewsPackage))]
    [KnownType(typeof(TopNewsCommunicatorRecentHeadlinesPackage))]
    [KnownType(typeof(TopNewsCommunicatorTrendingPackage))]
    public class AbstractTopNewsCommunicatorPackage : IPackage
    {
    }
}
