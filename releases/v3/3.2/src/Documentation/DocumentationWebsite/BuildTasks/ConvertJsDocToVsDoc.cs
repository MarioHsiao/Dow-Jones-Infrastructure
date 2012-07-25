using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using JsXmlDocParser;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DowJones.Documentation.BuildTasks
{
    public class ConvertJsDocToVsDoc : Task
    {
        private readonly JsDocToVsDocConverter _converter;

        public bool ContinueOnError
        {
            get { return _converter.ContinueOnError; }
            set { _converter.ContinueOnError = value; }
        }

        public string Filename { get; set; }

        [Required]
        public ITaskItem[] JsDocFiles { get; set; }

        [Output]
        public ITaskItem[] VsDocFiles
        {
            get { return _vsDocFiles.ToArray(); }
            set { _vsDocFiles = value; }
        }
        private IList<ITaskItem> _vsDocFiles;


        public ConvertJsDocToVsDoc()
        {
            _converter = new JsDocToVsDocConverter();
            _vsDocFiles = new List<ITaskItem>();
        }


        public override bool Execute()
        {
            var jsDocFiles = JsDocFiles.Select(x => x.ItemSpec).Where(x => !string.IsNullOrWhiteSpace(x)).Where(File.Exists).ToArray();
            
            var writerSettings = new XmlWriterSettings {
                                         Encoding = Encoding.UTF8,
                                         Indent = true,
                                         IndentChars = "\t",
                                         CloseOutput = true,
                                     };

            var assemblies = jsDocFiles.Where(x => x.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)).ToArray();
            foreach (var assembly in assemblies)
            {
                ConvertAssembly(assembly, writerSettings);
            }

            ConvertPhysicalFiles(Filename, jsDocFiles.Except(assemblies), writerSettings);

            return true;
        }

        /// <summary>
        /// Converts multiple physical JSDoc files into a single VSDoc file
        /// </summary>
        internal void ConvertAssembly(string assemblyFilename, XmlWriterSettings writerSettings)
        {
            Log.LogMessage("Converting JsDocs in assembly {0}...", assemblyFilename);

            string vsDocFilename = Path.Combine(
                Path.GetDirectoryName(assemblyFilename),
                Path.GetFileNameWithoutExtension(assemblyFilename) + ".jsdoc.xml");

            var assembly = Assembly.LoadFile(assemblyFilename);
            
            using (var writer = XmlWriter.Create(vsDocFilename, writerSettings))
            {
                _converter.Convert(assembly, writer);
                writer.Flush();
            }

            AddVsDocFile(vsDocFilename);
        }

        /// <summary>
        /// Converts multiple physical JSDoc files into a single VSDoc file
        /// </summary>
        internal void ConvertPhysicalFiles(string filename, IEnumerable<string> jsDocFiles, XmlWriterSettings writerSettings)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(filename));
            Contract.Requires(jsDocFiles != null);

            var filenames = jsDocFiles.ToArray();

            Log.LogMessage("Converting JsDoc in {0}...", string.Join(", ", filenames));

            var assemblyName = Path.GetFileNameWithoutExtension(filename);

            using(var writer = XmlWriter.Create(filename, writerSettings))
            {
                _converter.Convert(filenames, writer, assemblyName);
                writer.Flush();
            }

            AddVsDocFile(filename);
        }

        private void AddVsDocFile(string vsDocFile)
        {
            Log.LogMessage("Generated VSDoc file {0}", vsDocFile);
            _vsDocFiles.Add(new TaskItem(vsDocFile));
        }
    }
}