using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Login
{
    [PacketHeader(ClientHeaders.LoginResult)]
    public class LoginResultPacket : Packet
    {
        [PacketData(0)]
        public bool Success { get; set; }

        [PacketData(1)]
        public string Name { get; set; }
    }
}
