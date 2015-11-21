using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Status
{
    [PacketHeader(ClientHeaders.ServerOffline)]
    public class ServerOfflinePacket : Packet
    {
    }
}
