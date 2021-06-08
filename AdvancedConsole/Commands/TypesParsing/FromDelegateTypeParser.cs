namespace AdvancedConsole.Commands.TypesParsing
{
    public delegate bool TryParseDelegate<T>(string input, out T result);
    public delegate bool TryParseDelegate(string input, out object result);

    public class FromDelegateTypeParser<T> : ITypeParser
    {
        private TryParseDelegate<T> TryParseDelegate { get; }

        public FromDelegateTypeParser(TryParseDelegate<T> tryParseDelegate)
        {
            TryParseDelegate = tryParseDelegate;
        }

        public bool TryParse(string input, out object result)
        {
            bool isParsed = TryParseDelegate(input, out T parseResult);
            result = parseResult;
            return isParsed;
        }
    }
}