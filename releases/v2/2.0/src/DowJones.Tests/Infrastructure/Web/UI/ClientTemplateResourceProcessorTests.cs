using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{
    [TestClass]
    public class ClientTemplateResourceProcessorTests : UnitTestFixtureBase<ClientTemplateResourceProcessor>
    {
        private ClientTemplateParser _parser;

        protected ClientTemplateResourceProcessor Processor
        {
            get { return UnitUnderTest; }
        }


        [TestMethod]
        public void ShouldGenerateJavaScriptTemplate()
        {
            ProcessedClientResource resource = CreateClientTemplateResource();

            string expectedParsedTemplate = _parser.Parse(resource.Content);

            Processor.Process(resource);

            // Don't really care WHERE the template is, as long as it exists somewhere.
            Assert.IsTrue(resource.Content.Contains(expectedParsedTemplate));
        }

        [TestMethod]
        public void ShouldParseClientTemplateIDFromNameAndRelativeTemplateID()
        {
            const string templateName = "Success";
            const string componentName = "DemoComponent";
            
            ProcessedClientResource resource = CreateClientTemplateResource(
                    name: "DowJones.Web.UI.Components." + componentName + "." + templateName + ".htm",
                    templateId: templateName
                );

            var templateInfo = new ClientTemplateResourceProcessor.ClientTemplateInfo(resource.ClientResource);

            Assert.AreEqual(
                string.Format("{0}.{1}.{2}.{3}", 
                              ClientTemplateResourceProcessor.JavaScriptClassNamespace,
                              componentName, 
                              ClientTemplateResourceProcessor.ClientComponentTemplateProperty,
                              templateName
                             ),
                templateInfo.FullTemplatePath
            );
        }

        [TestMethod]
        public void ShouldParseClientTemplateIDFromNameAndRelativeTemplateIDAndIgnoreClientTemplatesSubfolder()
        {
            const string templateName = "Success";
            const string componentName = "DemoComponent";

            ProcessedClientResource resource = CreateClientTemplateResource(
                    name: "DowJones.Web.UI.Components." + componentName + ".ClientTemplates." + templateName + ".htm",
                    templateId: templateName
                );

            var templateInfo = new ClientTemplateResourceProcessor.ClientTemplateInfo(resource.ClientResource);

            Assert.AreEqual(
                string.Format("{0}.{1}.{2}.{3}",
                              ClientTemplateResourceProcessor.JavaScriptClassNamespace,
                              componentName,
                              ClientTemplateResourceProcessor.ClientComponentTemplateProperty,
                              templateName
                             ),
                templateInfo.FullTemplatePath
            );
        }

        [TestMethod]
        public void ShouldReturnAbsoluteTemplateID()
        {
            const string absoluteTemplateID = "DJ.UI.templates.Foo";
            
            var resource = CreateClientTemplateResource( templateId: absoluteTemplateID );

            var templateInfo = new ClientTemplateResourceProcessor.ClientTemplateInfo(resource.ClientResource);

            Assert.AreEqual(
                absoluteTemplateID,
                templateInfo.FullTemplatePath
            );
        }


        private static ProcessedClientResource CreateClientTemplateResource(string content = null, string name = null, string templateId = null)
        {
            content = content ?? "<div />";
            name = name ?? "DowJones.Web.UI.Components.DemoComponent.ClientTemplates.Success.htm";
            templateId = templateId ?? "DemoTemplate";

            return new ProcessedClientResource(
                new ClientResource
                    {
                        Name = name,
                        ResourceKind = ClientResourceKind.ClientTemplate, 
                        TemplateId = templateId,
                    },
                content
                );
        }

        protected override ClientTemplateResourceProcessor CreateUnitUnderTest()
        {
            _parser = new ClientTemplateParser();
            
            ClientTemplateResourceProcessor unitUnderTest = new ClientTemplateResourceProcessor(_parser);

            return unitUnderTest;
        }
    }
}