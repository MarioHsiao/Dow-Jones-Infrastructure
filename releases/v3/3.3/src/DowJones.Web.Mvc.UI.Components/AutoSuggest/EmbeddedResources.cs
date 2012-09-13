using System.Web.UI;
using DowJones.Web;

[assembly: WebResource("DowJones.Web.Mvc.UI.Components.AutoSuggest.scripts.jquery.autocomplete.js", KnownMimeTypes.JavaScript)]
[assembly: WebResource("DowJones.Web.Mvc.UI.Components.AutoSuggest.scripts.jquery.jsonp-2.3.1.min.js", KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Components.AutoSuggest
{
    [ScriptResourceAttribute("autocomplete-plugin", ResourceName = "DowJones.Web.Mvc.UI.Components.AutoSuggest.scripts.jquery.autocomplete.js", ResourceKind = ClientResourceKind.Script, DeclaringType = typeof(EmbeddedResources))]
    [ScriptResourceAttribute("jquery-jsonp-plugin", ResourceName = "DowJones.Web.Mvc.UI.Components.AutoSuggest.scripts.jquery.jsonp-2.3.1.min.js", ResourceKind = ClientResourceKind.Script, DeclaringType = typeof(EmbeddedResources))]
    public static class EmbeddedResources {}
}
