using System.IO;

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
            writer.WriteLine("Usage:  {0} {1} [filename]",
                             ExecutableName, Command);
        }

        protected override void Execute(ClientResourceConfiguration configuration)
        {
            foreach (var alias in configuration.Aliases.ToArray())
            {
                alias.Alias = alias.OriginalAlias;
                alias.OriginalAlias = null;
            }
        }
    }
}