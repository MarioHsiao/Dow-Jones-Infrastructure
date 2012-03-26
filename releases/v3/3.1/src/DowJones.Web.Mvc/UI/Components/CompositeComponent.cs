using System.Collections.Generic;
using System.Web.UI;
using DowJones.Extensions.Web;

namespace DowJones.Web.Mvc.UI
{
    public abstract class CompositeComponent<TState> : ViewComponentBase<TState> 
        where TState : class
    {
        public override string ClientPluginName
        {
            get { return "dj_Composite"; }
        }


        protected CompositeComponent()
        {
        }

        protected CompositeComponent(IEnumerable<IViewComponent> children)
            : base(children)
        {
        }


        protected override void WriteContent(HtmlTextWriter writer)
        {
            writer.RenderSection("dj_content", WriteContentArea);

        }

        protected virtual void WriteContentArea(HtmlTextWriter writer)
        {
            if (IsSectionDefined("ContentArea"))
                RenderSection("ContentArea", writer);
            else
                writer.Write(TemplateContent);
        }


    }
}