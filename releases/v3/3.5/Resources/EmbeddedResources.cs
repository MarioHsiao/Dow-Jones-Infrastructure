using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.Resources;

[assembly: WebResource(EmbeddedResources.Js.ColorPicker, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ColorPickerMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Common, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.CompositeComponent, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.CrossDomain, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.CrossDomainMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DowJonesJQueryExtensions, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Ellipsis, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.EllipsisMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ErrorManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Highcharts, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighchartsMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighchartsMore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighchartsMoreMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQuery, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCarousel, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCarouselMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCycle, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryCycleMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJScrollPane, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJScrollPaneMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryJson, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryPopupBalloon, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryPopupBalloonMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryDataTables, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryDataTablesMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DataTablesScroller, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DataTablesScrollerMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTimeAgo, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTimeAgoMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTools, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryTouchMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUICore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUICoreMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIEffects, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIEffectsMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIInteractions, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIInteractionsMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIWidgets, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryUIWidgetsMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Json2, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Json2Min, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Overlay, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ODSManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.PubSubManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Require, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.DomReady, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Text, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.RequireMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.ServiceProxy, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.TmpLoad, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.Underscore, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.UnderscoreMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.WindowManager, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryMobile, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.JQueryMobileMin, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.Resources
{
    public static class EmbeddedResources
    {

        [ScriptResource(ResourceName = ColorPicker, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "color-picker")]
        [ScriptResource(ResourceName = Common, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "common")]
        [ScriptResource(ResourceName = CompositeComponent, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "composite-component")]
        [ScriptResource(ResourceName = CrossDomain, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "crossdomain")]
        [ScriptResource(ResourceName = DowJonesJQueryExtensions, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "dj-jquery-ext")]
        [ScriptResource(ResourceName = Ellipsis, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "ellipsis")]
        [ScriptResource(ResourceName = ErrorManager, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "error-manager")]
        [ScriptResource(ResourceName = Highcharts, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highcharts")]
        [ScriptResource(ResourceName = HighchartsMore, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highcharts-more", DependsOn = new[] {"highcharts"})]
        [ScriptResource(ResourceName = JQuery, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "jquery")]
        [ScriptResource(ResourceName = JQueryCarousel, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-carousel")]
        [ScriptResource(ResourceName = JQueryCycle, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-cycle")]
        [ScriptResource(ResourceName = JQueryJScrollPane, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-scrollpane")]
        [ScriptResource(ResourceName = JQueryJson, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "jquery-json")]
        [ScriptResource(ResourceName = JQueryPopupBalloon, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-popupballon")]
        [ScriptResource(ResourceName = JQueryTableSorter, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-tablesorter")]
        [ScriptResource(ResourceName = JQueryDataTables, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-datatables")]
        [ScriptResource(ResourceName = DataTablesScroller, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-datatablescroller")]
        [ScriptResource(ResourceName = JQueryTimeAgo, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-timeago")]
        [ScriptResource(ResourceName = JQueryTools, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-tools")]
        [ScriptResource(ResourceName = JQueryTouchMin, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-touch")]
        [ScriptResource(ResourceName = JQueryUICore, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "jquery-ui")]
        [ScriptResource(ResourceName = JQueryUIEffects, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-ui-effects", DependsOn = new[] { "jquery-ui" })]
        [ScriptResource(ResourceName = JQueryUIInteractions, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-ui-interactions", DependsOn = new[] { "jquery-ui" })]
        [ScriptResource(ResourceName = JQueryUIWidgets, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-ui-widgets", DependsOn = new[] { "jquery-ui" })]
        [ScriptResource(ResourceName = Json2, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "JSON")]
		[ScriptResource(ResourceName = ODSManager, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "ods-manager")]
        [ScriptResource(ResourceName = Overlay, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "overlay")]
        [ScriptResource(ResourceName = PubSubManager, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "pubsub")]
        [ScriptResource(ResourceName = Require, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "require")]
        [ScriptResource(ResourceName = DomReady, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "domReady")]
        [ScriptResource(ResourceName = Text, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "text")]
        [ScriptResource(ResourceName = ServiceProxy, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "service-proxy", DependsOn = new[] { "error-manager" })]
        [ScriptResource(ResourceName = TmpLoad, DependencyLevel = ClientResourceDependencyLevel.Global, Name = "tmpload", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = Underscore, DependencyLevel = ClientResourceDependencyLevel.Core, Name = "underscore")]
        [ScriptResource(ResourceName = JQueryMobileMin, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-mobile", DependsOn = new[] { "jquery-ui", "jquery-ui-widgets" })]
      
        public static class Js
        {
            
            private const string JsRoot = "DowJones.Web.Mvc.Resources.js.";

            public const string ColorPicker = JsRoot + "color-picker.js";
            public const string ColorPickerMin = JsRoot + "color-picker.min.js";
            public const string Common = JsRoot + "common.js";
            public const string CompositeComponent = JsRoot + "composite-component.js";
            public const string CrossDomain = JsRoot + "crossdomain.js";
            public const string CrossDomainMin = JsRoot + "crossdomain.min.js";
            public const string DowJonesJQueryExtensions = JsRoot + "dj-jquery-ext.js";
            public const string Ellipsis = JsRoot + "ellipsis.js";
            public const string EllipsisMin = JsRoot + "ellipsis.min.js";
            public const string ErrorManager = JsRoot + "error-manager.js";
            public const string Highcharts = JsRoot + "highcharts.js";
			public const string HighchartsMin = JsRoot + "highcharts.min.js";
            public const string HighchartsMore = JsRoot + "highcharts-more.js";
			public const string HighchartsMoreMin = JsRoot + "highcharts-more.min.js";
            public const string JQuery = JsRoot + "jquery.js";
			public const string JQueryMin = JsRoot + "jquery.min.js";
            public const string JQueryCarousel = JsRoot + "jquery.jcarousel.js";
            public const string JQueryCarouselMin = JsRoot + "jquery.jcarousel.js";
            public const string JQueryCycle = JsRoot + "jquery.cycle.all.js";
            public const string JQueryCycleMin = JsRoot + "jquery.cycle.all.js";
            public const string JQueryJScrollPane = JsRoot + "jquery.jscrollpane.js";
            public const string JQueryJScrollPaneMin = JsRoot + "jquery.jscrollpane.js";
            public const string JQueryJson = JsRoot + "jquery-json.js";
            public const string JQueryPopupBalloon = JsRoot + "jquery.popupballoon.js";
            public const string JQueryPopupBalloonMin = JsRoot + "jquery.popupballoon.js";
            public const string JQueryTableSorter = JsRoot + "jquery.tablesorter.js";
            public const string JQueryTableSorterMin = JsRoot + "jquery.tablesorter.js";
            public const string JQueryDataTables = JsRoot + "jquery.dataTables.js";
            public const string JQueryDataTablesMin = JsRoot + "jquery.dataTables.min.js";
            public const string DataTablesScroller = JsRoot + "dataTables.scroller.js";
            public const string DataTablesScrollerMin = JsRoot + "dataTables.scroller.min.js";
            public const string JQueryTimeAgo = JsRoot + "jquery.timeago.js";
            public const string JQueryTimeAgoMin = JsRoot + "jquery.timeago.js";
            public const string JQueryTools = JsRoot + "jquery.tools.min.js";
            public const string JQueryTouchMin = JsRoot + "jquery.touchwipe.min.js";
            public const string JQueryUICore = JsRoot + "jquery-ui.core.js";
			public const string JQueryUICoreMin = JsRoot + "jquery-ui.core.min.js";
            public const string JQueryUIEffects = JsRoot + "jquery-ui.effects.js";
			public const string JQueryUIEffectsMin = JsRoot + "jquery-ui.effects.min.js";
            public const string JQueryUIInteractions = JsRoot + "jquery-ui.interactions.js";
			public const string JQueryUIInteractionsMin = JsRoot + "jquery-ui.interactions.min.js";
            public const string JQueryUIWidgets = JsRoot + "jquery-ui.widgets.js";
			public const string JQueryUIWidgetsMin = JsRoot + "jquery-ui.widgets.min.js";
            public const string Json2 = JsRoot + "json2.js";
			public const string Json2Min = JsRoot + "json2.min.js";
			public const string ODSManager = JsRoot + "ODSManager.js";
            public const string Overlay = JsRoot + "overlay.js";
            public const string PubSubManager = JsRoot + "pubsub.js";
            public const string Require = JsRoot + "require.js";
            public const string DomReady = JsRoot + "domReady.js";
            public const string Text = JsRoot + "text.js";
			public const string RequireMin = JsRoot + "require.min.js";
            public const string ServiceProxy = JsRoot + "service-proxy.js";
            public const string TmpLoad = JsRoot + "tmpload.js";
            public const string Underscore = JsRoot + "underscore.js";
			public const string UnderscoreMin = JsRoot + "underscore.min.js";
            public const string WindowManager = JsRoot + "window-manager.js";
            public const string JQueryMobile = JsRoot + "jquery.mobile.js";
            public const string JQueryMobileMin = JsRoot + "jquery.mobile.min.js";
        }
    }
}
