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
        public static CommandsListener Listener { get; set; } = new CommandsListener();

        private static void Main()
        {
            Listener.AddModule<Commands>();
            Listener.AddModule<Calculator>();
            Listener.AddModule<TextModule>();
            Listener.AddModule(new ClassWithCtor("String from ctor"));
            Listener.StartListening(new ScriptReader("../../../Script.txt"), true);
            if(Listener.TryExecuteFunction("Text Reverse abcdefgh", out string result)) 
                Console.WriteLine(result);
            Listener.StartListening();
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
}