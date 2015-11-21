using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Map
{
    [PacketHeader(ClientHeaders.GiveChunk)]
    public class GiveChunkPacket : Packet
    {
        [PacketData(0)]
        public int X { get; set; }
        [PacketData(1)]
        public int Y { get; set; }
        [PacketData(2)]
        public string Data { get; set; }
    }
}
