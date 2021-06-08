using System;
using System.Reflection;
using AdvancedConsole.Commands.Modules;

namespace AdvancedConsole.Commands.CommandParsing
{
    public struct CommandToken
    {
        public string[] Path { get; private set; }
        public Type OutputType { get; private set; }

        public CommandToken(string[] path, Type outputType)
        {
            Path = path;
            OutputType = outputType;
        }
        public CommandToken(string[] path) : this(path, typeof(void)){}
    }
}