using DataAccessLayer.Classes;
using DataAccessLayer.Interfaces;
using System.Configuration;

namespace Server.Server.Data
{
    class DataConnection
    {
        public IDatabaseRepository Repo
        {
            get { return _repo; }
        }

        private IDatabaseRepository _repo;

        private static DataConnection _instance;
        public static DataConnection Resolve()
        {
            return _instance ?? (_instance = new DataConnection());
        }

        private DataConnection()
        {
            _repo = new DatabaseRepository(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }
    }
}
