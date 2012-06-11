using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VSDocSplitter;

namespace VSDocPreprocessor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var program = new Program();

            IEnumerable<string> vsDocFiles = args;

            if (Directory.Exists(vsDocFiles.First()))
            {
                program.OutputDirectory = args.First();
                vsDocFiles = vsDocFiles.Skip(1);
            }

            if (!vsDocFiles.Any())
            {
                Trace.TraceInformation("No VSDoc files specified - discovering from current directory");
                var currentDirectory = Directory.GetCurrentDirectory();
                vsDocFiles = Directory.GetFiles(currentDirectory, "*.xml");
            }

            program.TransformVSDocFiles(vsDocFiles.ToArray());

            System.Console.WriteLine("DONE!");
            System.Console.ReadLine();
        }


        private readonly Parser _parser;
        private readonly Serializer _serializer;

        public string OutputDirectory { get; set; }

        public Program()
        {
            _parser = new Parser();
            _serializer = new Serializer();
            OutputDirectory = Directory.GetCurrentDirectory();
        }

        public void TransformVSDocFiles(IEnumerable<string> filenames)
        {
            var existingFiles = filenames.Where(File.Exists).ToArray();

            Trace.TraceInformation("Output directory: {0}", OutputDirectory);
            Trace.TraceInformation("Found {0} VSDoc files", existingFiles.Count());

            foreach (var vsDocFile in existingFiles)
            {
                TransformVSDocFile(vsDocFile);
            }
        }

        public void TransformVSDocFile(string filename)
        {
            Trace.TraceInformation("\r\nTransforming {0}...", filename);
            Trace.IndentLevel += 1;

            var types = Parse(filename).ToArray();

            Trace.TraceInformation("Found {0} Types", types.Count());

            foreach (var type in types)
            {
                WriteToDisk(type);
            }

            Trace.IndentLevel -= 1;
            Trace.TraceInformation("Finished transforming {0}.", filename);
        }

        private void WriteToDisk(Type type)
        {
            string filename = Path.Combine(OutputDirectory, type.FullName + ".xml");

            Trace.TraceInformation("Writing {0} to {1}...", type.FullName, filename);

            try
            {
                using(var writer = new StreamWriter(filename, false))
                {
                    _serializer.Serialize(type, writer);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error writing {0} to disk: {1}", type.FullName, ex);
            }
        }

        private IEnumerable<Type> Parse(string vsDocFile)
        {
            try
            {
                Trace.TraceInformation("Parsing {0}...", vsDocFile);

                var types = _parser.Parse(vsDocFile);

                Trace.TraceInformation("Done parsing {0}.", vsDocFile);

                return types;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error parsing {0}: {1}", vsDocFile, ex);
                return Enumerable.Empty<Type>();
            }
        }
    }
}
