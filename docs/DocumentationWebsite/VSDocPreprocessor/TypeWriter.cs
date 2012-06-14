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

        public bool AssemblyFolders { get; set; }

        public Encoding Encoding { get; set; }

        public string OutputDirectory { get; set; }

        public bool SingleFile { get; set; }


        public TypeWriter(DocumentEntityConverter converter = null)
        {
            _converter = converter ?? new DocumentEntityConverter();
            AssemblyFolders = true;
            Encoding = Encoding.UTF8;
            OutputDirectory = Directory.GetCurrentDirectory();
            SingleFile = false;
        }


        public void Write(Type type)
        {
            Contract.Requires(type != null);

            Trace.TraceInformation("Writing {0} to disk...", type.FullName);

            XDocument document = ConvertToXDocument(type);

            if (document == null || document.Root == null || !document.Root.HasElements)
                return;

            string assemblyDirectory = Path.Combine(OutputDirectory, type.Assembly);

            EnsureDirectoryExists(assemblyDirectory);

            if(SingleFile)
            {
                var filename = Path.Combine(assemblyDirectory, type.Name + ".xml");
                document.Save(filename);
            }
            else
            {
                var typeDirectory = Path.Combine(assemblyDirectory, type.Name);
                RenderChild(typeDirectory, "Constructors", document);
                RenderChild(typeDirectory, "Events", document);
                RenderChild(typeDirectory, "Methods", document);
                RenderChild(typeDirectory, "Properties", document);
            }
        }

        private static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
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

            var elementName = name.ToLower();
            var element = document.Root.Element(elementName);

            if (element == null || element.HasElements == false)
            {
                Trace.WriteLine(string.Format("Element {0} is empty - skipping...", elementName));
                return;
            }

            EnsureDirectoryExists(directory);

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