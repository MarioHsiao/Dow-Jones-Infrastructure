using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.Resources;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.DowJonesCommon, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ErrorManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HeadJs, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Highcharts, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQuery, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCycle, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJson, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUnobtrusiveAjax, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUICore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIEffects, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIInteractions, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIWidgets, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryValidate, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryValidateUnobtrusive, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Json2, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Modernizer, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ServiceProxy, KnownMimeTypes.JavaScript, PerformSubstitution = true)]
[assembly: WebResource(EmbeddedResources.Js.Underscore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DateFormat, KnownMimeTypes.JavaScript)]

#endregion

namespace DowJones.Web.Mvc.Resources
{
    public static class EmbeddedResources
    {

        [ScriptResource(ResourceName = DowJonesCommon, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = ErrorManager, DependencyLevel = ClientResourceDependencyLevel.Global, PerformSubstitution = true)]
        [ScriptResource(ResourceName = HeadJs, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = Highcharts, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQuery, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = JQueryCycle, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryJson, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = JQueryUICore, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = JQueryUIEffects, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUIInteractions, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUIWidgets, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryUnobtrusiveAjax, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryValidate, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryValidateUnobtrusive, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = Json2, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = Modernizer, DependencyLevel = ClientResourceDependencyLevel.Global)]
        [ScriptResource(ResourceName = ServiceProxy, DependencyLevel = ClientResourceDependencyLevel.Global,PerformSubstitution = true)]
        [ScriptResource(ResourceName = Underscore, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = DateFormat, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
        [ScriptResource(ResourceName = JQueryTouch, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]

        public static class Js
        {
            public const string DowJonesCommon = "DowJones.Web.Mvc.Resources.js.common.js";
            public const string ErrorManager = "DowJones.Web.Mvc.Resources.js.ErrorManager.js";
            public const string HeadJs = "DowJones.Web.Mvc.Resources.js.head.js";
            public const string Highcharts = "DowJones.Web.Mvc.Resources.js.highcharts.js";
            public const string JQuery = "DowJones.Web.Mvc.Resources.js.jquery.js";
            public const string JQueryCycle = "DowJones.Web.Mvc.Resources.js.jquery.cycle.all.js";
            public const string JQueryJson = "DowJones.Web.Mvc.Resources.js.jquery-json.js";
            public const string JQueryUICore = "DowJones.Web.Mvc.Resources.js.jquery-ui.core.js";
            public const string JQueryUIEffects = "DowJones.Web.Mvc.Resources.js.jquery-ui.effects.js";
            public const string JQueryUIInteractions = "DowJones.Web.Mvc.Resources.js.jquery-ui.interactions.js";
            public const string JQueryUIWidgets = "DowJones.Web.Mvc.Resources.js.jquery-ui.widgets.js";
            public const string JQueryUnobtrusiveAjax = "DowJones.Web.Mvc.Resources.js.jquery.unobtrusive-ajax.js";
            public const string JQueryValidate = "DowJones.Web.Mvc.Resources.js.jquery.validate.js";
            public const string JQueryValidateUnobtrusive = "DowJones.Web.Mvc.Resources.js.jquery.validate.unobtrusive.js";
            public const string Json2 = "DowJones.Web.Mvc.Resources.js.json2.js";
            public const string Modernizer = "DowJones.Web.Mvc.Resources.js.modernizr.js";
            public const string ServiceProxy = "DowJones.Web.Mvc.Resources.js.ServiceProxy.js";
            public const string Underscore = "DowJones.Web.Mvc.Resources.js.underscore.js";
            public const string DateFormat = "DowJones.Web.Mvc.Resources.js.dateFormat.js";
            public const string JQueryTouch = "DowJones.Web.Mvc.Resources.js.jquery.touchwipe.js";
        }

    }

}