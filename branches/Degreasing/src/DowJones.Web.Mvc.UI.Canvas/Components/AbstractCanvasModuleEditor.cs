using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;

[assembly: WebResource(AbstractCanvasModuleEditor.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Canvas
{
    internal sealed class AbstractCanvasModuleEditor
    {
        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Canvas.Components";
        internal const string ScriptFile = BaseDirectory + ".AbstractCanvasModuleEditor.js";
    }

    [ScriptResource(
        "AbstractCanvasModuleEditor",
        ResourceName = AbstractCanvasModuleEditor.ScriptFile,
        DeclaringType = typeof(AbstractCanvasModuleEditor),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel
    )]
    public abstract class AbstractCanvasModuleEditor<TModel> : ViewComponentBase<TModel>
        where TModel : class
    {
        protected AbstractCanvasModuleEditor()
        {
            CssClass = "dj_Editor ";
        }
    }
    
}
