using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "newsPagePackage", Namespace = "")]
    public class NewsPagePackage : IPackage
    {
        [DataMember(Name = "newsPage")]
        public NewsPage NewsPage { get; set; }
    }
}
