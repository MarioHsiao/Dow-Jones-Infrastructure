using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using DowJones.Web.Mvc.Resources;

namespace DowJones.Web.Mvc.UI
{
    public class YepNopeScriptRegistryWriter : ScriptRegistryWriter
    {
        protected ClientResource YepNopeResource
        {
            get
            {
                if (_yepNopeResource == null)
                {
                    lock (_yepNopeResourceLock)
                    {
                        if(_yepNopeResource == null)
                        {
                            var assembly = typeof(EmbeddedResources).Assembly;
                            _yepNopeResource = 
                                new EmbeddedClientResource(assembly, EmbeddedResources.Js.YepNope)
                                    {
                                        DependencyLevel = ClientResourceDependencyLevel.Core
                                    };
                        }
                    }
                }

                return _yepNopeResource;
            }
        }
        private static ClientResource _yepNopeResource;
        private static readonly object _yepNopeResourceLock = new object();

        public bool RenderScriptBlock { get; set; }


        public YepNopeScriptRegistryWriter(IClientResourceManager clientResourceManager, CultureInfo culture, ClientResourceCombiner resourceCombiner, HttpContextBase httpContext)
            : base(clientResourceManager, culture, resourceCombiner, httpContext)
        {
            RenderScriptBlock = true;
        }


        public override void Render(ScriptRegistry scriptRegistry, HtmlTextWriter writer)
        {
            var clientResourceFilter = IsPartialRequest ? PartialResultResourcesFilter : NonGlobalResourceFilter;
            
            var scripts = scriptRegistry.GetScripts();
            var clientTemplates = scriptRegistry.GetClientTemplates();

            if (clientResourceFilter != null)
                scripts = scripts.Where(clientResourceFilter);

            if (clientResourceFilter != null)
                clientTemplates = clientTemplates.Where(clientResourceFilter);


            Action<HtmlTextWriter> writeScriptStatements =
                x => WriteScriptStatements(x, scriptRegistry, false);

            if (RenderScriptBlock)
                OpenScriptBlock(writer);

            // If we have client templates, daisy-chain them as the complete action
            // and write the script statements when we're done
            if (clientTemplates.Any())
            {
                WriteYepNopeStatement(
                    writer, scripts,
                    x => WriteYepNopeStatement(x, clientTemplates, writeScriptStatements)
                );
            }
            // Otherwise, just write the script includes and execute the statements
            else
            {
                WriteYepNopeStatement(writer, scripts, writeScriptStatements);
            }

            if(RenderScriptBlock)
                CloseScriptBlock(writer);
        }
        public override string RenderHead(ScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer()
                .Render(writer => WriteHead(writer, scriptRegistry));
        }

        private void WriteHead(HtmlTextWriter writer, ScriptRegistry scriptRegistry)
        {
            // Include the Global scripts as direct references
            var globalScripts = scriptRegistry.GetScripts().Where(GlobalResourceFilter).ToList();

            WriteScriptIncludes(writer, new [] {YepNopeResource});
            WriteScriptIncludes(writer, globalScripts);
        }



        private void WriteYepNopeStatement(HtmlTextWriter writer, IEnumerable<ClientResource> scripts, Action<HtmlTextWriter> complete = null)
        {
            var scriptUrls = GenerateUrls(scripts).Select(url => string.Format("\"{0}\"", url));

#if(DEBUG)
            // If we're in debug mode, add line separators between URLs 
            // so they're easier to read
            const string urlIncludeSeparator = ",\r\n";
#else
            const string urlIncludeSeparator = ",";
#endif

            var concatListOfUrls = string.Join(urlIncludeSeparator, scriptUrls);

            writer.WriteLine("yepnope({");

                writer.Write("load: [{0}\r\n]", concatListOfUrls);

                if (complete != null)
                {
                    writer.WriteLine(",");
                    writer.WriteLine("complete: function() {");
                    complete(writer);
                    writer.WriteLine("}");
                }

            writer.WriteLine("});");
        }
    }
}