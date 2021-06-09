using System;
using System.Collections.Generic;

namespace AdvancedConsole.Commands.TypesParsing
{
    public class TypesParser
    {
        private Dictionary<Type, ITypeParser> TypeParsers { get; set; } = new ();
        
        public void AddParser<T>(TryParseDelegate<T> parser)
        {
            AddParser(new FromDelegateTypeParser<T>(parser));
        }
        public void AddParser<T>(ITypeParser<T> parser)
        {
            TypeParsers.Add(typeof(T), parser);
        }
        public IEnumerable<object> Parse(string input)
        {
            foreach (KeyValuePair<Type, ITypeParser> typeParser in TypeParsers)
            {
                if (typeParser.Value.TryParse(input, out object parseResult)) yield return parseResult;
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
    }
}