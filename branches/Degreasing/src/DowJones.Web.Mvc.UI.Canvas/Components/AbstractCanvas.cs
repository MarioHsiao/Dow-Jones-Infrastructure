using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;

[assembly: WebResource(AbstractCanvas.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Canvas
{
    internal static class AbstractCanvas
    {
        internal const string ScriptFile = "DowJones.Web.Mvc.UI.Canvas.Components.AbstractCanvas.js";
    }


    [ScriptResource(
        "AbstractCanvas",
        DeclaringType = typeof(AbstractCanvas),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel,
        ResourceName = AbstractCanvas.ScriptFile,
        DependsOn = new[] { "error-manager", "jquery-ui-interactions", "AbstractCanvasModule" }
    )]
    public abstract class AbstractCanvas<TState> : CompositeComponent<TState>
        where TState : class, IViewComponentModel
    {

        public new IEnumerable<ICanvasModule> Children
        {
            get
            {
                return base.Children.Select(x => x as ICanvasModule);
            }
        }

        public override string ClientPluginName
        {
            get
            {
                return "dj_Canvas";
            }
        }

        protected AbstractCanvas()
        {
            CssClass += " dj_Canvas";
        }

        public override void AddChild(IViewComponent child)
        {
            if (!(child is ICanvasModule))
                throw new ApplicationException("Only Canvas Modules can be added to a Canvas");

            base.AddChild(child);
        }

        protected override void WriteContent(HtmlTextWriter writer)
        {
            RenderChildren(writer);
        }
    }
}
