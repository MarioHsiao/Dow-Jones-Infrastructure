﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.AutoSuggest.AutoSuggest.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.AutoSuggest.ClientTemplates.Success.htm", "text/html")]

namespace DowJones.Web.Mvc.UI.Components.AutoSuggest
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
    
    // Last Generated Timestamp: 07/26/2012 04:24 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.AutoSuggest.AutoSuggest.js", DependsOn=new string[] {
            "autocomplete-plugin",
            "crossdomain"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.AutoSuggest.AutoSuggestComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.AutoSuggest.ClientTemplates.Success.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="success", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.AutoSuggest.AutoSuggestComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class AutoSuggestComponent : DowJones.Web.Mvc.UI.ViewComponentBase<AutoSuggestModel>
    {
#line hidden

        public AutoSuggestComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_AutoSuggestComponent";
            }
        }
        public override void ExecuteTemplate()
        {





   CssClass = "dj_AutoSuggest"; 

WriteLiteral("\r\n");


        }
    }
}
