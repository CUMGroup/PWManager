namespace PWManager.Application.Services.Interfaces {
    public interface ILoginService {
        public bool Login(string username, string password, string dbPath);

        public bool CheckPassword(string username, string password);
    }
}
