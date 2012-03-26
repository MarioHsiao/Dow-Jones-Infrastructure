// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoDiscoveryWebServiceHandlerFactory.cs" company="Dow Jones">
//  © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Automatic Discovery WebService Handler Factory
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using System.Web.UI;
using DowJones.Utilities.Generators;

namespace DowJones.Tools.WebServices
{
    /// <summary>
    /// Automatic Discovery WebService Handler Factory
    /// </summary>
    public class AutoDiscoveryWebServiceHandlerFactory : Page, IHttpAsyncHandler, IRequiresSessionState
    {
        #region Implementation of IHttpAsyncHandler

        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="cb">The <see cref="T:System.AsyncCallback"/> to call when the asynchronous method call is complete. If <paramref name="cb"/> is null, the delegate is not called.</param>
        /// <param name="extraData">Any extra data needed to process the request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.
        /// </returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return AspCompatBeginProcessRequest(context, cb, extraData);
        }

        /// <summary>
        /// Provides an asynchronous process End method when the process ends.
        /// </summary>
        /// <param name="result">An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.</param>
        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            Page.ID = RandomKeyGenerator.GetRandomKey(10, RandomKeyGenerator.CharacterSet.Alpha);

            var handler = new AutomaticDiscoveryWebServiceHandlerFactory().GetHandler(
                    Context,
                    Context.Request.HttpMethod,
                    Context.Request.FilePath,
                    Context.Request.PhysicalPath);

            if (handler != null)
            {
                handler.ProcessRequest(Context);
            }

            Context.ApplicationInstance.CompleteRequest();
        }
    }


    /// <summary>
    /// A web service handler factory that discovers what external assembly, file or factory to use.
    /// </summary>
    public class AutomaticDiscoveryWebServiceHandlerFactory : IHttpHandlerFactory
    {
        /// <summary>
        /// Synchronization object
        /// </summary>
        private static readonly object _syncObject = new object();

        private static readonly Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();

        /// <summary>
        /// Use one of the visible attributes in the assembly to get a reference to the "System.Web.Extentions" Library.
        /// </summary>
        private Assembly _ajaxAssembly;

        /// <summary>
        /// Used to remember which Factory has been used
        /// </summary>
        private IHttpHandlerFactory _usedHandlerFactory;

        private Assembly AjaxAssembly
        {
            get { return _ajaxAssembly ?? (_ajaxAssembly = typeof(GenerateScriptTypeAttribute).Assembly); }
        }

        #region IHttpHandlerFactory Members

        /// <summary>
        /// Checks the request type and returns the corresponding httpd handler or factory
        /// </summary>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            //IHttpHandler httpHandler;

            try
            {
                // Request Hosting permissions
                new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

                // Try to get the type associated with the request (On a name to type basis)
                var webServiceType = GetServiceType(Path.GetFileNameWithoutExtension(pathTranslated));

                // if we did not find any send it on to the original ajax script service handler.
                if (webServiceType == null)
                {
                    // [REFLECTION] Get the internal class System.Web.Script.Services.ScriptHandlerFactory create it.
                    var scriptHandlerFactory = (IHttpHandlerFactory)Activator.CreateInstance(AjaxAssembly.GetType("System.Web.Script.Services.ScriptHandlerFactory"), true);
                    _usedHandlerFactory = scriptHandlerFactory;
                    return scriptHandlerFactory.GetHandler(context, requestType, url, pathTranslated);
                }

                // [REFLECTION] get the Handlerfactory : RestHandlerFactory (Handles Javascript proxy Generation and actions)
                var javascriptHandlerFactory = (IHttpHandlerFactory)Activator.CreateInstance(AjaxAssembly.GetType("System.Web.Script.Services.RestHandlerFactory"), true);
                if (javascriptHandlerFactory == null)
                    throw new NullReferenceException();

                // [REFLECTION] Check if the current request is a Javasacript method
                // JavascriptHandlerfactory.IsRestRequest(context);
                var isScriptRequestMethod = javascriptHandlerFactory.GetType().GetMethod("IsRestRequest", BindingFlags.Static | BindingFlags.NonPublic);
                if ((bool)isScriptRequestMethod.Invoke(null, new object[] { context }))
                {
                    // Remember the used factory for later in ReleaseHandler
                    _usedHandlerFactory = javascriptHandlerFactory;

                    // Check and see if it is a Javascript Request or a request for a Javascript Proxy.
                    var isJavascriptDebug = string.Equals(context.Request.PathInfo, "/jsdebug", StringComparison.OrdinalIgnoreCase);
                    var isJavascript = string.Equals(context.Request.PathInfo, "/js", StringComparison.OrdinalIgnoreCase);
                    var assemblyModifiedDate = GetAssemblyModifiedTime(webServiceType.Assembly);
                    ConstructorInfo webServiceDataConstructor;
                    if (isJavascript || isJavascriptDebug)
                    {
                        // [REFLECTION] fetch the constructor for the WebServiceData Object
                        webServiceDataConstructor = AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Type), typeof(bool) }, null);
                        if (webServiceDataConstructor == null)
                            throw new NullReferenceException();

                        // [REFLECTION] fetch the constructor for the WebServiceClientProxyGenerator
                        var webServiceClientProxyGeneratorConstructor = AjaxAssembly.GetType("System.Web.Script.Services.WebServiceClientProxyGenerator").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(bool) }, null);
                        if (webServiceClientProxyGeneratorConstructor == null)
                            throw new NullReferenceException();

                        // [REFLECTION] get the method from WebServiceClientProxy to create the javascript : GetClientProxyScript
                        var getClientProxyScriptMethod = AjaxAssembly.GetType("System.Web.Script.Services.ClientProxyGenerator").GetMethod("GetClientProxyScript", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData") }, null);

                        // [REFLECTION] We invoke : 
                        // new WebServiceClientProxyGenerator(url,false).WebServiceClientProxyGenerator.GetClientProxyScript(new WebServiceData(WebServiceType));
                        var javascript = (string)getClientProxyScriptMethod.Invoke(
                                                      webServiceClientProxyGeneratorConstructor.Invoke(new object[] { url, isJavascriptDebug })
                                                      , new[]
                                                            {
                                                                webServiceDataConstructor.Invoke(new object[] {webServiceType, false})
                                                            }

                                                     );

                        // The following caching code was copied from the original assembly, read with Reflector, comments were added manualy.

                        #region Caching

                        // Check the assembly modified time and use it as caching http header

                        // See "if Modified since" was requested in the http headers, and check it with the assembly modified time
                        var s = context.Request.Headers["If-Modified-Since"];

                        DateTime tempDate;
                        if (((s != null) && DateTime.TryParse(s, out tempDate)) && (tempDate >= assemblyModifiedDate))
                        {
                            context.Response.StatusCode = 0x130;
                            return null;
                        }

                        // Add HttpCaching data to the http headers
                        if (!isJavascriptDebug && (assemblyModifiedDate.ToUniversalTime() < DateTime.UtcNow))
                        {
                            var cache = context.Response.Cache;
                            cache.SetCacheability(HttpCacheability.Public);
                            cache.SetLastModified(assemblyModifiedDate);
                        }

                        #endregion

                        // Set Add the javascript to a new custom handler and set it in HttpHandler.
                        return new JavascriptProxyHandler(javascript);
                    }
                    var javascriptHandler = (IHttpHandler)Activator.CreateInstance(AjaxAssembly.GetType("System.Web.Script.Services.RestHandler"), true);

                    // [REFLECTION] fetch the constructor for the WebServiceData Object
                    webServiceDataConstructor = AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Type), typeof(bool) }, null);

                    // [REFLECTION] get method : JavaScriptHandler.CreateHandler
                    var createHandlerMethod = javascriptHandler.GetType().GetMethod("CreateHandler", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { AjaxAssembly.GetType("System.Web.Script.Services.WebServiceData"), typeof(string) }, null);

                    // [REFLECTION] Invoke CreateHandlerMethod :
                    // HttpHandler = JavaScriptHandler.CreateHandler(WebServiceType,false);
                    return (IHttpHandler) createHandlerMethod.Invoke(javascriptHandler, new[]
                                                                                            {
                                                                                                webServiceDataConstructor.Invoke(new object[] { webServiceType, false }),
                                                                                                context.Request.PathInfo.Substring(1)
                                                                                            }
                                              );
                }
                // Remember the used factory for later in ReleaseHandler
                IHttpHandlerFactory webServiceHandlerFactory = new WebServiceHandlerFactory();
                _usedHandlerFactory = webServiceHandlerFactory;

                // [REFLECTION] Get the method CoreGetHandler
                var coreGetHandlerMethod = _usedHandlerFactory.GetType().GetMethod("CoreGetHandler", BindingFlags.NonPublic | BindingFlags.Instance);

                // [REFLECTION] Invoke the method CoreGetHandler :
                // WebServiceHandlerFactory.CoreGetHandler(WebServiceType,context,context.Request, context.Response);
                return (IHttpHandler) coreGetHandlerMethod.Invoke(_usedHandlerFactory, new object[]
                                                                                           {
                                                                                               webServiceType, context, context.Request, context.Response
                                                                                           });
            }

                // Because we are using Reflection, errors generated in reflection will be an InnerException, 
            // to get the real Exception we throw the InnerException it.
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        /// <summary>
        /// Release the handler in the used factory
        /// </summary>
        /// <param name="handler"></param>
        public void ReleaseHandler(IHttpHandler handler)
        {
            if (_usedHandlerFactory != null)
            {
                _usedHandlerFactory.ReleaseHandler(handler);
            }
        }

        #endregion

        /// <summary>
        /// Searches all Services and tries to find a class with the specified name
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The get service type.</returns>
        private static Type GetServiceType(string typeName)
        {
            if (_typeCache.ContainsKey(typeName))
            {
                return _typeCache[typeName];
            }

            // If not in cache go through all assemblies
            foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var classType = loadedAssembly.GetType(typeName);
                if (classType == null)
                {
                    continue;
                }

                if (!_typeCache.ContainsKey(typeName))
                {
                    lock (_syncObject)
                    {
                        if (!_typeCache.ContainsKey(typeName))
                        {
                            _typeCache.Add(typeName, classType);
                        }
                    }
                }

                return classType;
            }

            return null;
        }

        /// <summary>
        /// Gets the assembly modified time.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c ref="DateTime">DateTime object</c></returns>
        private static DateTime GetAssemblyModifiedTime(Assembly assembly)
        {
            var lastWriteTime = File.GetLastWriteTime(new Uri(assembly.GetName().CodeBase).LocalPath);
            return new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day, lastWriteTime.Hour, lastWriteTime.Minute, 0);
        }
    }

    /// <summary>
    /// A custom handler to deliver the generated Javascript.
    /// </summary>
    internal class JavascriptProxyHandler : IHttpHandler
    {
        private readonly string _javascript = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavascriptProxyHandler"/> class.
        /// </summary>
        /// <param name="javascript">The javascript.</param>
        public JavascriptProxyHandler(string javascript)
        {
            _javascript = javascript;
        }

        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/x-javascript";
            context.Response.Write(_javascript);
        }

        #endregion
    }
}