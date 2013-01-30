<%@ Page Language="C#"%>
<%@ Import Namespace="DowJones.Web.Mvc.Routing" %>
<%@ Import Namespace="System.Diagnostics" %>
<script runat="server">

[DebuggerStepThrough]
protected void Page_Load(object sender, EventArgs e)
{
    HttpContext httpContext = HttpContext.Current;

    IISVersion iisVersion = new HttpRequestWrapper(httpContext.Request).GetIISVersion();
    string redirect = RouteInfo.ApplyRoutingExtension("Home/index", iisVersion);
    
    httpContext.RewritePath(redirect);
    
    IHttpHandler httpHandler = new MvcHttpHandler();
    httpHandler.ProcessRequest(httpContext);
}

</script>