using System;
using System.Linq;
using System.Text;
using AdvancedConsole.Commands;
using AdvancedConsole.Commands.Attributes;

namespace UsageExample
{
    internal static class Program
    {
        private static CommandsListener Listener { get; set; } = new CommandsListener();
        private static void Main()
        {
            Listener.AddModule<Commands>();
            Listener.StartListening(false);
            string tryExecuteFunction = Listener.TryExecuteFunction<string>("run reverse abcdefgh");
            Console.WriteLine(tryExecuteFunction);
            Listener.Wait();
        }
    }
    
    [Module(Name = "execute")]
    [Alias("run")]
    class Commands
    {
        private int _temporaryField = 0;
        private int TemporaryProperty { get; set; } = 0;

        [Command]
        public void GetTemporary()
        {
            Console.WriteLine(_temporaryField);
            Console.WriteLine(TemporaryProperty);
            _temporaryField++;
            TemporaryProperty++;
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