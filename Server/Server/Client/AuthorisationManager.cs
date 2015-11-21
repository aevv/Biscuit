using DapperExtensions;
using Server.Server.Data;
using System.Collections.Generic;

namespace Server.Server.Client
{
    class AuthorisationManager
    {

        private static AuthorisationManager _instance;

        private AuthorisationManager()
        {

        }

        public static AuthorisationManager Resolve()
        {
            return _instance ?? (_instance = new AuthorisationManager());
        }

        public Account TryLogin(string user, string pass)
        {
            var data = DataConnection.Resolve();
            var group = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            group.Predicates.Add(Predicates.Field<Account>(a => a.Username, Operator.Eq, user));
            group.Predicates.Add(Predicates.Field<Account>(a => a.Password, Operator.Eq, pass));

            return data.Repo.Get<Account>(group);
        }
    }
}
