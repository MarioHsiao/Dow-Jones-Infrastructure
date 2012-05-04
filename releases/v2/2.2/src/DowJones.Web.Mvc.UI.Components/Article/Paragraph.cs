﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
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
    using System.Text;
    using DowJones.Infrastructure;
    using DowJones.Extensions;
    using DowJones.Ajax.Article;
    using DowJones.Web.Mvc.UI.Components.Article;
    using Factiva.Gateway.Messages.Archive.V2_0;
    using System.Text.RegularExpressions;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 05/04/2012 05:22 PM
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class Paragraph : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.Models.Article.ParagraphModel>
    {
#line hidden

        public Paragraph()
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
        case MarkUpType.PostProcessing:
            switch( renderItem.ItemPostProcessData.Type )
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
         case MarkUpType.ArticleElink:
            var elinkItemText = "";
            foreach (var elinkItem in renderItem.ElinkItems)
            {
                if (elinkItem.ItemMarkUp == MarkUpType.ArticleElinkHighlight)
                {elinkItemText = elinkItemText + "<span class='dj_article_highlight'>" + elinkItem.ItemText + "</span>";}
                else if (elinkItem.ItemMarkUp == MarkUpType.Plain)
                {elinkItemText = elinkItemText + elinkItem.ItemText;}
            }

WriteLiteral("            <a class=\"dj_article_elink\" href=\"javascript:void(0)\" data-href=\"");


                                                                        Write(renderItem.ItemValue.EscapeForHtml());

WriteLiteral("\">");


                                                                                                               Write(elinkItemText);

WriteLiteral("</a>\r\n");


             break;
             case MarkUpType.Plain:
                
            Write(renderItem.ItemText);

                                      
            break;
             case MarkUpType.EntityLink:
            if (renderItem.Highlight)
            {

WriteLiteral("                  <a class=\"dj_article_entity dj_article_highlight ");


                                                              Write(renderItem.ItemEntityData.Category);

WriteLiteral("\" href=\"javascript:void(0);\" data-entity=\"");


                                                                                                                                           Write(renderItem.ItemEntityData.ToJson().EscapeForHtml());

WriteLiteral("\">");


                                                                                                                                                                                                 Write(renderItem.ItemText);

WriteLiteral("</a>\r\n");



WriteLiteral("                  <a class=\"dj_article_entity ");


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

WriteLiteral("</a></span>                                                                      " +
"                                                                    ");


                                                                                                                                                                                                                                                                                                                                    break;
         case MarkUpType.Anchor:

WriteLiteral("            <span><a class=\"dj_article_anchor\" href=\"javascript:void(0);\" data-hr" +
"ef=\"");


                                                                                 Write(renderItem.ItemValue.EscapeForHtml());

WriteLiteral("\">");


                                                                                                                          Write(renderItem.ItemText);

WriteLiteral("</a></span>\r\n");


            break;
    }    
}          


        }
    }
}
