﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.208
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.PortalArticle.PortalArticle.css", "text/css", PerformSubstitution=true)]

namespace DowJones.Web.Mvc.UI.Components.PortalArticle
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
    
    // Last Generated Timestamp: 03/04/2011 09:58 AM
    [DowJones.Web.StylesheetResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.PortalArticle.PortalArticle.css", PerformSubstitution=true, ResourceKind=DowJones.Web.ClientResourceKind.Stylesheet, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.PortalArticle.PortalArticleControl))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class PortalArticleControl : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.PortalArticle.PortalArticleModel>
    {
#line hidden

        public PortalArticleControl()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_PortalArticle";
            }
        }
        public override void ExecuteTemplate()
        {



WriteLiteral(@"
<div class=""articleContainer"" style=""width:560px; height:642px;"">
    <div class=""jspPane"" style=""padding:0px; top:0px; width:539px"" >
        <div id=""dj_modal-article-content-wrap"">
            <div id=""dj_modal-article-meta"">
                <span class=""article-meta-source"">");


                                             Write(Model.ArticleObject.sourceName);

WriteLiteral("</span> - <span class=\"article-meta-date\">");


                                                                                                                      Write(Model.ArticleObject.publicationDate.ToLongDateString());

WriteLiteral("</span>\r\n            </div>\r\n            <h4 id=\"dj_modal-article-title\">\r\n      " +
"          <a href=\"#\">");


                       Write(Model.RenderArticleTitle());

WriteLiteral("</a>\r\n            </h4>\r\n            <div id=\"dj_modal-article-body\">\r\n          " +
"      ");


           Write(Model.RenderArticleParagraph());

WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");


        }
    }
}
