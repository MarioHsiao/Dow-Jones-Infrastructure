﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.ClientTemplates.modalDialog" +
    ".htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.ClientTemplates.saveOptions" +
    ".htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.ClientTemplates.formatOptio" +
    "ns.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.HeadlinePostProcessing.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing
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
    
    // Last Generated Timestamp: 06/29/2012 12:50 PM
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.ClientTemplates.modalDialog" +
        ".htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="modalDialog", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.HeadlinePostProcessingComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.ClientTemplates.saveOptions" +
        ".htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="saveOptions", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.HeadlinePostProcessingComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.ClientTemplates.formatOptio" +
        "ns.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="formatOptions", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.HeadlinePostProcessingComponent))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.HeadlinePostProcessing.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing.HeadlinePostProcessingComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class HeadlinePostProcessingComponent : DowJones.Web.Mvc.UI.ViewComponentBase<HeadlinePostProcessingModel>
    {
#line hidden

        public HeadlinePostProcessingComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_HeadlinePostProcessing";
            }
        }
        public override void ExecuteTemplate()
        {

WriteLiteral("\r\n");


WriteLiteral("\t\t   \r\n");


WriteLiteral("\r\n");





WriteLiteral("\r\n");


   CssClass = "dj_HeadlinePostProcessing"; 

WriteLiteral("<ul class=\"actions\">\r\n");


  	if (Model!=null && Model.HeadlinePostProcessingOptions!=null)
		{
			foreach (var item in Model.HeadlinePostProcessingOptions)
			{

WriteLiteral("\t\t\t<li>\r\n\t\t\t\t<span title=\"");


            Write(Html.DJ().Token(item.ToString().ToLower() + "ArticlesTitle"));

WriteLiteral("\" class=\"dj_icon dj_icon-inactive-");


                                                                                                           Write(item.ToString().ToLower());

WriteLiteral("\"></span>\r\n\t\t\t</li>\r\n");


				
				 
			}
		}
	

WriteLiteral("</ul>\r\n\r\n");


        }
    }
}