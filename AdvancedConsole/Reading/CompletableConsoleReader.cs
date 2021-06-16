using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AdvancedConsole.Reading.Completing;

namespace AdvancedConsole.Reading
{
    public class CompletableConsoleReader : ICommandReader
    {
        public event Action EnterPressed;
        public event Action TabPressed;
        public event Action LeftArrowPressed;
        public event Action RightArrowPressed;
        public event Action BackSpacePressed;
        public event Action UpArrowPressed;
        public event Action DownArrowPressed;
        public event Action<ConsoleKeyInfo> WritableCharacterInputted;
        public event Action<ConsoleKeyInfo> UnwritableCharacterInputted;
        protected List<ITabCompleter> Completers { get; } = new List<ITabCompleter>();
        protected TabCompletionSession CompletionSession { get; private set; }
        protected string Buffer { get; private set; } = "";
        protected int LastBufferLength { get; private set; }
        protected int CursorLeft { get; set; }
        protected LinkedList<string> History { get; set; } = new();
        protected LinkedListNode<string> CurrentHistoryElement;
        protected string BeforeHistorySelection;
        protected int BeforeHistorySelectionCursorLeft;
        public bool IsReading { get; private set; }

        public CompletableConsoleReader()
        {
            EnterPressed += OnEnterPressed;
            TabPressed += OnTabPressed;
            BackSpacePressed += OnBackspacePressed;
            WritableCharacterInputted += OnWritableCharacterInputted;
            DownArrowPressed += OnDownArrowPressed;
            UpArrowPressed += OnUpArrowPressed;
        }

        public void RegisterTabCompleter(ITabCompleter tabCompleter)
        {
            Completers.Add(tabCompleter);
        }

        private void UpdateConsole()
        {
            Console.CursorVisible = false;
            Console.Write($"\r{Buffer}" + new string(' ', Math.Max(LastBufferLength - Buffer.Length, 0)));
            Console.CursorLeft = CursorLeft;
            Console.CursorVisible = true;
        }

        private void UpdateCompletionSession()
        {
            CompletionSession = new TabCompletionSession(Completers, Buffer, CursorLeft);
        }

        public string ReadCommand()
        {
            IsReading = true;
            Buffer = "";
            LastBufferLength = default(int);
            CursorLeft = default(int);
            UpdateCompletionSession();

            while (IsReading)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Enter) EnterPressed?.Invoke();

                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.Tab:
                        TabPressed?.Invoke();
                        break;
                    case ConsoleKey.Backspace:
                        BackSpacePressed?.Invoke();
                        break;
                    case ConsoleKey.LeftArrow:
                        CursorLeft = Math.Max(CursorLeft - 1, 0);
                        LeftArrowPressed?.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        CursorLeft = Math.Min(CursorLeft + 1, Buffer.Length);
                        RightArrowPressed?.Invoke();
                        break;
                    case ConsoleKey.DownArrow:
                        DownArrowPressed?.Invoke();
                        break;
                    case ConsoleKey.UpArrow:
                        CursorLeft = Math.Min(CursorLeft + 1, Buffer.Length);
                        UpArrowPressed?.Invoke();
                        break;
                    default:
                    {
                        if (IsCharacterWritable(consoleKeyInfo.KeyChar))
                            WritableCharacterInputted?.Invoke(consoleKeyInfo);
                        else UnwritableCharacterInputted?.Invoke(consoleKeyInfo);
                    }
                        break;
                }
                if(consoleKeyInfo.Key != ConsoleKey.Tab) UpdateCompletionSession();

                UpdateConsole();
                LastBufferLength = Buffer.Length;
            }

            Console.WriteLine();
            return Buffer;
        }

        protected virtual bool IsCharacterWritable(char character)
        {
            return character > 31 && character < 136;
        }

        protected virtual void OnEnterPressed()
        {
            IsReading = false;
            if (Buffer.Length != 0 && !History.Contains(Buffer))
                History.AddFirst(Buffer);
            CurrentHistoryElement = null;
        }
        
        protected virtual void OnTabPressed()
        {
            Buffer = CompletionSession.NextCompleteString;
            CursorLeft = Buffer.Length;
        }

        protected virtual void OnWritableCharacterInputted(ConsoleKeyInfo c)
        {
            Buffer = Buffer.Insert(CursorLeft, new string(c.KeyChar, 1));
            CursorLeft++;
        }

        protected virtual void OnBackspacePressed()
        {
            if (Buffer.Length > 0 && Console.CursorLeft > 0) Buffer = Buffer.Remove(Console.CursorLeft - 1, 1);
            CursorLeft = Math.Max(CursorLeft - 1, 0);
        }

        protected virtual void OnUpArrowPressed()
        {
            if (CurrentHistoryElement is null)
            {
                BeforeHistorySelection = Buffer;
                BeforeHistorySelectionCursorLeft = CursorLeft;
            }

            CurrentHistoryElement = CurrentHistoryElement?.Next ?? History.First;
            if (CurrentHistoryElement is null) return;
            Buffer = CurrentHistoryElement.Value;
            CursorLeft = Buffer.Length;
        }

        protected virtual void OnDownArrowPressed()
        {
            if (CurrentHistoryElement is null) return;
            CurrentHistoryElement = CurrentHistoryElement.Previous;
            if (CurrentHistoryElement is null)
            {
                Buffer = BeforeHistorySelection;
                CursorLeft = BeforeHistorySelectionCursorLeft;
                return;
            }

            Buffer = CurrentHistoryElement.Value;
            CursorLeft = Buffer.Length;
        }
    }
}