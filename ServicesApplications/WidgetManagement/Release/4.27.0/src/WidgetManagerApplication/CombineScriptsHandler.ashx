<%@ WebHandler Language="C#" Class="CombineScriptsHandler" %>

using System;
using System.Web;
using EMG.widgets.ui.core;

public class CombineScriptsHandler : IHttpHandler
{
    /// <summary>
    /// ProcessRequest implementation outputs the combined script file
    /// </summary>
    /// <param name="context"></param>
    void IHttpHandler.ProcessRequest(HttpContext context)
    {
        if (!ScriptCombiner.OutputCombinedScriptFile(context))
        {
            throw new InvalidOperationException("Combined script file output failed unexpectedly.");
        }
    }

    /// <summary>
    /// IsReusable implementation returns true since this class is stateless
    /// </summary>
    bool IHttpHandler.IsReusable
    {
        get { return true; }
    }
}
