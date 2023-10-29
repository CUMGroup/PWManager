using PWManager.Domain.Common;

namespace PWManager.Domain.Entities {
    public class Account : Entity {
        public string LoginName { get; set; }
        public string Password { get; set; }
    
        public Account(string loginName, string password) : base() { 
            LoginName = loginName;
            Password = password;
        }

        public Account(string id, DateTimeOffset created, DateTimeOffset updated, string loginName, string password) : base(id, created, updated) {
            LoginName = loginName;
            Password = password;
        }
    }
}
