using System;
using System.Collections.Specialized;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.Character
{
    [PacketHeader(ServerHeaders.DeleteCharacter)]
    public class DeleteCharacterPacket : Packet
    {
        [PacketData(0)]
        public string Name { get; set; }
        [PacketData(1)]
        public Guid Id { get; set; }
    }
}
