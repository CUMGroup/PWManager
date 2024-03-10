namespace PWManager.Application.Services.Interfaces; 

public interface IAccountService {

    public List<string> GetCurrentAccountNames();

    public void AddNewAccount(string identifier, string loginname, string password);
    
    public void CopyPasswordToClipboard(string identifier);
    public void CopyLoginnameToClipboard(string identifier);

    public void RegeneratePassword(string identifier);

    public void DeleteAccount(string identifier);

}