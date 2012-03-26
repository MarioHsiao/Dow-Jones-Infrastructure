﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFiltersManager.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFilters.js", "text/javascript")]

namespace DowJones.Web.Mvc.Search.UI.Components.Filters
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
    
    // Last Generated Timestamp: 11/03/2011 03:43 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFiltersManager.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFiltersComponent))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFilters.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFiltersComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class SearchFiltersComponent : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.Search.UI.Components.Filters.SearchFilters>
    {
#line hidden

        public SearchFiltersComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_SearchFilters";
            }
        }
        public override void ExecuteTemplate()
        {




WriteLiteral("\r\n<input type=\"hidden\" name=\"filters\" class=\"filters\" value=\'");


                                                      Write(Model.SerializedFilters);

WriteLiteral("\' />\r\n\r\n\r\n");



WriteLiteral("\r\n");


 if (!Model.HasFilters) { 
    return; 
}

WriteLiteral("\r\n\r\n<div class=\"dj_SearchFilters clearfix dj_search-filters ui-component js_searc" +
"h-filters\">\r\n    <div class=\"wrap\">\r\n        <h2 class=\"filters-label\">");


                             Write(DJ.Token("filters"));

WriteLiteral("</h2>\r\n        \r\n            <div class=\"filters clearfix js_filters\">\r\n         " +
"   \r\n                <ul class=\"filter-controls js_filter-controls\">\r\n          " +
"          <li class=\"clear-filters js_clear-filters\" tooltip=\"");


                                                                   Write(DJ.Token("clearFilters"));

WriteLiteral("\">");


                                                                                              Write(DJ.Token("clear"));

WriteLiteral("</li> \r\n                    <li style=\"display:none;\" less-label=\"");


                                                     Write(DJ.Token("less"));

WriteLiteral("\" more-label=\"");


                                                                                    Write(DJ.Token("more"));

WriteLiteral("\" class=\"display-toggle js_display-toggle\" tooltip=\"");


                                                                                                                                                         Write(DJ.Token("showMore"));

WriteLiteral("\"> \r\n                        <span class=\"label\">");


                                       Write(DJ.Token("more"));

WriteLiteral("</span> \r\n                        <span class=\"dj_icon\"></span>\r\n                " +
"    </li>\r\n                </ul><!-- end: .clear-expand -->\r\n\r\n                <" +
"ul class=\"dj_pill-list yellow clearfix js_pill-list\">\r\n\r\n");


                 foreach (var group in Model.Groups)
                {

WriteLiteral("                    <li class=\"filter-group\">\r\n                        <span clas" +
"s=\"section-category\">");


                                                  Write(DJ.Token(group.Category));

WriteLiteral(":</span>\r\n                        <ul class=\"filter-list\">\r\n\r\n");


                         foreach (var filter in group)
                        {
                        

WriteLiteral("                            <li class=\"dj_pill\" data-code=\"");


                                                      Write(filter.Code);

WriteLiteral("\" data-name=\"");


                                                                               Write(filter.Name);

WriteLiteral("\">\r\n                                    <span href=\"#\" class=\"filter\">");


                                                             Write(filter.Name);

WriteLiteral("</span>\r\n                                    <span class=\"remove\"></span>\r\n      " +
"                      </li>\r\n");


                        
                        }

WriteLiteral("\r\n                        </ul><!-- end: .filter-list -->\r\n                    </" +
"li>");



WriteLiteral("<!-- end: .filter-group -->\r\n");


                }

WriteLiteral("\r\n                </ul><!-- end: .dj_pill-list-->\r\n                    \r\n        " +
"    </div><!-- end: .filters -->\r\n    </div>\r\n</div>");


        }
    }
}
