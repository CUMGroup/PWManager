using PWManager.Domain.Common;

namespace PWManager.Domain.Entities {
    public class Account : Entity {
        
        public string Identifier { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
    
        public Account(string identifier, string loginName, string password) : base() {
            Identifier = identifier;
            LoginName = loginName;
            Password = password;
        }

        public Account(string id, DateTimeOffset created, DateTimeOffset updated, string identifier, string loginName, string password) : base(id, created, updated) {
            Identifier = identifier;
            LoginName = loginName;
            Password = password;
        }
    }
}
