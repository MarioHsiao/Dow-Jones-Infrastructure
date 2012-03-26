using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Utilities.Extensions.Web;
using DowJones.Web.Mvc.Resources;

namespace DowJones.Web.Mvc.UI
{
    public class HeadJsScriptRegistryWriter : ScriptRegistryWriter
    {
        internal static readonly string HeadJsUrl = typeof (EmbeddedResources).Assembly.GetWebResourceUrl(EmbeddedResources.Js.HeadJs);
        
        // HACK:  This is part of the name hack below
        private int _resourceCounter = 0;

        protected override string OnDocumentReadyFunctionStart
        {
            get { return "head.ready(function() {"; }
        }
        protected override string OnDocumentReadyFunctionEnd
        {
            get { return "});"; }
        }

        protected override string OnWindowUnloadFunctionStart
        {
            get { return "head.ready(function() { jQuery(window).unload(function(){"; }
        }
        protected override string OnWindowUnloadFunctionEnd
        {
            get { return "}) /* End jQuery.unload */ }); /* End head.ready() */"; }
        }


        public HeadJsScriptRegistryWriter(IClientResourceManager clientResourceManager, CultureInfo culture, ClientResourceCombiner resourceCombiner)
            : base(clientResourceManager, culture, resourceCombiner)
        {
        }


        public override string Render(ScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer()
                .Render(writer =>
                {
                    WriteScriptIncludes(writer, scriptRegistry, NonGlobalResourceFilter);
                    WriteClientTemplateIncludes(writer, scriptRegistry);
                    WriteScriptStatements(writer, scriptRegistry);
                });
        }

        public override string RenderHead(ScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer()
                .Render(writer => WriteHead(writer, scriptRegistry));
        }

        private void WriteHead(HtmlTextWriter writer, ScriptRegistry scriptRegistry)
        {
            // Include head.js as a direct reference
            writer.RenderScriptInclude(HeadJsUrl);

            // Include the Global scripts as a direct reference
            base.WriteScriptIncludes(writer, scriptRegistry, GlobalResourceFilter);
        }

        protected override void WriteScriptIncludes(HtmlTextWriter writer, ScriptRegistry scriptRegistry, Func<ClientResource, bool> clientResourceFilter = null, bool? isPartialRequest = null)
        {
            IEnumerable<ClientResource> clientResources = scriptRegistry.GetScripts();

            if (clientResourceFilter != null)
                clientResources = clientResources.Where(clientResourceFilter);

            clientResources = clientResources.Union(scriptRegistry.GetClientTemplates());

            writer.WriteLine("<script type='text/javascript'>");
            writer.WriteLine(GetHeadJsIncludeStatement(clientResources));
            writer.WriteLine("</script>");
        }

        protected override void WriteClientTemplateIncludes(HtmlTextWriter writer, ScriptRegistry scriptRegistry, Func<ClientResource, bool> clientResourceFilter = null, bool? isPartialRequest = null)
        {
            // Do nothing - client templates are included in script includes
        }

        private string GetHeadJsIncludeStatement(IEnumerable<ClientResource> clientResources, string onLoadFunction = null)
        {
            List<string> headJsIncludes =
                GenerateUrls(clientResources)
                    .Select(url => string.Format("\"{0}\"", url))
                    .ToList();


            #region head.js Name Hack

            // HACK: The above line should work, but head.js was ignoring
            //       some of the scripts if they didn't have names.
            //       Giving every script a head.js name seems to avoid this...
            //       Feel free to remove this hack when/if this is fixed
            //       (i.e. if you can delete this for statement and the page still works)
            for (int i = 0; i < headJsIncludes.Count; i++)
                headJsIncludes[i] = string.Format("{{ '{0}': {1} }}", _resourceCounter++, headJsIncludes[i]);

            #endregion

#if(DEBUG)
            // If we're in debug mode, add line separators between URLs 
            // so they're easier to read
            const string urlIncludeSeparator = ",\r\n";
#else
            const string urlIncludeSeparator = ",";
#endif

            var concatListOfUrls = string.Join(urlIncludeSeparator, headJsIncludes);

            if(string.IsNullOrWhiteSpace(onLoadFunction))
                return string.Format("head.js({0});", concatListOfUrls);
            else
                return string.Format("head.js({0}, function() {{ {1} }});", concatListOfUrls, onLoadFunction);
        }
    }
}