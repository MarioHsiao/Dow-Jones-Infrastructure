using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using DowJones.Tools.ClientResourceAliasMapper.Commands;

namespace DowJones.Tools.ClientResourceAliasMapper
{
    class Program : ClientResourceCommand
    {
        protected static TextWriter _writer = Console.Out;
        protected static IEnumerable<IClientResourceCommand> Commands = new IClientResourceCommand[] {
                new ClientResourceAliasConverter(_writer),
                new ClientResourceAliasDeconverter(_writer),
                new ClientResourceConfigurationGenerator(_writer)
            };

        public Program(TextWriter writer) : base(writer)
        {
        }

        public static void Main(string[] args)
        {
            _writer.WriteLine();
            _writer.WriteLine("*****  Client Resource Mapping Tool  *****");
            _writer.WriteLine();

            IClientResourceCommand command = null;

            if (args.Length > 0)
            {
                command = Commands.FirstOrDefault(x => string.Equals(x.Command, args[0], StringComparison.OrdinalIgnoreCase));
                args = args.Skip(1).ToArray();
            }

            command = command ?? new Program(_writer);

            try
            {
                command.Execute(args);
            }
            catch(Exception ex)
            {
                _writer.WriteLine("\r\n\r\n** Error **\r\n" + ex.Message);
            }

            _writer.WriteLine();
            _writer.WriteLine("Press any key...");
            Console.ReadKey();
        }

        public override string Command
        {
            get { throw new NotImplementedException(); }
        }

        public override void Execute(IEnumerable<string> args)
        {
            ShowHelp(Writer);
        }

        protected override void ExecuteInternal(IEnumerable<string> args)
        {
            throw new NotImplementedException();
        }

        public override void ShowHelp(TextWriter writer)
        {
            writer.WriteLine("Usage:  {0} [command] [parameters]", Assembly.GetEntryAssembly().GetName().Name);
            writer.WriteLine();
            writer.WriteLine("   command:       one of: [{0}]", string.Join(", ", Commands.Select(x => x.Command)));
            writer.WriteLine("   parameters:    command-specific parameters");
            writer.WriteLine();
        }
    }
}
