using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "declinePageRequest", Namespace = "")]
    public class DeclinePageRequest : IPageRequest
    {
        public bool IsValid()
        {
            return PageId.IsNotEmpty();
        }

        [DataMember(Name = "pageId")]
        public string PageId { get; set; }
    }
}
