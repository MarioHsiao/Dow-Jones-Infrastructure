﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SimpleAlert.ClientTemplates.Options.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SimpleAlert.ClientTemplates.NewsFilter.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.SimpleAlert.SimpleAlert.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.SimpleAlert
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
    
    // Last Generated Timestamp: 12/09/2011 10:55 AM
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SimpleAlert.ClientTemplates.Options.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="options", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SimpleAlert.SimpleAlertControl))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SimpleAlert.ClientTemplates.NewsFilter.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="newsFilter", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SimpleAlert.SimpleAlertControl))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.SimpleAlert.SimpleAlert.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.SimpleAlert.SimpleAlertControl))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class SimpleAlertControl : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.Models.SimpleAlertModel>
    {
#line hidden

        public SimpleAlertControl()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_SimpleAlert";
            }
        }
        public override void ExecuteTemplate()
        {







   
    CssClass = "dj_SimpleAlert";
    Html.DJ().ScriptRegistry().WithUnderscoreJS();


WriteLiteral("<div>\r\n    <div class=\"title\"></div>\r\n    <table cellspacing=\"0\" cellpadding=\"0\" " +
"width=\"100%\">\r\n        <tbody>\r\n            <tr>\r\n                <td class=\"fir" +
"st\">");


                             Write(Html.DJ().Token("enterFldrName"));

WriteLiteral(":\r\n                </td>\r\n                <td>\r\n                    <input type=\"" +
"text\" class=\"folderName\" maxlength=\"25\" />\r\n                </td>\r\n            <" +
"/tr>\r\n            <tr>\r\n                <td class=\"first\">");


                             Write(Html.DJ().Token("sources"));

WriteLiteral(":\r\n                </td>\r\n                <td>\r\n                    <select class" +
"=\"sourceList\">\r\n                    </select>\r\n                </td>\r\n          " +
"  </tr>\r\n            <tr>\r\n                <td class=\"first\">");


                             Write(Html.DJ().Token("searchCriteria"));

WriteLiteral(@":
                </td>
                <td>
                    <input type=""text"" size=""30"" maxlength=""500"" class=""searchText"" />
                </td>
            </tr>
            <tr class=""newsFilter"">
                <td class=""first"">
                    <span class=""newsFilter"">");


                                        Write(Html.DJ().Token("newsFilters"));

WriteLiteral("</span>\r\n                </td>\r\n                <td></td>\r\n            </tr>\r\n   " +
"         <tr>\r\n                <td class=\"first\">");


                             Write(Html.DJ().Token("enterEmail"));

WriteLiteral(":\r\n                </td>\r\n                <td>\r\n                    <input type=\"" +
"text\" disabled=\"disabled\" class=\"emailAddress\" />\r\n                </td>\r\n      " +
"      </tr>\r\n            <tr>\r\n                <td class=\"first\">");


                             Write(Html.DJ().Token("emailFmt"));

WriteLiteral(":\r\n                </td>\r\n                <td>\r\n                    <select class" +
"=\"emailFormat\">\r\n                    </select>\r\n                </td>\r\n         " +
"   </tr>\r\n            <tr>\r\n                <td class=\"first\">");


                             Write(Html.DJ().Token("selectDeliveryTime"));

WriteLiteral(":\r\n                </td>\r\n                <td>\r\n                    <select class" +
"=\"deliveryTime\">\r\n                    </select>\r\n                </td>\r\n        " +
"    </tr>\r\n            <tr>\r\n                <td class=\"first\">");


                             Write(Html.DJ().Token("removeDuplicateArticles"));

WriteLiteral(@":
                </td>
                <td>
                    <select class=""removeDuplicate"">
                    </select>
                </td>
            </tr>
        </tbody>
    </table>
    <div class=""buttons"">
        <ul class=""dc_list"">
            <li class=""dc_item""><a href=""javascript:void(0);"" class=""dashboard-control dc_btn dc_btn-3"">");


                                                                                                   Write(Html.DJ().Token("cancel"));

WriteLiteral("</a>\r\n            </li>\r\n            <li class=\"dc_item\"><a href=\"javascript:void" +
"(0);\" class=\"dashboard-control dc_btn dc_btn-1\">");


                                                                                                   Write(Html.DJ().Token("startTracking"));

WriteLiteral("</a>\r\n            </li>\r\n        </ul>\r\n        <div class=\"clear\"></div>\r\n    </" +
"div>\r\n</div>\r\n");


        }
    }
}
