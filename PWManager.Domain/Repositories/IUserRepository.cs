using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    internal interface IUserRepository {
        public bool AddUser(User user);
        public bool CheckPasswordAttempt(string passwordHash);
    }
}
