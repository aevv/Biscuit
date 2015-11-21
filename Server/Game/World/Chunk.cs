using System.Linq;
using DapperExtensions;
using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;
using Server.Server.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Game.World
{
    [Table(Name = "Chunk")]
    class Chunk
    {
        [Column(Name = "Chunk_X")]
        public int X { get; set; }
        [Column(Name = "Chunk_Y")]
        public int Y { get; set; }
        [Column(Name = "Chunk_Id"), PrimaryKey(KeyType = KeyType.Guid)]
        public Guid Id { get; set; }
        [Column(Name = "Chunk_Map_Id")]
        public Guid MapId { get; set; }

        private Dictionary<int, int[,]> _layers;

        public Chunk()
        {
            _layers = new Dictionary<int, int[,]>();
        }

        public void LoadChunk()
        {
            var layers =
                DataConnection.Resolve().Repo.GetList<Layer>(Predicates.Field<Layer>(l => l.ChunkId, Operator.Eq, Id));
            foreach (var l in layers.Where(l => File.Exists(l.Filepath)))
            {
                using (var sr = new StreamReader(l.Filepath))
                {
                    string line = sr.ReadLine();
                    var layer = Convert.ToInt32(line);

                    if (!_layers.ContainsKey(layer))
                    {
                        _layers.Add(layer, new int[64, 64]);
                    }

                    int row = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tiles = line.Split(',');
                        for (int y = 0; y < tiles.Length; y++)
                        {
                            _layers[layer][row, y] = Convert.ToInt32(tiles[y]);
                        }
                        row++;
                    }
                }
            }
        }

        public List<string> GetDataAsString()
        {
            var result = new List<string>();
            foreach (var layer in _layers)
            {
                StringBuilder sr = new StringBuilder();
                sr.AppendLine(string.Format("{0}-", layer.Key));
                for (int x = 0; x < layer.Value.GetLength(0); x++)
                {
                    for (int y = 0; y < layer.Value.GetLength(1); y++)
                    {
                        sr.Append(string.Format("{0},", layer.Value[x, y]));
                    }
                    sr.AppendLine();
                }
                result.Add(sr.ToString());
            }
            return result;
        }
    }
}
