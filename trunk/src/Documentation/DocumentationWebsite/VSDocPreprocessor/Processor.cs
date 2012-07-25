using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VSDocSplitter;

namespace VSDocPreprocessor
{
    public class Processor
    {
        private readonly Parser _parser;
        private readonly TypeWriter _typeWriter;

        public Processor(Parser parser = null, TypeWriter typeWriter = null)
        {
            _parser = parser ?? new Parser();
            _typeWriter = typeWriter ?? new TypeWriter();
        }

        public void TransformVSDocFiles(IEnumerable<string> filenames)
        {
            var existingFiles = filenames.Distinct().Where(File.Exists).ToArray();

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
                _typeWriter.Write(type);
            }

            Trace.IndentLevel -= 1;
            Trace.TraceInformation("Finished transforming {0}.", filename);
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
