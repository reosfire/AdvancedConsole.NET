using System;
using System.Collections.Generic;

namespace AdvancedConsole.Commands.TypesParsing
{
    public class TypesParser
    {
        private Dictionary<Type, List<ITypeParser>> TypeParsers { get; set; } = new ();
        
        public void AddParser<T>(TryParseDelegate<T> parser)
        {
            AddParser(new FromDelegateTypeParser<T>(parser));
        }
        public void AddParser<T>(ITypeParser<T> parser)
        {
            if(!TypeParsers.ContainsKey(typeof(T))) TypeParsers.Add(typeof(T), new List<ITypeParser>());
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
            if (!TypeParsers.ContainsKey(type))
            {
                parsed = null;
                return false;
            }
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
    }
}