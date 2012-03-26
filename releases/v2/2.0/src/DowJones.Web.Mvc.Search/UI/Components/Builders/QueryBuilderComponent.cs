using System.Web;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Web;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using DowJones.Web.Mvc.UI;
using Newtonsoft.Json;

[assembly: WebResource(QueryBuilderComponent.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.Search.UI.Components.Builders
{
    public abstract class QueryBuilderComponent : QueryBuilderComponent<SearchBuilder>
    {
        internal const string BaseDirectory = "DowJones.Web.Mvc.Search.UI.Components.Builders";
        internal const string ScriptFile = BaseDirectory + ".QueryBuilder.js";
    }

    [ScriptResource(
        "QueryBuilderComponent",
        ResourceName = QueryBuilderComponent.ScriptFile,
        DeclaringType = typeof(QueryBuilderComponent),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel
    )]
    public abstract class QueryBuilderComponent<TModel> : CompositeComponent<TModel> where TModel : SearchBuilder
    {
        protected QueryBuilderComponent()
        {
            CssClass += " dj_QueryBuilder ";
        }

        protected override void WriteContent(HtmlTextWriter writer)
        {
            writer.WriteLine("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\">", "SearchBuilderContext", Model.SearchBuilderContext.EscapeForHtml());

            base.WriteContent(writer);
        }
    }
}
