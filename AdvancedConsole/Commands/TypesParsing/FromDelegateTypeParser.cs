namespace AdvancedConsole.Commands.TypesParsing
{
    public delegate bool TryParseDelegate<T>(string input, out T result);

    public class FromDelegateTypeParser<T> : ITypeParser<T>
    {
        private TryParseDelegate<T> TryParseDelegate { get; }

        public FromDelegateTypeParser(TryParseDelegate<T> tryParseDelegate)
        {
            TryParseDelegate = tryParseDelegate;
        }

        public bool TryParse(string input, out T result)
        {
            return TryParseDelegate(input, out result);
        }
    }
}