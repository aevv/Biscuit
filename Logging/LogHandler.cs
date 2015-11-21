using System;

namespace Logging
{
    public class LogHandler
    {
        public static void Log(Exception exe)
        {
            throw exe;
        }

        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
