using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedConsole.Commands.TypesParsing
{
    public class TypesParser
    {
        private Dictionary<Type, List<ITypeParser>> TypeParsers { get; set; } = new();

        public void AddParser<T>(TryParseDelegate<T> parser)
        {
            AddParser(new FromDelegateTypeParser<T>(parser));
        }

        public void AddParser<T>(ITypeParser<T> parser)
        {
            if (!TypeParsers.ContainsKey(typeof(T))) TypeParsers.Add(typeof(T), new List<ITypeParser>());
            TypeParsers[typeof(T)].Add(parser);
        }

        public IEnumerable<object> Parse(string input)
        {
            foreach (KeyValuePair<Type, List<ITypeParser>> typeParser in TypeParsers)
            {
                foreach (ITypeParser parser in typeParser.Value)
                {
                    if (parser.TryParse(input, out object parseResult)) yield return parseResult;
                }
            }
        }

        public IEnumerable<object>[] Parse(ArraySegment<string> inputs)
        {
            IEnumerable<object>[] parseResults = new IEnumerable<object>[inputs.Count];
            for (int i = 0; i < inputs.Count; i++)
            {
                parseResults[i] = Parse(inputs[i]);
            }

            return parseResults;
        }

        public bool TryParse<T>(string input, out T parsed)
        {
            bool isParsed = TryParse(typeof(T), input, out object parseResult);
            parsed = (T) parseResult;
            return isParsed;
        }

        public bool TryParse(Type type, string input, out object parsed)
        {
            parsed = null;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                if (elementType is null) return false;
                if (TryParseArray(elementType, input, out object[] objects))
                {
                    Array convertedArray = Array.CreateInstance(elementType, objects.Length);
                    Array.Copy(objects, convertedArray, objects.Length);
                    parsed = convertedArray;
                    return true;
                }
                return false;
            }

            if (!TypeParsers.ContainsKey(type)) return false;
            foreach (ITypeParser parser in TypeParsers[type])
            {
                if (parser.TryParse(input, out object parseResult))
                {
                    parsed = parseResult;
                    return true;
                }
            }

            parsed = default;
            return false;
        }

        public bool TryParseArray(Type type, string input, out object[] result)
        {
            result = null;
            if (IsValidBrackets(input)) input = input[1..^1];
            else return false;

            string[] inputs = ParseArrayArgs(';', input).ToArray();
            result = new object[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                if (TryParse(type, inputs[i], out object parsed)) result[i] = parsed;
                else return false;
            }

            return true;
        }

        private bool IsValidBrackets(string input)
        {
            int opened = 0;
            foreach (char c in input)
            {
                if (c == '[') opened++;
                else if (c == ']') opened--;
            }

            return opened == 0 && input.StartsWith("[") && input.EndsWith("]");
        }
        private IEnumerable<string> ParseArrayArgs(char separator, string input)
        {
            StringBuilder argBuilder = new();
            int openedBrackets = 0;
            foreach (char c in input)
            {
                if (c == '[') openedBrackets++;
                else if (c == ']') openedBrackets--;
                if (openedBrackets == 0 && c == separator)
                {
                    yield return argBuilder.ToString();
                    argBuilder.Clear();
                }
                else argBuilder.Append(c);
            }

            yield return argBuilder.ToString();
        }
    }
}