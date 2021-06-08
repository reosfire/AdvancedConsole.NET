namespace AdvancedConsole.Commands.CommandParsing
{
    public interface IParser
    {
        string[] Parse(string command);
    }
}