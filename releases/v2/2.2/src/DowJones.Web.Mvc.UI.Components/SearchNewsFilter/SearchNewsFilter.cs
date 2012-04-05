﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchNewsFilter.SearchNewsFilter.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.SearchNewsFilter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Search;
    using DowJones.Utilities.Search.Core;
    using DowJones.Globalization;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 04/05/2012 09:57 AM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchNewsFilter.SearchNewsFilter.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchNewsFilter.SearchNewsFilter))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class SearchNewsFilter : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.Models.SearchNewsFilterModel>
    {
#line hidden

        public SearchNewsFilter()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_SearchNewsFilter";
            }
        }
        public override void ExecuteTemplate()
        {








   
    CssClass = "dj_SearchNewsFilter";
    if (Model.Filters != null)
    {
        string removeFilter = ResourceTextManager.Instance.GetString("removeFilter");
        var filterCategories = new[] { NewsFilterCategory.Company, NewsFilterCategory.Executive, NewsFilterCategory.Author, 
        NewsFilterCategory.Industry, NewsFilterCategory.Subject, NewsFilterCategory.Region, NewsFilterCategory.Source,
        NewsFilterCategory.DateRange, NewsFilterCategory.Keyword };
        

WriteLiteral("        <ul class=\"dj_pill-list yellow clearfix js_pill-list\">\n");


          
        foreach (var category in filterCategories)
        {
            if (category == NewsFilterCategory.Keyword)
            {
                if (Model.Filters.Keyword != null && Model.Filters.Keyword.Count() > 0)
                {

WriteLiteral("                    <li class=\"filter-group\" data-type=\"");


                                                    Write(((int)NewsFilterCategory.Keyword).ToString());

WriteLiteral("\">\n                        <span class=\"section-category\">");


                                                  Write(Html.DJ().Token(Model.GetToken(NewsFilterCategory.Keyword, Model.Filters.Keyword.Count())));

WriteLiteral(":</span>\n                        <ul class=\"filter-list\">\n");


                          
                            foreach (var keyword in Model.Filters.Keyword)
                            {

WriteLiteral("                                <li class=\"dj_pill\">\n                            " +
"        <span class=\"filter\">");


                                                    Write(keyword);

WriteLiteral("</span>\n                                    <span class=\"remove\" tooltip=\"");


                                                             Write(removeFilter);

WriteLiteral("\"></span>\n                                </li>\r\n");


                            }
                        

WriteLiteral("                        </ul>\n                    </li>\r\n");


                }
            }
            else if (category == NewsFilterCategory.DateRange)
            {
                if (Model.Filters.DateRange != null )
                {

WriteLiteral("                    <li class=\"filter-group\" data-type=\"");


                                                    Write(((int)NewsFilterCategory.DateRange).ToString());

WriteLiteral("\">\n                        <span class=\"section-category\">");


                                                  Write(Html.DJ().Token(Model.GetToken(NewsFilterCategory.DateRange, 0)));

WriteLiteral(":</span>\n                        <ul class=\"filter-list\">\n                       " +
"     <li class=\"dj_pill\" data-code=\"");


                                                       Write(Model.Filters.DateRange.Code);

WriteLiteral("\">\n                                <span class=\"filter\">");


                                                Write(Model.Filters.DateRange.Desc);

WriteLiteral("</span>\n                                <span class=\"remove\" tooltip=\"");


                                                         Write(removeFilter);

WriteLiteral("\"></span>\n                            </li>\r\n                        </ul>\n      " +
"              </li>\r\n");


                }
            }
            else
            {
                var filters = Model.GetFilters(category);
                if (filters != null && filters.Count() > 0)
                {

WriteLiteral("                    <li class=\"filter-group\" data-type=\"");


                                                    Write(((int)category).ToString());

WriteLiteral("\">\n                        <span class=\"section-category\">");


                                                  Write(Html.DJ().Token(Model.GetToken(category, filters.Count())));

WriteLiteral(":</span>\n                        <ul class=\"filter-list\">\n");


                          
                            foreach (var filter in filters)
                            {

WriteLiteral("                                <li class=\"dj_pill\" data-code=\"");


                                                          Write(filter.Code);

WriteLiteral("\" data-codetype=\"");


                                                                                       Write(filter.CodeType);

WriteLiteral("\">\n                                    <span class=\"filter\">");


                                                    Write(filter.Desc);

WriteLiteral("</span>\n                                    <span class=\"remove\" tooltip=\"");


                                                             Write(removeFilter);

WriteLiteral("\"></span>\n                                </li>\r\n");


                            }
                        

WriteLiteral("                        </ul>\n                    </li>\r\n");


                }
            }
        }
        

WriteLiteral("        </ul>\r\n");


    }



        }
    }
}
