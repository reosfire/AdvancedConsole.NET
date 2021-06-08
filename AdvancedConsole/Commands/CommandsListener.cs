using System;
using System.Collections.Generic;
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
        public void StartListening(bool wait = true)
        {
            if(IsListening) return;
            IsListening = true;
            ListeningTask = new Task(Listen);
            ListeningTask.Start();
            if(wait) Wait();
        }
        public void Wait()
        {
            ListeningTask.Wait();
        }

        public void StopListening()
        {
            IsListening = false;
        }

        public void ExecuteProcedure(string command)
        {
            string[] tokens = CommandParser.Parse(command);
            Modules.Walk(tokens, (args, node) =>
            {
                if (node is not Command commandNode) return;
                IEnumerable<object>[] parsedArgs = TypesParser.Parse(args);
                commandNode.TryExecute(parsedArgs, out object _);
            });
        }

        public T TryExecuteFunction<T>(string command)
        {
            string[] tokens = CommandParser.Parse(command);
            Type resultType = typeof(T);
            T result = default;
            Modules.Walk(tokens, (args, node) =>
            {
                if (node is not Command commandNode) return;
                if (commandNode.Output != resultType) return;
                IEnumerable<object>[] parsedArgs = TypesParser.Parse(args);
                if (commandNode.TryExecute(parsedArgs, out object executionResult)) result = (T)executionResult;
            });
            return result;
        }
        private void Listen()
        {
            while (IsListening)
            {
                string readCommand = Reader.ReadCommand();
                try
                {
                    ExecuteProcedure(readCommand);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}