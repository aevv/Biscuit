using System;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Map
{
    [PacketHeader(ClientHeaders.RemoveEntity)]
    public class RemoveEntityPacket : Packet
    {
        [PacketData(0)]
        public Guid EntityId { get; set; }
    }
}
