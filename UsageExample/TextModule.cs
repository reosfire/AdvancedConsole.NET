using System.Linq;
using System.Text;
using AdvancedConsole.Commands.Attributes;

namespace UsageExample
{
    [Module(Name = "Text")]
    public class TextModule
    {
        [Command]
        public string Reverse(string input)
        {
            return new StringBuilder().AppendJoin("", input.Reverse()).ToString();
        }
        [Command]
        public string Substring(string input, int startIndex, int lenght)
        {
            return input.Substring(startIndex, lenght);
        }
    }
}