using System;
using ConsoleClient.Config;
using ConsoleClient.Forms;
using ConsoleClient.Network;
using System.Windows.Forms;
using ConsoleClient.Properties;
using Packets.Infrastructure;

namespace ConsoleClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Initial setup for the game to work. 
            PacketRegister.CreateRegistry();
            ConfigManager<ConfigMappings> config = ConfigManager<ConfigMappings>.Resolve("settings.ini");
            Connection connection = Connection.Create(config);
            FormManager formManager = new FormManager();
            Game game = new Game(connection, formManager, config, Convert.ToDouble(Resources.VERSION));
            formManager.Game = game;

            LoginForm form = formManager.OpenOrGetForm<LoginForm>();
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                formManager.DisposeForm(form);
                game.Run(120, 120);
            }
        }
    }
}
