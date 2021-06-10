using System;
using System.Linq;
using System.Text;
using AdvancedConsole.Commands;
using AdvancedConsole.Commands.Attributes;
using AdvancedConsole.Reading;

namespace UsageExample
{
    internal static class Program
    {
        private static CommandsListener Listener { get; set; } = new CommandsListener();

        private static void Main()
        {
            Listener.AddModule<Commands>();
            Listener.AddModule<Calculator>();
            Listener.StartListening(new ScriptReader("../../../Script.txt"),true);
            string tryExecuteFunction = Listener.TryExecuteFunction<string>("run reverse abcdefgh");
            Console.WriteLine(tryExecuteFunction);
            Listener.StartListening();
        }
    }

    [Module(Name = "execute")]
    [Alias("run")]
    public class Commands
    {
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

        [Command(Name = "reverse")]
        public string Function(string input)
        {
            return new StringBuilder().AppendJoin("", input.Reverse()).ToString();
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
}