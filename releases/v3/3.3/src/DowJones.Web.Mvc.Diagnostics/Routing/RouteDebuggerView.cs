﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DowJones.Web.Mvc.Diagnostics.Routing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.Mvc.Html;
    using DowJones.Web.Mvc.Extensions;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0")]
    public class RouteDebuggerView : DowJones.Web.Mvc.UI.ViewComponentBase<RouteDebuggerViewModel>
    {
#line hidden

        public RouteDebuggerView()
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


WriteLiteral(@"<html>
<head>
    <title>Route Debugger</title>
    <style type='text/css'>
        table  
        {
            border-collapse: collapse;
        }
        
        td
        {
            font: 10pt Arial;
            border: solid 1px black;
            padding: 3px; 
        }
        
        .hilite 
        {
            background-color: lightgreen;
        }
    </style>
</head>
<body>
    <h1>
        Route Debugger</h1>
    <form action=''>
        ");


   Write(Html.Label("Pattern", "Pattern:"));

WriteLiteral("\r\n        ");


   Write(Html.TextBox("Pattern", Model.Url, new { size="60" }));

WriteLiteral("\r\n        ");


   Write(Html.DropDownList("HttpMethod", Model.HttpMethods, Model.HttpMethod));

WriteLiteral(@"
        <input type='submit' value='Debug' />
    </form>
    <table>
        <caption>Routes</caption>
        <thead>
            <tr>
                <th>Matches</th>
                <th>Pattern</th>
                <th>Defaults</th>
                <th>Constraints</th>
                <th>DataTokens</th>
                <th>Values</th>
            </tr>
        </thead>
        <tbody>
");


             foreach(var route in Model.Routes) {

WriteLiteral("                <tr class=\'");


                       Write(route.IsMatch ? "hilite" : string.Empty);

WriteLiteral("\'>\r\n                    <td>");


                    Write(route.IsMatch ? "Yes" : string.Empty);

WriteLiteral("</td>\r\n                    <td>");


                   Write(route.Url);

WriteLiteral("</td>\r\n                    <td>");


                   Write(route.Defaults);

WriteLiteral("</td>\r\n                    <td>");


                   Write(route.Constraints);

WriteLiteral("</td>\r\n                    <td>");


                   Write(route.DataTokens);

WriteLiteral("</td>\r\n                    <td>");


                   Write(route.Values);

WriteLiteral("</td>\r\n                </tr>\r\n");


            }

WriteLiteral("        </tbody>\r\n    </table>\r\n</body>\r\n</html>\r\n");


        }
    }
}