using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DowJones.Tools.ClientResourceAliasMapper.Commands
{
    public interface IClientResourceCommand
    {

        string Command { get; }

        void Execute(IEnumerable<string> args);

        void ShowHelp(TextWriter writer);

    }

    public abstract class ClientResourceCommand : IClientResourceCommand
    {
        protected readonly string ExecutableName = Assembly.GetEntryAssembly().GetName().Name;
        protected TextWriter Writer { get; private set; }

        protected ClientResourceCommand(TextWriter writer)
        {
            Writer = writer;
        }

        public virtual void Execute(IEnumerable<string> args)
        {
            if(args == null || !args.Any())
            {
                ShowHelp(Writer);
                return;
            }

            ExecuteInternal(args);
        }

        public abstract string Command { get; }
        protected abstract void ExecuteInternal(IEnumerable<string> args);
        public abstract void ShowHelp(TextWriter writer);

        protected string GetArg(IEnumerable<string> args, int position, string defaultValue)
        {
            var array = (args as string[]) ?? args.ToArray();
            return (array.Length > position) ? array[position] : defaultValue;
        }
    }
}