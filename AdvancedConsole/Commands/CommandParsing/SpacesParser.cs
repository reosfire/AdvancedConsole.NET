namespace AdvancedConsole.Commands.CommandParsing
{
    public class SpacesParser : IParser
    {
        
        public SpacesParser()
        {
            
        }
        public string[] Parse(string command)
        {
            return command.Split();
        }
    }
}