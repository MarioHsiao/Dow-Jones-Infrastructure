﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DowJones.Web.Mvc.Search.UI.Components.SearchNavigator
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
    
    // Last Generated Timestamp: 09/27/2011 04:48 PM
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.9.3.26911")]
    public class SearchNavigatorTree : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.Search.UI.Components.Filters.SearchNavigatorNode>
    {
#line hidden

        public SearchNavigatorTree()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return null;
            }
        }
        public override void ExecuteTemplate()
        {


  
    TagName = "ul";
    CssClass += " filters ";


WriteLiteral("\r\n");


 foreach (var filter in Model) {

WriteLiteral("    <li class=\"item branch ");


                       Write(filter.IsSelected ? "expanded" : string.Empty);

WriteLiteral("\"\r\n        data-group=\"");


               Write(filter.GroupCode);

WriteLiteral("\" data-code=\"");


                                             Write(filter.Ref);

WriteLiteral("\"\r\n        ");


    Write(filter.IsSecondaryGroup ? "data-secondary-group=true" : string.Empty);

WriteLiteral("\r\n    >\r\n\r\n");


         if (filter.HasChildren) {

WriteLiteral("            <span class=\"trigger dj_icon dj_browse-tree-toggle\">");


                                                            Write(filter.IsSelected ? "-" : "+");

WriteLiteral("</span>\r\n");


        }

WriteLiteral("        \r\n");


         if (filter.IsSelectable) {

WriteLiteral("            <a href=\"#\" class=\"child branch browse-item  name\">");


                                                          Write(filter.DisplayName);

WriteLiteral("</a>\r\n");


        }
        else {

WriteLiteral("            <span class=\"child branch browse-item name\">");


                                                   Write(filter.DisplayName);

WriteLiteral("</span>\r\n");


        }

WriteLiteral("            \r\n");


         if (filter.HasCount) {

WriteLiteral("            <span class=\"count\">(");


                            Write(Format(filter.ResultCount));

WriteLiteral(")</span>\r\n");


        }

WriteLiteral("         \r\n");


         if (filter.HasChildren) {
            
            
        Write(ComponentFactory.RenderComponent<SearchNavigatorTree>(filter));

                                                                            
            
        }

WriteLiteral("    </li>\r\n");


}


        }
    }
}