using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSDocPreprocessor;
using VSDocSplitter;

namespace DowJones.Documentation.Tests.Tools.VSDocPreprocessor
{
    [TestClass]
    public class ParserTests
    {
        private readonly Parser _parser;
        private readonly DocumentEntityConverter _converter;

        public ParserTests()
        {
            _parser = new Parser();
            _converter = new DocumentEntityConverter();
        }

        [TestMethod]
        public void ShouldParseVSDoc()
        {
            var vsDoc = LoadFromResource("TestData.Input.Tests.XML");
            var types = _parser.Parse(vsDoc);

            foreach (var entity in types)
            {
                var resourcename = string.Format("TestData.Expected.{0}.xml", entity.Name);
                var expected = LoadFromResource(resourcename);
                var actual = _converter.Convert(entity);

                Assert.AreEqual(
                    expected.Document.ToString(),
                    actual.Document.ToString(),
                    entity.FullName + " not the same");
            }
        }

        private XDocument LoadFromResource(string resourcename)
        {
            var type = GetType();
            var stream = type.Assembly.GetManifestResourceStream(type, resourcename);
            var xml = new StreamReader(stream).ReadToEnd();
            return XDocument.Parse(xml);
        }
    }
}
