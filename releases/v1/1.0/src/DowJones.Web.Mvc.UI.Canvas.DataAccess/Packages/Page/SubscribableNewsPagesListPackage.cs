using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "subscribableNewsPagesListPackage", Namespace = "")]
    public class SubscribableNewsPagesListPackage : IPackage
    {
        [DataMember(Name = "newsPages")]
        public List<NewsPage> NewsPages { get; set; }
    }
}
