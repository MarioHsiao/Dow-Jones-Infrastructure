using System.IO;
using System.Linq;

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

            var aliasesToConvert = configuration.Aliases.Where(IsNotExcluded).ToArray();
            
            foreach(var alias in aliasesToConvert)
            {
                alias.OriginalAlias = alias.Alias;
                alias.Alias = (incrementer++).ToString();
            }
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} {1} [filename] (-Exclude Alias|ResourceName)", 
                             ExecutableName, Command);
        }
    }
}