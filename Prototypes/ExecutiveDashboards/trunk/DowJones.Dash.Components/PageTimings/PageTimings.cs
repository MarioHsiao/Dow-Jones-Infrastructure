﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.PageTimings.PageTimings.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.PageTimings.ClientTemplates.container.html", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Dash.Components.PageTimings.ClientTemplates.success.html", "text/html")]

namespace DowJones.Dash.Components.PageTimings
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
    
    // Last Generated Timestamp: 09/18/2012 04:21 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Dash.Components.PageTimings.PageTimings.js", DependsOn=new string[] {
            "highcharts-more"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Dash.Components.PageTimings.PageTimings))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Dash.Components.PageTimings.ClientTemplates.container.html", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="container", DeclaringType=typeof(DowJones.Dash.Components.PageTimings.PageTimings))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Dash.Components.PageTimings.ClientTemplates.success.html", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="headlineSuccess", DeclaringType=typeof(DowJones.Dash.Components.PageTimings.PageTimings))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class PageTimings : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Dash.Components.Models.PageTimings.PageTimingsModel>
    {
#line hidden

        public PageTimings()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_PageTimings";
            }
        }
        public override void ExecuteTemplate()
        {






   CssClass = "dj_PageTimings"; 

        }
    }
}
