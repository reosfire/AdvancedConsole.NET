using System;
using AdvancedConsole.Colors;
using AdvancedConsole.Commands;
using AdvancedConsole.Commands.Attributes;
using AdvancedConsole.Commands.Modules;
using AdvancedConsole.Reading;

namespace UsageExample
{
    internal static class Program
    {
        private static readonly CommandsListener _listener = new();

        private static void Main()
        {
            _listener.AddModule<Reoslang>();
            _listener.StartListening(new ScriptReader("../../../Script.txt"));
            Console.WriteLine("Program ended. Press any key to exit");
            Console.ReadLine();
        }
    }
    
    [Module(Name = "execute")]
    [Alias("run")]
    public class Commands
    {
        [Command]
        public void ColorizedString(string input = "Default value")
        {
            ConsoleColor.Blue.Set(input).WriteLine();
            //default
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Hello ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("World");
            Console.ResetColor();
            //Lib1
            (ConsoleColor.Green.Set("Hello ") + ConsoleColor.Red.Set("World")).WriteLine();
            //lib2
            ("Hello ".SetColor(ConsoleColor.Green) + "World".SetColor(ConsoleColor.Red)).WriteLine();
        }
        [Command]
        public void QuotedStrings(string first, string second)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(first);
            Console.WriteLine(second);
            Console.ForegroundColor = foregroundColor;
        }
        [Command]
        public void Add(int[] added)
        {
            if (added.Length == 0) return;
            int result = 0;
            foreach (int i in added)
            {
                result += i;
            }
            Console.WriteLine(result);
        }

        [Command]
        [Alias("HW")]
        public void HelloWorld()
        {
            Console.WriteLine("Hello world!");
        }

        [Command(Name = "MM")]
        public void MultiMatch(string input)
        {
            Console.WriteLine(input);
        }

        [Module(Name = "MM")]
        public class MultiMatchClass
        {
            [Command(Name = "something")]
            public void MultiMatch()
            {
                Console.WriteLine("something2");
            }
        }
    }

    [Module]
    public class ClassWithCtor
    {
        private string InitialValue { get; set; }

        public ClassWithCtor(string initialValue)
        {
            InitialValue = initialValue;
        }

        [Command]
        public void PrintInitialValue()
        {
            Console.WriteLine(InitialValue);
        }
    }
    [Module("Console")]
    public class ConsoleModule
    {
        [Command]
        public void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
        
        [Command]
        public void PrintLine(string input, ConsoleColor color = ConsoleColor.Gray)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(input);
            Console.ForegroundColor = startColor;
        }
    }
}