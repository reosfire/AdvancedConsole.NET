using System;
using System.Collections;
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
        public void HelloWorld()
        {
            Console.WriteLine("Hello world!");
        }
        [Command(Name = "HelloWorld")]
        public void GetNumbersBefore(int i)
        {
            IEnumerable enumerable = Enumerable.Range(0,i);
            StringBuilder builder = new StringBuilder();
            foreach(int num in enumerable)
            {
                builder.AppendJoin(", ", num);
            }
            Console.WriteLine(builder);
        }
    }
}