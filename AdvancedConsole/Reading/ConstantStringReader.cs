using System;
using System.Collections.Generic;
using AdvancedConsole.Commands;

namespace AdvancedConsole.Reading
{
    public class ConstantStringReader : ICommandReader
    {
        private string[] _content;
        private int _idx;
        
        public ConstantStringReader(string content)
        {
            _content = content.Split("; ");
        }
        public string ReadCommand()
        {
            if (_idx >= _content.Length) throw new ReadingCancellationToken();
            return _content[_idx++];
        }
    }
}