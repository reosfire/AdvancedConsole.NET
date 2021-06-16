using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvancedConsole.Reading.Completing;

namespace AdvancedConsole.Reading
{
    public class TabCompletionSession
    {
        private List<ITabCompleter> Completers { get; }
        private string _trimmedBuffer = null;
        private string _buffer;
        private string Buffer
        {
            get
            {
                if (_trimmedBuffer == null)
                {
                    int lastIndexOf = _buffer?.LastIndexOf(' ') ?? 0;
                    lastIndexOf++;
                    _trimmedBuffer = _buffer?[0..lastIndexOf];
                }
                return _trimmedBuffer;
            }
            set => _buffer = value;
        }
        private int Index { get; set; }
        private IEnumerable<string> Completions => Completers.SelectMany(tabCompleter => tabCompleter.GetCompletion(_buffer, Index));
        private IEnumerator<string> _currentCompletion;

        private string CurrentCompletion
        {
            get
            {
                _currentCompletion ??= Completions.GetEnumerator();
                bool moved = _currentCompletion.MoveNext();
                if (!moved) _currentCompletion = Completions.GetEnumerator();
                return _currentCompletion.Current;
            }
        }
        public string NextCompleteString
        {
            get
            {
                string completion = CurrentCompletion;
                if (completion == null) return _buffer;
                return Buffer + completion;
            }
        }       

        public TabCompletionSession(List<ITabCompleter> completers, string buffer, int index)
        {
            Completers = completers;
            Buffer = buffer;
            Index = index;
        }
    }
}