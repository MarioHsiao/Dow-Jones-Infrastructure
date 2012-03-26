using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "addModuleToPageRequest", Namespace = "")]
    public class AddModuleToPageRequest : IAddModuleToPageRequest
    {
        public bool IsValid()
        {
            return PageId.IsNotEmpty();
        }

        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "moduleId")]
        public string ModuleId { get; set; }
    }
}
