﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarousel.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.jquery.ui.headline-carousel.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.VideoX.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.VideoY.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.HeadlineHitCounts" +
    "Package.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.DiscoveredEntitie" +
    "sPackage.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.NoData.htm", "text/html")]

namespace DowJones.Web.Mvc.UI.Components.HeadlineCarousel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 05/09/2012 12:42 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarousel.js", DependsOn=new string[] {
            "jquery-ui-widgets"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.jquery.ui.headline-carousel.js", DependsOn=new string[] {
            "jquery-ui-widgets"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.VideoX.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="videoX", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.VideoY.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="videoY", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.HeadlineHitCounts" +
        "Package.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="headlineHitCountsPackage", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.DiscoveredEntitie" +
        "sPackage.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="discoveredEntitiesPackage", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlineCarousel.ClientTemplates.NoData.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="noData", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselControl))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class HeadlineCarouselControl : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.HeadlineCarousel.HeadlineCarouselModel>
    {
#line hidden

        public HeadlineCarouselControl()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_HeadlineCarousel";
            }
        }
        public override void ExecuteTemplate()
        {










  
    CssClass = "dj_headlineListCarousel";
    DJ.ScriptRegistry()
        .WithJQueryUIWidgets()
        .WithJQueryTouch();
 

        }
    }
}
