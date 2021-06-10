using System;
using System.IO;
using AdvancedConsole.Commands;

namespace AdvancedConsole.Reading
{
    public class ScriptReader : ICommandReader, IDisposable
    {
        private TextReader File { get; set; }
        public ScriptReader(TextReader file)
        {
            File = file;
        }
        public ScriptReader(string path)
        {
            File = System.IO.File.OpenText(path);
        }
        public string ReadCommand()
        {
            return File.ReadLine() ?? throw new ReadingCancellationToken();
        }

        public void Dispose()
        {
            File?.Dispose();
        }
    }
}