﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.Search.UI.Components.Results.Headlines.resources.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.Search.UI.Components.Results.Headlines.HeadlineSearchResults.js", "text/javascript")]

namespace DowJones.Web.Mvc.Search.UI.Components.Results.Headlines
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Web.Mvc.UI.Components.PostProcessing;
    using DowJones.Web.Mvc.Search.Results;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 12/06/2011 04:40 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Results.Headlines.resources.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Results.Headlines.HeadlineSearchResultsComponent))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.Search.UI.Components.Results.Headlines.HeadlineSearchResults.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.Search.UI.Components.Results.Headlines.HeadlineSearchResultsComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class HeadlineSearchResultsComponent : DowJones.Web.Mvc.UI.ViewComponentBase<HeadlineSearchResults>
    {
#line hidden

        public HeadlineSearchResultsComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_HeadlineSearchResults";
            }
        }
        public override void ExecuteTemplate()
        {









  
    Html.DJ().ScriptRegistry()
        .WithJQueryUIWidgets();


WriteLiteral("\r\n");



WriteLiteral("\r\n<input type=\'hidden\' name=\"articleDisplayOption\" value=\'");


                                                   Write(Model.ArticleDisplayOption);

WriteLiteral("\' />\r\n<input type=\'hidden\' name=\'HighlightString\' value=\'");


                                              Write(Model.CanonicalQueryString);

WriteLiteral("\' />\r\n<input type=\'hidden\' name=\'pictureSize\' value=\'");


                                          Write(Model.PictureSize);

WriteLiteral("\' />\r\n\r\n");



WriteLiteral("\r\n\r\n<input type=\'hidden\' name=\'StartIndexReference\' value=\'");


                                                  Write(String.Join(",", Model.FirstResultIndexReference));

WriteLiteral("\' />\r\n<input type=\'hidden\' name=\'previousIndex\' value=\'");


                                            Write(Model.PreviousResultIndex);

WriteLiteral("\' />\r\n<input type=\'hidden\' name=\'nextIndex\' value=\'");


                                        Write(Model.NextResultIndex);

WriteLiteral("\' />\r\n\r\n<div class=\"columns clearfix\">\r\n\t\t\t\t\r\n\t<div class=\"column headlines\">\t\r\n\t" +
"<!-- RELATED CONCEPTS COMPONENT -->\r\n");


 	if(!Model.HideRelatedConcepts && Model.RelatedConcepts != null )
 {

WriteLiteral("\t    <div class=\"dj_related-concepts ui-component clearfix\">\r\n\t\t    <div class=\"l" +
"ist-wrap\">\r\n\t\t\t    <h2>");


      Write(DJ.Token( "relatedConcepts" ));

WriteLiteral("</h2>\r\n\t\t\t    ");


  Write(DJ.Render( Model.RelatedConcepts ));

WriteLiteral("\r\n\t\t    </div>\r\n\t\t    <span class=\"dj_icon dj_close-section dj_icon-close-dark-gr" +
"ay\"></span>        \r\n\t    </div>\r\n");


     
 }


 	if (!Model.HideNewsVolume && Model.NewsVolume != null)
 {

WriteLiteral("\t\t<div class=\"dj_DateNavigator ui-component\">\r\n\t\t\t<span class=\"dj_icon dj_close-s" +
"ection dj_icon-close-dark-gray\"></span>\r\n\t\t\t<h2>");


  Write(DJ.Token("newsVolumeByDate"));

WriteLiteral("</h2>\r\n\t\t\t");


Write(DJ.Render(Model.NewsVolume));

WriteLiteral("\r\n\t\t</div>\r\n");


        
 }

WriteLiteral("    ");


Write(Html.Hidden("hideRelatedConcepts", Model.HideRelatedConcepts.ToString().ToLower()));

WriteLiteral("\r\n    ");


Write(Html.Hidden("hideNewsVolume", Model.HideNewsVolume.ToString().ToLower()));

WriteLiteral("\r\n\r\n\t");


Write(Html.DJ().Render(Model.Headlines));

WriteLiteral(@"

    </div><!-- end: .column headlines -->
				
    <div class=""column articles"">

	    <div class=""dj_ArticleOptionsContainer dj_search-article-options"">
	        <div class=""controls"">
                <div class=""back-to-headlines"">
    		        <a class=""returnToHeadlines"" href=""javascript:void(0);"">");


                                                                 Write(Html.DJ().Token("returnToHeadlines"));

WriteLiteral("</a>\r\n        \t    </div><!-- end: .back-to-headlines -->\r\n\r\n\t\t        <div class" +
"=\"drop-down-button sort\">\r\n\t\t\t        <div class=\"selected-option\">\r\n\t\t\t\t       " +
" <span class=\"\">");


                      Write(Html.DJ().Token("options"));

WriteLiteral("</span> \r\n                        <span class=\"dj_icon dj_icon-d-arrow-g\"></span>" +
"\r\n\r\n\t\t\t\t        <div class=\"options sort-options dj_options\">\r\n\t\t\t\t\t        <lab" +
"el class=\"select-prompt\">");


                                     Write(Html.DJ().Token("selectOne"));

WriteLiteral(":</label>\r\n\t\t\t\t\t        ");


        Write(Html.RadioButtonList("articleDisplayOptions", Model.ArticleDisplayOptions, HtmlHelpers.ListType.Ordered, "select-options part"));

WriteLiteral("\r\n                            <input class=\"button dj_optionsOk\" type=\"button\" va" +
"lue=\"");


                                                                               Write(Html.DJ().Token("ok"));

WriteLiteral(@""">
				        </div><!-- end: .sort-options -->

			        </div><!-- end: .selected-option -->
		        </div><!-- end: .drop-down-button -->
		    </div><!-- end: .controls --> 
	    </div><!-- end: .dj_search-article-options -->

	    <div class=""dj_ArticleContainer dj_search-article-main""></div>

    </div><!-- end: .column articles -->
				
</div><!-- end: .columns -->

<!-- start: modals -->
<div class=""dj_modal simplemodal-container dj_entity-info"" style=""width:480px;height:207px;"">
	<div class=""dj_modal-header"">
		<h3 class=""dj_modal-title""><span class=""dj_entity-category""></span></h3>
		<p class=""dj_modal-close"" onclick=""$().overlay.hide('#'+$(this).closest('.dj_modal').attr('id'));"">
			<a href=""javascript:void(0);""></a>
		</p>
	</div>
	<div class=""dj_modal-content"" style=""height:156px"">
		<div class=""dj_modal-content-wrap"">
			<div class=""modal-content"">
				<h4 class=""dj_enity-name""></h4>
			</div>
		</div>
	</div>
</div>
<!-- end: modals -->
");


        }
    }
}
