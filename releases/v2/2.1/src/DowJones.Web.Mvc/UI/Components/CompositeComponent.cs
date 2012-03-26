using System.Collections.Generic;
using System.Web.UI;
using DowJones.Extensions.Web;
using DowJones.Web;
using DowJones.Web.Mvc.UI;

[assembly: WebResource(CompositeComponent.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI
{
    public abstract class CompositeComponent : CompositeComponent<object>
    {
        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Components";
        internal const string ScriptFile = BaseDirectory + ".CompositeComponent.js";
    }

    [ScriptResource(
        "CompositeComponent",
        ResourceName = CompositeComponent.ScriptFile,
        DeclaringType = typeof(CompositeComponent),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel
        )]
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
            ComponentFactory.ScriptRegistry().WithPubSubManager();
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