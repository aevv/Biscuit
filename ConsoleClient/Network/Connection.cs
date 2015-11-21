using ConsoleClient.Config;
using ConsoleClient.Properties;
using Logging;
using Packets;
using Packets.Attributes;
using Packets.Client.Status;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Packets.Infrastructure;

namespace ConsoleClient.Network
{
    /// <summary>
    /// Client connection class. Singleton, one connection allowed on the process. Handles incoming reads and delegates 
    /// packet creation to the correct handlers, then publishes them to assigned subscribers.
    /// </summary>
    class Connection : PacketSubscriber, IDisposable
    {
        private TcpClient _client;
        /// <summary>
        /// Writer to write to in order to send data to the server.
        /// </summary>
        private BinaryWriter _writer;
        /// <summary>
        /// Reader to read data from the server.
        /// </summary>
        private BinaryReader _reader;

        private readonly PacketReader _packetReader;

        public BinaryWriter Writer { get { return _writer; } }
        public BinaryReader Reader { get { return _reader; } }

        private bool _active;

        private readonly List<PacketSubscriber> _subs;

        private readonly int _port = 7654;
        private readonly string _server = "localhost";

        private static Connection _connection;

        public static Connection Create(ConfigManager<ConfigMappings> config)
        {
            return _connection ?? (_connection = new Connection(config.TryGet(a => a.Server), config.TryGet(a => a.Port)));
        }

        private Connection(string server, int port)
        {
            _port = port;
            _server = server;
            _packetReader = PacketReader.Resolve<ClientPacketReader>();
            _packetReader.InitialiseMapping();

            _subs = new List<PacketSubscriber>();
            AddSubscriber(this);
        }

        public void Begin()
        {
            _client = new TcpClient(_server, _port);
            _client.NoDelay = true;
            _writer = new BinaryWriter(_client.GetStream());
            _reader = new BinaryReader(_client.GetStream());
            Task.Factory.StartNew(TransferData);
        }

        public void AddSubscriber(PacketSubscriber subscriber)
        {
            if (!_subs.Contains(subscriber))
            {
                _subs.Add(subscriber);
            }
        }

        public void Dispose()
        {
            _client.Close();
            _writer.Dispose();
            _reader.Dispose();
            _active = false;
        }

        private void TransferData()
        {
            _active = true;
            while (_active)
            {
                try
                {
                    short header = _reader.ReadInt16();

                    if (!_packetReader.CanRead(header)) continue;
                    Packet packet = _packetReader.ReadPacket(header, _reader);

                    foreach (var sub in _subs)
                    {
                        sub.ReceivePacket(packet);
                    }
                }
                catch (Exception ex)
                {
                    Out.Red(string.Format("Closing connection: {0}", ex));
                }
            }
        }

        [PacketMethod(typeof(ServerOfflinePacket))]
        public void OnServerOffline(Packet packet)
        {
            MessageBox.Show(Resources.Connection_GivePacket_Server_has_shut_down_);
            Environment.Exit(0);
        }
    }
}
