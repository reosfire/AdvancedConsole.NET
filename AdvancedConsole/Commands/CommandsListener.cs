using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedConsole.Commands.CommandParsing;
using AdvancedConsole.Commands.Modules;
using AdvancedConsole.Commands.Modules.Building;
using AdvancedConsole.Commands.TypesParsing;
using AdvancedConsole.Reading;
using AdvancedConsole.Reading.Completing;

namespace AdvancedConsole.Commands
{
    public class CommandsListener
    {
        public ICommandReader Reader { get; set; }
        public ModulesTree Modules { get; private set; }
        public IParser CommandParser { get; set; }
        public TypesParser TypesParser { get; set; }
        private Task ListeningTask { get; set; }
        public bool IsListening { get; private set; }
        
        public CommandsListener()
        {
            Modules = new ModulesTree();
            CompletableConsoleReader reader = new();
            reader.RegisterTabCompleter(new MethodsTreeTabCompleter(Modules));
            Reader = reader;
            CommandParser = new SpacesParser();
            TypesParser = new TypesParser();
            TypesParser.AddDefaultTypesParsers();
        }

        public void AddModule<T>()
        {
            Modules.Add(new ReflectiveModuleBuilder(), typeof(T));
        }
        
        public void StartListeningAsync()
        {
            if(IsListening) return;
            IsListening = true;
            ListeningTask = new Task(Listen);
        }
        public void StartListeningSync()
        {
            if(IsListening) return;
            IsListening = true;
            ListeningTask = new Task(Listen);
            ListeningTask.Start();
            ListeningTask.Wait();
        }

        public void StopListening()
        {
            
        }
        private void Listen()
        {
            while (IsListening)
            {
                string readCommand = Reader.ReadCommand();
                CommandToken commandToken = CommandParser.Parse(readCommand);
                try
                {
                    Modules.Walk(commandToken.Path, (args, node) =>
                    {
                        if (node is not Command command) return;
                        IEnumerable<object>[] parsedArgs = TypesParser.Parse(args);
                        command.TryExecute(parsedArgs);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}