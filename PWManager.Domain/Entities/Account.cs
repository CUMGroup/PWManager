using PWManager.Domain.Common;
using PWManager.Domain.ValueObjects;

namespace PWManager.Domain.Entities {
    public class Account : Entity {
        public string LoginName { get; set; }
        public Password Password { get; set; }
    
        public Account(string loginName, Password password) : base() { 
            LoginName = loginName;
            Password = password;
        }

        public Account(string id, DateTimeOffset created, DateTimeOffset updated, string loginName, Password password) : base(id, created, updated) {
            LoginName = loginName;
            Password = password;
        }
    }
}
