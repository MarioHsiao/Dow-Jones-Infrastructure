﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DowJones.Web.Mvc.UI.Components.Article
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Infrastructure;
    using DowJones.Extensions;
    using Ajax.Article;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 08/24/2012 02:20 PM
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class ParagraphComponent : DowJones.Web.Mvc.UI.ViewComponentBase<Article.ParagraphModel>
    {
#line hidden

        public ParagraphComponent()
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





  
    TagName = Model.TagName.IsNullOrEmpty() ? "p" : Model.TagName;
    CssClass = Model.ClassName;


WriteLiteral("\r\n");


 foreach(IRenderItem renderItem in Model.Items)
{
    switch(renderItem.ItemMarkUp){
        case MarkUpType.ArticleHighlight:

WriteLiteral("            <span class=\"dj_article_highlight\">");


                                           Write(renderItem.ItemText);

WriteLiteral("</span>\r\n");


            break;
        case MarkUpType.Image:

WriteLiteral("            <img alt=\"\" src=\"");


                         Write(renderItem.ItemValue);

WriteLiteral("\" />\r\n");


            break;
        case MarkUpType.ImageFigure:
                   if (!string.IsNullOrEmpty(renderItem.EnlargedImageUrl)){ 

WriteLiteral("                <div class=\"figure\"><div class=\"figImages\"><img alt=\"");


                                                                 Write(renderItem.Title.EscapeForHtml());

WriteLiteral("\" src=\"");


                                                                                                           Write(renderItem.ItemValue);

WriteLiteral("\" title=\"");


                                                                                                                                           Write(renderItem.Title.EscapeForHtml());

WriteLiteral("\" /><a class=\"dj_article_enlargeImg_link\" href=\"javascript:void(0)\" data-href=\"");


                                                                                                                                                                                                                                                             Write(renderItem.EnlargedImageUrl);

WriteLiteral("\">Enlarge</a></div><div class=\"figCredit\">");


                                                                                                                                                                                                                                                                                                                                     Write(renderItem.Credit);

WriteLiteral("</div><div class=\"figSource\">");


                                                                                                                                                                                                                                                                                                                                                                                      Write(renderItem.Source);

WriteLiteral("</div><div class=\"figCaption\">");


                                                                                                                                                                                                                                                                                                                                                                                                                                        Write(renderItem.Caption);

WriteLiteral("</div></div>\r\n");


                   }
                   else{

WriteLiteral("                <div class=\"figure\"><img alt=\"");


                                          Write(renderItem.Title.EscapeForHtml());

WriteLiteral("\" src=\"");


                                                                                    Write(renderItem.ItemValue);

WriteLiteral("\" title=\"");


                                                                                                                    Write(renderItem.Title.EscapeForHtml());

WriteLiteral("\" /><div class=\"figCredit\">");


                                                                                                                                                                                  Write(renderItem.Credit);

WriteLiteral("</div><div class=\"figSource\">");


                                                                                                                                                                                                                                   Write(renderItem.Source);

WriteLiteral("</div><div class=\"figCaption\">");


                                                                                                                                                                                                                                                                                     Write(renderItem.Caption);

WriteLiteral("</div></div>\r\n");


                   }
                   break;
        case MarkUpType.PostProcessing:
            switch (renderItem.ItemPostProcessData.Type)
            {
                case PostProcessing.Print:
WriteLiteral("<span class=\'dj_article_colorLinks\'>");


                                                                          Write(renderItem.ItemPostProcessData.ElinkValue);

WriteLiteral(" [");


                                                                                                                        Write(renderItem.ItemPostProcessData.ElinkText);

WriteLiteral("]</span>\r\n");


                     break;
                case PostProcessing.Save:
                                     Write(renderItem.ItemPostProcessData.ElinkValue);

                                                                                      
                                                                                  Write(renderItem.ItemPostProcessData.ElinkText);

                                                                                                                                 
                     break;
            }
            break;
        case MarkUpType.Html:

WriteLiteral("            <div class=\"embededHtml\">");


                                 Write(Html.Raw(renderItem.ItemText));

WriteLiteral("</div>\r\n");


            break;
        case MarkUpType.ArticleElink:
            var elinkItemText = "";
            if (renderItem.ElinkItems.Count > 0)
            {
                foreach (var elinkItem in renderItem.ElinkItems)
                {
                    if (elinkItem.ItemMarkUp == MarkUpType.ArticleElinkHighlight)
                    { elinkItemText = elinkItemText + "<span class='dj_article_highlight'>" + elinkItem.ItemText + "</span>"; }
                    else if (elinkItem.ItemMarkUp == MarkUpType.Plain)
                    { elinkItemText = elinkItemText + elinkItem.ItemText; }
                }
            }
            else { elinkItemText = renderItem.ItemText; }

WriteLiteral("            <a class=\"dj_article_elink\" href=\"javascript:void(0)\" data-href=\"");


                                                                        Write(renderItem.ItemValue.EscapeForHtml());

WriteLiteral("\">");


                                                                                                               Write(elinkItemText);

WriteLiteral("</a>\r\n");


            break;
        case MarkUpType.Plain:

WriteLiteral("            <span class=\"dj_article_plain\">");


                                       Write(renderItem.ItemText);

WriteLiteral("</span>\r\n");


            break;
        case MarkUpType.EntityLink:
            if (renderItem.Highlight){

WriteLiteral("                <a class=\"dj_article_entity dj_article_highlight ");


                                                            Write(renderItem.ItemEntityData.Category);

WriteLiteral("\" href=\"javascript:void(0);\" data-entity=\"");


                                                                                                                                         Write(renderItem.ItemEntityData.ToJson().EscapeForHtml());

WriteLiteral("\">");


                                                                                                                                                                                               Write(renderItem.ItemText);

WriteLiteral("</a>\r\n");


            }
            else { 

WriteLiteral("                <a class=\"dj_article_entity ");


                                       Write(renderItem.ItemEntityData.Category);

WriteLiteral("\" href=\"javascript:void(0);\" data-entity=\"");


                                                                                                                    Write(renderItem.ItemEntityData.ToJson().EscapeForHtml());

WriteLiteral("\">");


                                                                                                                                                                          Write(renderItem.ItemText);

WriteLiteral("</a>\r\n");


            }
            break;
        case MarkUpType.SpanAnchor:

WriteLiteral("            <span><a class=\"dj_article_entity\" href=\"javascript:void(0);\" data-en" +
"tity=\"");


                                                                                   Write(renderItem.ItemEntityData.ToJson().EscapeForHtml());

WriteLiteral("\">");


                                                                                                                                          Write(renderItem.ItemEntityData.Name);

WriteLiteral("</a></span>\r\n");


            break;                                                                                                                              break;
        case MarkUpType.Anchor:

WriteLiteral("            <span><a class=\"dj_article_anchor\" href=\"javascript:void(0);\" data-hr" +
"ef=\"");


                                                                                 Write(renderItem.ItemValue.EscapeForHtml());

WriteLiteral("\">");


                                                                                                                          Write(renderItem.ItemText);

WriteLiteral("</a></span>\r\n");


            break;                                                                                                              break;
        case MarkUpType.Unknown:

WriteLiteral("            <span><a class=\"dj_article_accessionNum\" href=\"javascript:void(0);\" d" +
"ata-accessionNum=\"");


                                                                                               Write(renderItem.ItemValue.EscapeForHtml());

WriteLiteral("\">");


                                                                                                                                        Write(renderItem.ItemText);

WriteLiteral("</a></span>\r\n");


            break;
     }
}
WriteLiteral("     ");


        }
    }
}
