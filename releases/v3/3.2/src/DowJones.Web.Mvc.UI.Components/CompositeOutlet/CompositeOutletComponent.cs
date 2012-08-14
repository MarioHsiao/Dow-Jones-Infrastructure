﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.CompositeOutlet.CompositeOutlet.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.CompositeOutlet
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
    using DowJones.Web.Mvc.Extensions;
    using DowJones.Web.Mvc.UI;
    using DowJones.Web.Mvc.UI.Components.OutletList;
    using DowJones.Web.Mvc.UI.Components.CompositeOutlet;
    
    // Last Generated Timestamp: 07/31/2012 12:09 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.CompositeOutlet.CompositeOutlet.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.CompositeOutlet.CompositeOutlet))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class CompositeOutlet : CompositeComponent<CompositeOutletModel>
    {
#line hidden

        public CompositeOutlet()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_CompositeOutlet";
            }
        }
        public override void ExecuteTemplate()
        {





WriteLiteral("\r\n");


WriteLiteral("\r\n");




   CssClass = "dj_CompositeOutlet"; 


  
	bool hasOutlets = Model.OutletList != null 
		&& Model.OutletList.Outlets != null
		&& Model.OutletList.Outlets.Any();

	string[] s = { "", "", "", "", "", "", "" };
	switch (Model.PageSize)
	{
		case 10: s[0] = " selected"; break;
		case 20: s[1] = " selected"; break;
		case 30: s[2] = " selected"; break;
		case 40: s[3] = " selected"; break;
		case 50: s[4] = " selected"; break;
		case 100: s[5] = " selected"; break;
		default: s[1] = " selected"; break;
	}

	int maxlen = Model.TotalPages.ToString().Length;
	bool noFirst = Model.CurrentPage == 1;
	bool noLast = Model.CurrentPage == Model.TotalPages;
	bool hasEllipsis = Model.TotalPages > Model.GOOGLE_PAGER_SIZE;



 if (hasOutlets)
{

WriteLiteral("<section class=\"dj_page dj_author-lookup wrap-9 clearfix\">\r\n");


 	if (String.IsNullOrEmpty(Model.Title) == false)
	{ 

WriteLiteral("\t<h3>");


Write(Model.Title);

WriteLiteral("</h3>\r\n");


	}

WriteLiteral("\t<p class=\"dj_author-list-name_preferences-link\">\r\n\t\t<a href=\"javascript:void(0);" +
"\" >");


                            Write(Html.DJ().Token("cmmColumnSelectPreferences"));

WriteLiteral(@"</a>
	</p>
	<div class=""dj_paging"">

		<div class=""dj_select-has-menu-wrapper"">
			<input type=""checkbox"" name=""dj_outlet-select-all"">
			<span class=""fi fi_gear""></span>
		</div>

		<select class=""entity-list-select-page-size"">
			<option value=""10""");


                Write(s[0]);

WriteLiteral(">10 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"20\"");


                Write(s[1]);

WriteLiteral(">20 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"30\"");


                Write(s[2]);

WriteLiteral(">30 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"40\"");


                Write(s[3]);

WriteLiteral(">40 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"50\"");


                Write(s[4]);

WriteLiteral(">50 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"100\"");


                 Write(s[5]);

WriteLiteral(">100 ");


                           Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t</select>\r\n\r\n\t\t<div class=\"dj_list-select-count hide\">\r\n\t\t\t<span cla" +
"ss=\"count\"></span> ");


                          Write(Html.DJ().Token("cmalSelectionPlural"));

WriteLiteral("\r\n\t\t\t<span class=\"dj_clear-all-btn\">");


                             Write(Html.DJ().Token("cmalClearAll"));

WriteLiteral("</span>\r\n\t\t</div>\r\n\r\n\t\t<ul class=\"paging-controls\">\r\n\r\n\t\t\t<li><strong>");


          Write(Format(Model.FirstResultIndex + 1));

WriteLiteral(" - ");


                                                Write(Format(Model.LastResultIndex));

WriteLiteral("</strong> ");


                                                                                        Write(Html.DJ().Token("of"));

WriteLiteral(" ");


                                                                                                               Write(Format(Model.TotalResultCount));

WriteLiteral("</li>\r\n");


 			if (noFirst == true)
			{

WriteLiteral("\t\t\t\t<li class=\'disabled\'>\r\n\t\t\t\t\t<span class=\"fi fi_btn-prev-inactive\" tooltip=\'");


                                               Write(Html.DJ().Token("cmalPrev"));

WriteLiteral("\'></span>\r\n\t\t\t\t</li>\r\n");


				

WriteLiteral("\t\t\t\t<li class=\"first-page-link disabled\">");


                                    Write(Html.DJ().Token("cmalFirst"));

WriteLiteral("</li>\r\n");


			}
			else
			{

WriteLiteral("\t\t\t\t<li>\r\n\t\t\t\t\t<a class=\'entity-list-nav\' direction=\'prev\' href=\"javascript:void(" +
"0)\">\r\n\t\t\t\t\t\t<span class=\"fi fi_btn-prev-active\" tooltip=\'");


                                              Write(Html.DJ().Token("cmalPrev"));

WriteLiteral("\'></span>\r\n\t\t\t\t\t</a>\r\n\t\t\t\t</li>\r\n");


				

WriteLiteral("\t\t\t\t<li class=\"first-page-link\">\r\n\t\t\t\t\t<a class=\"entity-list-nav\" direction=\"firs" +
"t\" href=\"javascript:void(0)\">");


                                                                       Write(Html.DJ().Token("cmalFirst"));

WriteLiteral("</a>\r\n\t\t\t\t</li>\r\n");


				if (hasEllipsis)
				{

WriteLiteral("\t\t\t\t<li>&hellip;</li>\r\n");


				}
			}

WriteLiteral("\t\t\t\r\n\t\t\t");


Write(new MvcHtmlString(Model.PagerLikeGoogle()));

WriteLiteral("        \r\n\t\t\t\r\n");


 			if (noLast == true)
			{

WriteLiteral("\t\t\t\t<li class=\"last-page-link disabled\">");


                                   Write(Html.DJ().Token("cmalLast"));

WriteLiteral("</li>\r\n");


				

WriteLiteral("\t\t\t\t<li class=\'disabled\'>\r\n\t\t\t\t\t<span class=\"fi fi_btn-next-inactive\" tooltip=\'");


                                               Write(Html.DJ().Token("cmalNext"));

WriteLiteral("\'></span>\r\n\t\t\t\t</li>\r\n");


			}
			else
			{
				if (hasEllipsis)
				{

WriteLiteral("\t\t\t\t<li>&hellip;</li>\r\n");


				}
				

WriteLiteral("\t\t\t\t<li class=\"last-page-link\">\r\n\t\t\t\t\t<a class=\"entity-list-nav\" direction=\"last\"" +
" href=\"javascript:void(0)\">");


                                                                      Write(Html.DJ().Token("cmalLast"));

WriteLiteral("</a>\r\n\t\t\t\t</li>\r\n");



WriteLiteral("\t\t\t\t<li>\r\n\t\t\t\t\t<a class=\'entity-list-nav\' direction=\'next\' href=\"javascript:void(" +
"0)\">\r\n\t\t\t\t\t\t<span class=\"fi fi_btn-next-active\" tooltip=\'");


                                              Write(Html.DJ().Token("cmalNext"));

WriteLiteral("\'></span>\r\n\t\t\t\t\t</a>\r\n\t\t\t\t</li>\r\n");


			}

WriteLiteral("\t\t\t\r\n\t\t</ul>         \r\n\t\t\t\r\n\t</div><!-- end: .dj_paging -->\r\n\r\n\t<div class=\'table" +
"-wrap\'>\r\n\t");


Write(Html.DJ().RenderComponent<OutletList>(Model.OutletList));

WriteLiteral(@"
	</div>

	<div class=""dj_paging"">

		<div class=""dj_select-has-menu-wrapper"">
			<input type=""checkbox"" name=""dj_outlet-select-all"">
			<span class=""fi fi_gear""></span>
		</div>

		<select class=""entity-list-select-page-size"">
			<option value=""10""");


                Write(s[0]);

WriteLiteral(">10 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"20\"");


                Write(s[1]);

WriteLiteral(">20 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"30\"");


                Write(s[2]);

WriteLiteral(">30 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"40\"");


                Write(s[3]);

WriteLiteral(">40 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"50\"");


                Write(s[4]);

WriteLiteral(">50 ");


                         Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t\t<option value=\"100\"");


                 Write(s[5]);

WriteLiteral(">100 ");


                           Write(Html.DJ().Token("perPage"));

WriteLiteral("</option>\r\n\t\t</select>\r\n\r\n\t\t<div class=\"dj_list-select-count hide\">\r\n\t\t\t<span cla" +
"ss=\"count\"></span> ");


                          Write(Html.DJ().Token("cmalSelectionPlural"));

WriteLiteral("\r\n\t\t\t<span class=\"dj_clear-all-btn\">");


                             Write(Html.DJ().Token("cmalClearAll"));

WriteLiteral("</span>\r\n\t\t</div>\r\n\r\n\t\t<ul class=\"paging-controls\">\r\n\r\n\t\t\t<li><strong>");


          Write(Format(Model.FirstResultIndex + 1));

WriteLiteral(" - ");


                                                Write(Format(Model.LastResultIndex));

WriteLiteral("</strong> ");


                                                                                        Write(Html.DJ().Token("of"));

WriteLiteral(" ");


                                                                                                               Write(Format(Model.TotalResultCount));

WriteLiteral("</li>\r\n");


 			if (noFirst == true)
			{

WriteLiteral("\t\t\t\t<li class=\'disabled\'>\r\n\t\t\t\t\t<span class=\"fi fi_btn-prev-inactive\" tooltip=\'");


                                               Write(Html.DJ().Token("cmalPrev"));

WriteLiteral("\'></span>\r\n\t\t\t\t</li>\r\n");


				

WriteLiteral("\t\t\t\t<li class=\"first-page-link disabled\">");


                                    Write(Html.DJ().Token("cmalFirst"));

WriteLiteral("</li>\r\n");


			}
			else
			{

WriteLiteral("\t\t\t\t<li>\r\n\t\t\t\t\t<a class=\'entity-list-nav\' direction=\'prev\' href=\"javascript:void(" +
"0)\">\r\n\t\t\t\t\t\t<span class=\"fi fi_btn-prev-active\" tooltip=\'");


                                              Write(Html.DJ().Token("cmalPrev"));

WriteLiteral("\'></span>\r\n\t\t\t\t\t</a>\r\n\t\t\t\t</li>\r\n");


				

WriteLiteral("\t\t\t\t<li class=\"first-page-link\">\r\n\t\t\t\t\t<a class=\"entity-list-nav\" direction=\"firs" +
"t\" href=\"javascript:void(0)\">");


                                                                       Write(Html.DJ().Token("cmalFirst"));

WriteLiteral("</a>\r\n\t\t\t\t</li>\r\n");


				if (hasEllipsis)
				{

WriteLiteral("\t\t\t\t<li>&hellip;</li>\r\n");


				}
			}

WriteLiteral("\t\t\t\r\n\t\t\t");


Write(new MvcHtmlString(Model.PagerLikeGoogle()));

WriteLiteral("        \r\n\t\t\t\r\n");


 			if (noLast == true)
			{

WriteLiteral("\t\t\t\t<li class=\"last-page-link disabled\">");


                                   Write(Html.DJ().Token("cmalLast"));

WriteLiteral("</li>\r\n");


				

WriteLiteral("\t\t\t\t<li class=\'disabled\'>\r\n\t\t\t\t\t<span class=\"fi fi_btn-next-inactive\" tooltip=\'");


                                               Write(Html.DJ().Token("cmalNext"));

WriteLiteral("\'></span>\r\n\t\t\t\t</li>\r\n");


			}
			else
			{
				if (hasEllipsis)
				{

WriteLiteral("\t\t\t\t<li>&hellip;</li>\r\n");


				}
				

WriteLiteral("\t\t\t\t<li class=\"last-page-link\">\r\n\t\t\t\t\t<a class=\"entity-list-nav\" direction=\"last\"" +
" href=\"javascript:void(0)\">");


                                                                      Write(Html.DJ().Token("cmalLast"));

WriteLiteral("</a>\r\n\t\t\t\t</li>\r\n");



WriteLiteral("\t\t\t\t<li>\r\n\t\t\t\t\t<a class=\'entity-list-nav\' direction=\'next\' href=\"javascript:void(" +
"0)\">\r\n\t\t\t\t\t\t<span class=\"fi fi_btn-next-active\" tooltip=\'");


                                              Write(Html.DJ().Token("cmalNext"));

WriteLiteral("\'></span>\r\n\t\t\t\t\t</a>\r\n\t\t\t\t</li>\r\n");


			}

WriteLiteral("\t\t\t\r\n\t\t</ul>         \r\n\t\t\t\r\n\t</div><!-- end: .dj_paging -->\r\n\r\n</section>\r\n");


}


        }
    }
}