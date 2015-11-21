using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.Login
{
    [PacketHeader(ServerHeaders.Logout)]
    public class LogoutPacket : Packet
    {
    }
}
