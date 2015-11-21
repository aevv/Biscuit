using System.Collections.Generic;
using DapperExtensions;
using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;
using DataAccessLayer.Interfaces;
using System;
using Server.Game.Chars;
using Server.Server.Data;

namespace Server.Server.Client
{
    [Table(Name = "Account")]
    class Account : IEntity
    {
        private string _password;

        public Account()
        {

        }

        public Account(string userName, string password, Guid id)
        {
            Username = userName;
            Password = password;
            Id = id;
        }

        public override string ToString()
        {
            return Username;
        }

        [Column(Name = "Acc_Username")]
        public string Username { get; private set; }

        [Column(Name = "Acc_Password")]
        public string Password
        {
            get { return "Hack"; }
            set { _password = value; }
        }

        [Column(Name = "Acc_Id"), PrimaryKey(KeyType = KeyType.Guid)]
        public Guid Id { get; private set; }

        #region Static DB Stuff
        public static IEnumerable<Character> GetCharactersForAccount(Account account)
        {
            var data = DataConnection.Resolve();
            var group = new PredicateGroup() {Operator = GroupOperator.And, Predicates = new List<IPredicate>()};
            group.Predicates.Add(Predicates.Field<Character>(a => a.AccountId, Operator.Eq, account.Id));
            group.Predicates.Add(Predicates.Field<Character>(a => a.Deleted, Operator.Eq, false));


            return data.Repo.GetList<Character>(group, buffered: false);
        }

        #endregion
    }
}
