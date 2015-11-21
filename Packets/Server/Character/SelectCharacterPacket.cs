using System;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.Character
{
    [PacketHeader(ServerHeaders.SelectCharacter)]
    public class SelectCharacterPacket : Packet
    {
        [PacketData(0)]
        public Guid Id { get; set; }
    }
}
