// ExportChart.cs
// Tek4.Highcharts.Exporting.ExportChart class.
// Tek4.Highcharts.Exporting assembly.
// ==========================================================================
// <summary>
// Processes web requests to export Highcharts JS JavaScript charts.
// </summary>
// ==========================================================================
// Author: Kevin P. Rice, Tek4(TM) (http://Tek4.com/)
//
// Based upon ASP.NET Highcharts export module by Clément Agarini
//
// Copyright (C) 2011 by Tek4(TM) - Kevin P. Rice
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// REVISION HISTORY:
// 2011-07-17 KPR Created.
// 

using System;
using System.Web;
using DowJones.Charting.Properties;
using DowJones.Generators;
using DowJones.Infrastructure;
using DowJones.Session;

namespace DowJones.Charting.Highcharts
{
    /// <summary>
  /// Processes web requests to export Highcharts JS JavaScript charts.
  /// </summary>
  internal static class ExportChart
  {
    /// <summary>
    /// Processes HTTP Web requests to export SVG.
    /// </summary>
    /// <param name="context">An HttpContext object that provides references 
    /// to the intrinsic server objects (for example, Request, Response, 
    /// Session, and Server) used to service HTTP requests.</param>
    internal static void ProcessExportRequest(HttpContext context)
    {
        if (context == null || context.Request.HttpMethod != "POST")
        {
            return;
        }

        var request = context.Request.Form;

        // Get HTTP POST form variables, ensuring they are not null.
        var filename = request["filename"];
        var type = request["type"];
        int width;
        var svg = request["svg"];

        Int32.TryParse(request["width"], out width);
        Guard.IsNotNullOrEmpty(type, "type");
        Guard.IsNotNullOrEmpty(svg, "svg");
        Guard.IsNotZeroOrNegative(width, "width");
        Guard.IsNotNullOrEmpty(filename, "filename");

        // Create a new chart export object using form variables.
        var export = new Exporter(type, width, svg, filename);

        // Write the exported chart to the HTTP Response object.
        export.WriteToHttpResponse(context.Response);

        // Short-circuit this ASP.NET request and end. Short-circuiting
        // prevents other modules from adding/interfering with the output.
        HttpContext.Current.ApplicationInstance.CompleteRequest();
        context.Response.End();
    }

    internal static string SaveImageRequest(HttpContext context)
    {
        if (context == null || context.Request.HttpMethod != "POST")
        {
            if (context != null)
            {
                throw new ApplicationException(string.Format("Invalid HttpMethod specified: '{0}'.", context.Request.HttpMethod));
            }
            throw new ApplicationException("Bad Request");
        }

        var request = context.Request.Form;
        var svg = request["svg"];
        var type = request["type"];
        int width;

        Int32.TryParse(request["width"], out width);

        Guard.IsNotNullOrEmpty(type, "type");
        Guard.IsNotNullOrEmpty(svg, "svg"); 
        Guard.IsNotZeroOrNegative(width, "width");

        var cacheKey = RandomKeyGenerator.GetRandomKey(16, RandomKeyGenerator.CharacterSet.AlphaNumeric);

        return string.Empty;
    }

    internal static void ProcessImageRequest(HttpContext context)
    {
        var request = context.Request.QueryString;
        var type = request["type"];
        int width;
        var cacheKey = request["cachekey"];

        Int32.TryParse(request["width"], out width);
        Guard.IsNotNullOrEmpty(type, "type");
        Guard.IsNotNullOrEmpty(cacheKey, "cacheKey");
        Guard.IsNotZeroOrNegative(width, "width");

        // Create a new chart export object using querystring parameters.
        var export = new Exporter(type, width, cacheKey);

        // Write the exported chart to the HTTP Response object.
        export.WriteToHttpResponse(context.Response);

        // Short-circuit this ASP.NET request and end. Short-circuiting
        // prevents other modules from adding/interfering with the output.
        HttpContext.Current.ApplicationInstance.CompleteRequest();
        context.Response.End();
    }

    private static IControlData GetControlData(HttpContext context)
    {
        var request = context.Request;
        var cd = new ControlData
                     {
                         EncryptedToken = Settings.Default.ExporterEncryptedToken,
                         AccessPointCode = request["apc"] ?? "o",
                         AccessPointCodeUsage = request["apc"],
                         ClientCode = request["clientcode"] ?? DowJones.Properties.Settings.Default.DefaultClientCodeType,
                     };

        return cd;
    }
  }
}