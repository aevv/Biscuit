using ConsoleClient.Network;
using Packets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleClient.Forms
{
    /// <summary>
    /// BaseForm that all UI forms should inherit from for use with FormManager.
    /// </summary>
    class BaseForm : Form
    {
        protected Game Game { get; set; }

        // Set this for searching for forms when multiple are open
        public string Criteria { get; set; }

        public virtual void GivePacket(Packet packet)
        {
            
        }

        protected BaseForm(Game game)
        {
            Game = game;
        }

        protected BaseForm()
        {
            
        }

        public void Run()
        {
            Task.Factory.StartNew(() => ShowDialog());
        }
    }
}
