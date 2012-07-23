using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentModel : IViewComponentModel
    {
        public virtual string ID
        {
            get; 
            set;
        }

        public virtual IEnumerable<IViewComponentModel> Children
        {
            get; 
            set;
        }
    }
}