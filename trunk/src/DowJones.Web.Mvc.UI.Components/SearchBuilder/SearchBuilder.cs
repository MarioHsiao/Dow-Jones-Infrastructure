﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.categoryOptions.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.filterOptions.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.modalDialog.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.exclusions.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.filterPill.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.sourceFilterPill.htm" +
    "", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.SearchBuilder
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
    using DowJones.Web.Mvc.UI.Components.Models;
    using DowJones.Web.Mvc.UI.Components.Models;
    using DowJones.Utilities.Search.Core;
    using DowJones.Search;
    using DowJones.Web.Mvc.UI.Components.SearchCategoriesLookUp;
    using DowJones.Web.Mvc.UI.Components.SearchNewsFilter;
    using System.Web.Mvc;
    
    // Last Generated Timestamp: 01/25/2012 08:48 AM
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.categoryOptions.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="categoryOptions", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.filterOptions.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="filterOptions", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.modalDialog.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="modalDialog", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.exclusions.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="exclusions", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.filterPill.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="filterPill", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.ClientTemplates.sourceFilterPill.htm" +
        "", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="sourceFilterPill", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SearchBuilder.SearchBuilder))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class SearchBuilder : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.Models.SearchBuilderModel>
    {
#line hidden

        public SearchBuilder()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_SearchBuilder";
            }
        }
        public override void ExecuteTemplate()
        {


















   
    CssClass = "dj_SearchBuilder";
    Html.DJ().ScriptRegistry().WithJQueryUIWidgets();


WriteLiteral("\r\n<div class=\"dj_search-builder_advanced-search\">\r\n    <div class=\"dj_advanced-se" +
"arch clearfix\">       \r\n\t    <textarea type=\"text\" rows=\"1\" cols=\"160\" class=\"te" +
"xt-field\">");


                                                              Write(!string.IsNullOrEmpty(Model.Data.FreeText)? @Html.Raw(Model.Data.FreeText):Html.DJ().Token("searchBuilderAutoCompleteText"));

WriteLiteral("</textarea>\r\n\t    <span>");


      Write(Html.DJ().Token("clear"));

WriteLiteral("</span>\r\n\t</div>\r\n    <div class=\"dj_select-box-alt\">\r\n        <select class=\"sea" +
"rchIn dj_selectbox dj_selectbox-small\">\r\n");


              
                foreach (var area in Model.SearchFreeTextArea)
                {

WriteLiteral("                    <option value=\"");


                              Write(area.Code);

WriteLiteral("\" ");


                                           Write(area.Code == ((int)Model.Data.SearchIn).ToString()?"selected":"");

WriteLiteral(">");


                                                                                                              Write(area.Desc);

WriteLiteral("</option>    \r\n");


                }
            

WriteLiteral("        </select>\r\n        <select class=\"sortBy dj_selectbox dj_selectbox-small\"" +
">\r\n");


              
                foreach (var sortOrder in Model.SortBy)
                {

WriteLiteral("                    <option value=\"");


                              Write(sortOrder.Code);

WriteLiteral("\" ");


                                                Write(sortOrder.Code == ((int)Model.Data.SortBy).ToString() ? "selected" : "");

WriteLiteral(">");


                                                                                                                          Write(sortOrder.Desc);

WriteLiteral("</option>    \r\n");


                }
            

WriteLiteral("        </select>\r\n        <select class=\"date dj_selectbox dj_selectbox-small\">\r" +
"\n");


              
                foreach (var dateRange in Model.DateRange)
                {

WriteLiteral("                    <option value=\"");


                              Write(dateRange.Code);

WriteLiteral("\" ");


                                                Write(dateRange.Code == ((int)Model.Data.DateRange).ToString() ? "selected" : "");

WriteLiteral(">");


                                                                                                                             Write(dateRange.Desc);

WriteLiteral("</option>    \r\n");


                }
            

WriteLiteral("        </select>\r\n        <div class=\"date-wrap\" style=\"");


                                  Write(Model.Data.DateRange == SearchDateRange.Custom ? "" : "display:none;");

WriteLiteral("\">\r\n\t\t\t<input type=\"text\" class=\"datepicker\" maxlength=\"10\" value=\"");


                                                          Write(Model.FormattedStartDate);

WriteLiteral("\" />\r\n\t\t</div>\r\n        <div class=\"date-wrap second\" style=\"");


                                         Write(Model.Data.DateRange == SearchDateRange.Custom ? "" : "display:none;");

WriteLiteral("\">\r\n\t\t\t<label>");


     Write(Html.DJ().Token("to"));

WriteLiteral("</label>\r\n\t\t\t<input type=\"text\" class=\"datepicker\" maxlength=\"10\" value=\"");


                                                          Write(Model.FormattedEndDate);

WriteLiteral("\" />\r\n\t\t</div>\r\n        <span class=\"dj_btn exclude dj_btn-rounded-square dj_btn-" +
"grey alt-text ");


                                                                           Write(Model.Data.IncludeSocialMedia?"disabled-btn":"");

WriteLiteral("\">");


                                                                                                                              Write(Html.DJ().Token("exclude"));

WriteLiteral(" (<span>");


                                                                                                                                                                  Write((!Model.Data.IncludeSocialMedia && Model.Data.ExclusionFilter != null) ? Model.Data.ExclusionFilter.Count().ToString() : "0");

WriteLiteral("</span>)</span>\r\n        <div class=\"check-filters\">\r\n\t\t\t<input type=\"checkbox\" c" +
"lass=\"duplicates\" ");


                                         Write(Model.Data.Duplicates ? "checked" : "");

WriteLiteral(" />\r\n\t\t\t<label>");


     Write(Html.DJ().Token("duplicatesOn"));

WriteLiteral("</label>\r\n\r\n\t\t\t<input type=\"checkbox\" class=\"socialMedia\" ");


                                          Write(Model.Data.IncludeSocialMedia ? "checked" : "");

WriteLiteral(" />\r\n\t\t\t<label>");


     Write(Html.DJ().Token("includeSocialMedia"));

WriteLiteral("</label>\r\n\t\t</div>\r\n    </div>\r\n</div>\r\n<div class=\"dj_search-builder_filters-hea" +
"der clearfix\">\r\n\t<h3>");


Write(Html.DJ().Token("filters"));

WriteLiteral("</h3>\r\n\t<p>");


Write(Html.DJ().Token("searchBuilderFilterDesc"));

WriteLiteral("</p>\r\n\t<span class=\"dj_btn dj_btn-drk-grey no-margin\">");


                                           Write(Html.DJ().Token("clear"));

WriteLiteral("</span>\r\n</div>\r\n<div class=\"dj_search-builder_filters\">\r\n    <ul class=\"dj_searc" +
"h-builder_filters-list clearfix\">\r\n\t\t<li data-type=\"");


            Write(FilterType.Language.ToString());

WriteLiteral("\" data-notdisabled=\"true\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t" +
"\t\t<h4>");


  Write(Html.DJ().Token("language"));

WriteLiteral(@"</h4>
            <div class=""dj_pill-list-wrap"">
				<ul class=""dj_pill-list yellow clearfix"">
                    <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
                </ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Company.ToString());

WriteLiteral("\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t\t\t<h4>");


  Write(Html.DJ().Token("companyLabel"));

WriteLiteral(@"</h4>
            <div class=""dj_pill-list-wrap"">
				<ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
                <ul class=""dj_pill-list not-filter red clearfix""></ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Author.ToString());

WriteLiteral("\"  data-notdisabled=\"true\" class=\"dj_search-builder_filters-category clearfix\">\r\n" +
"\t\t\t<h4>");


  Write(Html.DJ().Token("cmAuthor"));

WriteLiteral(@"</h4>
			<div class=""dj_pill-list-wrap"">
                <ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
                <ul class=""dj_pill-list not-filter red clearfix""></ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Executive.ToString());

WriteLiteral("\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t\t\t<h4>");


  Write(Html.DJ().Token("executive"));

WriteLiteral(@"</h4>
			<div class=""dj_pill-list-wrap"">
                <ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
                <ul class=""dj_pill-list not-filter red clearfix""></ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Subject.ToString());

WriteLiteral("\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t\t\t<h4>");


  Write(Html.DJ().Token("subject"));

WriteLiteral(@"</h4>
			<div class=""dj_pill-list-wrap"">
                <ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
                <ul class=""dj_pill-list not-filter red clearfix""></ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Industry.ToString());

WriteLiteral("\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t\t\t<h4>");


  Write(Html.DJ().Token("industry"));

WriteLiteral(@"</h4>
			<div class=""dj_pill-list-wrap"">
                <ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
                <ul class=""dj_pill-list not-filter red clearfix""></ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Region.ToString());

WriteLiteral("\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t\t\t<h4>");


  Write(Html.DJ().Token("regionLabel"));

WriteLiteral(@"</h4>
			<div class=""dj_pill-list-wrap"">
                <ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
                <ul class=""dj_pill-list not-filter red clearfix""></ul>
            </div>
		</li>
        <li data-type=""");


                  Write(FilterType.Source.ToString());

WriteLiteral("\" data-notdisabled=\"true\" class=\"dj_search-builder_filters-category clearfix\">\r\n\t" +
"\t\t<h4>");


  Write(Html.DJ().Token("sourcesLabel"));

WriteLiteral(@"</h4>
			<div class=""dj_pill-list-wrap source"">
                <ul class=""dj_pill-list yellow clearfix"">
	                <li class=""dj_pill add"">
	                    <span>&nbsp;</span>
	                </li>
	            </ul>
            </div>
		</li>
	</ul>
</div>
");


  
    if (Model.Data != null && Model.Data.NewsFilters != null)
    {

WriteLiteral("        <div class=\"news-filter\">\r\n            <div class=\"dj_search-builder_filt" +
"ers\">\r\n                <div class=\"dj_search-filters\">\r\n                    <div" +
" class=\"wrap\">\r\n                        <h4>");


                       Write(Html.DJ().Token("newsFilters"));

WriteLiteral("</h4>\r\n\t\t                <div class=\"filters clearfix js_filters\">\r\n             " +
"               ");


                        Write(CreateChildControl<SearchNewsFilter>(new SearchNewsFilterModel { Filters = Model.Data.NewsFilters }));

WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </d" +
"iv>\r\n            </div>\r\n        </div>\r\n");


    }   


WriteLiteral("<div style=\"display: none;\" class=\"dummyLookUp\">\r\n    ");


Write(CreateChildControl<SearchCategoriesLookUp>(new SearchCategoriesLookUpModel { FilterType = FilterType.Language, ProductId = Model.ProductId, 
    TaxonomyServiceUrl=Model.TaxonomyServiceUrl, QueriesServiceUrl=Model.QueriesServiceUrl }));

WriteLiteral("\r\n</div>");


        }
    }
}
