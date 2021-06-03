using System;

namespace AdvancedConsole.Commands.CommandParsing
{
    public struct CommandToken
    {
        public string[] Path { get; private set; }
        public Type[] Signature { get; private set; }

        public CommandToken(string[] path, Type[] signature)
        {
            Path = path;
            Signature = signature;
        }
        public CommandToken(string[] path) : this(path, new Type[0]){}
    }
}