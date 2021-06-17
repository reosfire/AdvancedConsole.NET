using System;

namespace AdvancedConsole.Colors
{
    public class ColoredString
    {
        public string String { get; private set; }
        public ConsoleColor Color { get; private set; }
        public ColoredString(string str, ConsoleColor color)
        {
            String = str;
            Color = color;
        }
    }
}