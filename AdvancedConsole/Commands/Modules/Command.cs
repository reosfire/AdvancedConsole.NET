using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdvancedConsole.Commands.CommandParsing;
using AdvancedConsole.Commands.TypesParsing;

namespace AdvancedConsole.Commands.Modules
{
    public class Command : ITreeNode
    {
        public string Name { get; private set; }
        public IReadOnlyCollection<string> Aliases { get; private set; }
        public IEnumerable<ITreeNode> SubNodes => new ITreeNode[0];
        public MethodInfo Method { get; private set; }
        public ParameterInfo[] InputParameters => Method.GetParameters();
        public Type Output => Method.ReturnType;

        public bool TryExecute(IEnumerable<object>[] argsVariants)
        {
            if (argsVariants.Length != InputParameters.Length) return false;
            object[] args = new object[InputParameters.Length];
            bool allMatched = true;
            for (int i = 0; i < InputParameters.Length; i++)
            {
                Type requiredType = InputParameters[i].ParameterType;
                bool matched = false;
                foreach (object arg in argsVariants[i])
                {
                    if (arg.GetType() == requiredType)
                    {
                        matched = true;
                        args[i] = arg;
                        break;
                    }
                }
                if (!matched)
                {
                    allMatched = false;
                    break;
                }
            }

            if (allMatched) Execute(args);
            return allMatched;
        }
        public void Execute(object[] args)
        {
            Method.Invoke(Activator.CreateInstance(Method.DeclaringType),args);
        }
        public void Execute()
        {
            Method.Invoke(Activator.CreateInstance(Method.DeclaringType),new object[0]);
        }

        public class Builder
        {
            private Command Built { get; set; }
            private List<string> Aliases { get; set; } = new List<string>();

            public Builder()
            {
                Built = new Command();
            }
            public Builder(Command command)
            {
                Built = command;
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
            public Builder SetMethod(MethodInfo method)
            {
                Built.Method = method;
                return this;
            }

            public Command Build()
            {
                Built.Aliases = Aliases;
                return Built;
            }
        }
    }
}