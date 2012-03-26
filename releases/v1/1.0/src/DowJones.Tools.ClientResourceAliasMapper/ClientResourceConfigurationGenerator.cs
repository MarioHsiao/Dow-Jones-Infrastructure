using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DowJones.Tools.ClientResourceAliasMapper
{
    public class ClientResourceConfigurationGenerator : ClientResourceCommand
    {
        public static readonly Func<string, string> GenerateDebugResourceAlias =
            name => Regex.Match(name, @".*\.(.*(-.*)?)\.\w*").Groups[1].Value;

        public override string Command
        {
            get { return "generate"; }
        }

        public ClientResourceConfigurationGenerator(TextWriter writer) : base(writer)
        {
        }

        protected override void ExecuteInternal(IEnumerable<string> args)
        {
            string directory = GetArg(args, 0, Environment.CurrentDirectory);
            string outFile = GetArg(args, 1, Path.Combine(Environment.CurrentDirectory, "ClientResources.xml"));
        
            if(!VerifyCanCreateConfiguration(outFile))
            {
                Writer.WriteLine("\nSkipping config file generation...");
                return;
            }   
         
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";

            var embeddedResources = GetEmbeddedResourceNames(Path.Combine(directory, "bin"));
            var resources = embeddedResources.ToArray();

            Writer.WriteLine("Found {0} Client Resource Attributes", resources.Count());
            Debug.WriteLine(string.Join("\r\n", resources));

            RenderResourceFile(resources, outFile);

            Writer.WriteLine("Wrote mappings to {0}", outFile);
        }

        private bool VerifyCanCreateConfiguration(string outFile)
        {
            if(File.Exists(outFile))
            {
                Writer.Write("Ouputfile already exists - overwrite (y/n)? ");
                return Console.ReadKey().Key == ConsoleKey.Y;
            }

            return true;
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} {1} [directory] [output file]", Assembly.GetEntryAssembly().GetName().Name, Command);
            writer.WriteLine();
            writer.WriteLine("   directory:     the directory containing assemblies to reflect");
            writer.WriteLine("   output file:   full path to the file to write the alias mappings");
            writer.WriteLine();
        }

        private static void RenderResourceFile(IEnumerable<string> resourceNames, string outFile)
        {
            var aliasMappings =
                from name in resourceNames
                let alias = GenerateDebugResourceAlias(name)
                select new ClientResourceAliasMapping
                           {
                               Alias = alias, 
                               ResourceName = name,
                           };

            var config = new ClientResourceConfiguration(null, aliasMappings);
            config.Save(outFile);
        }

        private static IEnumerable<string> GetEmbeddedResourceNames(string path)
        {
            Type clientResourceAttribute = GetClientResourceAttribute(path);

            var attributes =
                from assemblyName in Directory.GetFiles(path, "DowJones*.dll")
                let assembly = LoadAssembly(assemblyName)
                from type in assembly.GetExportedTypes()
                from attribute in type.GetCustomAttributes(true)
                where clientResourceAttribute.IsAssignableFrom(attribute.GetType())
                select (dynamic)attribute;

            return attributes.Select(x => (string)x.ResourceName);
        }

        private static Type GetClientResourceAttribute(string path)
        {
            string infrastructureAssemblyLocation = Path.Combine(path, "DowJones.Infrastructure.dll");

            if (!File.Exists(infrastructureAssemblyLocation))
                throw new FileNotFoundException("Could not locate Dow Jones Infrastructure assembly", infrastructureAssemblyLocation);

            Assembly infrastructureAssembly = LoadAssembly(infrastructureAssemblyLocation);
            return infrastructureAssembly.GetType("DowJones.Web.ClientResourceAttribute");
        }

        private static Assembly LoadAssembly(string path)
        {
            try
            {
                return Assembly.LoadFrom(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
