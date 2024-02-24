using PWManager.Domain.Common;

namespace PWManager.Domain.Entities {
    public class Group : Entity {
        public string UserId { get; set; }
        
        public string Identifier { get; set; }
        
        public List<Account> Accounts { get; set; }

        public Group(string userId) : this(userId, new List<Account>()) {}
        public Group(string id, DateTimeOffset created, DateTimeOffset updated, string userId, string identifier) : this(id, created, updated, userId, identifier,new List<Account>()) {}

        public Group(string userId, List<Account> accounts) : base() {
            UserId = userId;
            Accounts = accounts;
        }

        public Group(string id, DateTimeOffset created, DateTimeOffset updated, string userId, string identifier, List<Account> accounts) : base(id, created, updated) {
            UserId = userId;
            Identifier = identifier;
            Accounts = accounts;
        }

        public void AddAccount(Account acc) {
            Accounts.Add(acc);
        }

        public Account? GetAccount(string accId) {
            return Accounts.Find(e => e.Id.Equals(accId));
        }

        public bool RemoveAccount(string accId) {
            var acc = GetAccount(accId);
            return acc is not null && RemoveAccount(acc);
        }

        public bool RemoveAccount(Account acc) {
            return Accounts.Remove(acc);
        }

    }
}
