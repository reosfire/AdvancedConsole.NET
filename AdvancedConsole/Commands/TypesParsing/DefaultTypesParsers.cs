using System;
using System.Net.Sockets;

namespace AdvancedConsole.Commands.TypesParsing
{
    public static class DefaultTypesParsers
    {
        public static void AddDefaultTypesParsers(this TypesParser parser)
        {
            parser.AddParser<bool>(bool.TryParse);
            parser.AddParser<byte>(byte.TryParse);
            parser.AddParser<sbyte>(sbyte.TryParse);
            parser.AddParser<ushort>(ushort.TryParse);
            parser.AddParser<short>(short.TryParse);
            parser.AddParser<int>(int.TryParse);
            parser.AddParser<uint>(uint.TryParse);
            parser.AddParser<long>(long.TryParse);
            parser.AddParser<ulong>(ulong.TryParse);
            parser.AddParser<float>(float.TryParse);
            parser.AddParser<double>(double.TryParse);
            parser.AddParser<decimal>(decimal.TryParse);
            parser.AddParser<char>(char.TryParse);
            parser.AddParser<string>(StringParser);
        }

        private static bool StringParser(string input, out string result)
        {
            result = input;
            return true;
        }
    }
}