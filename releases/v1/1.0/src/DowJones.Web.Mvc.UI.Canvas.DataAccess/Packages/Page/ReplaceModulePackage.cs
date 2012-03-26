using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using DowJones.Web.Mvc.UI.Models.NewsPages.Modules;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "replaceModulePackage", Namespace = "")]
    public class ReplaceModulePackage : IPackage
    {
        [DataMember(Name = "newsPageModule")]
        public NewsPageModule NewsPageModule { get; set; }
    }
}
