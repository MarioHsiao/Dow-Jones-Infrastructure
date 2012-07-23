using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Generators;

namespace DowJones.Web.Handlers
{
    public class BasicHttpHandlerFactory : Page, IHttpAsyncHandler, IRequiresSessionState
    {
        protected override void OnInit(EventArgs e)
        {
            Page.ID = RandomKeyGenerator.GetRandomKey(10, RandomKeyGenerator.CharacterSet.Alpha);

            var handler = new Basic_HttpHandlerFactory().GetHandler(
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

        #region Implementation of IHttpAsyncHandler

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return AspCompatBeginProcessRequest(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }

        #endregion
    }

    public class Basic_HttpHandlerFactory : IHttpHandlerFactory
    {
        private static readonly Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();
        private static readonly object _syncObject = new object();

        #region Implementation of IHttpHandlerFactory

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
             // Try to get the type associated with the request (On a name to type basis)
             var typeName = Path.GetFileNameWithoutExtension(pathTranslated);

            if (typeName.StartsWith("DowJones.Utilities.Handlers.Items"))
            {
                typeName = "DowJones.Utilities.Handlers.Items.ItemHandler";
            }

            try
             {
                 // Request Hosting permissions
                 new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();
                
                 var handlerType = GetServiceType(typeName);

                 // if we did not find any send it on to the original ajax script service handler.
                 if (handlerType != null)
                 {
                     Object h;
                     try
                     {
                         // Create the handler by calling class abc or class xyz.
                         if (handlerType.IsCompatibleWith(typeof(BaseHttpHandler)))
                         {

                             h = Activator.CreateInstance(handlerType, new object[] { context.Request.ContentType });
                         }
                         else 
                         {
                             h = Activator.CreateInstance(handlerType);
                         }                         
                     }
                     catch (Exception e)
                     {
                         throw new HttpException("Factory couldn't create instance " +
                                                 "of type " + typeName, e);
                     }
                     return (IHttpHandler) h;
                 }
             }
            // Because we are using Reflection, errors generated in reflection will be an InnerException, 
            // to get the real Exception we throw the InnerException it.
            catch (TargetInvocationException e)
            {
                throw e.InnerException;

            }
            throw new HttpException("Unable to find requested handler: " + typeName );
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        #endregion


        /// <summary>
        /// Searches all Services and tries to find a class with the specified name
        /// </summary>
        public static Type GetServiceType(string typeName)
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
                    continue;
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

    }
}
