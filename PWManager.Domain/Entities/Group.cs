using PWManager.Domain.Common;

namespace PWManager.Domain.Entities {
    public class Group : Entity {
        public string UserID { get; set; }
        public List<Account> Accounts { get; set; }

        public Group(string userID) : this(userID, new List<Account>()) {}
        public Group(string id, DateTimeOffset created, DateTimeOffset updated, string userID) : this(id, created, updated, userID, new List<Account>()) {}

        public Group(string userID, List<Account> accounts) : base() {
            UserID = userID;
            Accounts = accounts;
        }

        public Group(string id, DateTimeOffset created, DateTimeOffset updated, string userID, List<Account> accounts) : base(id, created, updated) {
            UserID = userID;
            Accounts = accounts;
        }
    }
}
