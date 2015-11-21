using BiscuitHeaders;
using System;

namespace Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerHeaderAttribute : Attribute
    {
        public ServerHeaders Header { get; set; }

        public HandlerHeaderAttribute(ServerHeaders header)
        {
            Header = header;
        }
    }
}
