using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    public abstract class ContainerComponent<TState> : ViewComponentBase<TState>
        where TState : class, IViewComponentModel
    {
        
        public override string ClientPluginName
        {
            get { return null; }
        }

        protected override void WriteContent(HtmlTextWriter writer)
        {
           // do nothing
        }
    }
}