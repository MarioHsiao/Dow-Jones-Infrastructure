﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview.CompanyOverviewCanvasModule.js" +
    "", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview.CompanyOverviewCanvasModule.cs" +
    "s", "text/css")]

namespace DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Web.Mvc.UI.Components.Quote;
    using DowJones.Web.Mvc.UI.Components.TagCloud;
    using DowJones.Web.Mvc.UI.Canvas.Editors.CompanyOverview;
    using DowJones.Web.Mvc.UI.Canvas.Properties;
    using DowJones.Web.Mvc.UI.Components.Pager;
    using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
    using DowJones.Web.Mvc.UI.Components.NewsChart;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 05/09/2011 10:48 AM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview.CompanyOverviewCanvasModule.js" +
        "", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview.CompanyOverviewCanvasModule))]
    [DowJones.Web.StylesheetResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview.CompanyOverviewCanvasModule.cs" +
        "s", ResourceKind=DowJones.Web.ClientResourceKind.Stylesheet, DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.Modules.CompanyOverview.CompanyOverviewCanvasModule))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class CompanyOverviewCanvasModule : DowJones.Web.Mvc.UI.Canvas.AbstractCanvasModule<DowJones.Web.Mvc.UI.Canvas.Models.CompanyOverviewModule>
    {
#line hidden

        public CompanyOverviewCanvasModule()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_CompanyOverviewCanvasModule";
            }
        }
        public override void ExecuteTemplate()
        {









WriteLiteral("              \r\n");




WriteLiteral("\r\n");


   
    CssClass = CssClass + " dj_CompanyOverviewCanvasModule dj_module-company";
    Html.DJ().ScriptRegistry().WithJQueryUIWidgets();
    Html.DJ().ScriptRegistry().WithHighCharts();


WriteLiteral("\r\n");


DefineSection("ContentArea", () => {

WriteLiteral("\r\n    ");



WriteLiteral(@"
    <script type=""text/javascript"">
        //Temp Code
        function Token(str) {
            return str;
        }
        //Temp Code
    </script>
	<div style=""display:none;"" class=""company-chart-row module-row three-col no-bottom-padding clearfix"">
        <div class=""module-col three-thirds""><div class=""module-col-wrap"">
                ");


            Write(CreateChildControl<NewsChartControl>(
                    new DowJones.Web.Mvc.UI.Components.Models.NewsChartModel()
                    {
                        GraphHeight = 300,
                        GraphWidth = 987,
                        NewsSeriesTitle = Html.DJ().Token("newsVolume").ToString(),
                        StockSeriesTitle = Html.DJ().Token("stockPriceToken").ToString()
                    })
                );

WriteLiteral(@"
        </div></div>
    </div><!-- end: .company-chart-row -->

    <div class=""company-snapshot-row module-row no-top-padding clearfix"">
        <div class=""company-snapshot-col module-col one-third first-col"">
            <div class=""module-col-wrap standard-padding"">
                <h4 class=""module-col-title"">
                    ");


               Write(Html.DJ().Token("companyProfile"));

WriteLiteral(@"
                </h4>
                <p class=""snapshot-datetime""></p>
                <div>
                <div class=""dj_module-company-snapshot"">

                <ul class=""dj_module-company-snapshot-tabs clearfix"">
                    <li><a href=""#dj_market-data-module");


                                                   Write(Module.ModuleId);

WriteLiteral("\" title=\"");


                                                                             Write(Html.DJ().Token("currentPriceToken"));

WriteLiteral("\">");


                                                                                                                    Write(Html.DJ().Token("currentPriceToken"));

WriteLiteral("</a></li>\r\n                    <li><a href=\"#dj_analysis-reports");


                                                 Write(Module.ModuleId);

WriteLiteral("\" title=\"");


                                                                           Write(Html.DJ().Token("analystReports"));

WriteLiteral("\">");


                                                                                                               Write(Html.DJ().Token("analystReports"));

WriteLiteral("</a></li>\r\n                    <li><a href=\"#dj_executives");


                                           Write(Module.ModuleId);

WriteLiteral("\" title=\"");


                                                                     Write(Html.DJ().Token("executives"));

WriteLiteral("\">");


                                                                                                     Write(Html.DJ().Token("executives"));

WriteLiteral("</a></li>\r\n                </ul>\r\n\r\n                    <div id=\"dj_market-data-m" +
"odule");


                                              Write(Module.ModuleId);

WriteLiteral("\" class=\"ui-tabs-hide dj_market-data dj_module-company-snapshot-content\">\r\n      " +
"              ");


                Write(CreateChildControl<QuoteControl>(
                        new DowJones.Web.Mvc.UI.Components.Models.QuoteModel() { })
                    );

WriteLiteral("\r\n                </div>\r\n\r\n                <div id=\"dj_analysis-reports");


                                        Write(Module.ModuleId);

WriteLiteral("\" class=\"ui-tabs-hide dj_analysis-reports dj_module-company-snapshot-content\">\r\n\r" +
"\n                    <h5 class=\"analysis-report-source\">Investext Select</h5>\r\n " +
"                   ");


                Write(CreateChildControl<PortalHeadlineListControl>(
                        new DowJones.Web.Mvc.UI.Components.Models.PortalHeadlineListModel() { 
                            MaxNumHeadlinesToShow = Model.MaxResultsToReturn, DisplayNoResultsToken=true
                        })
                    );

WriteLiteral("                                \r\n                    <h5 class=\"analysis-report-" +
"source\">Zacks Investment Research</h5>\r\n\r\n                    ");


                Write(CreateChildControl<PortalHeadlineListControl>(
                        new DowJones.Web.Mvc.UI.Components.Models.PortalHeadlineListModel() { 
                            MaxNumHeadlinesToShow = Model.MaxResultsToReturn, DisplayNoResultsToken=true
                        })
                    );

WriteLiteral("\r\n            \r\n                    <h5 class=\"analysis-report-source\">Data Monit" +
"or Company Profiles</h5>\r\n                    ");


                Write(CreateChildControl<PortalHeadlineListControl>(
                        new DowJones.Web.Mvc.UI.Components.Models.PortalHeadlineListModel() { 
                            MaxNumHeadlinesToShow = Model.MaxResultsToReturn, DisplayNoResultsToken=true
                        })
                    );

WriteLiteral("\r\n\r\n                </div><!-- end: .dj_analysis-reports -->\r\n\r\n                <" +
"div id=\"dj_executives");


                                  Write(Module.ModuleId);

WriteLiteral(@""" class=""dj_company-executives dj_module-company-snapshot-content"">
                    <div></div>
                </div>

                </div>
                </div><!-- end: .dj_module-company-snapshot -->
                <div class=""company-snapshot-error""></div>
            </div>
        </div><!-- end: .company-snapshot-col -->
                    
        <div class=""trending-chart-col module-col one-third"" style=""position: relative;"">
            <div class=""module-col-wrap standard-padding"" style=""min-height: 339px;display:none;"">
                <h4 class=""module-col-title"">
                    ");


               Write(Html.DJ().Token("keywords"));

WriteLiteral("\r\n                    ");



WriteLiteral("\r\n                </h4>\r\n                <div class=\"trending-chart dj-bubblechar" +
"t\">\r\n                    ");


                Write(CreateChildControl<TagCloud>(
                        new DowJones.Web.Mvc.UI.Components.Models.TagCloudModel() { EnableEventFiring=true})
                    );

WriteLiteral(@"
                </div>
            </div>          
        </div>
        <div class=""article-group-col module-col one-third"" style=""position: relative;"">
            <div class=""module-col-wrap standard-padding"" style=""min-height: 339px;display:none;"">
                <div class=""article-group"">
                    <h4 class=""module-col-title"">
                        ");


                   Write(Html.DJ().Token("recentArticles"));

WriteLiteral("\r\n                        ");



WriteLiteral("\r\n                    </h4>\r\n                    ");


                Write(CreateChildControl<PortalHeadlineListControl>(
                        new DowJones.Web.Mvc.UI.Components.Models.PortalHeadlineListModel() { 
                            MaxNumHeadlinesToShow = Model.MaxResultsToReturn, DisplayNoResultsToken=true
                        })
                    );

WriteLiteral("\r\n                    <ul class=\"dc_list view-all-btn\">\r\n                        " +
"<li class=\"dc_item\">\r\n                            <a style=\"display:none;\" class" +
"=\"dashboard-control dc_btn dc_btn-1\" href=\"javascript:void(0);\">");


                                                                                                                     Write(Html.DJ().Token("viewAll"));

WriteLiteral("</a>\r\n                        </li>\r\n                    </ul>\r\n                <" +
"/div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"no-result" +
"s\" style=\"display:none;\">\r\n        ");


   Write(Html.DJ().Token("noResults"));

WriteLiteral("\r\n    </div>\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("OptionsArea", () => {

WriteLiteral("\r\n    ");



WriteLiteral("\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("InfoArea", () => {

WriteLiteral("\r\n    ");



WriteLiteral("\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("EditArea", () => {

WriteLiteral("\r\n    ");


Write(CreateChildControl<CompanyOverviewModuleEditor>(Model.Editor));

WriteLiteral("\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("MessageArea", () => {

WriteLiteral("\r\n    ");



WriteLiteral("\r\n");


});

WriteLiteral("\r\n\r\n");


DefineSection("LoadingArea", () => {

WriteLiteral("\r\n    ");



WriteLiteral("\r\n");


});

WriteLiteral("\r\n");


        }
    }
}
