using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.World
{
    [PacketHeader(ServerHeaders.MoveCharacter)]
    public class MoveCharacterPacket : Packet
    {
        [PacketData(0)]
        public float X { get; set; }
        [PacketData(1)]
        public float Y { get; set; }
    }
}
