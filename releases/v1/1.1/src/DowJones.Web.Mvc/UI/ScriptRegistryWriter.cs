using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Utilities.Extensions.Web;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI
{
    public class ScriptRegistryWriter : IScriptRegistryWriter
    {
        internal static JsonSerializer Serializer
        {
            get { return _serializer = _serializer ?? new JsonSerializer(); }
        }
        private static JsonSerializer _serializer;

        internal static readonly Func<ClientResource, bool> GlobalResourceFilter =
            resource => resource.DependencyLevel == ClientResourceDependencyLevel.Global;

        internal static readonly Func<ClientResource, bool> NonGlobalResourceFilter =
            resource => resource.DependencyLevel != ClientResourceDependencyLevel.Global;

        internal static readonly Func<ClientResource, bool> PartialResultResourcesFilter =
            resource => resource.DependencyLevel < ClientResourceDependencyLevel.Global;


        protected ClientResourceCombiner ClientResourceCombiner { get; set; }

        protected IClientResourceManager ClientResourceManager { get; set; }

        protected CultureInfo Culture { get; set; }


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


        public ScriptRegistryWriter(IClientResourceManager clientResourceManager, CultureInfo culture, ClientResourceCombiner resourceCombiner)
        {
            ClientResourceManager = clientResourceManager;
            Culture = culture;
            ClientResourceCombiner = resourceCombiner;
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
                .Render(writer =>
                {
                    WriteScriptIncludes(writer, scriptRegistry, NonGlobalResourceFilter);
                    WriteClientTemplateIncludes(writer, scriptRegistry);
                    WriteScriptStatements(writer, scriptRegistry);
                });
        }

        public virtual string RenderHead(ScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer().Render(writer =>
                WriteScriptIncludes(writer, scriptRegistry, GlobalResourceFilter));
        }

        public virtual void RenderPartial(ScriptRegistry scriptRegistry, HtmlTextWriter writer)
        {
            WriteScriptIncludes(writer, scriptRegistry, PartialResultResourcesFilter, isPartialRequest: true);
            WriteClientTemplateIncludes(writer, scriptRegistry, PartialResultResourcesFilter, isPartialRequest: true);
            WriteScriptStatements(writer, scriptRegistry, isPartialRequest: true);
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

        protected virtual void WriteScriptIncludes(HtmlTextWriter writer, ScriptRegistry scriptRegistry, Func<ClientResource, bool> clientResourceFilter = null, bool? isPartialRequest = null)
        {
            var scriptResources = scriptRegistry.GetScripts();
            
            if (clientResourceFilter != null)
                scriptResources = scriptResources.Where(clientResourceFilter);

            IEnumerable<string> scriptUrls = GenerateUrls(scriptResources);

            foreach (var scriptUrl in scriptUrls)
            {
                writer.RenderScriptInclude(scriptUrl);
            }
        }

        protected virtual void WriteClientTemplateIncludes(HtmlTextWriter writer, ScriptRegistry scriptRegistry, Func<ClientResource, bool> clientResourceFilter = null, bool? isPartialRequest = null)
        {
            var clientTemplateResources = scriptRegistry.GetClientTemplates();

            if (clientResourceFilter != null)
                clientTemplateResources = clientTemplateResources.Where(clientResourceFilter);

            IEnumerable<string> scriptUrls = GenerateUrls(clientTemplateResources);

            foreach (var scriptUrl in scriptUrls)
            {
                writer.RenderScriptInclude(scriptUrl);
            }
        }

        protected virtual void WriteScriptStatements(HtmlTextWriter writer, ScriptRegistry scriptRegistry, bool? isPartialRequest = null)
        {
            string cleanUpScripts = WriteCleanUpScripts(scriptRegistry).ToString();

            var registeredComponents = scriptRegistry.ComponentRegistry.GetRegisteredComponents().Reverse();

            bool shouldWriteOnDocumentReady = !registeredComponents.IsEmpty() || !scriptRegistry.OnDocumentReadyActions.IsEmpty() || !scriptRegistry.OnDocumentReadyStatements.IsEmpty();
            bool shouldWriteOnWindowUnload = !scriptRegistry.OnWindowUnloadActions.IsEmpty() || !scriptRegistry.OnWindowUnloadStatements.IsEmpty() || cleanUpScripts.Trim().HasValue();

            if (shouldWriteOnDocumentReady || shouldWriteOnWindowUnload)
            {
                OpenScriptBlock(writer);

                if (shouldWriteOnDocumentReady)
                {
                    if (!isPartialRequest.GetValueOrDefault())
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
                        writer.WriteLine(statement);
                        EndScriptCommand(writer);
                    }

                    if (!isPartialRequest.GetValueOrDefault())
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
                        writer.WriteLine(statement);
                        EndScriptCommand(writer);
                    }

                    StartScriptCommand(writer);
                    writer.WriteLine(cleanUpScripts); // write clean up scripts
                    EndScriptCommand(writer);

                    writer.WriteLine(OnWindowUnloadFunctionEnd);
                }

                CloseScriptBlock(writer);
            }
        }

        protected void StartScriptCommand(HtmlTextWriter writer)
        {
            writer.Write("try{ ");
        }

        protected void EndScriptCommand(HtmlTextWriter writer)
        {
            writer.WriteLine("}catch(ex){$dj.debug(ex);}");
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