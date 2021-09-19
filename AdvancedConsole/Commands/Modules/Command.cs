using System;
using System.Collections.Generic;
using System.Reflection;

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
        
        public object Execute(object[] args, Dictionary<Type, object> executionContextsCache) //TODO command shouldn't mutate parameter
        {
            Type declaringType = Method.DeclaringType;
            if (declaringType is null) throw new Exception("declaring type can not be null to execute command"); //TODO check this when building tree
            if (!executionContextsCache.ContainsKey(declaringType))
                executionContextsCache.Add(declaringType, Activator.CreateInstance(declaringType));
            return Method.Invoke(executionContextsCache[declaringType], args);
        }
        public object Execute(Dictionary<Type, object> executionContextsCache)
        {
            return Execute(new object[0], executionContextsCache);
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