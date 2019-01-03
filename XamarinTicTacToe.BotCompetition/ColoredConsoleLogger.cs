using System;
using ITCC.Logging.Core;
using ITCC.Logging.Core.Interfaces;

namespace XamarinTicTacToe.BotCompetition
{
    public class ColoredConsoleLogger : ILogReceiver
    {
        public void WriteEntry(object sender, LogEntryEventArgs args)
        {
            if (args.Level > Level)
                return;
            lock (_lockObject)
            {
                switch (args.Level)
                {
                    case LogLevel.Critical:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case LogLevel.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.Info:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogLevel.Trace:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case LogLevel.None:
                    case LogLevel.Debug:
                        // Default color
                        break;
                }
                Console.WriteLine(args);
                Console.ResetColor();
            }
        }

        public LogLevel Level { get; set; }

        public ColoredConsoleLogger()
        {
            Level = Logger.Level;
        }

        public ColoredConsoleLogger(LogLevel level)
        {
            Level = level;
        }

        private readonly object _lockObject = new object();
    }
}