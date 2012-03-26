using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.TopNewsCommunicator;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult
{
    [DataContract(Name = "topNewsCommunicatorServicePartResult", Namespace = "")]
    public class TopNewsCommunicatorServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractTopNewsCommunicatorPackage
    {
    }
}
