using System;
using System.Reflection;
using AdvancedConsole.Commands.Attributes;

namespace AdvancedConsole.Commands.Modules.Building
{
    public class ReflectiveModuleBuilder : IModuleBuilder<Type>
    {
        private static Module Build(Type type, Module.Builder previous)
        {
            ModuleAttribute moduleAttribute = type.GetCustomAttribute<ModuleAttribute>();
            Module.Builder moduleBuilder = new();

            foreach (Type nestedType in type.GetNestedTypes())
            {
                Build(nestedType, moduleBuilder);
            }

            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                CommandAttribute commandAttribute = methodInfo.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is null) continue;
                Command.Builder commandBuilder = new ();
                commandBuilder.SetName(commandAttribute.Name ?? methodInfo.Name);
                commandBuilder.SetMethod(methodInfo);
                foreach (AliasAttribute aliasAttribute in methodInfo.GetCustomAttributes<AliasAttribute>())
                {
                    commandBuilder.AddAlias(aliasAttribute.Value);
                }
                Command command = commandBuilder.Build();
                if (moduleAttribute is null) previous?.AddCommand(command);
                else moduleBuilder.AddCommand(command);
            }

            Module module;
            if (moduleAttribute is null)
            {
                module = moduleBuilder.Build();
                previous?.AddCommands(module.Commands);
                previous?.AddSubModules(module.SubModules);
                module = previous?.Build();
            }
            else
            {
                foreach (AliasAttribute aliasAttribute in type.GetCustomAttributes<AliasAttribute>())
                {
                    moduleBuilder.AddAlias(aliasAttribute.Value);
                }
                module = moduleBuilder.SetName(moduleAttribute.Name ?? type.Name).Build();
                previous.AddSubModule(module);
                module = new Module.Builder().AddSubModule(module).Build();
            }
            return module;
        }
        
        Module IModuleBuilder<Type>.Build(Type type) => Build(type, new Module.Builder());

        public Module Build<T>() => Build(typeof(T), new Module.Builder());
    }
}