using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Listener.StartListeningSync();
        }
    }
    
    [Module(Name = "execute")]
    [Alias("run")]
    class Commands
    {
        [Command(Name = "HelloWorld")]
        [Alias("HW")]
        public void HelloWorld()
        {
            Console.WriteLine("Hello world!");
        }
        [Command(Name = "Sas")]
        public void GetNumbersBefore(int first, int second)
        {
            IEnumerable<int> enumerable = Enumerable.Range(first,second);
            StringBuilder builder = new StringBuilder();
            builder.AppendJoin(", ", enumerable);
            Console.WriteLine(builder);
        }
    }
}