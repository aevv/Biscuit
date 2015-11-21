using Logging;
using Packets.Infrastructure;
using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            PacketRegister.CreateRegistry();
            var server = new Server.Server();
            try
            {
                server.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                LogHandler.Log(ex.Message);
                Console.WriteLine();
                LogHandler.Log(ex.StackTrace);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
