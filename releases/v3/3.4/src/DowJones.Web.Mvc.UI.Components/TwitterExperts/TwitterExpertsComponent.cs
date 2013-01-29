﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TwitterExperts.TwitterExperts.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TwitterExperts.ClientTemplates.experts.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TwitterExperts.ClientTemplates.noData.htm", "text/html")]

namespace DowJones.Web.Mvc.UI.Components.TwitterExperts
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
    
    // Last Generated Timestamp: 08/15/2012 11:11 AM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TwitterExperts.TwitterExperts.js", DependsOn=new string[] {
            "ods-manager"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TwitterExperts.TwitterExpertsComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TwitterExperts.ClientTemplates.experts.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="experts", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TwitterExperts.TwitterExpertsComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TwitterExperts.ClientTemplates.noData.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="noData", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TwitterExperts.TwitterExpertsComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.27108")]
    public class TwitterExpertsComponent : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.SocialMedia.ExpertsModel>
    {
#line hidden

        public TwitterExpertsComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_TwitterExperts";
            }
        }
        public override void ExecuteTemplate()
        {






   
    CssClass = "dj_twitter-top-experts dj_TwitterExperts ";
    TagName = "ul";
 


 foreach (var expert in Model.Experts)
{

WriteLiteral("    <li data-user-id=\"");


                 Write(expert.Id);

WriteLiteral("\" data-screen-name=\"");


                                               Write(expert.ScreenName);

WriteLiteral("\" data-full-name=\"");


                                                                                   Write(expert.FullName);

WriteLiteral("\" class=\"dj_experts-item clearfix\">\r\n        <img class=\"expert-profile-link\" alt" +
"=\"");


                                         Write(expert.FullName);

WriteLiteral("\" src=\"");


                                                                Write(expert.ProfileImageUrl);

WriteLiteral("\" />\r\n        <div class=\"dj_author-meta\">\r\n            <a title=\"");


                 Write(expert.FullName);

WriteLiteral("\" href=\"");


                                         Write(expert.ProfileUrl);

WriteLiteral("\" class=\"dj_full-name\" target=\"_blank\">\r\n                ");


           Write(expert.FullName);

WriteLiteral(" </a><a class=\"dj_screen-name\" href=\"");


                                                                Write(expert.ProfileHashUrl);

WriteLiteral("\" target=\"_blank\">");


WriteLiteral("@");


                                                                                                          Write(expert.ScreenName);

WriteLiteral("</a>\r\n        </div>\r\n        <span class=\"dj_social-btn follow\"><span></span>");


                                                   Write(Html.DJ().Token("follow"));

WriteLiteral(" </span>\r\n    </li>\r\n");



}


        }
    }
}
