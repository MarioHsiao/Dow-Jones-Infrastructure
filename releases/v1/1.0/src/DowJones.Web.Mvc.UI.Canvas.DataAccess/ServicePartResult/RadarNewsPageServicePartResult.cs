using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult
{
    [DataContract(Name = "radarNewspageModuleServicePartResult", Namespace = "")]
    public class RadarNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> where TPackage : RadarPackage
    {
    }
}
