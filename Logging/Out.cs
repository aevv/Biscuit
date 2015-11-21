using System;

namespace Logging
{
    public static class Out
    {
        public static void Log(object message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, Console.ForegroundColor, newline, withDate);
        }

        public static void Red(string message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, ConsoleColor.Red, newline, withDate);
        }

        public static void Green(string message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, ConsoleColor.Green, newline, withDate);
        }

        public static void Blue(object message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, ConsoleColor.Blue, newline, withDate);
        }

        public static void Cyan(object message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, ConsoleColor.Cyan, newline, withDate);
        }

        public static void Yellow(object message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, ConsoleColor.Yellow, newline, withDate);
        }

        public static void DarkYellow(object message, bool newline = true, bool withDate = true)
        {
            WriteOut(message, ConsoleColor.DarkYellow, newline, withDate);
        }

        private static void WriteOut(object message, ConsoleColor color, bool newline, bool withDate)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            string toOut = message.ToString();
            if (withDate)
            {
                toOut = string.Format("{0} : {1}", DateTime.Now.ToString("HH:mm:ss"), toOut);
            }

            if (newline)
                Console.WriteLine(toOut);
            else
                Console.Write(toOut);
            Console.ForegroundColor = prev;
        }
    }
}
