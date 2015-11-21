using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.Character
{
    [PacketHeader(ServerHeaders.RequestCharacters)]
    public class RequestCharactersPacket : Packet
    {
    }
}
