using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;
using System;

namespace Server.Game.World
{
    [Table(Name = "Layer")]
    class Layer
    {
        [Column(Name = "Layer_Filepath")]
        public string Filepath { get; set; }
        [Column(Name = "Layer_Id"), PrimaryKey(KeyType = KeyType.Guid)]
        public Guid Id { get; set; }
        [Column(Name = "Layer_Chunk_Id")]
        public Guid ChunkId { get; set; }
    }
}
