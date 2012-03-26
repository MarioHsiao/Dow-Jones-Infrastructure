using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public class SwapModuleEditorRequest : IRequest
    {
        public ModuleType ModuleType { get; set; }

        public bool HasModuleType
        {
            get { return !string.IsNullOrWhiteSpace(ModuleType.ToString()); }
        }


        public virtual bool IsValid()
        {
            return HasModuleType;
        }
    }
}
