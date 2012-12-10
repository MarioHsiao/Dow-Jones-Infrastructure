using System.Collections.Generic;
using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI;

[assembly: WebResource("DowJones.Dash.Components.DashboardComponent.DashboardComponent.js", "text/javascript")]
[assembly: WebResource("DowJones.Dash.Components.DashboardComponent.ClientTemplates.baseContainer.html", "text/html")]

namespace DowJones.Dash.Components
{
    internal sealed class DashboardComponent
    {
        internal const string BaseDirectory = "DowJones.Dash.Components.DashboardComponent";
        internal const string ScriptFile = BaseDirectory + ".DashboardComponent.js";
    }

    [ScriptResource(
        "DashboardComponent",
        ResourceName = DashboardComponent.ScriptFile,
        DeclaringType = typeof(DashboardComponent),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel
    )]
    [ClientTemplateResourceAttribute(null, 
        ResourceName = "DowJones.Dash.Components.DashboardComponent.ClientTemplates.baseContainer.html", 
        ResourceKind = ClientResourceKind.ClientTemplate, 
        TemplateId = "baseContainer", DeclaringType = typeof(DashboardComponent))]
    public abstract class DashboardComponent<TCompositeComponent> : CompositeComponent<TCompositeComponent>
        where TCompositeComponent : class
    {
        protected TCompositeComponent Component
        {
            get { return Component; }
        }

        protected DashboardComponent()
            : this(null)
        {
        }

        protected DashboardComponent(IEnumerable<IViewComponent> children)
            : base(children)
        {
            CssClass = "dj_dashboardComponent";
        }      
    }
}