namespace PWManager.Application.Services.Interfaces; 

public interface IAccountService {

    public List<string> GetCurrentAccountNames();

    public void AddNewAccount(string identifier, string loginname, string password);

}