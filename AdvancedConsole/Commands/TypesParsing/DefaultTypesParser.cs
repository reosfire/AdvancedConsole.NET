using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace AdvancedConsole.Commands.TypesParsing
{
    public class DefaultTypesParser : TypesParser
    {
        public DefaultTypesParser()
        {
            AddParser<bool>(bool.TryParse);
            AddParser<byte>(byte.TryParse);
            AddParser<sbyte>(sbyte.TryParse);
            AddParser<ushort>(ushort.TryParse);
            AddParser<short>(short.TryParse);
            AddParser<int>(int.TryParse);
            AddParser<uint>(uint.TryParse);
            AddParser<long>(long.TryParse);
            AddParser<ulong>(ulong.TryParse);
            AddParser<float>(float.TryParse);
            AddParser<double>(double.TryParse);
            AddParser<decimal>(decimal.TryParse);
            AddParser<char>(char.TryParse);
            AddParser<string>(StringParser);
        }
        private static bool StringParser(string input, out string result)
        {
            result = input;
            return true;
        }
    }
}