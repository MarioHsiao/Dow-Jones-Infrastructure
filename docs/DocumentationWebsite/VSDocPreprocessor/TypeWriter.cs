using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace VSDocPreprocessor
{
    public class TypeWriter
    {
        private readonly DocumentEntityConverter _converter;

        public Encoding Encoding { get; set; }

        public string OutputDirectory { get; set; }

        public bool SingleFile { get; set; }


        public TypeWriter(DocumentEntityConverter converter = null)
        {
            _converter = converter ?? new DocumentEntityConverter();
            Encoding = Encoding.UTF8;
            OutputDirectory = Directory.GetCurrentDirectory();
            SingleFile = false;
        }


        public void Write(Type type)
        {
            XDocument document = ConvertToXDocument(type);

            if (document == null || document.Root == null || !document.Root.HasElements)
                return;

            var directory = InitializeDirectory(type);

            if(SingleFile)
            {
                var filename = Path.Combine(directory, type.Name + ".xml");
                document.Save(filename);
            }
            else
            {
                RenderChild(directory, "Constructors", document);
                RenderChild(directory, "Events", document);
                RenderChild(directory, "Methods", document);
                RenderChild(directory, "Properties", document);
            }
        }

        protected virtual string InitializeDirectory(Type type)
        {
            string directory = Path.Combine(OutputDirectory, type.Name);
            
            if(!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }

        private XDocument ConvertToXDocument(Type type)
        {
            try
            {
                return _converter.Convert(type);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error serializing {0}: {1}", type.FullName, ex);
            }

            return null;
        }

        private void RenderChild(string directory, string name, XDocument document)
        {
            var element = document.Root.Element(name.ToLower());
            var filename = Path.Combine(directory, name + ".xml");

            try
            {
                element.Save(filename);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error saving {0} to {1}: {2}", 
                                 name, filename, ex);
            }
        }
    }
}