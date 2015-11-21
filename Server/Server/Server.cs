using Logging;
using Packets;
using Server.Game;
using Server.Properties;
using Server.Server.Chat;
using Server.Server.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Server
{
    class Server
    {
        private const int Port = 7654;

        public static double Version { get; private set; }

        public bool Running { get; private set; }

        private readonly TcpListener _serverListener;
        private readonly GameClientManager _clientManager;
        private readonly ChatManager _chatManager;
        private readonly GameLoop _game;

        public Server()
        {
            Version = Convert.ToDouble(Resources.VERSION);

            _serverListener = new TcpListener(IPAddress.Any, Port);
            _clientManager = new GameClientManager();
            _chatManager = ChatManager.Resolve();
            _game = new GameLoop();
            PacketReader.Resolve<ServerPacketReader>().InitialiseMapping();
        }

        public void Listen()
        {
            Out.Log("Beginning server start up...");
            if (_serverListener != null)
            {
                Out.Log("Server startup ", false);

                try
                {
                    _serverListener.Start();
                }
                catch (Exception ex)
                {
                    Out.Red("failed!", withDate: false);
                    LogHandler.Log(ex);
                    return;
                }

                Out.Green("success!", withDate: false);
                Out.Log(string.Format("Listening on {0}", _serverListener.LocalEndpoint));
                Running = true;

                Task.Factory.StartNew(GameLoop);
                Task.Factory.StartNew(InputLoop);

                // Running may be set to false, this will stop connections from being accepted.
                while (Running)
                {
                    try
                    {
                        var c = new GameClient(_serverListener.AcceptTcpClient(), Guid.NewGuid(), _clientManager,
                            _chatManager, _game);
                        _clientManager.AddClient(c);
                    }
                    catch
                    {
                        // TODO: Decide how to handle blocking failure on shut down.
                    }
                }
            }
            else
            {
                Out.Red("Server was not correctly initialised.");
            }
        }

        private void ShutDown()
        {
            Out.Yellow("Server shutting down.");
            _clientManager.ShutDown();
            _serverListener.Stop();
            _game.Save();
            Running = false;
            Out.Yellow("Server shut down success.");
        }

        private void GameLoop()
        {
            var sw = new Stopwatch();
            _game.Load();
            while (Running)
            {
                sw.Stop();

                _game.Tick(sw.ElapsedMilliseconds);

                sw.Reset();
                sw.Start();
                Thread.Sleep(16);
            }
        }

        private void InputLoop()
        {
            while (Running)
            {
                // TODO: Make this sit at bottom always?
                string line;
                if ((line = Console.ReadLine()) == null) continue;
                var split = line.Split(' ');

                if (split.Length == 1)
                {
                    HandleCommand(line);
                }
                else
                {
                    List<string> args = new List<string>();
                    for (int x = 1; x < split.Length; x++)
                    {
                        args.Add(split[x]);
                    }
                    HandleCommand(split[0], args.ToArray());
                }
            }
        }

        private void HandleCommand(string command)
        {
            var cmd = command.ToLower();
            switch (cmd)
            {
                case "exit":
                    ShutDown();
                    break;
                case "players":
                    Out.DarkYellow(_clientManager.GetAll());
                    break;
            }
        }

        private void HandleCommand(string command, string[] args)
        {
            var cmd = command.ToLower();
        }
    }
}
