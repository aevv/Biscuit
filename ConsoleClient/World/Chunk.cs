using ConsoleClient.Graphics;
using ConsoleClient.Properties;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.World
{
    class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }

        public float WorldX
        {
            get { return X * 64 * 32; }
        }

        public float WorldY
        {
            get { return Y * 64 * 32; }
        }
        public Camera Camera { get; set; }

        public bool CanDraw
        {
            get
            {
                float xCam = 0f, yCam = 0f;

                if (Camera != null)
                {
                    xCam = Camera.X;
                    yCam = Camera.Y;
                }

                var xMin = WorldX + xCam;
                var xMax = ((X + 1) * 64 * 32) - 1 + xCam;
                var yMin = WorldY + yCam;
                var yMax = ((Y + 1) * 64 * 32) - 1 + yCam;

                if (xMax < 0 || yMax < 0)
                    return false;
                if (xMin > 1024f || yMin > 768f)
                    return false;

                return true;
            }
        }

        private Dictionary<int, List<Tile>> _layers;

        public Chunk(int x, int y)
        {
            _layers = new Dictionary<int, List<Tile>>();
            X = x;
            Y = y;
        }
        public bool OnRenderFrame(FrameEventArgs args)
        {
            if (!CanDraw)
                return false;

            foreach (var layer in _layers)
            {
                foreach (var tile in layer.Value)
                {
                    tile.OnRenderFrame(args);
                }
            }

            return true;
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {

        }

        public void LoadFromString(string data)
        {
            var total = data.Split(new[] { "-\r\n" }, StringSplitOptions.None);
            var header = total[0];
            var layer = Convert.ToInt32(header);

            if (!_layers.ContainsKey(layer))
            {
                _layers.Add(layer, new List<Tile>());
            }

            string[] rows = total[1].Split(new[] { ",\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int y = 0; y < rows.Length; y++)
            {
                string[] tiles = rows[y].Split(',');
                for (int x = 0; x < tiles.Length; x++)
                {
                    var tileId = Convert.ToInt32(tiles[x]);
                    int tileX = 0, tileY = 0;
                    if (tileId == -1)
                    {
                        tileX = -1;
                        tileY = -1;
                    }
                    else
                    {
                        tileY = tileId == 0 ? 0 : tileId / 32;
                        tileX = tileId == 0 ? 0 : tileId % 32;
                    }
                    _layers[layer].Add(new Tile((x * 32) + WorldX, (y * 32) + WorldY, 32, 32, Resources.TILESET_2, tileX, tileY) { Camera = Camera });
                }
            }

            _layers = _layers.OrderBy(a => a.Key).ToDictionary(a => a.Key, b => b.Value);
        }

        public override string ToString()
        {
            return string.Format("Chunk: {0},{1}", X, Y);
        }
    }
}
