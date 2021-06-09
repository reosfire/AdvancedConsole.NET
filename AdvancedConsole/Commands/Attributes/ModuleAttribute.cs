using System;

namespace AdvancedConsole.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        public string Name { get; set; }
    }
}