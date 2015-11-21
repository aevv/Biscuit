using System;

namespace ConsoleClient
{
    class User
    {
        public Guid OnlineId { get; set; }
        public string UserName { get; set; }
        public bool LoggedIn { get; set; }
    }
}
