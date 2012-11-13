﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Factiva.Currents.Components.Trending.Trending.js", "text/javascript")]

namespace DowJones.Factiva.Currents.Components.Trending
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using Trending;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 11/13/2012 04:13 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Factiva.Currents.Components.Trending.Trending.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Factiva.Currents.Components.Trending.TrendingComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class TrendingComponent : DowJones.Web.Mvc.UI.ViewComponentBase<TrendingComponentModel>
    {
#line hidden

        public TrendingComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_Trending";
            }
        }
        public override void ExecuteTemplate()
        {





   CssClass = "dj_Trending"; 

WriteLiteral("\r\n<ul class=\"dj_Trending items\">\r\n");


 	if (Model.HasData)
	{
        foreach (var topEntity in Model.trendingTopEntitiesPackageModel)
		{

WriteLiteral("\t\t\t<li class=\"dj_entry ");


                  Write(Model.GetSelectionStatus(topEntity));

WriteLiteral(" ");


                                                       Write(topEntity.Descriptor);

WriteLiteral("  ");


                                                                              Write(topEntity.Descriptor);

WriteLiteral("\">\r\n\t\t\t\t<div class=\"article-wrap\">\r\n\t\t\t\t\t<h4 class=\"article-headline\">\r\n\t\t\t\t\t    " +
"<a href=\"");


             Write(Model.GetTrendingUrl(topEntity, Url));

WriteLiteral("\" class=\"article-view-trigger\" target=\"_blank\">");


                                                                                                 Write(topEntity.Descriptor);

WriteLiteral("</a>\r\n\t\t\t\t\t</h4>\r\n\t\t\t\t</div>\r\n\r\n\t\t\t</li>\r\n");


		}
	}
	else
		  {

WriteLiteral("\t\t\t  <li><span class=\"dj_noResults\">");


                               Write(DJ.Token("noResults"));

WriteLiteral("</span></li>\r\n");


		  }

WriteLiteral("</ul>");


        }
    }
}
