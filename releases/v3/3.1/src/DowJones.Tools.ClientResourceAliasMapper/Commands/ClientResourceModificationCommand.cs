using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DowJones.Tools.ClientResourceAliasMapper.Mappings;

namespace DowJones.Tools.ClientResourceAliasMapper.Commands
{
    public abstract class ClientResourceModificationCommand : ClientResourceCommand
    {
        protected IEnumerable<string> Args { get; private set; }

        public IEnumerable<string> Exclusions
        {
            get { return _exclusions ?? ParseExclusionsFromArgs(); }
            set { _exclusions = value; }
        }
        private IEnumerable<string> _exclusions;

        private IEnumerable<string> ParseExclusionsFromArgs()
        {
            var enumerator = Args.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals("-Exclude", StringComparison.OrdinalIgnoreCase))
                {
                    enumerator.MoveNext();
                    yield return enumerator.Current;
                }
            }
        }


        protected ClientResourceModificationCommand(TextWriter writer) : base(writer)
        {
        }
        

        protected override void ExecuteInternal(IEnumerable<string> args)
        {
            Args = args;

            var configFilename = GetArg(Args, 0, "ClientResources.xml");

            ClientResourceConfiguration configuration;

            if (!TryLoadConfiguration(configFilename, out configuration))
                return;

            Execute(configuration);

            configuration.Save(configFilename);

            Writer.WriteLine("Converted aliases to incremental numbers");

            Args = null;
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

        protected bool IsNotExcluded(ClientResourceAliasMapping alias)
        {
            // If the current alias is already an int,
            // just skip it and continue on
            int currentAliasId;
            if (int.TryParse(alias.Alias, out currentAliasId))
                return false;

            if (Exclusions.Contains(alias.ResourceName) || Exclusions.Contains(alias.Alias))
                return false;

            return true;
        }

        protected abstract void Execute(ClientResourceConfiguration configuration);
    }
}