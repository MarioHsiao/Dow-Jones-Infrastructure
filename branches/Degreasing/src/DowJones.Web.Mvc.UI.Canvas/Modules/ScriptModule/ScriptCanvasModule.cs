﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.ScriptModule.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule
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
    
    // Last Generated Timestamp: 08/20/2012 04:40 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.ScriptModule.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.ScriptCanvasModule))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class ScriptCanvasModule : AbstractCanvasModule<Modules.ScriptModule.ScriptModule>
    {
#line hidden

        public ScriptCanvasModule()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_ScriptModule";
            }
        }
        public override void ExecuteTemplate()
        {




  
    var colSpan = Module.ColumnSpan * 3; // Convert from 4col to 12col grid
    CssClass += " span" + colSpan;



DefineSection("ContentArea", () => {

WriteLiteral("<div class=\"script-component-container\">");


                                                         Write(Html.Raw(Model.Html ?? string.Empty));

WriteLiteral("</div>");


});

WriteLiteral("\r\n");


DefineSection("EditArea", () => {


});


        }
    }
}
