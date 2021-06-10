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
        public ModulesTree Modules { get; private set; }
        public IParser CommandParser { get; set; }
        public TypesParser TypesParser { get; set; }
        public bool IsListening { get; private set; }
        private ICommandReader Reader { get; set; }
        private Task ListeningTask { get; set; }
        private Dictionary<Type, object> ExecutionContextsCache { get; }

        public CommandsListener()
        {
            Modules = new ModulesTree();
            CommandParser = new SpacesParser();
            TypesParser = new DefaultTypesParser();
            ExecutionContextsCache = new Dictionary<Type, object>();
        }

        public void AddModule<T>()
        {
            Modules.Add(new ReflectiveModuleBuilder(), typeof(T));
        }
        public void StartListening(ICommandReader reader, bool wait = true)
        {
            if(IsListening) return;
            Reader = reader;
            ListeningTask = new Task(Listen);
            ListeningTask.Start();
            IsListening = true;
            if(wait) Wait();
        }
        public void StartListening(bool wait = true)
        {
            CompletableConsoleReader reader = new CompletableConsoleReader();
            reader.RegisterTabCompleter(new MethodsTreeTabCompleter(Modules));
            StartListening(reader, wait);
        }
        public void Wait()
        {
            ListeningTask.Wait();
        }

        public void StopListening()
        {
            IsListening = false;
        }
        public void ClearExecutionContextsCache()
        {
            ExecutionContextsCache.Clear();
        }

        public void ExecuteProcedure(string command)
        {
            string[] tokens = CommandParser.Parse(command);
            Modules.Walk(tokens, (args, node) =>
            {
                if (node is not Command commandNode) return;
                if (commandNode.TryParseArgs(args.ToArray(), TypesParser, out object[] parsedArgs))
                {
                    commandNode.Execute(parsedArgs, ExecutionContextsCache);
                }
            });
        }

        public bool TryExecuteFunction<TOutput>(string command, out TOutput result)
        {
            string[] tokens = CommandParser.Parse(command);
            Type resultType = typeof(TOutput);
            TOutput lastResult = default;
            bool isExecuted = false;
            Modules.Walk(tokens, (args, node) =>
            {
                if (node is not Command commandNode) return;
                if (commandNode.Output != resultType) return;
                if (commandNode.TryParseArgs(args.ToArray(), TypesParser, out object[] parsedArgs))
                {
                    lastResult = (TOutput)commandNode.Execute(parsedArgs, ExecutionContextsCache);
                    isExecuted = true;
                }
            });
            result = lastResult;
            return isExecuted;
        }
        private void Listen()
        {
            while (IsListening)
            {
                try
                {
                    string readCommand = Reader.ReadCommand();
                    ExecuteProcedure(readCommand);
                }
                catch (ReadingCancellationToken e)
                {
                    if(Reader is IDisposable disposableReader) disposableReader.Dispose();
                    StopListening();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}