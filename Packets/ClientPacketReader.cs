using Packets.Attributes;
using System.Linq;
using System.Reflection;

namespace Packets
{
    public class ClientPacketReader : PacketReader
    {
        public override void InitialiseMapping()
        {
            base.InitialiseMapping();
            if (_initialised) return;
            _initialised = true;
            var assembly = Assembly.GetExecutingAssembly();
            var types =
                assembly.GetTypes()
                    .Where(t => t.Namespace.Contains("Client"))
                    .Where(t => t.GetCustomAttributes(false).OfType<PacketHeaderAttribute>().Any());
            foreach (var type in types)
            {
                var attribs = type.GetCustomAttributes(false).OfType<PacketHeaderAttribute>().First();
                _mappings.Add(attribs.Header, type);
            }
        }
    }
}
