using System.Collections.Generic;
using System.Text;

namespace AdvancedConsole.Commands.CommandParsing
{
    public class SpacesParser : IParser
    {
        public char SplitCharacter { get; set; } = ' ';

        public string[] Parse(string command)
        {
            List<string> resultTokens = new();
            StringBuilder tokenBuilder = new();
            bool quoteOpened = false;
            foreach (char c in command)
            {
                if (c == '"')
                {
                    quoteOpened = !quoteOpened;
                }
                else if (c == SplitCharacter && !quoteOpened)
                {
                    resultTokens.Add(tokenBuilder.ToString());
                    tokenBuilder.Clear();
                }
                else tokenBuilder.Append(c);
            }
            if(tokenBuilder.Length != 0) resultTokens.Add(tokenBuilder.ToString());

            return resultTokens.ToArray();
        }
    }
}