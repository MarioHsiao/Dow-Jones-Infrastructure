﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.PersonalizationFilters.ClientTemplates.main.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.PersonalizationFilters.ClientTemplates.modalDialog" +
    ".htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.PersonalizationFilters.ClientTemplates.filterPill." +
    "htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.PersonalizationFilters.PersonalizationFilters.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.PersonalizationFilters
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
    using DowJones.Web.Mvc.UI.Components.Models;
    using DowJones.Web.Mvc.UI.Components.TaxonomySearchBrowse;
    using DowJones.Web.Mvc.UI.Components.AutoSuggest;
    using DowJones.Web.Mvc.UI.Components.PersonalizationFilters;
    
    // Last Generated Timestamp: 10/12/2011 04:11 PM
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.PersonalizationFilters.ClientTemplates.main.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="main", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.PersonalizationFilters.PersonalizationFilters))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.PersonalizationFilters.ClientTemplates.modalDialog" +
        ".htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="modalDialog", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.PersonalizationFilters.PersonalizationFilters))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.PersonalizationFilters.ClientTemplates.filterPill." +
        "htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="filterPill", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.PersonalizationFilters.PersonalizationFilters))]
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.PersonalizationFilters.PersonalizationFilters.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.PersonalizationFilters.PersonalizationFilters))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.9.3.26911")]
    public class PersonalizationFilters : DowJones.Web.Mvc.UI.ViewComponentBase<PersonalizationFiltersModel>
    {
#line hidden

        public PersonalizationFilters()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_PersonalizationFilters";
            }
        }
        public override void ExecuteTemplate()
        {












   
    CssClass = "dj_PersonalizationFilters";
    DJ.ScriptRegistry().WithJQueryUIWidgets().WithServiceProxy();
    DJ.AutoSuggest();


WriteLiteral("<div style=\"display: none;\">\r\n    ");


Write(CreateChildControl<TaxonomySearchBrowse>(Model.TaxonomySearch));

WriteLiteral("\r\n</div>\r\n");


        }
    }
}
