﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent.Editor.Embed" +
    "dedContentEditor.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent.Editor
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
    
    // Last Generated Timestamp: 08/03/2012 03:48 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent.Editor.Embed" +
        "dedContentEditor.js", DependsOn=new string[] {
            "AbstractCanvasModuleEditor"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent.Editor.EmbeddedContentEditorComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class EmbeddedContentEditorComponent : AbstractCanvasModuleEditor<CanvasModules.EmbeddedContent.Editor.EmbeddedContentEditor>
    {
#line hidden

        public EmbeddedContentEditorComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_EmbeddedContentEditor";
            }
        }
        public override void ExecuteTemplate()
        {




   CssClass += " dj_EmbeddedContentEditor"; 

WriteLiteral("\r\n<fieldset>\r\n    <p>\r\n        <label>");


          Write(DJ.Token("width"));

WriteLiteral("</label>\r\n        <input type=\"text\" class=\"width\" value=\"");


                                           Write(Model.Width);

WriteLiteral("\" />\r\n    </p>\r\n    <p>\r\n        <label>");


          Write(DJ.Token("height"));

WriteLiteral("</label>\r\n        <input type=\"text\" class=\"height\" value=\"");


                                            Write(Model.Height);

WriteLiteral("\" />\r\n    </p>\r\n    <p>\r\n        <label>");


          Write(DJ.Token("url"));

WriteLiteral("</label>\r\n        <input type=\"text\" class=\"url\" value=\"");


                                         Write(Model.Url);

WriteLiteral("\" />\r\n    </p>\r\n</fieldset>");


        }
    }
}
