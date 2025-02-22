using System;

namespace LeageAccManager
{
    internal enum LogType
    {
        Info,
        Warning,
        Error
    }

    internal static class Logger
    {
        private const bool FirstInit = true;

        public static void Log(string message, LogType type, bool force = false)
        {
            if (FirstInit || force)
            {
                switch (type)
                {
                    case LogType.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("INFO");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] ");
                        Console.WriteLine(message);
                        break;

                    case LogType.Warning:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("WARNING");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] ");
                        Console.WriteLine(message);
                        break;

                    case LogType.Error:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("ERROR");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] ");
                        Console.WriteLine(message);
                        break;
                }
            }
        }
    }
}