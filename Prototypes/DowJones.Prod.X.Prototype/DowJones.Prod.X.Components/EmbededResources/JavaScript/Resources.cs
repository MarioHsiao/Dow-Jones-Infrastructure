using System.Web.UI;
using DowJones.Prod.X.Components.EmbededResources.JavaScript;
using DowJones.Web;

#region Scripts

[assembly: WebResource(Resources.JavaScript.BootstrapJs, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.BootstrapJsMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.BootstrapTooltip, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.BootstrapModal, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.BootstrapDatePicker, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.JQueryUI, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.ParsleyJs, KnownMimeTypes.JavaScript)]

[assembly: WebResource(Resources.JavaScript.OmnitureJs, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.BootstrapJsMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.JQueryUI, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.JQueryUIMin, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.ParsleyJs, KnownMimeTypes.JavaScript)]


[assembly: WebResource(Resources.JavaScript.HighChartsExportingNew, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.HighChartsMoreNew, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.HighChartsNew, KnownMimeTypes.JavaScript)]

[assembly: WebResource(Resources.JavaScript.ResponseJs, KnownMimeTypes.JavaScript)]
[assembly: WebResource(Resources.JavaScript.ResponseMinJs, KnownMimeTypes.JavaScript)]

#endregion


namespace DowJones.Prod.X.Components.EmbededResources.JavaScript
{
    public static class Resources
	{
        [ScriptResource(ResourceName = BootstrapJs, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "bootstrap", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = OmnitureJs, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "omniture", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = BootstrapTooltip, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "bootstrapTooltip", DependsOn = new[] { "bootstrap" })]
        [ScriptResource(ResourceName = BootstrapDatePicker, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "bootstrapDatePicker", DependsOn = new[] { "bootstrap" })]
        [ScriptResource(ResourceName = BootstrapModal, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "bootstrapModal", DependsOn = new[] { "bootstrap" })]
        [ScriptResource(ResourceName = JQueryUI, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jqueryUI", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = JQueryUIMin, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jqueryUIMin", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = ParsleyJs, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "parsley", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = SortElements, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "sortElements", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = NestedSortable, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "nestedSortable", DependsOn = new[] { "jqueryUI" })]
        [ScriptResource(ResourceName = NestedSortable, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "nestedSortable", DependsOn = new[] { "jqueryUI" })]


        [ScriptResource(ResourceName = HighChartsNew, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highchartsNew", DependsOn = new[] { "jquery" })]
        [ScriptResource(ResourceName = HighChartsMoreNew, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highchartsMoreNew", DependsOn = new[] { "highchartsNew" })]
        [ScriptResource(ResourceName = HighChartsExportingNew, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highchartsExporting", DependsOn = new[] { "highchartsMoreNew" })]

        [ScriptResource(ResourceName = ResponseJs, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "responseJs", DependsOn = new[] { "jquery" })]
       
		public static class JavaScript
		{
            internal const string BasePath = "DowJones.Prod.X.Components.EmbededResources.JavaScript.";
            internal const string BootstrapBasePath = BasePath + "Bootstrap.";
            internal const string OmnitureBasePath = BasePath + "Omniture.";
            internal const string JQueryUiBasePath = BasePath + "jQueryUI.";
            internal const string HighChartsBasePath = BasePath + "Highcharts.";

            public const string BootstrapJs = BootstrapBasePath + "bootstrap.js";
            public const string OmnitureJs = OmnitureBasePath + "omniture.js";
            public const string BootstrapTooltip = BootstrapBasePath + "bootstrap-tooltip.js";
            public const string BootstrapDatePicker = BootstrapBasePath + "bootstrap-datepicker.js";
            public const string BootstrapModal = BootstrapBasePath + "bootstrap-modal.js";
            public const string BootstrapJsMin = BootstrapBasePath + "bootstrap.min.js";
            public const string JQueryUI = JQueryUiBasePath + "jquery-ui.js";
            public const string JQueryUIMin = JQueryUiBasePath + "jquery-ui.min.js";
            public const string ParsleyJs = BasePath + "parsley.js";
            public const string SortElements = BasePath + "jquery.sortElements.js";
            public const string NestedSortable = BasePath + "jquery.mjs.nestedSortable.js";

            public const string HighChartsNew = HighChartsBasePath + "highcharts.src.js";
            public const string HighChartsMoreNew = HighChartsBasePath + "highcharts-more.src.js";
            public const string HighChartsExportingNew = HighChartsBasePath + "exporting.src.js";

            public const string ResponseJs = BasePath + "response.js";
            public const string ResponseMinJs = BasePath + "response.min.js";
		}
	}
}
