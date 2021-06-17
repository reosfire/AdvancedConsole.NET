using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AdvancedConsole.Commands.TypesParsing;

namespace AdvancedConsole.Commands.CommandParsing
{
    public class SpacesCommandParser : ICommandParser
    {
        private TypesParser TypesParser { get; set; }

        public SpacesCommandParser(TypesParser typesParser)
        {
            TypesParser = typesParser;
        }

        public char SplitCharacter { get; set; } = ' ';
        public char ExplicitAssigmentCharacter { get; set; } = '=';

        public string[] ParseTokens(string command)
        {
            List<string> resultTokens = new();
            StringBuilder tokenBuilder = new();
            bool quoteOpened = false;
            char? previousCharacter = null;
            foreach (char c in command)
            {
                if (c == '"')
                {
                    if (quoteOpened && previousCharacter != '\\') quoteOpened = false;
                    else quoteOpened = true;
                }

                if (!quoteOpened && c == SplitCharacter && c != ExplicitAssigmentCharacter)
                {
                    resultTokens.Add(tokenBuilder.Replace("\\\"", "\"").ToString());
                    tokenBuilder.Clear();
                }
                else tokenBuilder.Append(c);

                previousCharacter = c;
            }

            if (tokenBuilder.Length != 0) resultTokens.Add(tokenBuilder.ToString());

            return resultTokens.ToArray();
        }

        public bool TryParseArgs(string[] inputs, ParameterInfo[] parameters, out object[] args)
        {
            if (inputs.Length != parameters.Length)
            {
                args = null;
                return false;
            }

            args = new object[parameters.Length];
            List<string> explicitArgs = new List<string>();
            List<string> implicitArgs = new List<string>();
            foreach (string input in inputs)
            {
                if (IsExplicitArg(input)) explicitArgs.Add(input);
                else implicitArgs.Add(input);
            }

            return TrySetExplicitArgs(explicitArgs, parameters, ref args)
                   && TrySetImplicitArgs(implicitArgs, parameters, ref args);
        }

        private bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

        private bool TrySetImplicitArgs(IEnumerable<string> implicitArgs, ParameterInfo[] parameters, ref object[] args)
        {
            int argsIndex = 0;

            void AdjustIndex(ref object[] a)
            {
                while (argsIndex < a.Length && a[argsIndex] is not null) argsIndex++;
            }

            AdjustIndex(ref args);

            foreach (string input in implicitArgs)
            {
                Type requiredType = parameters[argsIndex].ParameterType;
                if (TypesParser.TryParse(requiredType, input, out object arg)) args[argsIndex] = arg;
                else
                {
                    if (parameters[argsIndex].HasDefaultValue) args[argsIndex] = parameters[argsIndex].DefaultValue;
                    else if (IsNullable(requiredType)) args[argsIndex] = null;
                    else return false;
                }
            }

            return true;
        }

        private bool TrySetExplicitArgs(IEnumerable<string> explicitArgs, ParameterInfo[] parameters, ref object[] args)
        {
            Dictionary<string, int> nameIndex = new Dictionary<string, int>();
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameterInfo = parameters[i];
                nameIndex.Add(parameterInfo.Name, i);
            }

            foreach (string explicitArg in explicitArgs)
            {
                string[] explicitArgElements = explicitArg.Replace("\"", "").Split('=');
                string name = explicitArgElements[0];
                string value = explicitArgElements[1];
                int argsIndex = nameIndex[name];
                ParameterInfo parameterInfo = parameters[argsIndex];
                if (TypesParser.TryParse(parameterInfo.ParameterType, value, out object arg)) args[argsIndex] = arg;
                else
                {
                    if (parameterInfo.HasDefaultValue) args[argsIndex] = parameterInfo.DefaultValue;
                    else if (IsNullable(parameterInfo.ParameterType)) args[argsIndex] = null;
                    else return false;
                }
            }

            return true;
        }

        private bool IsExplicitArg(string arg)
        {
            bool quoteOpened = false;
            int equalsCount = 0;
            foreach (char c in arg)
            {
                if (c == '"') quoteOpened = !quoteOpened;
                if (c == ExplicitAssigmentCharacter && !quoteOpened) equalsCount++;
                if (equalsCount > 1) return false;
            }

            return equalsCount == 1;
        }
    }
}