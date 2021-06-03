using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedConsole.Reading.Completing
{
    public interface ITabCompleter
    {
         IEnumerable<string> GetCompletion(string currentInput, int cursorIndex);
    }
}
