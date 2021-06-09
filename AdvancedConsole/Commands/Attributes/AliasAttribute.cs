using System;
using System.Security;

namespace AdvancedConsole.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AliasAttribute : Attribute
    {
        public string Value { get; set; }

        public AliasAttribute(string value)
        {
            Value = value;
        }
    }
}