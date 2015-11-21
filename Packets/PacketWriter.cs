using Packets.Attributes;
using Packets.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Packets
{
    public static class PacketWriter
    {
        public static void WritePacketAsync(Packet packet, BinaryWriter writer)
        {
            var packetType = packet.GetType();

            // TODO: Optimise.
            var properties =
                packetType.GetProperties()
                    .Where(p => p.GetCustomAttributes(false).OfType<PacketDataAttribute>().Any())
                    .OrderBy(p => p.GetCustomAttributes(false).OfType<PacketDataAttribute>().First().Order);

            Task.Factory.StartNew(() =>
                                  {
                                      lock (writer)
                                      {
                                          try
                                          {

                                              writer.Write(packet.Header);
                                              foreach (var prop in properties)
                                              {
                                                  prop.GetValue(packet).Write(prop.PropertyType, writer);
                                              }
                                              writer.Flush();
                                          }
                                          catch (Exception ex)
                                          {
                                              // TODO: Something.
                                          }
                                      }
                                  });
        }

        public static void WritePacket(Packet packet, BinaryWriter writer)
        {
            var packetType = packet.GetType();

            // TODO: Optimise.
            var properties =
                packetType.GetProperties()
                    .Where(p => p.GetCustomAttributes(false).OfType<PacketDataAttribute>().Any())
                    .OrderBy(p => p.GetCustomAttributes(false).OfType<PacketDataAttribute>().First().Order);

            lock (writer)
            {
                try
                {

                    writer.Write(packet.Header);
                    foreach (var prop in properties)
                    {
                        prop.GetValue(packet).Write(prop.PropertyType, writer);
                    }
                    writer.Flush();
                }
                catch (Exception ex)
                {
                    // TODO: Something.
                }
            }
        }
    }
}
