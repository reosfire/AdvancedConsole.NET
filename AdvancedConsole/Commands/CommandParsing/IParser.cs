namespace AdvancedConsole.Commands.CommandParsing
{
    public interface IParser
    {
        CommandToken Parse(string command);
    }
}