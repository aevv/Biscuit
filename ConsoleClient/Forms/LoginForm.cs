using ConsoleClient.Forms.Misc;
using ConsoleClient.Properties;
using Logging;
using Packets;
using Packets.Attributes;
using Packets.Client.Login;
using Packets.Infrastructure;
using Packets.Server.Login;
using System;
using System.Windows.Forms;

namespace ConsoleClient.Forms
{
    /// <summary>
    /// Form used to log in to the game. A bit hard coded to be created on launch and prior to any graphics. 
    /// </summary>
    [OpenWindows(1)]
    partial class LoginForm : BaseForm
    {
        private bool _loggingIn;
        private LoginFormReceiver _receiver;

        public LoginForm(Game game) : base(game)
        {
            InitializeComponent();
            _receiver = new LoginFormReceiver(LoginRequest, LoginResult);
            Game.Connection.AddSubscriber(_receiver);
        }

        private void ControlState(bool state)
        {
            userTextBox.Call(() => userTextBox.Enabled = state);
            passwordTextBox.Call(() => passwordTextBox.Enabled = state);
            loginButton.Call(() => loginButton.Enabled = state);
        }

        private void LoginRequest(Packet packet)
        {
            loginButton.Call(() => loginButton.Text = Resources.LoginForm_LoginRequest_Login);

            if (Game.ConfigManager.Query(c => !string.IsNullOrEmpty(c.Account)))
            {
                userTextBox.Call(() => userTextBox.Text = Game.ConfigManager.TryGet(c => c.Account));
            }

            if (Game.ConfigManager.Query(c => !string.IsNullOrEmpty(c.Password)))
            {
                passwordTextBox.Call(() => passwordTextBox.Text = Game.ConfigManager.TryGet(c => c.Password));
            }

            if (!Game.ConfigManager.Query(c => c.AutoLogin)) return;
            if (userTextBox.Text != "" && passwordTextBox.Text != "")
            {
                Login();
            }
        }

        private void LoginResult(Packet packet)
        {
            var p = (LoginResultPacket)packet;

            if (p.Success)
            {
                DialogResult = DialogResult.OK;
                this.Call(Close);
            }
            else
            {
                infoLabel.Call(() => infoLabel.Text = string.Format("Login failed for user {0}.", userTextBox.Text));
                _loggingIn = false;
                ControlState(true);
                loginButton.Call(() => loginButton.Text = Resources.LoginForm_LoginRequest_Login);
            }
        }

        private void Login()
        {
            if (_loggingIn) return;
            ControlState(false);
            PacketWriter.WritePacketAsync(new LoginPacket { Username = userTextBox.Text, Password = passwordTextBox.Text }, Game.Connection.Writer);
            _loggingIn = true;
            loginButton.Call(() => loginButton.Text = Resources.LoginForm_Login_Logging_in___);
        }

        private void LoginButtonClick(object sender, EventArgs e)
        {
            Login();
        }

        private void PasswordBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void UserBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                passwordTextBox.Focus();
            }
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            try
            {
                Game.Connection.Begin();
                ControlState(true);
            }
            catch (Exception ex)
            {
                infoLabel.Text = Resources.LoginForm_LoginForm_Shown_Failed_to_connect_to_server_;
            }
        }
    }

    class LoginFormReceiver : PacketSubscriber
    {
        private Action<Packet> _req, _res;
        public LoginFormReceiver(Action<Packet> request, Action<Packet> result)
        {
            _req = request;
            _res = result;
        }

        [PacketMethod(typeof (LoginRequestPacket))]
        public void OnRequest(Packet packet)
        {
            _req(packet);
        }

        [PacketMethod(typeof (LoginResultPacket))]
        public void OnResult(Packet packet)
        {
            _res(packet);
        }
    }
}
