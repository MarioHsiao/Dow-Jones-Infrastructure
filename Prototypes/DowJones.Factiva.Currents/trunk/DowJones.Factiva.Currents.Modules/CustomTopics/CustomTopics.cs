﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Factiva.Currents.Modules.CustomTopics.CustomTopics.js", "text/javascript")]

namespace DowJones.Factiva.Currents.Modules.CustomTopics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using Web.Mvc.Extensions;
    using DowJones.Factiva.Currents.Models;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 11/15/2012 12:01 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Factiva.Currents.Modules.CustomTopics.CustomTopics.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Factiva.Currents.Modules.CustomTopics.CustomTopics))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class CustomTopics : DowJones.Web.Mvc.UI.CompositeComponent<CustomTopicsModel>
    {
#line hidden

        public CustomTopics()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_CustomTopic";
            }
        }
        public override void ExecuteTemplate()
        {


WriteLiteral("\r\n");




WriteLiteral("\r\n");


   
    CssClass += " dj_CustomTopics";


WriteLiteral("\r\n<div class=\"module\">\r\n    <header>\r\n        <i class=\"icon-th-large icon-white\"" +
"></i>\r\n        <span>Custom Topics</span>\r\n    </header>\r\n    <div class=\"conten" +
"t\">\r\n\t\t<div class=\"row\">\r\n");


 			foreach(var headline in Model.CurrentsHeadlines.Take(3))
			{

WriteLiteral("\t\t\t\t<div class=\"span4\">");


                  Write(Html.DJ().Render(headline));

WriteLiteral("</div>\r\n");


			}

WriteLiteral("\t\t</div>\r\n    </div>\r\n</div>");


        }
    }
}
