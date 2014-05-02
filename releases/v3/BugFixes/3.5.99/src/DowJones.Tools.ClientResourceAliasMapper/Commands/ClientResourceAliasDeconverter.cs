using System.IO;
using System.Linq;

namespace DowJones.Tools.ClientResourceAliasMapper.Commands
{
    public class ClientResourceAliasDeconverter : ClientResourceModificationCommand
    {
        public override string Command
        {
            get { return "deconvert"; }
        }

        public ClientResourceAliasDeconverter(TextWriter writer) : base(writer)
        {
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} {1} [filename] (-Exclude Alias|ResourceName)",
                             ExecutableName, Command);
        }

        protected override void Execute(ClientResourceConfiguration configuration)
        {
            var aliasesToConvert = configuration.Aliases.Where(IsNotExcluded).ToArray();

            foreach (var alias in aliasesToConvert)
            {
                alias.Alias = alias.OriginalAlias;
                alias.OriginalAlias = null;
            }
        }
    }
}