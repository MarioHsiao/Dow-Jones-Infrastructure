﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TopicEditor.ClientTemplates.main.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TopicEditor.ClientTemplates.filters.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TopicEditor.TopicEditor.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.TopicEditor
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
    using DowJones.Web.Mvc.UI.Components.AutoSuggest;
    
    // Last Generated Timestamp: 07/18/2012 05:20 PM
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TopicEditor.ClientTemplates.main.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="main", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TopicEditor.TopicEditorComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TopicEditor.ClientTemplates.filters.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="filters", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TopicEditor.TopicEditorComponent))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TopicEditor.TopicEditor.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TopicEditor.TopicEditorComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class TopicEditorComponent : DowJones.Web.Mvc.UI.ViewComponentBase<TopicEditorModel>
    {
#line hidden

        public TopicEditorComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_TopicEditor";
            }
        }
        public override void ExecuteTemplate()
        {








   
    CssClass = "dj_TopicEditor";
    Html.DJ().ScriptRegistry().WithJQueryUIWidgets().WithColorPicker();


WriteLiteral("\r\n\t");


        }
    }
}