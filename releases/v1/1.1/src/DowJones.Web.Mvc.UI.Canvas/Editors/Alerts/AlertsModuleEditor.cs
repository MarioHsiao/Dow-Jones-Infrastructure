﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.ClientTemplates.UserAlertsList.htm", "text/html", PerformSubstitution=true)]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.ClientTemplates.PublishAlertsOverlay.ht" +
    "m", "text/html", PerformSubstitution=true)]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.AlertsModuleEditor.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Canvas.Editors.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Web.Mvc.UI.Canvas;
    using DowJones.Web.Mvc.UI.Canvas.Properties;
    using DowJones.Web.Mvc.UI.Canvas.Editors.Alerts;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 04/22/2011 11:22 AM
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.ClientTemplates.UserAlertsList.htm", PerformSubstitution=true, ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="DJ.UI.AlertsModuleEditor.prototype.templates.userAlertsList", DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.AlertsModuleEditor))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.ClientTemplates.PublishAlertsOverlay.ht" +
        "m", PerformSubstitution=true, ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="DJ.UI.AlertsModuleEditor.prototype.templates.publishAlertsOverlay", DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.AlertsModuleEditor))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.AlertsModuleEditor.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Canvas.Editors.Alerts.AlertsModuleEditor))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.9.3.26911")]
    public class AlertsModuleEditor : AbstractCanvasModuleEditor<AlertsModuleEditor>
    {
#line hidden

        public AlertsModuleEditor()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_AlertsModuleEditor";
            }
        }
        public override void ExecuteTemplate()
        {









    Html.DJ().ScriptRegistry().WithUnderscoreJS();
    CssClass = "dj_Editor dj_AlertsModuleEditor"; 


WriteLiteral("<div class=\"edit-details dj_Edit_PageDescription\">\r\n</div>\r\n<div class=\"dj_edit-c" +
"ontent clearfix\">\r\n    <div class=\"dj_edit-col\">\r\n        <div class=\"dj_edit-in" +
"put-wrap\">\r\n            <label for=\"\" class=\"dj_edit-label\">\r\n                ");


           Write(Html.DJ().Token("moduleName"));

WriteLiteral(@"</label>
            <div class=""dj_text-element-wrapper"">
                <input type=""text"" id=""txtModuleName"" maxlength=""25""></div>
        </div>
        <div class=""dj_edit-input-wrap"">
            <label for="""" class=""dj_edit-label"">
                ");


           Write(Html.DJ().Token("description"));

WriteLiteral("</label>\r\n            <span class=\"dj_max-char-note\">(");


                                       Write(Html.DJ().Token("max250Chars"));

WriteLiteral(")</span>\r\n            <div class=\"dj_text-element-wrapper\">\r\n                <tex" +
"tarea id=\"txtDescription\"></textarea></div>\r\n        </div>\r\n    </div>\r\n    <di" +
"v class=\"dj_edit-col two-thirds\">\r\n        <label for=\"\" class=\"dj_edit-label\">\r" +
"\n            ");


       Write(Html.DJ().Token("alerts"));

WriteLiteral("</label>\r\n        <div class=\"dj_edit-lists-overflow\">\r\n            <span class=\"" +
"dj_edit-list-empty-msg\">");


                                            Write(Html.DJ().Token("noAlertsInModule"));

WriteLiteral("</span>\r\n            <ul class=\"dj_edit-lists ui-sortable\" id=\"dj_alert_principal" +
"_list\" style=\"\">\r\n            </ul>\r\n        </div>\r\n        <div class=\"dj_edit" +
"-lists-add-controls\">\r\n            <div class=\"list-message hide\">\r\n            " +
"    ");


           Write(Html.DJ().Token("maxAlertsInModule"));

WriteLiteral("\r\n            </div>\r\n            <div class=\"add-new-list-item\">\r\n              " +
"  <label for=\"\" class=\"dj_edit-label\">\r\n                    ");


               Write(Html.DJ().Token("selectAlertForModule"));

WriteLiteral("\r\n                </label>\r\n                <select class=\"user-alerts-list\" id=\"" +
"ddlAlerts\">\r\n                    <option value=\"Select an alert\" class=\"default-" +
"option\">");


                                                                      Write(Html.DJ().Token("selectAnAlert"));

WriteLiteral("</option>\r\n                </select>\r\n                <ul class=\"dc_list\">\r\n     " +
"               <li class=\"dc_item\"><a class=\"dashboard-control dc_btn dc_btn-2 d" +
"c_btn-add\">");


                                                                                           Write(Html.DJ().Token("add"));

WriteLiteral("</a>\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n     " +
"   </div>\r\n    </div>\r\n</div>\r\n<div class=\"modalDialog dj_modal alertsOverlay \">" +
"\r\n    <div class=\"modalHeader\">\r\n        <h3 class=\"modalTitle\">");


                          Write(Html.DJ().Token("createUpdateModule"));

WriteLiteral(@"</h3>
        <p class=""modalClose"">
            <a href=""javascript:void(0);""></a>
        </p>
    </div>
    <div class=""modalContent sm-indent"">
    </div>
    <div class=""dj_form-submit-buttons"">
        <ul class=""dc_list"" id=""dj_modal-controls"">
            <li class=""dc_item""><a href=""javascript:void(0)"" class=""dj_modal-proceed dashboard-control dc_btn dc_btn-1"">");


                                                                                                                   Write(Html.DJ().Token("proceed"));

WriteLiteral("</a> </li>\r\n            <li class=\"dc_item\"><a href=\"javascript:void(0)\" class=\"d" +
"j_modal-close dashboard-control dc_btn dc_btn-3\">");


                                                                                                                 Write(Html.DJ().Token("cancel"));

WriteLiteral("</a> </li>\r\n        </ul>\r\n    </div>\r\n\r\n</div>\r\n\r\n");


        }
    }
}
