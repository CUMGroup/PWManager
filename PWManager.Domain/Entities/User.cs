using PWManager.Domain.Common;

namespace PWManager.Domain.Entities {
    public class User : Entity {
        
        public string UserName { get; init; }

        public User(string id, DateTimeOffset created, DateTimeOffset updated, string userName) : base(id, created, updated) {
            UserName = userName;
        }
    }
}
