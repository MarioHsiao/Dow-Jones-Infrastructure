using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DowJones.Tools.ClientResourceAliasMapper.Mappings;

namespace DowJones.Tools.ClientResourceAliasMapper.Commands
{
    public class ClientResourceConfigurationGenerator : ClientResourceCommand
    {
        public const string ClientResourcesFilename = "ClientResources.xml";

        public static readonly Func<string, string> GenerateDebugResourceAlias =
            name => Regex.Match(name, @".*[./\\](.*(-.*)?[./\\]\w*)").Groups[1].Value;

        private static int MaxDuplicateFixLoopCount = 5;

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
            string outFile = GetArg(args, 1, Path.Combine(Environment.CurrentDirectory, ClientResourcesFilename));

            var existingMappings = Enumerable.Empty<ClientResourceMapping>();

            if (ConfigurationFileExists(outFile))
            {
                if (!VerifyCanCreateConfiguration(outFile))
                {
                    Writer.WriteLine("\nSkipping config file generation...");
                    return;
                }

                var configuration = ClientResourceConfiguration.Load(outFile);
                existingMappings = configuration.Mappings;

                string backupFile = outFile + ".bak";
                File.Copy(outFile, backupFile, true);
                File.SetAttributes(backupFile, FileAttributes.Normal);

                Writer.WriteLine("\r\nBacked up original file to " + backupFile);
            }

            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";

            var embeddedResources = GetEmbeddedResourceNames(Path.Combine(directory, "bin")).ToArray();
            Writer.WriteLine("Found {0} Client Resource Attributes", embeddedResources.Count());
            Debug.WriteLine(string.Join("\r\n", embeddedResources));

            var resourceNames = embeddedResources.Union(existingMappings.Select(x => x.Name));

            RenderResourceFile(existingMappings, resourceNames, outFile);

            Writer.WriteLine("Wrote mappings to {0}", outFile);
        }

        private bool VerifyCanCreateConfiguration(string outFile)
        {
            Writer.Write("Ouput file {0} already exists - overwrite (y/n)? ", outFile);
            return Console.ReadKey().Key == ConsoleKey.Y;
        }

        private static bool ConfigurationFileExists(string outFile)
        {
            return File.Exists(outFile);
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} {1} [directory] [output file]", Assembly.GetEntryAssembly().GetName().Name, Command);
            writer.WriteLine();
            writer.WriteLine("   directory:     the root directory of the website to generate the resource mappings for");
            writer.WriteLine("   output file:   full path to the file to write the alias mappings [default: ClientResources.xml]");
            writer.WriteLine();
        }

        private static void RenderResourceFile(IEnumerable<ClientResourceMapping> resources, IEnumerable<string> resourceNames, string outFile)
        {
            var aliasMappings =
                from name in resourceNames
                let alias = GenerateDebugResourceAlias(name)
                orderby name
                select new ClientResourceAliasMapping
                           {
                               Alias = alias, 
                               ResourceName = name,
                           };

            aliasMappings = FixDuplicateAliases(aliasMappings.ToArray());

            var config = new ClientResourceConfiguration(resources, aliasMappings);
            config.Save(outFile);
        }

        private static IEnumerable<ClientResourceAliasMapping> FixDuplicateAliases(IEnumerable<ClientResourceAliasMapping> aliasMappings, int? level = null)
        {
            var duplicates = 
                aliasMappings
                    .GroupBy(x => x.Alias)
                    .Where(x => x.Count() > 1)
                    .SelectMany(x => x);

            // If we've eliminated all dupes or tried too many times, quit
            if(duplicates.Count() == 0 || level > MaxDuplicateFixLoopCount)
                return aliasMappings;

            // Iterate through and use the next peice of the path
            // to further qualify/uniquify the alias
            // e.g. for "DowJones.Web.Mvc.UI.Canvas.Components.[Qualifier].[Alias]"
            //      the new alias should be [Qualifier].[Alias]
            foreach(var mapping in aliasMappings)
            {
                var name = mapping.ResourceName;
                var @alias = mapping.Alias;

                var substringLength = name.Length - @alias.Length - 1;
                if(substringLength < 0)
                    continue;

                var prefix = name.Substring(0, substringLength);
                var qualifier = prefix.Substring(prefix.LastIndexOf('.') + 1);

                mapping.Alias = string.Format("{0}.{1}", qualifier, alias);
            }

            return FixDuplicateAliases(aliasMappings, (level ?? 0)+1);
        }

        private static IEnumerable<string> GetEmbeddedResourceNames(string path)
        {
            Type clientResourceAttribute = GetClientResourceAttribute(path);

            IEnumerable<dynamic> attributes =
                from assemblyName in Directory.GetFiles(path, "DowJones*.dll")
                let assembly = LoadAssembly(assemblyName)
                from type in assembly.GetExportedTypes()
                from attribute in type.GetCustomAttributes(true)
                where clientResourceAttribute.IsAssignableFrom(attribute.GetType())
                select (dynamic)attribute;

            return attributes.Select(x => (string)x.ResourceName).Distinct();
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
