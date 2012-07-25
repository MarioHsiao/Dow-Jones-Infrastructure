using System.IO;
using System.Xml.Linq;

namespace DowJones.Documentation.Tests
{
    public class TestClassWithEmbeddedResources
    {
        protected Stream LoadResource(string resourcename)
        {
            var type = GetType();
            var stream = type.Assembly.GetManifestResourceStream(type, resourcename);
            return stream;
        }

        protected StreamReader LoadResourceStreamReader(string resourcename)
        {
            var type = GetType();
            var stream = type.Assembly.GetManifestResourceStream(type, resourcename);
            return new StreamReader(stream);
        }

        protected string LoadResourceString(string resourcename)
        {
            var reader = LoadResourceStreamReader(resourcename);
            var content = reader.ReadToEnd();
            return content;
        }

        protected XDocument LoadResourceXDocument(string resourcename)
        {
            var xml = LoadResourceString(resourcename);
            return XDocument.Parse(xml);
        }
    }
}