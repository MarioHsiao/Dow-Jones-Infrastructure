﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.RegionalMap.RegionalMap.js", "text/javascript", PerformSubstitution=true)]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.RegionalMap.jquery.regionalmap.css", "text/css")]

namespace DowJones.Web.Mvc.UI.Components.RegionalMap
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Extensions;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 03/04/2011 01:53 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.RegionalMap.RegionalMap.js", PerformSubstitution=true, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.RegionalMap.RegionalMap))]
    [DowJones.Web.StylesheetResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.RegionalMap.jquery.regionalmap.css", ResourceKind=DowJones.Web.ClientResourceKind.Stylesheet, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.RegionalMap.RegionalMap))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.9.3.26911")]
    public class RegionalMap : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.RegionalMap.RegionalMapModel>
    {
#line hidden

        public RegionalMap()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_RegionalMap";
            }
        }
        public override void ExecuteTemplate()
        {






  
    CssClass = "dj_RegionalMap";
    Html.DJ().ScriptRegistry().WithHighCharts();


        }
    }
}
