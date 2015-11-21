using ConsoleClient.Network;
using Packets;
using Packets.Attributes;
using Packets.Client.Chat;
using System;
using Packets.Infrastructure;

namespace ConsoleClient.Chat
{
    /// <summary>
    /// Chat manager, listens for chat related packets. 
    /// TODO: Maintain channels and handle full chat functionality.
    /// </summary>
    class ChatManager : PacketSubscriber
    {
        private readonly Game _game;
        public ChatManager(Game game)
        {
            _game = game;
        }

        [PacketMethod(typeof(ChatMessagePacket))]
        public void OnChatMessage(Packet packet)
        {
            var p = (ChatMessagePacket)packet;
            Console.WriteLine(p.Message);
        }
    }
}
