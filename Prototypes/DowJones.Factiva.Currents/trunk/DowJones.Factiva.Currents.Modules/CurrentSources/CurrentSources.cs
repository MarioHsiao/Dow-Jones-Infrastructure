﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Factiva.Currents.Modules.CurrentSources.CurrentSources.js", "text/javascript")]

namespace DowJones.Factiva.Currents.Modules.CurrentSources
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
    
    // Last Generated Timestamp: 11/14/2012 06:47 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Factiva.Currents.Modules.CurrentSources.CurrentSources.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Factiva.Currents.Modules.CurrentSources.CurrentSources))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class CurrentSources : DowJones.Web.Mvc.UI.CompositeComponent<CurrentSourcesModel>
    {
#line hidden

        public CurrentSources()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_CurrentSources";
            }
        }
        public override void ExecuteTemplate()
        {


WriteLiteral("\r\n");




WriteLiteral("\r\n");


   
    CssClass += " dj_CurrentSources";


WriteLiteral("\r\n<div class=\"module\">\r\n    <header>\r\n        <i class=\"icon-tasks icon-white\"></" +
"i>\r\n        <span>Sources</span>\r\n    </header>\r\n    <div class=\"content\">\r\n\t\t<d" +
"iv class=\"row\">\r\n");


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
