using System.Collections.Generic;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Web;
using DowJones.Dash.Components;
using DowJones.Web.Mvc.UI.Components.Menu;
using DowJones.Dash.Components.Models;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Canvas;

[assembly: WebResource(Dashboard.ScriptFile, KnownMimeTypes.JavaScript)]
namespace DowJones.Dash.Components
{
    internal sealed class Dashboard
    {
        internal const string BaseDirectory = "DowJones.Dash.Components.DashboardComponent";
        internal const string ScriptFile = BaseDirectory + ".Dashboard.js";
    }

    [ScriptResource(
        "Dashboard",
        ResourceName = Dashboard.ScriptFile,
        DeclaringType = typeof(Dashboard),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel
    )]
  
    public abstract class Dashboard<TModule> : CompositeComponent<TModule>
        where TModule : class
    {
        protected TModule Module
        {
            get { return Model; }
        }

        protected Dashboard()
            : this(null)
        {
        }

        protected Dashboard(IEnumerable<IViewComponent> children)
            : base(children)
        {
            CssClass = "dj_dashboard";
        }      
    }
}