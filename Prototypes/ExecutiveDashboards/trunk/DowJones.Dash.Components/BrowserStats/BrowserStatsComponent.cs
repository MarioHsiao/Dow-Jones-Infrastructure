﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.BrowserStats.BrowserStats.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.BrowserStats.ClientTemplates.container.html", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.BrowserStats.ClientTemplates.statBars.html", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.BrowserStats.ClientTemplates.navPills.html", "text/html")]

namespace DowJones.Dash.Components.BrowserStats
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
    
    // Last Generated Timestamp: 12/04/2012 01:45 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Dash.Components.BrowserStats.BrowserStats.js", DependsOn=new string[] {
            "jquery-counter"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Dash.Components.BrowserStats.BrowserStatsComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Dash.Components.BrowserStats.ClientTemplates.container.html", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="container", DeclaringType=typeof(DowJones.Dash.Components.BrowserStats.BrowserStatsComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Dash.Components.BrowserStats.ClientTemplates.statBars.html", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="statBars", DeclaringType=typeof(DowJones.Dash.Components.BrowserStats.BrowserStatsComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Dash.Components.BrowserStats.ClientTemplates.navPills.html", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="navPills", DeclaringType=typeof(DowJones.Dash.Components.BrowserStats.BrowserStatsComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class BrowserStatsComponent : DowJones.Dash.Components.Dashboard<DowJones.Dash.Components.Models.BrowserStats.BrowserStatsModel>
    {
#line hidden

        public BrowserStatsComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_BrowserStats";
            }
        }
        public override void ExecuteTemplate()
        {







   CssClass = "dj_BrowserStats"; 

WriteLiteral("\r\n");


        }
    }
}
