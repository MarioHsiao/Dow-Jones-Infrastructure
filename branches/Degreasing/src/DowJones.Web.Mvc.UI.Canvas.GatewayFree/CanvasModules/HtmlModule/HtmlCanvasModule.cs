﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.HtmlModule.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule
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
    
    // Last Generated Timestamp: 08/02/2012 03:37 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.HtmlModule.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.HtmlModule.HtmlCanvasModule))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class HtmlCanvasModule : DowJones.Web.Mvc.UI.Canvas.AbstractCanvasModule<CanvasModules.HtmlModule.HtmlModule>
    {
#line hidden

        public HtmlCanvasModule()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_HtmlModule";
            }
        }
        public override void ExecuteTemplate()
        {



WriteLiteral("\r\n");


DefineSection("ContentArea", () => {

WriteLiteral("\r\n    ");


Write(Html.Raw(Model.Html));

WriteLiteral("\r\n\r\n");


     if (Model.HasScript) {

WriteLiteral("        <script class=\"script\" type=\"text/delayed-execution-javascript\">\r\n       " +
" ");


   Write(Html.Raw(Model.Script));

WriteLiteral("\r\n        </script>\r\n");


    }


});


        }
    }
}