namespace AdvancedConsole.Commands.CommandParsing
{
    public class SpacesParser : IParser
    {
        
        public SpacesParser()
        {
            
        }
        public CommandToken Parse(string command)
        {
            return new CommandToken(command.Split());
        }
    }
}