using System;
using System.Collections.Generic;

namespace AdvancedConsole.Colors
{
    public class ColoredText
    {
        private List<ColoredString> Strings { get; set; }

        public ColoredText(List<ColoredString> strings)
        {
            Strings = strings;
        }

        public ColoredText() : this(new List<ColoredString>())
        {
            
        }

        public ColoredText(ColoredString coloredString) : this()
        {
            Strings.Add(coloredString);
        }

        public void Write()
        {
            ConsoleColor startColor = Console.ForegroundColor;
            foreach (ColoredString coloredString in Strings)
            {
                Console.ForegroundColor = coloredString.Color;
                Console.Write(coloredString.String);
            }

            Console.ForegroundColor = startColor;
        }
        public void WriteLine()
        {
            Write();
            Console.WriteLine();
        }

        public static ColoredText operator +(ColoredText first, ColoredText second)
        {
            first.Strings.AddRange(second.Strings);
            return first;
        }

        public static implicit operator ColoredText(string str)
        {
            return new (new ColoredString(str, ConsoleColor.Gray));
        }
    }
}