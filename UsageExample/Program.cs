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
            Listener.AddModule<Calculator>();
            Listener.StartListening(false);
            string tryExecuteFunction = Listener.TryExecuteFunction<string>("run reverse abcdefgh");
            Console.WriteLine(tryExecuteFunction);
            Listener.Wait();
        }
    }

    [Module(Name = "execute")]
    [Alias("run")]
    public class Commands
    {
        public int TemporaryProperty { get; set; } = 0;

        [Command]
        public void Minus(int i)
        {
            TemporaryProperty -= i;
            Console.WriteLine(TemporaryProperty);
        }

        [Command]
        public void Add(int i)
        {
            TemporaryProperty += i;
            Console.WriteLine(TemporaryProperty);
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
    [Module(Name = "Calculator")]
    public class Calculator
    {
        private double _temp = 0;

        public double Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                Console.WriteLine(value);
            }
        }

        [Command]
        public void Clear() => Temp = 0;
        [Command]
        public void Add(double n) => Temp += n;
        [Command]
        public void Subtract(double n) => Temp -= n;
        [Command]
        public void Multiply(double n) => Temp *= n;
        [Command]
        public void Divide(double n) => Temp /= n;
        [Command]
        public void Pow(double n) => Temp = Math.Pow(Temp, n);
        [Command]
        public void Root(double n) => Temp = Math.Pow(Temp, 1/n);
    }
}