using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.UI
{
    public class ContentContainerModel : ViewComponentModel
    {
        public  ContentContainerModel(IEnumerable<IViewComponentModel> childComponentStates)
        {
            Children = childComponentStates ?? Enumerable.Empty<IViewComponentModel>();
        }
    }
}