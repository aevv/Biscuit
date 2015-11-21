using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;
using System;

namespace Server.Game.Chars.DataObject
{
    [Table(Name = "CharacterLocation")]
    class CharacterLocation
    {
        [Column(Name = "Loc_Id"), PrimaryKey(KeyType = KeyType.Guid)]
        public Guid Id { get; set; }
        [Column(Name = "Loc_X")]
        public float X { get; set; }
        [Column(Name = "Loc_Y")]
        public float Y { get; set; }
        [Column(Name = "Loc_Map_Id")]
        public Guid MapId { get; set; }
        [Column(Name = "Loc_Char_Id")]
        public Guid CharacterId { get; set; }
    }
}
