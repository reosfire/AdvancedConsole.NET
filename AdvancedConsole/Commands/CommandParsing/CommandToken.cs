using System;
using System.Reflection;

namespace AdvancedConsole.Commands.CommandParsing
{
    public struct CommandToken
    {
        public string[] Path { get; private set; }
        public Type[] InputParametersTypes { get; private set; }
        public Type OutputType { get; private set; }

        public CommandToken(string[] path, Type[] inputParametersTypes, Type outputType)
        {
            Path = path;
            InputParametersTypes = inputParametersTypes;
            OutputType = outputType;
        }
        public CommandToken(string[] path) : this(path, new Type[0], typeof(void)){}
    }
}