﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.CompanySparkline.CompaniesSparklinesComponent.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.CompanySparkline
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Web.Mvc.UI.Components.Models.CompanySparkline;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 05/09/2012 10:33 AM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.CompanySparkline.CompaniesSparklinesComponent.js", DependsOn=new string[] {
            "DowJones.Web.Mvc.UI.Components.CompanySparkline.CompanySparkline"}, ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.CompanySparkline.CompaniesSparklinesComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class CompaniesSparklinesComponent : CompositeComponent<CompaniesSparklinesComponentModel>
    {
#line hidden

        public CompaniesSparklinesComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_CompaniesSparklinesComponent";
            }
        }
        public override void ExecuteTemplate()
        {





   
    CssClass = "dj_CompaniesSparklines dj_company-sparklines";
    Html.DJ().ScriptRegistry().WithHighCharts();


WriteLiteral("\r\n");


        }
    }
}
