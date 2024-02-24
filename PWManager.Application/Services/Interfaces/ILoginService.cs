namespace PWManager.Application.Services.Interfaces {
    public interface ILoginService {
        public void Login(string username, string password, string dbPath);
    }
}
