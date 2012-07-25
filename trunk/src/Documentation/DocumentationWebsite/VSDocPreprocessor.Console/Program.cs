using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VSDocPreprocessor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var typeWriter = new TypeWriter();
            var processor = new Processor(typeWriter: typeWriter);

            IEnumerable<string> vsDocFiles = args;

            if (Directory.Exists(vsDocFiles.First()))
            {
                typeWriter.OutputDirectory = args.First();
                vsDocFiles = vsDocFiles.Skip(1);
            }

            if (!vsDocFiles.Any())
            {
                Trace.TraceInformation("No VSDoc files specified - discovering from current directory");
                var currentDirectory = Directory.GetCurrentDirectory();
                vsDocFiles = Directory.GetFiles(currentDirectory, "*.xml");
            }

            Trace.TraceInformation("Output directory: {0}", typeWriter.OutputDirectory);
            processor.TransformVSDocFiles(vsDocFiles.ToArray());

            System.Console.WriteLine("DONE!");
            System.Console.ReadLine();
        }
    }
}
