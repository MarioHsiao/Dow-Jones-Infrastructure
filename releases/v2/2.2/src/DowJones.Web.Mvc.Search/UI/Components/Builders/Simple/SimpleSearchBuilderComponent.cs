﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.Search.UI.Components.Builders.Simple.SimpleSearchBuilder.js", "text/javascript")]

namespace DowJones.Web.Mvc.Search.UI.Components.Builders.Simple
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
    using DowJones.Web.Mvc.Extensions;
    using DowJones.Web.Mvc.UI.Components.AutoSuggest;
    using DowJones.Web.Mvc.Search.UI.Components.Builders;
    using DowJones.Extensions;
    
    // Last Generated Timestamp: 04/16/2012 12:44 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Builders.Simple.SimpleSearchBuilder.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Builders.Simple.SimpleSearchBuilderComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class SimpleSearchBuilderComponent : QueryBuilderComponent<Simple.SimpleSearchBuilder>
    {
#line hidden

        public SimpleSearchBuilderComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_SimpleSearchBuilder";
            }
        }
        public override void ExecuteTemplate()
        {










   
	CssClass += "dj_SimpleSearchBuilder";
	Html.DJ().AutoSuggest();



Write(Html.Hidden("kind", "simple"));

WriteLiteral("\r\n");


Write(Html.HiddenFor("inclusions", Model.Inclusions));

WriteLiteral("\r\n<input type=\"hidden\" name=\"startDate\" value=\"");


                                        Write(Model.StartDate);

WriteLiteral("\" />\r\n<input type=\"hidden\" name=\"endDate\" value=\"");


                                      Write(Model.EndDate);

WriteLiteral("\" />\r\n<div class=\"dj_search-simple-search ui-component js_search-simple-search\">\r" +
"\n\t<a href=\"#\" class=\"advanced-search\">");


                                Write(DJ.Token("advancedSearch"));

WriteLiteral("</a>\r\n\t<div class=\"search-options\">\r\n\t\t<div class=\"source-range-options\">\r\n\t\t\t<se" +
"lect id=\"source\" name=\"source\" class=\"dj_selectbox dj_selectbox-small\">\r\n\t\t\t\t<op" +
"tion value=\"\">");


                Write(DJ.Token("allSources"));

WriteLiteral("</option>\r\n");


  				
					if (Model.SourceCollection != null)
					{

						if (Model.SourceCollection.TopLevelSourceGrouping != null && Model.SourceCollection.TopLevelSourceGrouping.Count > 0)
						{
							foreach (var source in Model.SourceCollection.TopLevelSourceGrouping)
							{

WriteLiteral("\t\t\t\t\t<option value=\"");


               Write(source.Key);

WriteLiteral("\" ");


                             Write(source.Key == Model.Source ? "selected" : "");

WriteLiteral(">");


                                                                            Write(source.Value);

WriteLiteral("</option>    \r\n");


							}
						}

						if (Model.SourceCollection.SourceList != null && Model.SourceCollection.SourceList.Count > 0)
						{

WriteLiteral("\t\t\t\t\t<optgroup label=\"");


                 Write(DJ.Token("savedLists"));

WriteLiteral("\">\r\n");


  						
							foreach (var source in Model.SourceCollection.SourceList)
							{

WriteLiteral("\t\t\t\t\t\t\t<option value=\"");


                 Write(source.Key);

WriteLiteral("\" ");


                               Write(source.Key == Model.Source ? "selected" : "");

WriteLiteral(">");


                                                                              Write(source.Value);

WriteLiteral("</option>    \r\n");


							}
						

WriteLiteral("\t\t\t\t\t</optgroup>\r\n");


						}
					}
				

WriteLiteral("\t\t\t</select>\r\n\t\t\t<select id=\"dateRange\" name=\"dateRange\" class=\"dj_selectbox dj_s" +
"electbox-small\">\r\n");


  				
					foreach (var dateRange in Model.DateRangeSelections)
					{

WriteLiteral("\t\t\t\t\t<option value=\"");


               Write(dateRange.Value);

WriteLiteral("\" ");


                                  Write(dateRange.Selected ? "selected" : "");

WriteLiteral(">");


                                                                         Write(dateRange.Text);

WriteLiteral("</option>    \r\n");


					}
				

WriteLiteral("\t\t\t</select>\r\n\t\t</div>\r\n\t\t<div class=\"search-bar\">\r\n\t\t\t");


Write(Html.TextBox("freeText", Model.FreeText, new { @class = "search-field" }));

WriteLiteral("\r\n\t\t\t<div class=\"search-buttons\">\r\n\t\t\t\t<input type=\"reset\" class=\"search-clear\" v" +
"alue=\"");


                                               Write(DJ.Token("clear"));

WriteLiteral("\" />\r\n\t\t\t\t<input type=\"submit\" class=\"search-submit\" value=\"");


                                                 Write(DJ.Token("search"));

WriteLiteral("\" />\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t\t<div class=\"save-options\">\r\n\t\t\t<span class=\"dj_btn d" +
"j_btn-lrg dj_btn-gray dj_btn-rounded-square save-as_btn\">\r\n\t\t\t\t");


Write(DJ.Token("saveAs"));

WriteLiteral("\r\n\t\t\t\t<span class=\"dj_btn-down-arrow\"></span></span>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n" +
"<div class=\"suggestions\">\r\n");


 	if (Model.SpellCorrection != null)
 {

WriteLiteral("\t\t<div class=\"dj_keyword-spellcheck ui-component\">\r\n\t\t\t<span class=\"title\">");


                  Write(DJ.Token("keywordSpellcheck"));

WriteLiteral("</span> <span class=\"replacement-text\">");


                                                                                       Write(Model.SpellCorrection);

WriteLiteral("</span>\r\n\t\t\t<span class=\"dj_close-section dj_icon dj_icon-close-dark-gray\"></span" +
">\r\n\t\t</div>\r\n");


 }


 	if (Model.HasRecognizedEntities)
 {
		
Write(ComponentFactory.Render(Model.DidYouMean));

                                            
		
Write(Html.Hidden("hideDYM", Model.DidYouMean.HideEntities.ToString().ToLower()));

                                                                             
 }

WriteLiteral("</div>\r\n<!-- Save As Menu -->\r\n<div class=\"menu saveAsMenu\" style=\"display: none;" +
"\">\r\n\t<div class=\"menuitems\">\r\n");


  		
			foreach (SelectListItem menu in Model.SaveOptions)
			{

WriteLiteral("\t\t\t<div class=\"menuitem\">\r\n\t\t\t\t<div class=\"label\" data-saveas=\"");


                               Write(menu.Value);

WriteLiteral("\">");


                                            Write(menu.Text);

WriteLiteral("</div>\r\n\t\t\t</div>\r\n");


			}
		

WriteLiteral("\t</div>\r\n</div>\r\n<!-- Save As Menu -->\r\n");


        }
    }
}
