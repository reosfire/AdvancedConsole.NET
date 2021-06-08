using System;
using System.Net.Sockets;

namespace AdvancedConsole.Commands.TypesParsing
{
    public static class DefaultTypesParsers
    {
        public static void AddDefaultTypesParsers(this TypesParser parser)
        {
            parser.AddParser(new FromDelegateTypeParser<bool>(bool.TryParse));
            parser.AddParser(new FromDelegateTypeParser<byte>(byte.TryParse));
            parser.AddParser(new FromDelegateTypeParser<sbyte>(sbyte.TryParse));
            parser.AddParser(new FromDelegateTypeParser<int>(int.TryParse));
            parser.AddParser(new FromDelegateTypeParser<string>(StringParser));
        }

        private static bool StringParser(string input, out string result)
        {
            result = input;
            return true;
        }
    }
}