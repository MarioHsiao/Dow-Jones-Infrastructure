using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "updatePagePositionsRequest", Namespace = "")]
    public class UpdatePagePositionsRequest : IUpdatePagePositionsRequest
    {
        [DataMember(Name = "pagePositions")]
        public List<PagePosition> PagePositions { get; set; }

        public bool IsValid()
        {
            return PagePositions != null && PagePositions.Count > 0;
        }
    }
}
