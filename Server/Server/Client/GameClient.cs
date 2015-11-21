using Logging;
using Packets;
using Packets.Attributes;
using Packets.Client.Character;
using Packets.Client.Login;
using Packets.Infrastructure;
using Packets.Server.Character;
using Packets.Server.Login;
using Server.Game;
using Server.Game.Chars;
using Server.Game.Chars.DataObject;
using Server.Server.Chat;
using Server.Server.Data;
using Server.Server.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server.Server.Client
{
    class GameClient : PacketSubscriber
    {
        private readonly TcpClient _self;
        private readonly BinaryReader _reader;
        private readonly BinaryWriter _writer;
        private readonly IClientKiller _clientKiller;
        private readonly PacketReader _packetReader;
        private readonly AuthorisationManager _auth;
        private readonly GameLoop _game;

        private readonly List<PacketSubscriber> _subs;

        private bool _active;
        private bool _disconnected;

        public Account Account { get; private set; }
        public Character Character { get; private set; }
        public bool LoggedIn { get; private set; }
        public Guid OnlineId { get; private set; }
        public BinaryReader Reader { get { return _reader; } }
        public BinaryWriter Writer { get { return _writer; } }

        public GameClient(TcpClient selfClient, Guid onlineId, IClientKiller killer, ChatManager manager, GameLoop game)
        {
            _self = selfClient;
            _self.NoDelay = true;
            _self.Client.NoDelay = true;
            _reader = new BinaryReader(_self.GetStream());
            _writer = new BinaryWriter(_self.GetStream());
            _clientKiller = killer;
            _game = game;
            ChatManager chatManager = manager;
            chatManager.Join(this);
            _auth = AuthorisationManager.Resolve();

            _packetReader = PacketReader.Resolve<ServerPacketReader>();
            _packetReader.InitialiseMapping();

            OnlineId = onlineId;
            _active = true;

            _subs = new List<PacketSubscriber>
                    {
                        this,
                        _game,
                        chatManager
                    };

            Task.Factory.StartNew(Receive);
        }

        public void AddSubscriber(PacketSubscriber subscriber)
        {
            _subs.Add(subscriber);
        }

        public void Kill(Exception ex = null)
        {
            if (ex != null)
            {
                Console.WriteLine(ex.Message);
            }

            _game.RemoveClient(this);

            Disconnect();
            _active = false;
            _clientKiller.RemoveClient(this);
        }

        public void Disconnect()
        {
            _self.Close();
            _disconnected = true;
        }

        private void Receive()
        {
            PacketWriter.WritePacketAsync(new LoginRequestPacket { Version = Server.Version, ClientId = OnlineId }, _writer);

            while (_active)
            {
                try
                {
                    short header = _reader.ReadInt16();

                    if (_packetReader.CanRead(header))
                    {
                        Packet packet = _packetReader.ReadPacket(header, _reader);
                        packet.ClientID = OnlineId;

                        foreach (var sub in _subs)
                        {
                            sub.ReceivePacket(packet);
                        }
                    }
                    else
                    {
                        throw new InvalidHeaderException(header);
                    }
                }
                catch (Exception ex)
                {
                    if (!_disconnected)
                    {
                        Kill(new ClientDisconnectedException(OnlineId, ex));
                    }
                }
            }
        }

        public override string ToString()
        {
            if (LoggedIn && !string.IsNullOrEmpty(Account.Username))
            {
                return Account.Username;
            }
            return OnlineId.ToString();
        }

        [PacketMethod(typeof(LoginPacket))]
        public void OnLogin(Packet packet)
        {
            if (Account != null) return;
            var p = (LoginPacket)packet;
            Account = _auth.TryLogin(p.Username, p.Password);
            if (Account != null)
            {
                LoggedIn = true;
                Out.Yellow(string.Format("{0} logged in.", this));
                PacketWriter.WritePacketAsync(new LoginResultPacket { Success = true, Name = Account.Username },
                    _writer);
            }
            else
            {
                Out.DarkYellow(string.Format("{0} failed to log in. User provided: {1}.", this, p.Username));
                PacketWriter.WritePacketAsync(new LoginResultPacket { Success = false, Name = p.Username }, _writer);
                Account = null;
            }
        }

        [PacketMethod(typeof(LogoutPacket))]
        public void OnLogout(Packet packet)
        {
            if (Account == null) return;
            Out.Cyan(string.Format("Client {0} logging out.", this));
            Kill();
        }

        [PacketMethod(typeof(RequestCharactersPacket))]
        public void OnRequestCharacters(Packet packet)
        {
            if (Account == null) return;
            var chars = Account.GetCharactersForAccount(Account).ToList();
            Out.Yellow(string.Format("{0} has requested characters - {1} found", Account, chars.Count()));
            foreach (var c in chars)
            {
                PacketWriter.WritePacketAsync(new SelectableCharacterPacket { Name = c.Name, Id = c.Id }, _writer);
            }
        }

        [PacketMethod(typeof(SelectCharacterPacket))]
        public void OnSelectCharacter(Packet packet)
        {
            if (Account == null) return;
            var p = (SelectCharacterPacket)packet;
            var character = Character.GetCharacter(Account, p.Id);
            if (character != null)
            {
                Character = character;
                PacketWriter.WritePacketAsync(new CharacterSelectionResultPacket { Success = true }, _writer);
                Out.Yellow(string.Format("{0} selected character {1}", this, character));
                _game.AddClient(this);
            }
            else
            {
                Out.Red(string.Format("{0} tried to selected invalid character: {1}", this, p.Id));
                PacketWriter.WritePacketAsync(new CharacterSelectionResultPacket { Success = false }, _writer);
            }
        }

        [PacketMethod(typeof(CreateCharacterPacket))]
        public void OnCreateCharacter(Packet packet)
        {
            if (Account == null) return;
            var p = (CreateCharacterPacket)packet;
            if (Character.GetCharacter(name: p.Name) == null)
            {
                if (Account.GetCharactersForAccount(Account).Count() < 3)
                {
                    var repo = DataConnection.Resolve().Repo;

                    var c = new Character { Id = Guid.NewGuid(), Name = p.Name, AccountId = Account.Id };
                    repo.Insert(c);
                    c.Location = new CharacterLocation
                    {
                        CharacterId = c.Id,
                        X = 0,
                        Y = 0,
                        MapId = _game.PrimaryMap.Id
                    };
                    repo.Insert(c.Location);
                    PacketWriter.WritePacketAsync(new CharacterCreationResultPacket { Success = true }, _writer);
                }
                else
                {
                    PacketWriter.WritePacketAsync(
                        new CharacterCreationResultPacket
                        {
                            Success = false,
                            Reason = "There are already three characters on this account."
                        }, _writer);
                }
            }
            else
            {
                PacketWriter.WritePacketAsync(
                    new CharacterCreationResultPacket { Success = false, Reason = "Character name already exists." },
                    _writer);
            }
        }

        [PacketMethod(typeof(DeleteCharacterPacket))]
        public void OnDeleteCharacter(Packet packet)
        {
            if (Account == null) return;
            var p = (DeleteCharacterPacket)packet;
            var c = Character.GetCharacter(Account, p.Id, p.Name);
            if (c != null)
            {
                var repo = DataConnection.Resolve().Repo;

                c.Deleted = true;
                repo.Update(c);
                PacketWriter.WritePacketAsync(new CharacterDeletionResultPacket { Success = true, Reason = "" }, _writer);
            }
            else
            {
                PacketWriter.WritePacketAsync(new CharacterDeletionResultPacket { Success = false, Reason = "Invalid character." }, _writer);
            }
        }

        public void Save()
        {
            Character.Save();
        }
    }
}
