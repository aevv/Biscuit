using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.Login
{
    [PacketHeader(ServerHeaders.Login)]
    public class LoginPacket : Packet
    {
        [PacketData(0)]
        public string Username { get; set; }
        [PacketData(1)]
        public string Password { get; set; }
    }
}
