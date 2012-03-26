using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Showcase.Components.SourcesFilter
{
    [ScriptResource(
        "SourcesFilterComponent",
        Url = "~/Components/SourcesFilter/SourcesFilter.js",
        DependencyLevel = ClientResourceDependencyLevel.Component
    )]
    public class SourcesFilterComponent : CompositeComponent<SourcesFilterModel>
    {
        public override string ClientPluginName
        {
            get { return "dj_SourcesFilter"; }
        }

        public SourcesFilterComponent()
        {
            CssClass = "SourcesFilter";
            RegisterSection("ContentArea", ContentArea);
        }

        private void ContentArea()
        {
            WriteLiteral("<div>");
            WriteLiteral("<ul class='filters'>");
            foreach (var source in Model.Sources)
            {
                WriteLiteral("<li class='selected' data-item='");
                Write(source);
                WriteLiteral("'>");
                Write(source);
                WriteLiteral("</li>");
            }
            WriteLiteral("</ul>");
            WriteLiteral("</div>");
        }
    }
}