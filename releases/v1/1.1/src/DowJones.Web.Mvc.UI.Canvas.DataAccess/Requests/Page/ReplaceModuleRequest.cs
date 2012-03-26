using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "replaceModuleRequest", Namespace = "")]
    public class ReplaceModuleRequest : IReplaceModuleReqeust
    {
        public bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleIdToAdd.IsNotEmpty() && ModuleIdToRemove.IsNotEmpty();
        }

        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "moduleIdToRemove")]
        public string ModuleIdToRemove { get; set; }

        [DataMember(Name = "moduleIdToAdd")]
        public string ModuleIdToAdd { get; set; }
    }
}
