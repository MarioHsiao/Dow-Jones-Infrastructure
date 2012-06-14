using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
            Contract.Requires(type != null);

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
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(OutputDirectory));
            Contract.Requires(!string.IsNullOrWhiteSpace(type.Assembly));
            Contract.Requires(!string.IsNullOrWhiteSpace(type.Name));

            string relativePath = Path.Combine(type.Assembly, type.Name);
            string directory = Path.Combine(OutputDirectory, relativePath);
            
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
            Contract.Requires(!string.IsNullOrWhiteSpace(directory));
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(document != null);
            Contract.Requires(document.Root != null);

            var filename = Path.Combine(directory, name + ".xml");

            var elementName = name.ToLower();
            var element = document.Root.Element(elementName);

            if (element == null || element.HasElements == false)
            {
                Trace.WriteLine("Element {0} is empty - skipping...", elementName);
                return;
            }   
             
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