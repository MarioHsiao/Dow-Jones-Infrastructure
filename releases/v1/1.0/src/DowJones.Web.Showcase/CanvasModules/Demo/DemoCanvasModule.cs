﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Showcase.CanvasModules.Demo.DemoCanvasModule.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Showcase.CanvasModules.Demo.DemoCanvasModule.css", "text/css")]

namespace DowJones.Web.Showcase.CanvasModules.Demo
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
    
    // Last Generated Timestamp: 03/07/2011 04:12 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Showcase.CanvasModules.Demo.DemoCanvasModule.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Showcase.CanvasModules.Demo.DemoCanvasModule))]
    [DowJones.Web.StylesheetResourceAttribute(null, ResourceName="DowJones.Web.Showcase.CanvasModules.Demo.DemoCanvasModule.css", ResourceKind=DowJones.Web.ClientResourceKind.Stylesheet, DeclaringType=typeof(DowJones.Web.Showcase.CanvasModules.Demo.DemoCanvasModule))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.9.3.26911")]
    public class DemoCanvasModule : DowJones.Web.Mvc.UI.Canvas.AbstractCanvasModule<DemoModule>
    {
#line hidden

        public DemoCanvasModule()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_DemoCanvasModule";
            }
        }
        public override void ExecuteTemplate()
        {




WriteLiteral("\r\n");


   
    CssClass += " DemoCanvasModule wrap-9";
    Html.DJ().ScriptRegistry().WithJQueryUIEffects();


WriteLiteral("\r\n");


DefineSection("LoadingArea", () => {

WriteLiteral("\r\n    Loading demo module...\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("HeaderArea", () => {

WriteLiteral("\r\n    ");



WriteLiteral("\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("ContentArea", () => {

WriteLiteral(@"
    <p>
        <label>Timestamp:</label>
        <span class='last-updated'></span>
    </p>

    <p>
        <label>Global Timestamp:</label>
        <span class='global-last-updated'></span>
    </p>

    <div class='actions-container'>
        <button class='update-button' href='#'>Update</button>
        <button class='clear-button' href='#'>Clear</button>
    </div>
");


});


        }
    }
}
