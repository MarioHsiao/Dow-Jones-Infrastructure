﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Factiva.Currents.Components.CurrentsHeadline.CurrentsHeadline.js", "text/javascript")]

namespace DowJones.Factiva.Currents.Components.CurrentsHeadline
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using CurrentsHeadline;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 12/06/2012 01:26 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Factiva.Currents.Components.CurrentsHeadline.CurrentsHeadline.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Factiva.Currents.Components.CurrentsHeadline.CurrentsHeadlineComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class CurrentsHeadlineComponent : DowJones.Web.Mvc.UI.ViewComponentBase<CurrentsHeadlineModel>
    {
#line hidden

        public CurrentsHeadlineComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_CurrentsHeadline";
            }
        }
        public override void ExecuteTemplate()
        {





   CssClass = "dj_CurrentsHeadline"; 

WriteLiteral("\r\n<ul class=\"dj_CurrentsHeadline items\">\r\n");


 	if (Model.HasData)
	{
		foreach (var headline in Model.Headlines)
		{

WriteLiteral("\t\t\t<li class=\"dj_entry ");


                  Write(Model.GetSelectionStatus(headline));

WriteLiteral(" ");


                                                      Write(headline.ContentCategoryDescriptor);

WriteLiteral(" ");


                                                                                          Write(headline.ContentSubCategoryDescriptor);

WriteLiteral("\">\r\n\t\t\t\t<div class=\"article-wrap\">\r\n\t\t\t\t\t<h4 class=\"article-headline\">\r\n\t\t\t\t\t    " +
"<a href=\"");


             Write(Model.GetHeadlineUrl(headline, Url));

WriteLiteral("\" class=\"article-view-trigger\" target=\"_blank\">");


                                                                                                 Write(headline.TruncatedTitle ?? headline.Title);

WriteLiteral("</a>\r\n\t\t\t\t\t</h4>\r\n");


 					if (Model.ShouldShowSource(headline) || Model.ShouldShowPublicationDateTime(headline))
					{

WriteLiteral("\t\t\t\t\t\t<div class=\"article-meta\">\r\n");


 							if (Model.ShouldShowSource(headline))
							{
								if (Model.SourceClickable)
								{

WriteLiteral("\t\t\t\t\t\t\t\t\t<span class=\"article-source source-clickable\" rel=\"");


                                                       Write(headline.SourceCode);

WriteLiteral(" \">");


                                                                              Write(headline.SourceDescriptor);

WriteLiteral(" </span>\r\n");


								}
								else
								{

WriteLiteral("\t\t\t\t\t\t\t\t\t<span class=\"article-source\">");


                                 Write(headline.SourceDescriptor);

WriteLiteral(" </span>\r\n");


								}
							} 

WriteLiteral("                            </div>\r\n");


         
							if (Model.ShouldShowPublicationDateTime(headline))
							{

WriteLiteral("                                <div class=\"article-meta\">\r\n\t\t\t\t\t\t\t\t    <span cla" +
"ss=\"date-stamp\">");


                                 Write(headline.PublicationDateDescriptor != null ?
                                        headline.PublicationDateDescriptor :headline.PublicationDateTimeDescriptor);

WriteLiteral(" </span>\r\n                                </div>\r\n");


                            }
              
                            if (Model.ShouldShowAuthor(headline))
							{

WriteLiteral("                                  <div class=\"article-meta\">\r\n");


                                 if (@headline.CodedAuthors != null)
                                {
                                   
                                      for (int j = 0, jCnt = @headline.CodedAuthors.Count; j < jCnt; j++)
                                      {
                                            var author = @headline.CodedAuthors[j];
                                            for (int k = 0, kcnt = @author.items.Count; k < kcnt; k++)
                                            {
                                                var item = @author.items[k];
                                                switch (@item.EntityType.ToString().ToLower())
                                                {
                                                    case "person":
                                                    case "author":

WriteLiteral("                                                        <span class=\"article-auth" +
"or article-clickable\">");


                                                                                                  Write(item.value);

WriteLiteral("</span>\r\n");


                                                         break;
                                                    case "textual":
                                                    default:

WriteLiteral("                                                        <span class=\"article-auth" +
"or\">");


                                                                                Write(item.value);

WriteLiteral("</span>\r\n");


                                                        break;
                                                }
                                            }
                                      }
                                }
                                else
                                {

WriteLiteral("                                    <span class=\"author\">");


                                                    Write(headline.Authors.Aggregate((a,b)=> a+","+b));

WriteLiteral("</span>\r\n");


                                } 

WriteLiteral("                                          </div>\r\n");


							}
                            if (Model.IsMultimediaContent(headline))
                            {

WriteLiteral("                               <div class=\"article-meta\">\r\n");


                                    if(@headline.ContentSubCategoryDescriptor == "video")
                                   { 

WriteLiteral("                                       <span class=\"fi fi_video\"> </span> \r\n");


                                   }
                                   else if(headline.ContentSubCategoryDescriptor == "audio")
                                   {

WriteLiteral("                                        <span class=\"fi fi_audio\"> </span>\r\n");


                                   }

WriteLiteral("                                \r\n                                <span class=\"me" +
"dia-length\">[");


                                                       Write(headline.MediaLength);

WriteLiteral("]</span>\r\n                            </div>\r\n");


                            }
					}

WriteLiteral("\t\t\t\t</div>\r\n\t\t\t</li>\r\n");


		}
	}
	else
		  {

WriteLiteral("\t\t\t  <li><span class=\"dj_noResults\">");


                               Write(DJ.Token("noResults"));

WriteLiteral("</span></li>\r\n");


		  }

WriteLiteral("</ul>");


        }
    }
}
