using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    public interface IUserRepository {
        public User AddUser(string username, string password);
        public User CheckPasswordAttempt(string username, string password);
    }
}