﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.EntityList.EntityList.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.EntityList.ClientTemplates.Entity.htm", "text/html")]

namespace DowJones.Web.Mvc.UI.Components.EntityList
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
    
    // Last Generated Timestamp: 06/29/2012 11:29 AM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.EntityList.EntityList.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.EntityList.EntityListComponent))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.EntityList.ClientTemplates.Entity.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="entity", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.EntityList.EntityListComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class EntityListComponent : DowJones.Web.Mvc.UI.ViewComponentBase<EntityListModel>
    {
#line hidden

        public EntityListComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_EntityList";
            }
        }
        public override void ExecuteTemplate()
        {





   
    CssClass = "dj_EntityList dj_pill-list yellow clearfix js_pill-list";
    TagName = HtmlTextWriterTag.Ul.ToString();


WriteLiteral("\r\n");


 foreach (var group in Model.Groups)
{

WriteLiteral("    <li class=\"pill-group\" data-group=\"");


                                  Write(group.Category);

WriteLiteral("\">\r\n        \r\n        <span class=\"section-category\">");


                                  Write(DJ.Token(group.Category));

WriteLiteral(":</span>\r\n        \r\n        <ul class=\"pill-list\">\r\n\r\n");


         foreach (var entity in group.Entities)
        {
                        

WriteLiteral("            <li class=\"dj_pill\" data-code=\"");


                                      Write(entity.Code);

WriteLiteral("\" data-name=\"");


                                                               Write(entity.Name);

WriteLiteral("\">\r\n                <span class=\"entity-name\">");


                                     Write(entity.Name);

WriteLiteral("</span>\r\n                <span class=\"remove\"></span>\r\n            </li>\r\n");


                        
        }

WriteLiteral("\r\n        </ul><!-- end: .pill-list -->\r\n\r\n    </li>");



WriteLiteral("<!-- end: .pill-group -->\r\n");


}


        }
    }
}