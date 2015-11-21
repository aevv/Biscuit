using System;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Login
{
    [PacketHeader(ClientHeaders.RequestLogin)]
    public class LoginRequestPacket : Packet
    {
        [PacketData(0)]
        public double Version { get; set; }

        [PacketData(1)]
        public Guid ClientId { get; set; }
    }
}
