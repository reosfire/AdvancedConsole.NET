using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AdvancedConsole.Commands.CommandParsing;
using AdvancedConsole.Commands.Modules.Building;

namespace AdvancedConsole.Commands.Modules
{
    public class ModulesTree
    {
        private readonly List<Command> _commands = new List<Command>();
        private readonly List<Module> _modules = new List<Module>();
        public IReadOnlyCollection<Command> Commands => _commands;
        public IReadOnlyCollection<Module> Modules => _modules;
        public IEnumerable<ITreeNode> Nodes => _commands.Concat(_modules.Cast<ITreeNode>());

        public void Add<T>(IModuleBuilder<T> moduleBuilder, T dataToBuild)
        {
            Module buildingResult = moduleBuilder.Build(dataToBuild);
            _commands.AddRange(buildingResult.Commands);
            _modules.AddRange(buildingResult.SubModules);
        }

        public IEnumerable<ITreeNode> GetNodes(string[] path)
        {
            if(path.Length == 0) yield break;
            foreach (ITreeNode treeNode in Nodes)
            {
                if(treeNode.Name != path[0] && !treeNode.Aliases.Contains(path[0])) continue;
                foreach (ITreeNode node in treeNode.GetNodes(path[1..]))
                {
                    yield return node;
                }
            }
        }
        private IEnumerable<Command> GetAllCommands(string[] path)
        {
            return GetNodes(path).OfType<Command>();
        }
        public IEnumerable<Command> GetCommands(CommandToken token)
        {
            foreach (Command command in GetAllCommands(token.Path))
            {
                if (command.IsWithSignature(token.Signature)) yield return command;
            }
        }
    }
}