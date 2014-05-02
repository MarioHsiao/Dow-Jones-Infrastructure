using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    public interface IViewComponentModel
    {
        string ID { get; set; }

        IEnumerable<IViewComponentModel> Children { get; set; }
    }
}