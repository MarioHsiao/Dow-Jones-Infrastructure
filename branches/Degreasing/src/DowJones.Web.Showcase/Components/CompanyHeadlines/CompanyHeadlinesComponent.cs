using DowJones.Web.Mvc.Extensions;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Web.Showcase.Components.CompanyHeadlines
{
    [ScriptResource(
        "CompanyHeadlinesComponent",
        Url = "~/Components/CompanyHeadlines/CompanyHeadlines.js",
        DependencyLevel = ClientResourceDependencyLevel.Component
    )]
    public class CompanyHeadlinesComponent : CompositeComponent<CompanyHeadlinesModel>
    {
        public override string ClientPluginName
        {
            get { return "dj_CompanyHeadlines"; }
        }

        public CompanyHeadlinesComponent()
        {
            CssClass = "CompanyHeadlines";
            RegisterSection("ContentArea", ContentArea);
        }

        private void ContentArea()
        {
            WriteLiteral("<div>");
            WriteLiteral("<ul class='filters'>");
            foreach (var company in Model.Companies)
            {
                WriteLiteral("<li class='selected' data-item='");
                Write(company);
                WriteLiteral("'>");
                Write(company);
                WriteLiteral("</li>");
            }
            WriteLiteral("</ul>");
            WriteLiteral("</div>");

            Write(Html.DJ().Render(Model.Headlines));
        }
    }
}