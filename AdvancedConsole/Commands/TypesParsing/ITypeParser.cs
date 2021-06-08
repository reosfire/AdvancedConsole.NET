namespace AdvancedConsole.Commands.TypesParsing
{
    public interface ITypeParser
    {
        bool TryParse(string input, out object result);
    }
}