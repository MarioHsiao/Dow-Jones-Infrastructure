using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "pageSubscriptionRequest", Namespace = "")]
    public class PageSubscriptionRequest : IAddPageByIdPageRequest
    {
        public bool IsValid()
        {
            return PageId.IsNotEmpty();
        }

        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }
     
    }
}
