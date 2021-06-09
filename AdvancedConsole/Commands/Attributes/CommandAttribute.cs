using System;

namespace AdvancedConsole.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandAttribute()
        {
            
        }
        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}