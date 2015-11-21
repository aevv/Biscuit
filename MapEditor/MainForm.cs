using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleClient.Config;
using MapEditor.Config;
using MapEditor.UI;

namespace MapEditor
{
    partial class MainForm : Form
    {
        private int _resolution = 24;
        private Random _random = new Random();
        private List<Row> _rows = new List<Row>();
        private readonly ConfigManager<ConfigMappings> _config;
        private readonly Dictionary<string, Image> _tileSets; 

        public Tile this[int x, int y]
        {
            get { return _rows.First(r => r.Y == y).Tiles.First(t => t.X == x); } 
        }

        public MainForm()
        {
            _config = ConfigManager<ConfigMappings>.Resolve("settings.ini");
            _tileSets = new Dictionary<string, Image>();
            InitializeComponent();
            InitialiseRows();
            InitialiseTileSets();
        }

        private void InitialiseTileSets()
        {
            DirectoryInfo di = new DirectoryInfo(_config.TryGet(c => c.TileSetsFolder));

            if (di.Exists)
            {
                foreach (var file in di.GetFiles())
                {
                    if (file.Extension.ToLower().Contains("png"))
                    {
                        _tileSets.Add(file.Name, Image.FromFile(file.FullName));
                        tileSetsListView.Items.Add(file.Name);
                    }
                }
            }
        }

        private void InitialiseRows()
        {
            for (int y = _resolution - 1; y > -1; y--)
            {
                Row row = new Row(y);
                row.Panel = new Panel();
                row.Panel.Dock = DockStyle.Top;
                row.Panel.Size = new Size(0, 32);
                row.Panel.BackColor = y % 2 == 0 ? Color.Red : Color.Blue;
                mapPanel.Controls.Add(row.Panel);
                _rows.Add(row);
                for (int x = _resolution - 1; x > -1; x--)
                {
                    var tile = new Tile(x, y);
                    tile.Panel = new Panel();
                    tile.Panel.Dock = DockStyle.Left;
                    tile.Panel.BackColor = Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255));
                    tile.Panel.Size = new Size(32, 32);
                    row.Panel.Controls.Add(tile.Panel);
                    row.Tiles.Add(tile);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void flash1010ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this[10, 10].Panel.BackColor = Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255));
                    Thread.Sleep(30);
                }
            });
        }

        private void flashALlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this[_random.Next(_resolution), _random.Next(_resolution)].Panel.BackColor = Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255));
                    Thread.Sleep(5);
                }
            });
        }
    }
}
