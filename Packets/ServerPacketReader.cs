using Packets.Attributes;
using System.Linq;
using System.Reflection;

namespace Packets
{
    public class ServerPacketReader : PacketReader
    {
        public override void InitialiseMapping()
        {
            base.InitialiseMapping();
            if (!_initialised)
            {
                _initialised = true;
                var assembly = Assembly.GetExecutingAssembly();
                var types =
                    assembly.GetTypes()
                        .Where(t => t.Namespace.Contains("Server"))
                        .Where(t => t.GetCustomAttributes(false).OfType<PacketHeaderAttribute>().Any());
                foreach (var type in types)
                {
                    var attribs = type.GetCustomAttributes(false).OfType<PacketHeaderAttribute>().First();
                    _mappings.Add(attribs.Header, type);
                }
            }
        }
    }
}
