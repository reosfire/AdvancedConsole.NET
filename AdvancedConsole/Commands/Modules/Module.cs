using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedConsole.Commands.Modules
{
    public class Module : ITreeNode
    {
        private List<Command> _commands;
        private List<Module> _subModules;
        public IReadOnlyCollection<Command> Commands => _commands;
        public IReadOnlyCollection<Module> SubModules => _subModules;
        public string Name { get; private set; }
        public IReadOnlyCollection<string> Aliases { get; private set; }

        public IEnumerable<ITreeNode> SubNodes => _commands.Concat(_subModules.Cast<ITreeNode>());

        public class Builder
        {
            private Module Built { get; set; }
            private List<string> Aliases { get; set; } = new List<string>();
            private List<Module> SubModules { get; set; } = new List<Module>();
            private List<Command> Commands { get; set; } = new List<Command>();

            public Builder()
            {
                Built = new Module();
            }
            public Builder(Module module)
            {
                Built = module;
            }

            public Builder SetName(string name)
            {
                Built.Name = name;
                return this;
            }
            public Builder AddAlias(string alias)
            {
                Aliases.Add(alias);
                return this;
            }
            public Builder AddAliases(IEnumerable<string> aliases)
            {
                foreach (string alias in aliases)
                {
                    AddAlias(alias);
                }

                return this;
            }
            
            public Builder AddSubModule(Module subModule)
            {
                if (subModule is null) throw new ArgumentNullException(nameof(subModule));
                SubModules.Add(subModule);
                return this;
            }
            public Builder AddSubModules(IEnumerable<Module> subModules)
            {
                if (subModules is null) throw new ArgumentNullException(nameof(subModules));
                SubModules.AddRange(subModules);
                return this;
            }
            public Builder SetSubModules(List<Module> subModules)
            {
                if (subModules is null) throw new ArgumentNullException(nameof(subModules));
                SubModules = subModules;
                return this;
            }
            public Builder AddCommand(Command command)
            {
                if (command is null) throw new ArgumentNullException(nameof(command));
                Commands.Add(command);
                return this;
            }
            public Builder AddCommands(IEnumerable<Command> commands)
            {
                if (commands is null) throw new ArgumentNullException(nameof(commands));
                Commands.AddRange(commands);
                return this;
            }
            public Builder SetCommands(List<Command> commands)
            {
                if (commands is null) throw new ArgumentNullException(nameof(commands));
                Commands = commands;
                return this;
            }

            public Module Build()
            {
                Built.Aliases = Aliases;
                Built._subModules = SubModules;
                Built._commands = Commands;
                return Built;
            }
        }
    }
}