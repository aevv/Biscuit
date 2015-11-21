using System;
using ConsoleClient.Forms.Misc;
using ConsoleClient.Users;
using Logging;
using Packets;
using Packets.Attributes;
using Packets.Client.Character;
using Packets.Infrastructure;
using Packets.Server.Character;

namespace ConsoleClient.Forms
{
    [OpenWindows(1)]
    partial class CharCreateForm : BaseForm
    {
        public void OnCharCreateresult(Packet packet)
        {
            var p = (CharacterCreationResultPacket)packet;
            if (p.Success)
            {
                this.Call(Close);
            }
            else
            {
                Out.Red(p.Reason);
                lblReason.Call(() => lblReason.Text = p.Reason);
            }

        }

        private CharCreateReceiver _receiver;

        public CharCreateForm(Game game)
            : base(game)
        {
            InitializeComponent();
            _receiver = new CharCreateReceiver(OnCharCreateresult);
            Game.Connection.AddSubscriber(_receiver);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                PacketWriter.WritePacket(new CreateCharacterPacket { Name = txtName.Text }, Game.Connection.Writer);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((txtName.Text)))
            {
                PacketWriter.WritePacket(new DeleteCharacterPacket { Name = txtName.Text }, Game.Connection.Writer);
            }
        }
    }

    class CharCreateReceiver : PacketSubscriber
    {
        private Action<Packet> _action;
        public CharCreateReceiver(Action<Packet> action)
        {
            _action = action;
        }
        [PacketMethod(typeof(CharacterCreationResultPacket))]
        public void OnCreateResult(Packet packet)
        {
            _action(packet);
        }
    }
}