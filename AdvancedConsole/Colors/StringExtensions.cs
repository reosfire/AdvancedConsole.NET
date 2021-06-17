using System;

namespace AdvancedConsole.Colors
{
    public static class StringExtensions
    {
        public static ColoredText SetColor(this string str, ConsoleColor color)
        {
            return new ColoredText(new ColoredString(str, color));
        }
        public static ColoredText Set(this ConsoleColor color, string str)
        {
            return new ColoredText(new ColoredString(str, color));
        }
    }
}