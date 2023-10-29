using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    public interface IUserRepository {
        public bool AddUser(User user);
        public bool CheckPasswordAttempt(string passwordHash);
    }
}