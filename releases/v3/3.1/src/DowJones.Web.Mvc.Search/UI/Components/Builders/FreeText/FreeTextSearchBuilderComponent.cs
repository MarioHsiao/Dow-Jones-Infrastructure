﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DowJones.Web.Mvc.Search.UI.Components.Builders.FreeText
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
    using DowJones.Attributes;
    using DowJones.Globalization;
    using DowJones.Extensions;
    using DowJones.Web.Mvc.UI.Components.CalloutPopup;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using DowJones.Infrastructure;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 02/08/2012 03:39 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Builders.FreeText.FreeTextSearchBuilder.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Builders.FreeText.FreeTextSearchBuilderComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.29745")]
    public class FreeTextSearchBuilderComponent : DowJones.Web.Mvc.Search.UI.Components.Builders.QueryBuilderComponent<DowJones.Web.Mvc.Search.UI.Components.Builders.FreeText.FreeTextSearchBuilder>
    {
#line hidden

        public FreeTextSearchBuilderComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_FreeTextSearchBuilder";
            }
        }
        public override void ExecuteTemplate()
        {












   CssClass += "dj_FreeTextSearchBuilder"; 


  
    DJ.ScriptRegistry().WithJQueryUIWidgets();
    DJ.CalloutPopup();



Write(Html.Hidden("kind", "freetext"));

WriteLiteral("\r\n\r\n<!-- QUERY SUMMARY COMPONENT -->\r\n<div class=\"dj_query-summary ui-component\">" +
"\r\n    <ul>\r\n        <li><div class=\"label\">");


                          Write(DJ.Token("search"));

WriteLiteral("</div></li>\r\n        <li class=\"section-category\">");


                                Write(DJ.Token("wordsPhrases"));

WriteLiteral("</li>\r\n        <li class=\"ellipsis\">");


                        Write(Model.FreeText);

WriteLiteral("</li>\r\n        <li class=\"section-category\">");


                                Write(DJ.Token("date"));

WriteLiteral("</li>\r\n        <li class=\"ellipsis\">");


                        Write(DJ.Token(Model.DateRange));

WriteLiteral("</li>\r\n        <li class=\"section-category\">");


                                Write(DJ.Token("sourcesLabel"));

WriteLiteral("</li>\r\n        <li class=\"ellipsis\">");


                        Write(DJ.DisplayFilters(Model.Source, "allSources"));

WriteLiteral("</li>\r\n    </ul>\r\n    <div class=\"clear-expand\">\r\n        <span class=\"more\" titl" +
"e=\'");


                             Write(DJ.Token("showMore"));

WriteLiteral("\'>");


                                                    Write(DJ.Token("more"));

WriteLiteral("</span>\r\n    </div>\r\n</div><!-- end: .dj_query-summary -->\r\n\r\n<div class=\"dj_save" +
"-search\">\r\n    <span class=\"dj_btn dj_btn-lrg dj_btn-gray dj_btn-rounded-square " +
"save-as_btn\">\r\n\t\t");


Write(DJ.Token("saveAs"));

WriteLiteral("\r\n\t\t<span class=\"dj_btn-down-arrow\"></span>\r\n\t</span>\r\n    <span class=\'dj_btn dj" +
"_btn-lrg dj_btn-blue dj_btn-rounded-square no-margin modifySearch\' title=\'");


                                                                                               Write(DJ.Token("modifySearch"));

WriteLiteral("\'>");


                                                                                                                          Write(DJ.Token("modifySearch"));

WriteLiteral(@"</span>
</div><!-- end: .dj_save-search -->

<!-- end: dj_search-phrases -->
<!-- This is the content of the more callout located in the query summary -->
<table class='dj_query-summary-table hide'>
    <thead>
        <tr>
            <th scope=""col"">
                ");


           Write(DJ.Token("categoryFilter"));

WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");


           Write(DJ.Token("query"));

WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n    " +
"        <th scope=\"row\">\r\n                ");


           Write(DJ.Token("wordsPhrases"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"<label>");


                  Write(Model.FreeText);

WriteLiteral("</label>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"" +
"row\">\r\n                ");


           Write(DJ.Token("searchIn"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.Token(Model.FreeTextIn));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("date"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"<label>");


                  Write(DJ.Token(Model.DateRange));

WriteLiteral("</label>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"" +
"row\">\r\n                ");


           Write(DJ.Token("sortBy"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.Token(Model.Sort));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("duplicates"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(Model.DeduplicationMode);

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("socialMedia"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(Model.SocialMedia);

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("exclusions"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.CombineEnums(Model.Exclusions, "allExclusions"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("sourcesLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"<label>");


                  Write(DJ.DisplayFilters(Model.Source, "allSources"));

WriteLiteral("</label>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"" +
"row\">\r\n                ");


           Write(DJ.Token("languageLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.CombineLanguages(Model.Languages, "allLanguages"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("companyLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.DisplayFilters(Model.Company, "allCompanies"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("authorLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.DisplayFilters(Model.Author, "allAuthors"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("executiveLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.DisplayFilters(Model.Executive, "allExecutives"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("subjectLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.DisplayFilters(Model.Subject, "allSubjects"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("industryLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.DisplayFilters(Model.Industry, "allIndustries"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <th scope=\"row\">\r\n " +
"               ");


           Write(DJ.Token("regionLabel"));

WriteLiteral("\r\n            </th>\r\n            <td headers=\"itemcolumn col1\">\r\n                " +
"");


           Write(DJ.DisplayFilters(Model.Region, "allRegions"));

WriteLiteral("\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n\r\n\r\n<!-- Save As Menu" +
" -->\r\n<div class=\"menu saveAsMenu\" style=\"display:none;\">\r\n    <div class=\"menui" +
"tems\">\r\n");


          
            foreach (SelectListItem menu in Model.SaveOptions)
            {

WriteLiteral("                <div class=\"menuitem\">\r\n                    <div class=\"label\" da" +
"ta-saveas=\"");


                                               Write(menu.Value);

WriteLiteral("\">");


                                                            Write(menu.Text);

WriteLiteral("</div>\r\n                </div>\r\n");


            }
        

WriteLiteral("    </div>\r\n</div>\r\n<!-- Save As Menu -->");


        }
    }
}
