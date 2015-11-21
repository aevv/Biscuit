using System.Collections.Generic;
using DapperExtensions;
using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;
using Server.Game.Chars.DataObject;
using System;
using Server.Server.Client;
using Server.Server.Data;

namespace Server.Game.Chars
{
    [Table(Name = "Character")]
    class Character
    {
        public CharacterLocation Location { get; set; }

        public Character()
        {
        }

        [Column(Name = "Char_Name")]
        public string Name { get; set; }
        [Column(Name = "Char_Id"), PrimaryKey(KeyType = KeyType.Guid)]
        public Guid Id { get; set; }
        [Column(Name = "Char_Acc_Id")]
        public Guid AccountId { get; set; }
        [Column(Name = "Char_Deleted")]
        public bool Deleted { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public void Save()
        {
            var data = DataConnection.Resolve().Repo;
            data.Update(this);
            data.Update(Location);
        }
        public CharacterLocation GetCharacterLocation(Guid defaultMapId)
        {
            var data = DataConnection.Resolve();
            var pred = Predicates.Field<CharacterLocation>(a => a.CharacterId, Operator.Eq, Id);

            var loc = data.Repo.Get<CharacterLocation>(pred) ?? new CharacterLocation
            {
                Id = Guid.NewGuid(),
                CharacterId = Id,
                MapId = defaultMapId,
                X = 25,
                Y = 25
            };

            return loc;
        }


        #region Static DB stuff

        public static Character GetCharacter(Account account = null, Guid id = default(Guid), string name = null)
        {
            var data = DataConnection.Resolve().Repo;
            var group = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            if (account != null)
                group.Predicates.Add(Predicates.Field<Character>(a => a.AccountId, Operator.Eq, account.Id));

            if (name != null)
                group.Predicates.Add(Predicates.Field<Character>(a => a.Name, Operator.Eq, name));

            if (id != default(Guid))
                group.Predicates.Add(Predicates.Field<Character>(a => a.Id, Operator.Eq, id));

            if (group.Predicates.Count == 0)
                throw new Exception("No predicates created");

            group.Predicates.Add(Predicates.Field<Character>(a => a.Deleted, Operator.Eq, false));

            return data.Get<Character>(group);
        }
        #endregion
    }
}
