﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.DiscoveryGraph.DiscoveryGraph.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.DiscoveryGraph.ClientTemplates.Success.htm", "text/html")]

namespace DowJones.Web.Mvc.UI.Components.DiscoveryGraph
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
    
    // Last Generated Timestamp: 06/08/2012 01:51 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.DiscoveryGraph.DiscoveryGraph.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.DiscoveryGraph.DiscoveryGraph))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.DiscoveryGraph.ClientTemplates.Success.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="success", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.DiscoveryGraph.DiscoveryGraph))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class DiscoveryGraph : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.DiscoveryGraph.DiscoveryGraphModel>
    {
#line hidden

        public DiscoveryGraph()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_DiscoveryGraph";
            }
        }
        public override void ExecuteTemplate()
        {





   
    CssClass = "dj_DiscoveryGraph";
    DJ.ScriptRegistry()
      .WithHighCharts()
      .WithServiceProxy()
      .WithJQueryTools()
      .WithJQueryUIInteractions(); 


WriteLiteral("\r\n<div class=\"dj_view_wrapper dj_widget_fullView\">\r\n    <div class=\"dj_discoveryG" +
"raph_item_wrap\">\r\n    </div>\r\n</div> \r\n");


        }
    }
}
