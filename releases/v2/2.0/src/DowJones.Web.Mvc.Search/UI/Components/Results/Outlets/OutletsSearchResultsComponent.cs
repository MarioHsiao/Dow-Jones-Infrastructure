﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.Search.UI.Components.Results.Outlets.OutletsSearchResults.js", "text/javascript")]

namespace DowJones.Web.Mvc.Search.UI.Components.Results.Outlets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using DowJones.Search.Navigation;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 01/23/2012 01:33 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Results.Outlets.OutletsSearchResults.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Results.Outlets.OutletsSearchResultsComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class OutletsSearchResultsComponent : DowJones.Web.Mvc.UI.ViewComponentBase<OutletsSearchResults>
    {
#line hidden

        public OutletsSearchResultsComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_OutletsSearchResults";
            }
        }
        public override void ExecuteTemplate()
        {



WriteLiteral("\r\n");




WriteLiteral(@"
<input type=""hidden"" id=""outlet-list-pageSize"" name=""EntityNavigator.pageSize"" />
<input type=""hidden"" id=""outlet-list-firstIndex"" name=""EntityNavigator.firstIndex"" />
<input type=""hidden"" id=""outlet-list-sortBy"" name=""EntityNavigator.sortBy"" />
<input type=""hidden"" id=""outlet-list-sortOrder"" name=""EntityNavigator.sortOrder"" />
<input type=""hidden"" id=""outlet-list-selectedEntityIds"" name=""EntityNavigator.selectedEntityIds"" />
<div class=""columns clearfix"">
	<div style=""height:650px;overflow:auto;"">
");


 	if (Model.RelatedConcepts != null && Model.HideRelatedConcepts == false)
	{

WriteLiteral("\t\t<div class=\"dj_related-concepts ui-component clearfix\">\r\n\t\t\t<div class=\"list-wr" +
"ap\">\r\n\t\t\t\t<h2>");


   Write(DJ.Token( "relatedConcepts" ));

WriteLiteral("</h2>\r\n\t\t\t\t");


Write(DJ.Render( Model.RelatedConcepts ));

WriteLiteral("\r\n\t\t\t</div>\r\n\t\t\t<span class=\"dj_icon dj_close-section dj_icon-close-dark-gray\"></" +
"span>        \r\n\t\t</div>\r\n");


	}

WriteLiteral("\t\r\n\t");


Write(Html.Hidden("hideRelatedConcepts", Model.HideRelatedConcepts.ToString().ToLower()));

WriteLiteral("\r\n\t");


Write(Html.DJ().Render( Model.Outlets ));

WriteLiteral("\r\n\t</div>\r\n</div>");


        }
    }
}
