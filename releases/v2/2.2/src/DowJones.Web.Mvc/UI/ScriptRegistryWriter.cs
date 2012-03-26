using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI
{
    public class ScriptRegistryWriter : IScriptRegistryWriter
    {
        private readonly HttpContextBase _httpContext;

        internal static JsonSerializer Serializer
        {
            get { return _serializer = _serializer ?? new JsonSerializer(); }
        }
        private static JsonSerializer _serializer;

        internal static readonly Func<ClientResource, bool> GlobalResourceFilter =
            resource => resource.DependencyLevel >= ClientResourceDependencyLevel.Global;

        internal static readonly Func<ClientResource, bool> NonGlobalResourceFilter =
            resource => !GlobalResourceFilter(resource);

        internal static readonly Func<ClientResource, bool> PartialResultResourcesFilter =
            resource => resource.DependencyLevel < ClientResourceDependencyLevel.Global;

        public bool WrapScriptCommands { get; set; }

        protected ClientResourceCombiner ClientResourceCombiner { get; set; }

        protected IClientResourceManager ClientResourceManager { get; set; }

        protected CultureInfo Culture { get; set; }

        protected bool IsPartialRequest
        {
            get { return _httpContext.Request.IsAjaxRequest(); }
        }


        protected virtual string OnDocumentReadyFunctionStart
        {
            get { return "jQuery(document).ready(function(){"; }
        }
        protected virtual string OnDocumentReadyFunctionEnd
        {
            get { return "});"; }
        }

        protected virtual string OnWindowUnloadFunctionStart
        {
            get { return "jQuery(window).unload(function(){"; }
        }
        protected virtual string OnWindowUnloadFunctionEnd
        {
            get { return "});"; }
        }


        public ScriptRegistryWriter(IClientResourceManager clientResourceManager, CultureInfo culture, ClientResourceCombiner resourceCombiner, HttpContextBase httpContext)
        {
            _httpContext = httpContext;
            ClientResourceManager = clientResourceManager;
            Culture = culture;
            ClientResourceCombiner = resourceCombiner;
            WrapScriptCommands = true;

#if(DEBUG)
            WrapScriptCommands = false;
#endif
        }


        protected IEnumerable<string> GenerateUrls(IEnumerable<ClientResource> resources)
        {
            if (ClientResourceCombiner != null)
                return ClientResourceCombiner.GenerateCombinedResourceUrls(resources);
            else
                return resources.Select(x => ClientResourceManager.GenerateUrl(x, Culture));
        }

        public virtual string Render(ScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer()
                .Render(writer => Render(scriptRegistry, writer));
        }

        public virtual void Render(ScriptRegistry scriptRegistry, HtmlTextWriter writer)
        {
            var scriptIncludesFilter = IsPartialRequest ? PartialResultResourcesFilter : NonGlobalResourceFilter;
            var clientTemplatesFilter = IsPartialRequest ? PartialResultResourcesFilter : null;

            WriteScriptIncludes(writer, scriptRegistry, scriptIncludesFilter);
            WriteClientTemplateIncludes(writer, scriptRegistry, clientTemplatesFilter);
            WriteScriptStatements(writer, scriptRegistry);
        }

        public virtual string RenderHead(ScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer().Render(writer =>
                WriteScriptIncludes(writer, scriptRegistry, GlobalResourceFilter));
        }

        protected virtual StringBuilder WriteCleanUpScripts(ScriptRegistry scriptRegistry)
        {
            var registeredComponents = scriptRegistry.ComponentRegistry.GetRegisteredComponents();

            StringWriter cleanupWriter = new StringWriter();

            foreach (var component in registeredComponents)
            {
                component.WriteCleanupScript(cleanupWriter);
                cleanupWriter.WriteLine();
            }

            return cleanupWriter.GetStringBuilder();
        }

        protected virtual void WriteScriptIncludes(HtmlTextWriter writer, ScriptRegistry scriptRegistry, Func<ClientResource, bool> clientResourceFilter = null)
        {
            var scriptResources = scriptRegistry.GetScripts();
            
            if (clientResourceFilter != null)
                scriptResources = scriptResources.Where(clientResourceFilter);

            WriteScriptIncludes(writer, scriptResources);
        }

        protected void WriteScriptIncludes(HtmlTextWriter writer, IEnumerable<ClientResource> scriptResources)
        {
            IEnumerable<string> scriptUrls = GenerateUrls(scriptResources);

            foreach (var scriptUrl in scriptUrls)
            {
                writer.RenderScriptInclude(scriptUrl);
            }
        }

        protected virtual void WriteClientTemplateIncludes(HtmlTextWriter writer, ScriptRegistry scriptRegistry, Func<ClientResource, bool> clientResourceFilter = null)
        {
            var clientTemplateResources = scriptRegistry.GetClientTemplates();

            if (clientResourceFilter != null)
                clientTemplateResources = clientTemplateResources.Where(clientResourceFilter);

            WriteScriptIncludes(writer, clientTemplateResources);
        }

        protected virtual void WriteScriptStatements(HtmlTextWriter writer, ScriptRegistry scriptRegistry, bool renderScriptBlock = true)
        {
            string cleanUpScripts = WriteCleanUpScripts(scriptRegistry).ToString();

            var registeredComponents = scriptRegistry.ComponentRegistry.GetRegisteredComponents().Reverse();

            bool shouldWriteOnDocumentReady = !registeredComponents.IsEmpty() || !scriptRegistry.OnDocumentReadyActions.IsEmpty() || !scriptRegistry.OnDocumentReadyStatements.IsEmpty();
            bool shouldWriteOnWindowUnload = !scriptRegistry.OnWindowUnloadActions.IsEmpty() || !scriptRegistry.OnWindowUnloadStatements.IsEmpty() || cleanUpScripts.Trim().HasValue();

            if (shouldWriteOnDocumentReady || shouldWriteOnWindowUnload)
            {
                if(renderScriptBlock)
                    OpenScriptBlock(writer);

                if(!IsPartialRequest)
                    RenderLogger(writer);

                if (shouldWriteOnDocumentReady)
                {
                    if (!IsPartialRequest)
                        writer.WriteLine(OnDocumentReadyFunctionStart);

                    foreach (var component in registeredComponents)
                    {
                        StartScriptCommand(writer);
                        component.WriteInitializationScript(writer);
                        EndScriptCommand(writer);
                    }

                    foreach (Action action in scriptRegistry.OnDocumentReadyActions)
                    {
                        StartScriptCommand(writer);
                        action();
                        EndScriptCommand(writer);
                    }

                    foreach (string statement in scriptRegistry.OnDocumentReadyStatements)
                    {
                        StartScriptCommand(writer);
                        writer.Write(statement);
                        EndScriptCommand(writer);
                    }

                    if (!IsPartialRequest)
                        writer.WriteLine(OnDocumentReadyFunctionEnd);
                }

                // pageUnload
                if (shouldWriteOnWindowUnload)
                {
                    writer.WriteLine(OnWindowUnloadFunctionStart);

                    foreach (Action action in scriptRegistry.OnWindowUnloadActions)
                    {
                        StartScriptCommand(writer);
                        action();
                        EndScriptCommand(writer);
                    }

                    foreach (string statement in scriptRegistry.OnWindowUnloadStatements)
                    {
                        StartScriptCommand(writer);
                        writer.Write(statement);
                        EndScriptCommand(writer);
                    }

                    StartScriptCommand(writer);
                    writer.WriteLine(cleanUpScripts); // write clean up scripts
                    EndScriptCommand(writer);

                    writer.WriteLine(OnWindowUnloadFunctionEnd);
                }

                if (renderScriptBlock)
                    CloseScriptBlock(writer);
            }
        }

        protected void RenderLogger(HtmlTextWriter writer)
        {
            writer.Write("if(!window['dj_log']) {");
                writer.Write("window['dj_log'] = function(message) {");
                    writer.Write("if(console && console.log) {");
                    writer.Write("console.log(message);");
                writer.Write("}");
            writer.Write("}");
            writer.WriteLine("} ");
        }

        protected void StartScriptCommand(HtmlTextWriter writer)
        {
            if(WrapScriptCommands)
                writer.Write("try{ ");
        }

        protected void EndScriptCommand(HtmlTextWriter writer)
        {
            if (WrapScriptCommands)
                writer.WriteLine("}catch(ex){dj_log(ex);}");
            else
                writer.WriteLine(";");
        }


        protected static void OpenScriptBlock(HtmlTextWriter writer)
        {
            writer.WriteLine("<script type=\"text/javascript\">");
            writer.WriteLine("//<![CDATA[");
        }

        protected static void CloseScriptBlock(HtmlTextWriter writer)
        {
            writer.WriteLine("//]]>");
            writer.WriteLine("</script>");
        }

    }

    
}