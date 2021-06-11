using System;
using System.Reflection;

namespace AdvancedConsole.Commands.CommandParsing
{
    public interface ICommandParser
    {
        string[] ParseTokens(string command);
        bool TryParseArgs(string[] inputs, ParameterInfo[] parameters, out object[] args);
    }
}