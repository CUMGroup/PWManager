using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    public interface IUserRepository {
        public User AddUser(string username, string password);
        public bool CheckPasswordAttempt(string username, string password);
    }
}