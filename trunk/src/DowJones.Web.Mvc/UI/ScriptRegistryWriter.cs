using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Properties;
using DowJones.Web.ClientResources;
using DowJones.Web.Configuration;

namespace DowJones.Web.Mvc.UI
{
    public class ScriptRegistryWriter : IScriptRegistryWriter
    {
        private static readonly IEnumerable<ClientResource> JQueryRequireResources =
            new[] { "jquery", "require" }
            .Select(x => (ClientResource)x);

        private static readonly IEnumerable<ClientResource> CommonResources =
            new[] {
                "underscore", "common", "pubsub", "composite-component", 
                "dj-jquery-ext", "jquery-json",
            }.Select(x => (ClientResource)x);

        internal static readonly Func<ClientResource, bool> IndependentScriptsFilter =
            resource => resource.DependencyLevel == ClientResourceDependencyLevel.Independent;

        internal static readonly Func<ClientResource, bool> ModuleFilter =
            resource => !IndependentScriptsFilter(resource);


        private readonly HttpContextBase _httpContext;
        private readonly ClientConfiguration _clientConfiguration;
        private readonly IList<string> _renderedItems;

        protected ClientResourceCombiner ClientResourceCombiner { get; set; }

        protected IClientResourceManager ClientResourceManager { get; set; }

        protected CultureInfo Culture { get; set; }

        protected bool IsPartialRequest
        {
            get { return _httpContext.Request.IsAjaxRequest(); }
        }

        public bool RegisterGlobalReferences { get; set; }


        public ScriptRegistryWriter(IClientResourceManager clientResourceManager, CultureInfo culture, ClientResourceCombiner resourceCombiner, HttpContextBase httpContext, ClientConfiguration clientConfiguration)
        {
            _httpContext = httpContext;
            _clientConfiguration = clientConfiguration;
            _renderedItems = new List<string>();
            ClientResourceManager = clientResourceManager;
            Culture = culture;
            ClientResourceCombiner = resourceCombiner;
            RegisterGlobalReferences = true;
        }


        protected IEnumerable<string> GenerateUrls(IEnumerable<ClientResource> resources)
        {
            if (ClientResourceCombiner != null)  
                return ClientResourceCombiner.GenerateCombinedResourceUrls(resources);
            else
                return resources.Select(x => ClientResourceManager.GenerateUrl(x, Culture));
        }

        public virtual void Render(IScriptRegistry scriptRegistry, TextWriter writer)
        {
            WriteScriptIncludes(scriptRegistry, writer);
            WriteScriptStatements(scriptRegistry, writer);
        }

        public virtual void RenderClientScripts(IScriptRegistry scriptRegistry, TextWriter writer, bool renderScriptBlock)
        {
            WriteScriptStatements(scriptRegistry, writer, renderScriptBlock);
        }

        public virtual void RenderHead(IScriptRegistry scriptRegistry, TextWriter writer)
        {
            var htmlWriter = writer as HtmlTextWriter ?? new HtmlTextWriter(writer);
            htmlWriter.RenderScriptInclude(ClientResourceManager.GenerateUrl(JQueryRequireResources, Culture));

            htmlWriter.OpenScriptBlock();
            RenderRequireConfiguration(writer);
            _clientConfiguration.WriteTo(writer);
            htmlWriter.CloseScriptBlock();
            
            htmlWriter.RenderScriptInclude(ClientResourceManager.GenerateUrl(CommonResources, Culture));

            if (RegisterGlobalReferences == false)
                return;

            htmlWriter.OpenScriptBlock();
            RenderGlobalJQueryAndUnderscoreRegistration(writer);
            htmlWriter.CloseScriptBlock();
        }

        private void RenderGlobalJQueryAndUnderscoreRegistration(TextWriter writer)
        {
            // Register global libraries if they didn't exist already
            writer.WriteLine("if (!window['jQuery']) { window['jQuery'] = DJ.jQuery; }");
            writer.WriteLine("if (!window['$']) { window['$'] = DJ.jQuery; }");
            writer.WriteLine("if (!window['_']) { window['_'] = DJ.underscore; }");
        }

        protected virtual void RenderRequireConfiguration(TextWriter writer)
        {
            var baseUrl = ClientResourceHandler.GenerateRequireJsBaseUrl(Culture);
            var requireConfig = new RequireJsConfiguration	() { BaseUrl = baseUrl };
            requireConfig.WriteTo(writer);
        }

        protected virtual void WriteScriptIncludes(IScriptRegistry scriptRegistry, TextWriter writer)
        {
            var scriptResources = scriptRegistry.GetScripts().ToArray();

            // If script combining is disabled then Require.js will
            // handle getting everything individually.
            // If it is enabled, write the combined scripts out to
            // "prefetch" the combined module definitions within them
            if (Settings.Default.ClientResourceCombiningEnabled && !IsPartialRequest)
            {
                var modules = scriptResources.Where(ModuleFilter);
                WriteScriptIncludes(modules, writer);
            }

            var localScriptIncludes = scriptResources.Where(IndependentScriptsFilter);
            WriteScriptIncludes(localScriptIncludes, writer);
        }

        protected virtual void WriteScriptIncludes(IEnumerable<ClientResource> scriptResources, TextWriter writer)
        {
            var scriptUrls = GenerateUrls(scriptResources);

            var htmlWriter = writer as HtmlTextWriter ?? new HtmlTextWriter(writer);
            foreach (var scriptUrl in scriptUrls)
            {
                htmlWriter.RenderScriptInclude(scriptUrl);
            }
        }

        protected virtual void WriteScriptStatements(IScriptRegistry scriptRegistry, TextWriter writer, bool renderScriptBlock = true)
        {
            var htmlWriter = writer as HtmlTextWriter ?? new HtmlTextWriter(writer);
            var initializationStatements = Unrendered(scriptRegistry.GetInitializationStatements().WhereNotNullOrEmpty().ToArray());
            var onDocumentReadyStatements = Unrendered(scriptRegistry.OnDocumentReadyStatements.WhereNotNullOrEmpty().ToArray());
            var onWindowUnloadStatements = Unrendered(scriptRegistry.OnWindowUnloadStatements.WhereNotNullOrEmpty().ToArray());
            var cleanUpScripts = Unrendered(scriptRegistry.GetCleanupStatements().WhereNotNullOrEmpty().ToArray());

            bool shouldWriteOnWindowUnload = onWindowUnloadStatements.Any() || cleanUpScripts.Any();

            if (renderScriptBlock)
                htmlWriter.OpenScriptBlock();

            WriteOnDocumentReadyFunctionStart(scriptRegistry, writer);

            foreach (string statement in initializationStatements)
            {
                writer.WriteLine(statement);
            }

            foreach (string statement in onDocumentReadyStatements)
            {
                writer.Write(statement);
            }

            // pageUnload
            if (shouldWriteOnWindowUnload)
            {
                WriteOnWindowUnloadFunctionStart(writer);

                foreach (string statement in onWindowUnloadStatements)
                {
                    writer.Write(statement);
                }

                foreach (string statement in cleanUpScripts)
                {
                    writer.WriteLine(statement);
                }

                WriteOnWindowUnloadFunctionEnd(writer);
            }

            WriteOnDocumentReadyFunctionEnd(writer);

            if (renderScriptBlock)
                htmlWriter.CloseScriptBlock();
        }


        protected virtual void WriteOnDocumentReadyFunctionStart(IScriptRegistry scriptRegistry, TextWriter writer)
        {
            var scripts = scriptRegistry.GetScripts();

            var scriptAliases = scripts.Select(x => ClientResourceManager.Alias(x));

            var moduleNames = scriptAliases.Select(x => new ClientResourceModuleName(x).Value);

            var dependencies =
                new[] { "$", "$dj", "_", "JSON" }
                .Union(moduleNames)
                .WhereNotNullOrEmpty();


            // OPEN $(function() {
            writer.Write("DJ.jQuery(function() { ");

            // OPEN require(function() {
            writer.Write("DJ.$dj.require([{0}], function($, $dj, _, JSON) {{ \r\n",
                         string.Join(",", dependencies.Select(x => string.Format("'{0}'", x))));
        }

        protected virtual void WriteOnDocumentReadyFunctionEnd(TextWriter writer)
        {
            // CLOSE require(function() {
            writer.Write("});");
            
            // CLOSE $(function() {
            writer.Write("});");
        }

        protected virtual void WriteOnWindowUnloadFunctionStart(TextWriter writer)
        {
            writer.Write("DJ.jQuery(window).unload(function(){");
        }

        protected virtual void WriteOnWindowUnloadFunctionEnd(TextWriter writer)
        {
            writer.Write("});");
        }


        protected IEnumerable<string> Unrendered(IEnumerable<string> source)
        {
            var unrendered =
                source
                    .Where(x => !_renderedItems.Any(item => item.Equals(x, StringComparison.OrdinalIgnoreCase)))
                    .ToArray();

            foreach (var item in unrendered)
                _renderedItems.Add(item);

            return unrendered;
        }
    }
}