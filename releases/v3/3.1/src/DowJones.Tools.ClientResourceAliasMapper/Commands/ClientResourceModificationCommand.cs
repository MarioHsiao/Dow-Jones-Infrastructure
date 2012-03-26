using System.Collections.Generic;
using System.IO;

namespace DowJones.Tools.ClientResourceAliasMapper.Commands
{
    public abstract class ClientResourceModificationCommand : ClientResourceCommand
    {
        protected ClientResourceModificationCommand(TextWriter writer) : base(writer)
        {
        }
        
        protected override void ExecuteInternal(IEnumerable<string> args)
        {
            var configFilename = GetArg(args, 0, "ClientResources.xml");

            ClientResourceConfiguration configuration;

            if (!TryLoadConfiguration(configFilename, out configuration))
                return;

            Execute(configuration);

            configuration.Save(configFilename);

            Writer.WriteLine("Converted aliases to incremental numbers");
        }

        protected bool TryLoadConfiguration(string configFilename, out ClientResourceConfiguration configuration)
        {
            bool invalidFilenameParameter = string.IsNullOrEmpty(configFilename);
            bool configFileDoesntExist = !File.Exists(configFilename);
            bool isInvalidConfig = invalidFilenameParameter || configFileDoesntExist;

            if (isInvalidConfig)
            {
                if (configFileDoesntExist)
                    Writer.WriteLine("Filename {0} does not exist", configFilename);

                ShowHelp(Writer);

                configuration = null;

                return false;
            }

            configuration = ClientResourceConfiguration.Load(configFilename);

            return true;
        }

        protected abstract void Execute(ClientResourceConfiguration configuration);
    }
}