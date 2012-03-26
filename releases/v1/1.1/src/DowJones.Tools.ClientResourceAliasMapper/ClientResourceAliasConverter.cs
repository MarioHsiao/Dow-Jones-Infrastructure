using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DowJones.Tools.ClientResourceAliasMapper
{
    public class ClientResourceAliasConverter : ClientResourceCommand
    {
        public override string Command
        {
            get { return "convert"; }
        }

        public ClientResourceAliasConverter(TextWriter writer) : base(writer)
        {
        }

        protected override void ExecuteInternal(IEnumerable<string> args)
        {
            var configFilename = GetArg(args, 0, "ClientResources.xml");

            bool invalidFilenameParameter = string.IsNullOrEmpty(configFilename);
            bool configFileDoesntExist = !File.Exists(configFilename);
            bool isInvalidConfig = invalidFilenameParameter || configFileDoesntExist;

            if (isInvalidConfig)
            {
                if (configFileDoesntExist)
                    Writer.WriteLine("Filename {0} does not exist", configFilename);

                ShowHelp(Writer);
                return;
            }

            var configuration = ClientResourceConfiguration.Load(configFilename);

            int incrementer = 1;

            foreach(var alias in configuration.Aliases.ToArray())
            {
                // If the current alias is already an int,
                // just skip it and continue on
                int currentAliasId;
                if (int.TryParse(alias.Alias, out currentAliasId))
                    continue;

                alias.OriginalAlias = alias.Alias;
                alias.Alias = (incrementer++).ToString();
            }

            configuration.Save(configFilename);

            Writer.WriteLine("Converted aliases to incremental numbers");
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} {1} [filename]", 
                             Assembly.GetEntryAssembly().GetName().Name, Command);
        }
    }
}