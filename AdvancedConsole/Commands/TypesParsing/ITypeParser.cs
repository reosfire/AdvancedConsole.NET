namespace AdvancedConsole.Commands.TypesParsing
{
    public interface ITypeParser
    {
        bool TryParse(string input, out object result);
    }
    public interface ITypeParser<T> : ITypeParser
    {
        bool TryParse(string input, out T result);

        bool ITypeParser.TryParse(string input, out object result)
        {
            bool isParsed = TryParse(input, out T parsed);
            result = parsed;
            return isParsed;
        }
    }
}