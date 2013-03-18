using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Razor;
using DowJones.Web.Razor.AuxiliaryContent;
using DowJones.Web.Razor.Runtime;

namespace DowJones.Web.Razor
{
    /// <summary>
    /// Razor ViewComponent templating logic extracted from the ClassGenerator
    /// in order to make it testable (can't new up an instance of the ClassGenerator)
    /// </summary>
    public class ViewComponentGenerator
    {
        private readonly IEnumerable<IAuxiliaryContentGenerator> _auxiliaryContentGenerators;
        private readonly EmbeddedViewComponentRazorHost _host;

        public ViewComponentGenerator(string virtualPath, string physicalPath, string generatedNamespace)
        {
            _auxiliaryContentGenerators = new [] { new ClientTemplateGenerator() };
            _host = new EmbeddedViewComponentRazorHost(virtualPath, physicalPath)
                        {DefaultNamespace = generatedNamespace};
        }

        public ViewComponentGenerationResult Generate(GeneratorResults generatorResults, CodeDomProvider provider, CodeGeneratorOptions options = null)
        {
            var generatedCode = RenderCode(generatorResults, provider, options);
            
            var auxiliaryContent =
                _auxiliaryContentGenerators.SelectMany(generator => 
                                                       generator.GenerateAuxiliaryContent(generatorResults));

            return new ViewComponentGenerationResult
                       {
                           GeneratedCode = generatedCode, 
                           AuxiliaryContent = auxiliaryContent.ToArray(),
                       };
        }

        public GeneratorResults GenerateRazorTemplate(string templateText)
        {
            // Create a Razor engine and pass it our host
            RazorTemplateEngine engine = new EmbeddedViewComponentRazorTemplateEngine(_host);

            using (TextReader reader = new StringReader(templateText))
            {
                GeneratorResults results = engine.GenerateCode(reader);
                return results;
            }
        }

        protected internal string RenderCode(GeneratorResults generatorResults, CodeDomProvider provider, CodeGeneratorOptions options = null)
        {
            options = options ?? new CodeGeneratorOptions();

            using (var writer = new StringWriter(new StringBuilder()))
            {
                //Generate the code
                provider.GenerateCodeFromCompileUnit(generatorResults.GeneratedCode, writer, options);
                writer.Flush();

                return writer.ToString();
            }
        }
    }
}
