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
                if (c == '"' && previousCharacter != '\\')
                {
                    quoteOpened = !quoteOpened;
                    continue;
                }

                if (c == SplitCharacter && !quoteOpened)
                {
                    resultTokens.Add(tokenBuilder.ToString());
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
            args = new object[parameters.Length];
            List<string> explicitArgs = new ();
            List<string> implicitArgs = new ();
            foreach (string input in inputs)
            {
                if (IsExplicitArg(input)) explicitArgs.Add(input);
                else implicitArgs.Add(input);
            }

            bool set = TrySetExplicitArgs(explicitArgs, parameters, args) 
                          && TrySetImplicitArgs(implicitArgs, parameters, args);
            if (!set) return false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null && parameters[i].HasDefaultValue) args[i] = parameters[i].DefaultValue;
                if (args[i] == null && !IsNullable(parameters[i].ParameterType)) return false;
            }

            return true;
        }

        private bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

        private bool TrySetImplicitArgs(IEnumerable<string> implicitArgs, ParameterInfo[] parameters, object[] args)
        {
            int argsIndex = 0;

            void AdjustIndex()
            {
                while (argsIndex < args.Length && args[argsIndex] is not null) argsIndex++;
            }

            AdjustIndex();

            foreach (string input in implicitArgs)
            {
                if (parameters.Length <= argsIndex) return false;
                Type requiredType = parameters[argsIndex].ParameterType;
                StringBuilder preprocessedArg = new (input);
                preprocessedArg.Replace("\\=", "=");
                preprocessedArg.Replace("\\\"", "\"");
                if (TypesParser.TryParse(requiredType, preprocessedArg.ToString(), out object arg))
                {
                    args[argsIndex] = arg;
                    AdjustIndex();
                }
                else
                {
                    if (IsNullable(requiredType)) args[argsIndex] = null;
                    else return false;
                }
            }

            return true;
        }

        private bool TrySetExplicitArgs(IEnumerable<string> explicitArgs, ParameterInfo[] parameters, object[] args)
        {
            Dictionary<string, int> nameIndex = new ();
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameterInfo = parameters[i];
                nameIndex.Add(parameterInfo.Name, i);
            }

            foreach (string explicitArg in explicitArgs)
            {
                string[] explicitArgElements = explicitArg.Split('=');
                string name = explicitArgElements[0];
                string value = explicitArgElements[1];
                if (!nameIndex.ContainsKey(name)) return false;
                int argsIndex = nameIndex[name];
                ParameterInfo parameterInfo = parameters[argsIndex];
                if (TypesParser.TryParse(parameterInfo.ParameterType, value, out object arg)) args[argsIndex] = arg;
                else
                {
                    if (IsNullable(parameterInfo.ParameterType)) args[argsIndex] = null;
                    else return false;
                }
            }

            return true;
        }

        private bool IsExplicitArg(string arg)
        {
            bool quoteOpened = false;
            int equalsCount = 0;
            char? prevChar = null;
            foreach (char c in arg)
            {
                if (c == '"') quoteOpened = !quoteOpened;
                if (c == ExplicitAssigmentCharacter && prevChar != '\\') equalsCount++;
                if (equalsCount > 1) return false;
                prevChar = c;
            }

            return equalsCount == 1;
        }
    }
}