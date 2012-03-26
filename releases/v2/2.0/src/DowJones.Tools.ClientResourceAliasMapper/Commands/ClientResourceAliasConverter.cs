using System.IO;
using System.Reflection;

namespace DowJones.Tools.ClientResourceAliasMapper.Commands
{
    public class ClientResourceAliasConverter : ClientResourceModificationCommand
    {
        public override string Command
        {
            get { return "convert"; }
        }

        public ClientResourceAliasConverter(TextWriter writer) : base(writer)
        {
        }

        protected override void Execute(ClientResourceConfiguration configuration)
        {
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
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} {1} [filename]", 
                             ExecutableName, Command);
        }
    }
}