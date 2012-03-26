using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "addPageByIdRequest", Namespace = "")]
    public class AddPageByIdRequest : IAddPageByIdPageRequest
    {
        public bool IsValid()
        {
            return PageId.IsNotEmpty();
        }

        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set;}
     }
}
